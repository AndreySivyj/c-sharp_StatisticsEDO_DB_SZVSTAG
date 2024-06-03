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
    public class DataFromPersoDB_SZVM
    {
        public string regNum;
        public string strnum;
        public string inn;
        public string kpp;
        public string otchYear;
        public string otchMonth;
        public DateTime dataANDtimeInsertToDB;

        //this.kurator = kurator;

        public DataFromPersoDB_SZVM(string regNum = "", string strnum = "",
                                string inn = "", string kpp = "", string otchYear = "", string otchMonth = "",
                                DateTime dataANDtimeInsertToDB = default(DateTime))
        {
            this.regNum = regNum;
            this.strnum = strnum;
            this.inn = inn;
            this.kpp = kpp;
            this.otchYear = otchYear;
            this.otchMonth = otchMonth;
            this.dataANDtimeInsertToDB = dataANDtimeInsertToDB;
        }

        public override string ToString()
        {
            return regNum + ";" + strnum + ";" + inn + ";" + kpp + ";" + otchYear + ";" + otchMonth + ";"; //+ dataANDtimeInsertToDB + ";" ;
        }

        public string ToString_tmp()
        {
            return regNum + ";" + strnum + ";" + inn + ";" + kpp + ";" + otchYear + ";" + otchMonth + ";" + dataANDtimeInsertToDB + ";";
        }
    }


    static class SelectDataFromPersoDB_SZVM
    {
        //Коллекция уник СНИЛС из БД Perso, реестр форм СЗВ-М без ОТМН форм, KEY (regNum + INN + SNILS + KPP)
        public static Dictionary<string, DataFromPersoDB_SZVM> uniqSNILS_SZV_M = new Dictionary<string, DataFromPersoDB_SZVM>();

        private static Dictionary<string, DataFromPersoDB_SZVM> dictionary_uniqSNILS_ISXD_SZV_M = new Dictionary<string, DataFromPersoDB_SZVM>();       //Коллекция данных
        private static Dictionary<string, DataFromPersoDB_SZVM> dictionary_uniqSNILS_OTMN_SZV_M = new Dictionary<string, DataFromPersoDB_SZVM>();       //Коллекция данных  

        //public static Dictionary<string, RegNumAndDataTime> dictionary_SZV_M_itogTMP = new Dictionary<string, RegNumAndDataTime>();       //Коллекция данных 


        public static string zagolovokPersoSZV_short = "№ п/п" + ";" + "Район" + ";" + "Наименование" + ";" + "РегНомер" + ";" + "СНИЛС" + ";" + "ИНН" + ";"
                                                + "КПП" + ";" + "ОтчГод" + ";" + "ОтчМесяц" + ";" + "Куратор" + ";";



        async public static void SelectDataFromPerso_ISXD(string query_ISXD)
        {
            try
            {
                using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=PERSDB;UID=regusr;PWD=password;"))
                {

                    //открываем соединение

                    await connection.OpenAsync();

                    DB2Command command_ISXD = connection.CreateCommand();
                    command_ISXD.CommandText = query_ISXD;

                    //Устанавливаем значение таймаута
                    command_ISXD.CommandTimeout = 570;

                    DbDataReader reader_ISXD = await command_ISXD.ExecuteReaderAsync();

                    //int i_ISXD = 0;

                    while (await reader_ISXD.ReadAsync())
                    {
                        // @"select distinct p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " 

                        //public DataFromPersoDB(string regNum = "", string strnum = "",
                        //        string inn = "", string kpp = "", string otchYear = "", string otchMonth = "",
                        //        DateTime dateINS = default(DateTime), DateTime timeINS = default(DateTime))

                        //Console.WriteLine(reader_ISXD[4].ToString());

                        //public DateTime(int year, int month, int day, int hour, int minute, int second);
                        DateTime dataANDtimeInsert = new DateTime(
                                                                            Convert.ToDateTime(reader_ISXD[4]).Year,
                                                                            Convert.ToDateTime(reader_ISXD[4]).Month,
                                                                            Convert.ToDateTime(reader_ISXD[4]).Day,
                                                                            Convert.ToDateTime(reader_ISXD[5]).Hour,
                                                                            Convert.ToDateTime(reader_ISXD[5]).Minute,
                                                                            Convert.ToDateTime(reader_ISXD[5]).Second
                                                                          );



                        //TODO: Сделать ключ словаря KEY: regNum + INN + SNILS + KPP (без otchMonth), вернуть использование функции "AddOtchMonth()", по возможности



                        //регНом+СНИЛС есть в словаре
                        //KEY: regNum + INN + SNILS + KPP + otchMonth
                        DataFromPersoDB_SZVM tmpData = new DataFromPersoDB_SZVM();
                        if (dictionary_uniqSNILS_ISXD_SZV_M.TryGetValue(
                                                                        ConvertRegNom(reader_ISXD[0].ToString()) +
                                                                        Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn +
                                                                        reader_ISXD[1].ToString() +
                                                                        Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp +
                                                                        reader_ISXD[3].ToString(),
                                                                                                                                                                 out tmpData))
                        {
                            //сверяем дату и время импорта в БД (больше)
                            if (dataANDtimeInsert > tmpData.dataANDtimeInsertToDB)
                            {
                                //KEY: regNum + INN + SNILS + KPP + otchMonth
                                dictionary_uniqSNILS_ISXD_SZV_M[
                                   ConvertRegNom(reader_ISXD[0].ToString()) +
                                   Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn +
                                   reader_ISXD[1].ToString() +
                                   Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp +
                                   reader_ISXD[3].ToString()
                                                              ] =
                                                           new DataFromPersoDB_SZVM(
                                                               ConvertRegNom(reader_ISXD[0].ToString()),
                                                               reader_ISXD[1].ToString(),
                                                               Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn,
                                                               Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp,
                                                               reader_ISXD[2].ToString(),
                                                               reader_ISXD[3].ToString(),
                                                               dataANDtimeInsert
                                                                               );
                            }
                            //сверяем дату и время импорта в БД (равны)
                            else if (dataANDtimeInsert == tmpData.dataANDtimeInsertToDB)
                            {
                                dictionary_uniqSNILS_ISXD_SZV_M[
                                    ConvertRegNom(reader_ISXD[0].ToString()) +
                                    Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn +
                                    reader_ISXD[1].ToString() +
                                    Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp +
                                    reader_ISXD[3].ToString()
                                                               ] =
                                                            new DataFromPersoDB_SZVM(
                                                                ConvertRegNom(reader_ISXD[0].ToString()),
                                                                reader_ISXD[1].ToString(),
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn,
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp,
                                                                reader_ISXD[2].ToString(),
                                                                reader_ISXD[3].ToString(),
                                                                dataANDtimeInsert
                                                                                );
                            }
                            else
                            {
                                continue;
                            }

                        }
                        else
                        {
                            dictionary_uniqSNILS_ISXD_SZV_M[
                                    ConvertRegNom(reader_ISXD[0].ToString()) +
                                    Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn +
                                    reader_ISXD[1].ToString() +
                                    Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp +
                                    reader_ISXD[3].ToString()
                                                               ] =
                                                            new DataFromPersoDB_SZVM(
                                                                ConvertRegNom(reader_ISXD[0].ToString()),
                                                                reader_ISXD[1].ToString(),
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_inn,
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader_ISXD[0].ToString())].insurer_kpp,
                                                                reader_ISXD[2].ToString(),
                                                                reader_ISXD[3].ToString(),
                                                                dataANDtimeInsert
                                                                                );
                        }

                        //i_ISXD++;
                    }
                    reader_ISXD.Close();

                    Console.WriteLine("Количество выбранных строк из БД Perso (ИСХД формы СЗВ-М): {0} ", dictionary_uniqSNILS_ISXD_SZV_M.Count());
                    //Console.WriteLine();

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

        async public static void SelectDataFromPerso_OTMN(string query_OTMN)
        {
            try
            {
                using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=PERSDB;UID=regusr;PWD=password;"))
                {
                    //открываем соединение
                    await connection.OpenAsync();

                    DB2Command command = connection.CreateCommand();
                    command.CommandText = query_OTMN;

                    //Устанавливаем значение таймаута
                    command.CommandTimeout = 570;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    //int i_OTMN = 0;

                    while (await reader.ReadAsync())
                    {
                        //public DateTime(int year, int month, int day, int hour, int minute, int second);
                        DateTime dataANDtimeInsert = new DateTime(
                                                                            Convert.ToDateTime(reader[4]).Year,
                                                                            Convert.ToDateTime(reader[4]).Month,
                                                                            Convert.ToDateTime(reader[4]).Day,
                                                                            Convert.ToDateTime(reader[5]).Hour,
                                                                            Convert.ToDateTime(reader[5]).Minute,
                                                                            Convert.ToDateTime(reader[5]).Second
                                                                          );



                        //регНом+СНИЛС есть в словаре
                        DataFromPersoDB_SZVM tmpData = new DataFromPersoDB_SZVM();
                        if (dictionary_uniqSNILS_OTMN_SZV_M.TryGetValue(
                                                                        ConvertRegNom(reader[0].ToString()) +
                                                                        Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn +
                                                                        reader[1].ToString() +
                                                                        Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp +
                                                                        reader[3].ToString(),
                                                                                                                                                            out tmpData))
                        {
                            //сверяем дату и время импорта в БД (больше)
                            if (dataANDtimeInsert > tmpData.dataANDtimeInsertToDB)
                            {
                                dictionary_uniqSNILS_OTMN_SZV_M[
                                    ConvertRegNom(reader[0].ToString()) +
                                    Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn +
                                    reader[1].ToString() +
                                    Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp +
                                    reader[3].ToString()
                                                               ] =
                                                            new DataFromPersoDB_SZVM(
                                                                ConvertRegNom(reader[0].ToString()),
                                                                reader[1].ToString(),
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn,
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp,
                                                                reader[2].ToString(),
                                                                reader[3].ToString(),
                                                                dataANDtimeInsert
                                                                                );
                            }
                            //сверяем дату и время импорта в БД (равны)
                            else if (dataANDtimeInsert == tmpData.dataANDtimeInsertToDB)
                            {
                                dictionary_uniqSNILS_OTMN_SZV_M[
                                     ConvertRegNom(reader[0].ToString()) +
                                     Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn +
                                     reader[1].ToString() +
                                     Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp +
                                     reader[3].ToString()
                                                                ] =
                                                             new DataFromPersoDB_SZVM(
                                                                ConvertRegNom(reader[0].ToString()),
                                                                reader[1].ToString(),
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn,
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp,
                                                                reader[2].ToString(),
                                                                reader[3].ToString(),
                                                                dataANDtimeInsert
                                                                                );
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            dictionary_uniqSNILS_OTMN_SZV_M[
                                     ConvertRegNom(reader[0].ToString()) +
                                     Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn +
                                     reader[1].ToString() +
                                     Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp +
                                     reader[3].ToString()
                                                                ] =
                                                             new DataFromPersoDB_SZVM(
                                                                ConvertRegNom(reader[0].ToString()),
                                                                reader[1].ToString(),
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_inn,
                                                                Program.dictionary_dataFromPKASVDB[ConvertRegNom(reader[0].ToString())].insurer_kpp,
                                                                reader[2].ToString(),
                                                                reader[3].ToString(),
                                                                dataANDtimeInsert
                                                                                );
                        }

                        //i_OTMN++;
                    }
                    reader.Close();

                    Console.WriteLine("Количество выбранных строк из БД Perso (ОТМН формы СЗВ-М): {0} ", dictionary_uniqSNILS_OTMN_SZV_M.Count());
                    //Console.WriteLine();




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



        public static void Compare_SZV_M_ISX_and_OTMN()
        {
            try
            {
                //Формируем реестр уникальных СНИЛС СЗВ-М с учетом отмененных форм
                //KEY: regNum + INN + SNILS + KPP + otchMonth
                foreach (var item_uniqSNILS_ISXD_SZV_M in dictionary_uniqSNILS_ISXD_SZV_M)
                {
                    //регНом+СНИЛС есть в словаре dictionary_uniqSNILS_OTMN_SZV_M
                    DataFromPersoDB_SZVM tmpData_OTMN = new DataFromPersoDB_SZVM();
                    if (dictionary_uniqSNILS_OTMN_SZV_M.TryGetValue(item_uniqSNILS_ISXD_SZV_M.Key, out tmpData_OTMN))
                    {
                        //сверяем дату и время импорта в БД (больше)
                        if (item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB > tmpData_OTMN.dataANDtimeInsertToDB)
                        {
                            //есть в словаре Program.uniqSNILS_SZV_M_no_OTMN
                            DataFromPersoDB_SZVM tmpData_no_OTMN = new DataFromPersoDB_SZVM();
                            if (uniqSNILS_SZV_M.TryGetValue(item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp, out tmpData_no_OTMN))
                            {

                                //KEY: regNum + INN + SNILS + KPP
                                uniqSNILS_SZV_M[item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp] =
                                                        new DataFromPersoDB_SZVM(
                                                            item_uniqSNILS_ISXD_SZV_M.Value.regNum,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.strnum,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.inn,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.kpp,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.otchYear,
                                                            AddOtchMonth(
                                                                uniqSNILS_SZV_M[item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp].otchMonth,
                                                                item_uniqSNILS_ISXD_SZV_M.Value.otchMonth
                                                                ),
                                                            item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB
                                                                            );
                            }
                            else   //нет в словаре Program.uniqSNILS_SZV_M_no_OTMN
                            {
                                uniqSNILS_SZV_M[item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp] =
                                                            new DataFromPersoDB_SZVM(
                                                                item_uniqSNILS_ISXD_SZV_M.Value.regNum,
                                                                item_uniqSNILS_ISXD_SZV_M.Value.strnum,
                                                                item_uniqSNILS_ISXD_SZV_M.Value.inn,
                                                                item_uniqSNILS_ISXD_SZV_M.Value.kpp,
                                                                item_uniqSNILS_ISXD_SZV_M.Value.otchYear,
                                                                AddOtchMonth(
                                                                    "",
                                                                    item_uniqSNILS_ISXD_SZV_M.Value.otchMonth
                                                                             ),
                                                                item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB
                                                                                );
                            }


                            ////TODO: если будут дубли принятые по разным регНомерам (филиалам), то они "всплывут" при последующейц сверке, сейчас их "заместит" при добавлении записи в словарь
                            ////создаем коллекцию для "подтягивания" регНомера после сверки hashSet_SZV_STAG и hashSet_SZV_M и формирования результирующего файла
                            ////Compare_SZVM_SZVSTAG.CreateExportFile
                            //dictionary_SZV_M_itogTMP[item_uniqSNILS_ISXD_SZV_M.Value.inn + "," +
                            //                            item_uniqSNILS_ISXD_SZV_M.Value.strnum + "," +
                            //                            item_uniqSNILS_ISXD_SZV_M.Value.kpp + "," +
                            //                            item_uniqSNILS_ISXD_SZV_M.Value.otchMonth.Replace(" ", "")     //otchMonth
                            //                           ] = new RegNumAndDataTime(item_uniqSNILS_ISXD_SZV_M.Value.regNum, item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB);

                            ////Формируем массив уникальных KEY (INN + SNILS + KPP + otchMonth) для сверки с СЗВ-СТАЖ
                            //Program.hashSet_SZVM_UniqSNILS.Add(item_uniqSNILS_ISXD_SZV_M.Value.inn + "," +
                            //                                    item_uniqSNILS_ISXD_SZV_M.Value.strnum + "," +
                            //                                    item_uniqSNILS_ISXD_SZV_M.Value.kpp + "," +
                            //                                    item_uniqSNILS_ISXD_SZV_M.Value.otchMonth.Replace(" ","")
                            //                                  );

                        }
                        //сверяем даты импорта в БД (равны)
                        else if (item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB == tmpData_OTMN.dataANDtimeInsertToDB)
                        {
                            Console.WriteLine("Внимание! Дата и время ИСХД(ДОП) СЗВ-М и ОТМН формы равны: {0} - {1}", item_uniqSNILS_ISXD_SZV_M.Value.regNum, item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB);

                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else   //нет в словаре dictionary_uniqSNILS_OTMN_SZV_M
                    {
                        //есть в словаре Program.uniqSNILS_SZV_M_no_OTMN
                        DataFromPersoDB_SZVM tmpData_no_OTMN = new DataFromPersoDB_SZVM();
                        if (uniqSNILS_SZV_M.TryGetValue(item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp, out tmpData_no_OTMN))
                        {

                            //KEY: regNum + INN + SNILS + KPP
                            uniqSNILS_SZV_M[item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp] =
                                                    new DataFromPersoDB_SZVM(
                                                        item_uniqSNILS_ISXD_SZV_M.Value.regNum,
                                                        item_uniqSNILS_ISXD_SZV_M.Value.strnum,
                                                        item_uniqSNILS_ISXD_SZV_M.Value.inn,
                                                        item_uniqSNILS_ISXD_SZV_M.Value.kpp,
                                                        item_uniqSNILS_ISXD_SZV_M.Value.otchYear,
                                                        AddOtchMonth(
                                                            uniqSNILS_SZV_M[item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp].otchMonth,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.otchMonth
                                                            ),
                                                        item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB
                                                                        );
                        }
                        else   //нет в словаре Program.uniqSNILS_SZV_M_no_OTMN
                        {
                            uniqSNILS_SZV_M[item_uniqSNILS_ISXD_SZV_M.Value.regNum + item_uniqSNILS_ISXD_SZV_M.Value.inn + item_uniqSNILS_ISXD_SZV_M.Value.strnum + item_uniqSNILS_ISXD_SZV_M.Value.kpp] =
                                                        new DataFromPersoDB_SZVM(
                                                            item_uniqSNILS_ISXD_SZV_M.Value.regNum,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.strnum,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.inn,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.kpp,
                                                            item_uniqSNILS_ISXD_SZV_M.Value.otchYear,
                                                            AddOtchMonth(
                                                                "",
                                                                item_uniqSNILS_ISXD_SZV_M.Value.otchMonth
                                                                         ),
                                                            item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB
                                                                            );
                        }

                        ////TODO: если будут дубли принятые по разным регНомерам (филиалам), то они "всплывут" при последующейц сверке, сейчас их "заместит" при добавлении записи в словарь
                        ////создаем коллекцию для "подтягивания" регНомера после сверки hashSet_SZV_STAG и hashSet_SZV_M и формирования результирующего файла
                        ////Compare_SZVM_SZVSTAG.CreateExportFile
                        //dictionary_SZV_M_itogTMP[item_uniqSNILS_ISXD_SZV_M.Value.inn + "," +
                        //                            item_uniqSNILS_ISXD_SZV_M.Value.strnum + "," +
                        //                            item_uniqSNILS_ISXD_SZV_M.Value.kpp + "," +
                        //                            item_uniqSNILS_ISXD_SZV_M.Value.otchMonth.Replace(" ", "")     //otchMonth
                        //                           ] = new RegNumAndDataTime(item_uniqSNILS_ISXD_SZV_M.Value.regNum, item_uniqSNILS_ISXD_SZV_M.Value.dataANDtimeInsertToDB);

                        ////Формируем массив уникальных KEY (INN + SNILS + KPP + otchMonth) для сверки с СЗВ-СТАЖ
                        //Program.hashSet_SZVM_UniqSNILS.Add(item_uniqSNILS_ISXD_SZV_M.Value.inn + "," +
                        //                                        item_uniqSNILS_ISXD_SZV_M.Value.strnum + "," +
                        //                                        item_uniqSNILS_ISXD_SZV_M.Value.kpp + "," +
                        //                                        item_uniqSNILS_ISXD_SZV_M.Value.otchMonth.Replace(" ", "")
                        //                                      );
                    }
                }


                if (uniqSNILS_SZV_M.Count != 0)
                {
                    //Формируем сводные данные на основании реестра уникальных СНИЛС в СЗВ-M с учетом отмененных форм
                    foreach (var item in uniqSNILS_SZV_M)
                    {
                        //Добавляем выбранные данные в коллекцию
                        int tmpData = 0;
                        if (Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVM.TryGetValue(item.Value.regNum, out tmpData))
                        {
                            //есть рег.Номер в словаре
                            ++Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVM[item.Value.regNum];
                        }
                        else
                        {
                            //нет рег.Номера в словаре
                            Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVM[item.Value.regNum] = 1;
                        }
                    }
                }
                //Console.Write("");

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

        private static string AddOtchMonth(string otchMonth_old, string otchMonth_new)
        {
            //Создаем массив уникальных месяцев из dictionary_uniqSNILS_ISXD_SZV_STAG.Value.otchMonth
            SortedSet<int> tmpStr = new SortedSet<int>();

            char[] separator = { ',' };    //список разделителей в строке
            string[] massiveStr = otchMonth_old.Split(separator);     //создаем массив из строк между разделителями

            if (massiveStr.Count() != 0)
            {
                foreach (var item in massiveStr)
                {
                    if (item != "")
                    {
                        tmpStr.Add(Convert.ToInt32(item.Replace(" ", "")));
                    }
                }
            }



            //Добавляем новый период 
            tmpStr.Add(Convert.ToInt32(otchMonth_new));



            string monthCollection = "";
            int tmpStrCount = tmpStr.Count();

            if (tmpStrCount == 1)
            {
                foreach (var item in tmpStr)
                {
                    monthCollection = monthCollection + item + " " + ",";
                }
            }
            else if (tmpStrCount > 1)
            {
                //Возвращаем строку из уникальных месяцев через запятую
                foreach (var item in tmpStr)
                {
                    --tmpStrCount;
                    if (tmpStrCount != 0)
                    {
                        monthCollection = monthCollection + item + " " + ",";
                    }
                    else
                    {
                        monthCollection = monthCollection + item + " ";
                    }
                }
            }
            else
            {
                monthCollection = "";
            }

            return monthCollection;

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
                else if (regNum.Count() == 14)
                {
                    return "042-0" + regNum[5] + regNum[6];
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
                string regNomConvert = "0" + regNomOld[0].ToString() + regNomOld[1].ToString() + "-" + regNomOld[2].ToString() + regNomOld[3] + regNomOld[4] + "-" + regNomOld[5] + regNomOld[6] + regNomOld[7] + regNomOld[8] + regNomOld[9] + regNomOld[10];


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
        //Формируем результирующий файл статистики
        public static void CreateExportFile(string resultFile, string zagolovok, Dictionary<string, DataFromPersoDB_SZVM> dictionary_Data)
        {
            try
            {
                //формируем результирующий файл статистики
                using (StreamWriter writer = new StreamWriter(resultFile, false, Encoding.GetEncoding(1251)))
                {
                    writer.WriteLine(zagolovok);

                    int i = 0;

                    foreach (var item in dictionary_Data)
                    {
                        i++;
                        writer.Write(i + ";");
                        writer.Write(SelectRaion(item.Value.regNum) + ";");
                        writer.Write(Program.dictionary_dataFromPKASVDB[item.Value.regNum].insurer_short_name.Replace(";", "---") + ";");
                        writer.Write(item.Value.ToString());
                        writer.WriteLine(Program.dictionary_dataFromPKASVDB[item.Value.regNum].kurator + ";");
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
        ////Формируем результирующий файл на основании данных из БД
        //public static void CreateExportFileSNILS(string zagolovok, Dictionary<string, DataFromPersoDB> uniqSNILS_ISXD_SZV_M_no_OTMN, string nameFile)
        //{
        //    try
        //    {
        //        //Добавляем в файл данные                
        //        using (StreamWriter writer = new StreamWriter(nameFile, true, Encoding.GetEncoding(1251)))
        //        {
        //            writer.WriteLine(zagolovok);

        //            int i = 0;

        //            foreach (var item in uniqSNILS_ISXD_SZV_M_no_OTMN)
        //            {
        //                i++;
        //                writer.Write(i + ";");
        //                writer.WriteLine(item.Value.ToString() + ";");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IOoperations.WriteLogError(ex.ToString());

        //        Console.WriteLine();
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine(ex.Message);
        //        Console.ForegroundColor = ConsoleColor.Gray;
        //    }
        //}


        ////------------------------------------------------------------------------------------------        
        ////Формируем результирующий файл на основании данных из БД
        //public static void CreateExportFile(string zagolovok, Dictionary<string, int> dictionary_svodDataFromPersoDB_UniqSNILS_SZVM, string nameFile)
        //{
        //    try
        //    {
        //        //Добавляем в файл данные                
        //        using (StreamWriter writer = new StreamWriter(nameFile, true, Encoding.GetEncoding(1251)))
        //        {
        //            writer.WriteLine(zagolovok);

        //            foreach (var item in dictionary_svodDataFromPersoDB_UniqSNILS_SZVM)
        //            {
        //                writer.WriteLine(item.Key.ToString() + ";" + item.Value.ToString() + ";");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IOoperations.WriteLogError(ex.ToString());

        //        Console.WriteLine();
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine(ex.Message);
        //        Console.ForegroundColor = ConsoleColor.Gray;
        //    }
        //}

    }
}


////наполняем словарь dictionary_uniqSNILS_ISXD_SZV_M последним по Дате (Времени) регНом+СНИЛС
//foreach (DataFromPersoDB_SZVM_Compare_v2 itemDataPerso in listReestrSZV_M_ISXD)
//{
//    //регНом+СНИЛС есть в словаре
//    DataFromPersoDB_SZVM_Compare_v2 tmpData = new DataFromPersoDB_SZVM_Compare_v2();
//    if (dictionary_uniqSNILS_ISXD_SZV_M.TryGetValue(itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth, out tmpData))
//    {
//        //сверяем даты импорта в БД (больше)
//        if (Convert.ToDateTime(itemDataPerso.dateINS) > Convert.ToDateTime(tmpData.dateINS))
//        {
//            dictionary_uniqSNILS_ISXD_SZV_M[itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth] = itemDataPerso;
//        }
//        //сверяем даты импорта в БД (равны)
//        if (Convert.ToDateTime(itemDataPerso.dateINS) == Convert.ToDateTime(tmpData.dateINS))
//        {
//            //тогда сверяем время импорта в БД (больше)
//            if (Convert.ToDateTime(itemDataPerso.timeINS) > Convert.ToDateTime(tmpData.timeINS))
//            {
//                dictionary_uniqSNILS_ISXD_SZV_M[itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth] = itemDataPerso;
//            }
//            else
//            {
//                continue;
//            }
//        }
//        else
//        {
//            continue;
//        }
//    }
//    else
//    {
//        dictionary_uniqSNILS_ISXD_SZV_M.Add(itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth, itemDataPerso);
//    }
//}



////наполняем словарь dictionary_uniqSNILS_OTMN_SZV_M последним по Дате (Времени) регНом+СНИЛС
//foreach (DataFromPersoDB_SZVM_Compare_v2 itemDataPerso in listReestrSZV_M_OTMN)
//{
//    //регНом+СНИЛС есть в словаре
//    DataFromPersoDB_SZVM_Compare_v2 tmpData = new DataFromPersoDB_SZVM_Compare_v2();
//    if (dictionary_uniqSNILS_OTMN_SZV_M.TryGetValue(itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth, out tmpData))
//    {
//        //сверяем даты импорта в БД (больше)
//        if (Convert.ToDateTime(itemDataPerso.dateINS) > Convert.ToDateTime(tmpData.dateINS))
//        {
//            dictionary_uniqSNILS_OTMN_SZV_M[itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth] = itemDataPerso;
//        }
//        //сверяем даты импорта в БД (равны)
//        if (Convert.ToDateTime(itemDataPerso.dateINS) == Convert.ToDateTime(tmpData.dateINS))
//        {
//            //тогда сверяем время импорта в БД (больше)
//            if (Convert.ToDateTime(itemDataPerso.timeINS) > Convert.ToDateTime(tmpData.timeINS))
//            {
//                dictionary_uniqSNILS_OTMN_SZV_M[itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth] = itemDataPerso;
//            }
//            else
//            {
//                continue;
//            }
//        }
//        else
//        {
//            continue;
//        }
//    }
//    else
//    {
//        dictionary_uniqSNILS_OTMN_SZV_M.Add(itemDataPerso.regNum + itemDataPerso.strnum + itemDataPerso.otchMonth, itemDataPerso);
//    }
//}