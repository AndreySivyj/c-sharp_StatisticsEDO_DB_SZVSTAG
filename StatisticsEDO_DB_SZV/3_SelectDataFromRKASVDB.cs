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
    static class SelectDataFromRKASVDB
    {
        //------------------------------------------------------------------------------------------        
        //Выбираем данные из РК АСВ
        async public static void SelectDataFromRKASV(string query)
        {
            //Очищаем коллекцию для данных из файлов
            Program.dictionary_dataFromPKASVDB.Clear();

            //Подключаемся к БД и выполняем запрос
            using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=asv;UID=db2inst;PWD=password;"))
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

                    DB2Command command = connection.CreateCommand();
                    command.CommandText = query;

                    //Устанавливаем значение таймаута
                    command.CommandTimeout = 570;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    int i = 0;

                    while (await reader.ReadAsync())
                    {
                        Program.dictionary_dataFromPKASVDB.Add(ConvertRegNom(reader[0].ToString()), new DataFromRKASVDB(ConvertRegNom(reader[0].ToString()), ConvertDataFromDB(reader[1].ToString()), ConvertDataFromDB(reader[2].ToString()),
                                    reader[3].ToString().Replace(";", string.Empty), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(),
                            ConvertDataFromDB(reader[7].ToString()), ConvertDataFromDB(reader[8].ToString()), reader[9].ToString(), reader[10].ToString(),
                            ConvertRaion(reader[11].ToString()), reader[12].ToString(), reader[13].ToString(), reader[14].ToString(), ConvertStatusID(reader[15].ToString())));

                        i++;
                    }
                    reader.Close();

                    //Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Количество выбранных строк из РК АСВ: {0} ", i);
                    //Console.ForegroundColor = ConsoleColor.Gray;
                    //Console.WriteLine();

                    //Добавляем значение куратора в выборку
                    foreach (var item in Program.dictionary_dataFromPKASVDB)
                    {
                        item.Value.kurator = SelectKurator(item.Value.raion, item.Value.insurer_reg_num);
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
        private static string ConvertStatusID(string statusID)
        {
            try
            {
                string tmpData = "";
                if (SelectDataFromStatusIDfile.dictionaryStatusID.TryGetValue(statusID, out tmpData))
                {
                    return SelectDataFromStatusIDfile.dictionaryStatusID[statusID];
                }
                else
                {
                    return "нет статуса в справочнике";
                }
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;

                return "ошибка поиска статуса";
            }
        }
                
        //------------------------------------------------------------------------------------------       
        //Импортируем в коллекции данных из Персо данные из РК АСВ
        public static void ImportDataFromRKASV()
        {
            foreach (var itemDataPerso in Program.listReestrSZVSTAG)
            {
                DataFromRKASVDB tmpData = new DataFromRKASVDB();
                if (Program.dictionary_dataFromPKASVDB.TryGetValue(itemDataPerso.regNum, out tmpData))
                {
                    itemDataPerso.nameStrah = tmpData.insurer_short_name;
                    itemDataPerso.dataPostPFR = tmpData.insurer_reg_start_date;
                    itemDataPerso.dataSnyatPFR = tmpData.insurer_reg_finish_date;
                    itemDataPerso.dataPostRO = tmpData.INSURER_REG_DATE_RO;
                    itemDataPerso.dataSnyatRO = tmpData.INSURER_UNREG_DATE_RO;
                    itemDataPerso.kategory = tmpData.category_code;
                    itemDataPerso.inn = tmpData.insurer_inn;
                    itemDataPerso.kpp = tmpData.insurer_kpp;
                }
            }
        }

        //------------------------------------------------------------------------------------------        
        //Формируем результирующий файл на основании данных из БД
        public static void CreateExportFile(string zagolovok, Dictionary<string, DataFromRKASVDB> dictionary_dataFromPKASVDB, string nameFile)
        {
            try
            {
                //Добавляем в файл данные                
                using (StreamWriter writer = new StreamWriter(nameFile, true, Encoding.GetEncoding(1251)))
                {
                    writer.WriteLine(zagolovok);

                    foreach (var item in dictionary_dataFromPKASVDB)
                    {
                        writer.WriteLine(item.Value.ToString());
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
        private static string ConvertRegNom(string regNom)
        {
            try
            {
                char[] regNomOld = regNom.ToCharArray();
                string regNomConvert = regNomOld[0].ToString() + regNomOld[1].ToString() + regNomOld[2].ToString() + "-" + regNomOld[3] + regNomOld[4] + regNomOld[5] + "-" + regNomOld[6] + regNomOld[7] + regNomOld[8] + regNomOld[9] + regNomOld[10] + regNomOld[11];


                return regNomConvert;
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
        private static string ConvertRaion(string raion)
        {
            try
            {
                if (raion.Count() == 1)
                {
                    return "042-00" + raion;
                }
                if (raion.Count() == 3)
                {
                    return "042-" + raion;
                }
                else
                {
                    return "042-0" + raion;
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

        public static string ConvertRegNomForSelect(string regNom)
        {
            char[] separator = { '-' };    //список разделителей в строке
            string[] massiveStr = regNom.Split(separator);     //создаем массив из строк между разделителями
            try
            {
                if (massiveStr.Count() == 3 && regNom.Count() == 14)
                {
                    return "42" + massiveStr[1] + massiveStr[2];
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());

                return "";
            }
        }
    }
}
