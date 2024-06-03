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
    static class SelectDataFromPersoDB_AllForm
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
                        //public string codZap;       //0
                        //public string regNum;       //54
                        //public string tip;          //2
                        //public string tipForm;      //4
                        //public string tipSveden;    //5   
                        //public string year;         //6
                        //public string priem;        //57
                        //public string dataPriem;    //8
                        //public string statVIO;      //58
                        //public string dataVIO;      //40
                        //public string statusRec;    //41
                        //public string kolZL;        //10
                        //public string kolZlLGT;     //61
                        //public string kolZl_PR_VIO; //59
                        //public string kolZl_NPR_VIO;//60
                        //public string shname;       //55
                        //public string dateINS;      //45
                        //public string timeINS;      //46        
                        //public string dataUved;     //48
                        //public string dataOtp;      //50

            //@"select " +
            //@"a.COD_ZAP, " +                            //0
            //@"a.TIP, " +                                //1
            //@"a.TIP_FORM, " +                           //2
            //@"a.TIP_SVEDEN, " +                         //3
            //@"a.GOD, " +                                //4
            //@"a.D_PRIEM, " +                            //5
            //@"a.DATE_VIO, " +                           //6
            //@"a.STATUS_REC, " +                         //7
            //@"a.COL_ZL, " +                             //8
            //@"a.DATE_INS, " +                           //9
            //@"a.TIME_INS, " +                           //10
            //@"a.DATE_UVED, " +                          //11
            //@"a.DATE_OTP, " +                           //12
            //@"regnumb, " +                              //13
            //@"u.shname, " +                             //14
            ////@"s.region, " +                             //закомментировал
            //@"CASE " +                                  //15
            //@"WHEN bpi='T' or ks ='T' THEN 'БПИ' " +
            //@"WHEN bpi='F' and ks ='F' and cod_uz <> -1 THEN 'Лично' " +
            //@"WHEN cod_uz = -1 THEN 'По почте' " +
            //@"END as priem, " +
            //@"CASE " +                                  //16
            //@"WHEN STVIO=0 THEN 'Не подавался' " +
            //@"WHEN STVIO=1 THEN 'Не проверен' " +
            //@"WHEN STVIO=2 THEN 'Принят' " +
            //@"WHEN STVIO=3 THEN 'Не принят' " +
            //@"WHEN STVIO=4 THEN 'Принят частично' " +
            //@"END as statvio, " +
            //@"value(col_vio_pr, 0) as col_pr_vio, " +   //17
            //@"value(col_vio_np, 0) as col_npr_vio, " +  //18
            //@"value(col_lgot, 0) as col_lgt " +         //19

                        //Проверка кода региона 042                        

                        if (GetRegionFromRegNum(reader[13].ToString()) == "042")
                        {
                            Program.listReestrSZVSTAG.Add(new DataFromPersoDB(reader[0].ToString(), SelectRaion(reader[13].ToString()), ConvertRegNom(reader[13].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(),
                                                                              reader[15].ToString(), "", ConvertDataFromDB(reader[5].ToString()), reader[16].ToString(), ConvertDataFromDB(reader[6].ToString()), reader[7].ToString(), reader[8].ToString(),
                                                                              reader[19].ToString(), reader[17].ToString(), reader[18].ToString(), reader[14].ToString(), ConvertDataFromDB(reader[9].ToString()), reader[10].ToString(),
                                                                              ConvertDataFromDB(reader[11].ToString()), ConvertDataFromDB(reader[12].ToString()))
                                                          );
                        }
                        //ConvertDataFromDB(reader[3].ToString()) );

                        //string regNumber = ConvertRegNom(reader[0].ToString());
                        //string raion = SelectRaion(reader[0].ToString());
                        //string curator = SelectKurator(raion, regNumber);


                        //listData.Add();
                        i++;

                    }
                    reader.Close();

                    //Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Количество выбранных строк из БД Perso: {0} ", i);
                    //Console.ForegroundColor = ConsoleColor.Gray;
                    //Console.WriteLine();

                    //Добавляем значение куратора и УП в выборку
                    foreach (var item in Program.listReestrSZVSTAG)
                    {
                        item.curator = SelectKurator(item.raion, item.regNum);


                        DataFromUPfile tmpData = new DataFromUPfile();
                        if (SelectDataFromUPfile.dictionary_UP.TryGetValue(item.regNum, out tmpData))
                        {
                            item.UP = tmpData.regNumUP;
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
        }

        private static string GetRegionFromRegNum(string regNum)
        {
            if (regNum.Count() == 11)
            {
                char[] regNomOld = regNum.ToCharArray();
                string region = "0" + regNomOld[0].ToString() + regNomOld[1].ToString();

                return region;
            }
            else
            {
                return "";
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
        public static void CreateExportFile(string zagolovok, List<DataFromPersoDB> listData, string nameFile)
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
}
