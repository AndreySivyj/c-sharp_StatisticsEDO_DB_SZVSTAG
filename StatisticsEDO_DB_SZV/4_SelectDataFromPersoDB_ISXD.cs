using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data.Common;
using IBM.Data.DB2;

namespace StatisticsEDO_DB_SZV
{
    #region считанная из файла информация
    public class DataFromPersoDB_ISXDform
    {
        // @"select distinct s.regnumb, w.strnum, g.DATE_BEG, g.DATE_END, o.DATE_INS, o.TIME_INS " +

        public string raion;
        public string regNum;
        public string strnum;
        public string dateBeg;
        public string dateEnd;
        public string dateINS;
        public string timeINS;

        public DataFromPersoDB_ISXDform(string raion = "", string regNum = "", string strnum = "", string dateBeg = "", string dateEnd = "", string dateINS = "", string timeINS = "")
        {
            this.raion = raion;
            this.regNum = regNum;
            this.strnum = strnum;
            this.dateBeg = dateBeg;
            this.dateEnd = dateEnd;
            this.dateINS = dateINS;
            this.timeINS = timeINS;

        }

        public override string ToString()
        {
            return raion + ";" + regNum + ";" + strnum + ";" + dateBeg + ";" + dateEnd + ";" + dateINS + ";" + timeINS + ";";
        }
        public string ToStringNoStag()
        {
            return raion + ";" + regNum + ";" + strnum + ";" + dateINS + ";" + timeINS + ";";
        }
    }
    #endregion


    class SelectDataFromPersoDB_ISXDform
    {
        private static SortedSet<string> sortedSet_dataFromPersoDB_UniqSNILS = new SortedSet<string>();                //Коллекция всех данных из БД Perso

        //------------------------------------------------------------------------------------------        
        //Выбираем данные из БД Perso

        async public static void SelectDataFromPersoDB(string query)
        //async public static void SelectDataFromPersoDB(string query, string regNumItem, Dictionary<string, int> dictionary_svodDataFromPersoDB_UniqSNILS_SZVSTAG)
        {
            //Подключаемся к БД и выполняем запрос
            using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=PERSDB;UID=regusr;PWD=password;"))
            {
                try
                {
                    //открываем соединение
                    await connection.OpenAsync();

                    DB2Command command = connection.CreateCommand();
                    command.CommandText = query;

                    //Устанавливаем значение таймаута
                    command.CommandTimeout = 570;

                    DbDataReader reader = await command.ExecuteReaderAsync();




                    int i = 0;

                    while (await reader.ReadAsync())
                    {
                        //                        0         1       2               3           4           5           6
                        // @"select distinct s.regnumb, w.strnum, g.DATE_BEG, g.DATE_END, o.DATE_INS, o.TIME_INS, w.REGNUMB " +

                        if (reader[6].ToString().Count() == 14)
                        {
                            Program.listReestrSZV_ISXD.Add(new DataFromPersoDB_ISXDform(SelectRaion_v2_korr(reader[6].ToString()), reader[6].ToString(), reader[1].ToString(),
                                                                                    ConvertDataFromDB(reader[2].ToString()), ConvertDataFromDB(reader[3].ToString()),
                                                                                    ConvertDataFromDB(reader[4].ToString()), ConvertTimeFromDB(reader[5].ToString()))
                                                      );
                        }
                        else
                        {


                            Program.listReestrSZV_ISXD.Add(new DataFromPersoDB_ISXDform(SelectRaion(reader[0].ToString()), ConvertRegNom(reader[0].ToString()), reader[1].ToString(),
                                                                                    ConvertDataFromDB(reader[2].ToString()), ConvertDataFromDB(reader[3].ToString()),
                                                                                    ConvertDataFromDB(reader[4].ToString()), ConvertTimeFromDB(reader[5].ToString()))
                                                      );
                        }                                   

                        i++;
                    }
                    reader.Close();


                    Console.WriteLine("Количество выбранных строк из БД Perso: {0} ", i);


                    if (Program.listReestrSZV_ISXD.Count != 0)
                    {
                        //return codZap + ";" + raion + ";" + regNum + ";" + tip + ";" + tipForm + ";" + tipSveden + ";" + dataPriem + ";" + statVIO_doc + ";" + strnum + ";" + statVIO_snils + ";" + korPeriod + ";" + korGod + ";";

                        string zagolovokPersoISXD = "№ п/п" + ";" + "Район" + ";" + "РегНомер" + ";" + "СНИЛС" + ";" 
                                                        + "Стажевый период с" + ";" + "Стажевый период по" + ";"
                                                        + "Дата записи в БД" + ";" + "Время записи в БД";

                        string nameResultFile_PersoISXD = IOoperations.katalogOut + @"\" + @"_9_СЗВ-СТАЖ_SelectFromPersoDB_ИСХД_СНИЛС_" + DateTime.Now.ToShortDateString() + ".csv";
                        if (File.Exists(nameResultFile_PersoISXD)) { File.Delete(nameResultFile_PersoISXD); }

                        //Формируем результирующий файл
                        CreateExportFile(zagolovokPersoISXD, Program.listReestrSZV_ISXD, nameResultFile_PersoISXD);
                    }

                }
                catch (Exception ex)
                {
                    IOoperations.WriteLogError(ex.ToString());

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        //------------------------------------------------------------------------------------------        
        //Формируем результирующий файл на основании данных из БД
        public static void CreateExportFile(string zagolovok, List<DataFromPersoDB_ISXDform> listData, string nameFile)
        {
            try
            {
                //Добавляем в файл данные                
                using (StreamWriter writer = new StreamWriter(nameFile, true, Encoding.GetEncoding(1251)))
                {
                    writer.WriteLine(zagolovok);

                    int i = 0;

                    foreach (var item in listData)
                    {
                        i++;
                        writer.Write(i + ";");
                        writer.WriteLine(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        //------------------------------------------------------------------------------------------
        private static string ConvertDataFromDB(string dataTime)
        {
            try
            {
                if (dataTime != "")
                {
                    DateTime date = Convert.ToDateTime(dataTime);
                    return date.ToShortDateString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;

                return "";
            }
        }

        //------------------------------------------------------------------------------------------
        private static string ConvertTimeFromDB(string dataTime)
        {
            try
            {
                if (dataTime != "")
                {
                    DateTime date = Convert.ToDateTime(dataTime);
                    return date.ToShortTimeString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;

                return "";
            }
        }

        //------------------------------------------------------------------------------------------
        private static string ConvertRegNom(string regNum)
        {
            try
            {
                if (regNum.Count() == 11)
                {
                    char[] regNomOld = regNum.ToCharArray();
                    string regNomConvert = "0" + regNomOld[0].ToString() + regNomOld[1].ToString() + "-" + regNomOld[2].ToString() + regNomOld[3] + regNomOld[4] + "-" + regNomOld[5] + regNomOld[6] + regNomOld[7] + regNomOld[8] + regNomOld[9] + regNomOld[10];

                    return regNomConvert;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;

                return "";
            }
        }

        //------------------------------------------------------------------------------------------
        private static string SelectRaion(string regNum)
        {
            try
            {
                if (regNum.Count() == 11)
                {
                    return "042-0" + regNum[3] + regNum[4];
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;

                return "";
            }
        }

        //------------------------------------------------------------------------------------------
        private static string SelectRaion_v2_korr(string regNum)
        {
            try
            {                
                    return "042-0" + regNum[5] + regNum[6];                
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;

                return "";
            }
        }

    }
}
