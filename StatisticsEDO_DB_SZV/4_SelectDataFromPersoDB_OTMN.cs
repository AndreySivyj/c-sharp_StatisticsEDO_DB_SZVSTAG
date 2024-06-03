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
    public class DataFromPersoDB_OTMNform
    {
        //@"select distinct s.regnumb, w.strnum, o.DATE_INS, o.TIME_INS "
        
        public string raion;
        public string regNum;        
        public string strnum;        
        public string dateINS;
        public string timeINS;

        public DataFromPersoDB_OTMNform(string raion = "", string regNum = "", string strnum = "", string dateINS = "", string timeINS = "")
        {            
            this.raion = raion;
            this.regNum = regNum;            
            this.strnum = strnum;
            this.dateINS = dateINS;
            this.timeINS = timeINS;

        }

        public override string ToString()
        {
            return raion + ";" + regNum + ";" + strnum + ";" + dateINS + ";" + timeINS + ";";
        }
    }
    #endregion


    #region Выбор данных из файла
    static class SelectDataFromPersoDB_OTMNform
    {
        //------------------------------------------------------------------------------------------        
        //Выбираем данные из БД Perso
        async public static void SelectDataFromPerso(string query)
        {
            //Подключаемся к БД и выполняем запрос
            using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=PERSDB;UID=regusr;PWD=password;"))
            {
                try
                {
                    //открываем соединение
                    await connection.OpenAsync();
                    //Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Соединение с БД: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(connection.State);
                    //Console.WriteLine();                    
                    //Console.WriteLine();                    

                    DB2Command command = connection.CreateCommand();
                    command.CommandText = query;

                    //Устанавливаем значение таймаута
                    command.CommandTimeout = 1770;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    int i = 0;

                    while (await reader.ReadAsync())
                    {
                        string korr_regNum = reader[4].ToString();

                        //                      0           1       2             3         4_new
                        //@"select distinct s.regnumb, w.strnum, o.DATE_INS, o.TIME_INS, w.REGNUMB "

                        //DataFromPersoDB_OTMNform(string raion = "", string regNum = "", string strnum = "", string dateINS = "", string timeINS = "")

                        if (reader[4].ToString().Count()==14)
                        {
                            Program.listReestrSZV_OTMN.Add(new DataFromPersoDB_OTMNform(SelectRaion_v2_korr(reader[4].ToString()), reader[4].ToString(), reader[1].ToString(),
                                                                                        ConvertDataFromDB(reader[2].ToString()), ConvertTimeFromDB(reader[3].ToString()))
                                                          );
                        }
                        else
                        {


                            Program.listReestrSZV_OTMN.Add(new DataFromPersoDB_OTMNform(SelectRaion(reader[0].ToString()), ConvertRegNom(reader[0].ToString()), reader[1].ToString(),
                                                                                        ConvertDataFromDB(reader[2].ToString()), ConvertTimeFromDB(reader[3].ToString()))
                                                          );
                        }

                        //Признак наличия ОТМН формы
                        Program.dictionary_ReestrSZV_OTMN[ConvertRegNom(reader[0].ToString())] = "Да";

                        i++;

                    }
                    reader.Close();

                    //Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Количество выбранных строк из БД Perso: {0} ", i);
                    //Console.ForegroundColor = ConsoleColor.Gray;
                    //Console.WriteLine();     

                    if (Program.listReestrSZV_OTMN.Count != 0)
                    {
                        //return codZap + ";" + raion + ";" + regNum + ";" + tip + ";" + tipForm + ";" + tipSveden + ";" + dataPriem + ";" + statVIO_doc + ";" + strnum + ";" + statVIO_snils + ";" + korPeriod + ";" + korGod + ";";

                        string zagolovokPersoOTMN = "№ п/п" + ";" + "Район" + ";" + "РегНомер" + ";" + "СНИЛС" + ";" + "Дата записи в БД" + ";" + "Время записи в БД";                                                    

                        string nameResultFile_PersoOTMN = IOoperations.katalogOut + @"\" + @"_9_СЗВ-СТАЖ_SelectFromPersoDB_ОТМН_СНИЛС_" + DateTime.Now.ToShortDateString() + ".csv";
                        if (File.Exists(nameResultFile_PersoOTMN)) { File.Delete(nameResultFile_PersoOTMN); }

                        //Формируем результирующий файл
                        CreateExportFile(zagolovokPersoOTMN, Program.listReestrSZV_OTMN, nameResultFile_PersoOTMN);
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

        private static string ConvertWSTVIO(string stvio)
        {
            string ret = "";

            switch (stvio)
            {
                case "1":
                    ret = "Не проверен";
                    break;
                case "2":
                    ret = "Проверен, ошибка";
                    break;
                case "3":
                    ret = "Проверен, не принят";
                    break;
                case "4":
                    ret = "Проверен, принят";
                    break;
                case "5":
                    ret = "Проверен, отменен";
                    break;
                case "6":
                    ret = "Проверен, не актуален";
                    break;

                default:
                    break;
            }
            return ret;
        }

        private static string ConvertOSTVIO(string stvio)
        {
            string ret = "";

            switch (stvio)
            {
                case "0":
                    ret = "Не подавался в ВИО";
                    break;
                case "1":
                    ret = "Не проверен";
                    break;
                case "2":
                    ret = "Принят";
                    break;
                case "3":
                    ret = "Не принят";
                    break;
                case "4":
                    ret = "Принят частично";
                    break;

                default:
                    break;
            }
            return ret;
        }

        //------------------------------------------------------------------------------------------        
        //Формируем результирующий файл на основании данных из БД
        public static void CreateExportFile(string zagolovok, List<DataFromPersoDB_OTMNform> listData, string nameFile)
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

        //------------------------------------------------------------------------------------------
        //Выбираем код куратора
        private static string SelectKurator(string raion, string regNum)
        {
            try
            {
                string tmpKurator = "";


                //TODO: Закомментировал "кураторы частично"
                ////Заполняем поле Куратор
                //if (raion == "042-001" || raion == "042-003")
                //{

                //    DataFromCuratorsFilePartial tmpData = new DataFromCuratorsFilePartial();
                //    if (SelectDataFromCuratorsFilePartial.dictionary_CuratorsPartial.TryGetValue(regNum, out tmpData))
                //    {
                //        tmpKurator = tmpData.curator;
                //    }
                //}
                //else
                //{
                    DataFromCuratorsFile tmpData = new DataFromCuratorsFile();
                    if (SelectDataFromCuratorsFile.dictionary_Curators.TryGetValue(raion, out tmpData))
                    {
                        tmpKurator = tmpData.curator;
                    }
                //}

                return tmpKurator;
            }
            catch (KeyNotFoundException ex)
            {
                IOoperations.WriteLogError(ex.ToString());
                return "";
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());
                return "";
            }
        }
    }
    #endregion
}
