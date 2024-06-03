using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace StatisticsEDO_DB_SZV
{
    static class CreateDataFromPersoSelect
    {
        //------------------------------------------------------------------------------------------
        //Общие реестры

        public static List<DataFromPersoDB> list_SZV_STAG = new List<DataFromPersoDB>();                //Коллекция данных из файлов "СЗВ-СТАЖ"
        public static List<DataFromPersoDB> list_SZV_KORR = new List<DataFromPersoDB>();                //Коллекция данных из файлов "СЗВ-КОРР"
        public static List<DataFromPersoDB> list_SZV_ISX = new List<DataFromPersoDB>();                 //Коллекция данных из файлов "СЗВ-ИСХ"        
        public static List<DataFromPersoDB> list_ODV_1 = new List<DataFromPersoDB>();                   //Коллекция данных из файлов "ОДВ-1"

        //------------------------------------------------------------------------------------------
        //"СЗВ-СТАЖ"

        //Все формы "СЗВ-СТАЖ"
        public static List<DataFromPersoDB> list_SZV_STAG_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_NO_prinyato = new List<DataFromPersoDB>();

        //"СЗВ-СТАЖ" "ИСХОДНАЯ" "ИСХОДНАЯ"
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_NO_prinyato = new List<DataFromPersoDB>();

        //"СЗВ-СТАЖ" "ИСХОДНАЯ" "ДОПОЛНЯЮЩАЯ"
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_DOP_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_DOP_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_DOP_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_ISXD_DOP_NO_prinyato = new List<DataFromPersoDB>();

        //"СЗВ-СТАЖ" "ИСХОДНАЯ" "НАЗНАЧЕНИЕ ПЕНСИИ" 
        public static List<DataFromPersoDB> list_SZV_STAG_NAZN_PENS_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_NAZN_PENS_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_NAZN_PENS_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_STAG_NAZN_PENS_NO_prinyato = new List<DataFromPersoDB>();

        //------------------------------------------------------------------------------------------
        //"СЗВ-КОРР"

        //Все формы "СЗВ-КОРР"
        public static List<DataFromPersoDB> list_SZV_KORR_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_NO_prinyato = new List<DataFromPersoDB>();

        //"СЗВ-КОРР" "ИСХОДНАЯ" "КОРРЕКТИРУЮЩАЯ"
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_KORR_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_KORR_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_KORR_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_KORR_NO_prinyato = new List<DataFromPersoDB>();

        //"СЗВ-КОРР" "ИСХОДНАЯ" "ОТМЕНЯЮЩАЯ"
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_OTMN_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_OTMN_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_OTMN_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_ISXD_OTMN_NO_prinyato = new List<DataFromPersoDB>();

        //"СЗВ-КОРР" "ИСХОДНАЯ" "ОСОБАЯ"
        public static List<DataFromPersoDB> list_SZV_KORR_OSOB_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_OSOB_partial_prinyato = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_OSOB_NO_proveren = new List<DataFromPersoDB>();
        public static List<DataFromPersoDB> list_SZV_KORR_OSOB_NO_prinyato = new List<DataFromPersoDB>();

        //------------------------------------------------------------------------------------------
        //Ошибки

        public static List<DataFromPersoDB> list_noName = new List<DataFromPersoDB>();                  //Коллекция данных из файлов "noName"

        public static List<DataFromPersoDB> list_errorKvitanciya = new List<DataFromPersoDB>();         //Коллекция данных из файлов "Ошибочное состояние квитанций"



        //------------------------------------------------------------------------------------------
        //Выбираем из коллекции нужные позиции           
        public static void CreateListCollections()
        {
            try
            {
                if (Program.listReestrSZVSTAG.Count() == 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Из БД Perso не выбрано никаких данных.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (Program.dictionary_dataFromPKASVDB.Count() == 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Из БД РК АСВ не выбрано никаких данных.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    //Очищаем коллекции для данных из файлов. Внимание! Не очищаются все коллекции!
                    list_noName.Clear();
                    list_errorKvitanciya.Clear();



                    //------------------------------------------------------------------------------------------
                    //Разбираем реестр Program.listReestrSZVSTAG на требуемые коллекции
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(new string('-', 107));
                    Console.WriteLine("Разбираем общий реестр форм СЗВ на части, пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                    Console.WriteLine(new string('-', 107));
                    Console.ForegroundColor = ConsoleColor.Gray;



                    //Формируем коллекции по типам сведений и результату обработки
                    foreach (var itemDataFromPersoDB in Program.listReestrSZVSTAG)
                    {
                        //------------------------------------------------------------------------------------------
                        //"СЗВ-СТАЖ"
                        if (itemDataFromPersoDB.tip == "СЗВ-СТАЖ")
                        {
                            list_SZV_STAG.Add(itemDataFromPersoDB);

                            //"СЗВ-СТАЖ" "Принят"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ИСХОДНАЯ" && itemDataFromPersoDB.statVIO == "Принят")
                            {
                                list_SZV_STAG_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ДОПОЛНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Принят")
                            {
                                list_SZV_STAG_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_DOP_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "НАЗНАЧЕНИЕ ПЕНСИИ" && itemDataFromPersoDB.statVIO == "Принят")
                            {
                                list_SZV_STAG_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_NAZN_PENS_prinyato.Add(itemDataFromPersoDB);
                            }

                            //"СЗВ-СТАЖ" "Принят частично"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ИСХОДНАЯ" && itemDataFromPersoDB.statVIO == "Принят частично")
                            {
                                list_SZV_STAG_partial_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_partial_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ДОПОЛНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Принят частично")
                            {
                                list_SZV_STAG_partial_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_DOP_partial_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "НАЗНАЧЕНИЕ ПЕНСИИ" && itemDataFromPersoDB.statVIO == "Принят частично")
                            {
                                list_SZV_STAG_partial_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_NAZN_PENS_partial_prinyato.Add(itemDataFromPersoDB);
                            }

                            //"СЗВ-СТАЖ" "Не проверен"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ИСХОДНАЯ" && itemDataFromPersoDB.statVIO == "Не проверен")
                            {
                                list_SZV_STAG_NO_proveren.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_NO_proveren.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ДОПОЛНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Не проверен")
                            {
                                list_SZV_STAG_NO_proveren.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_DOP_NO_proveren.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "НАЗНАЧЕНИЕ ПЕНСИИ" && itemDataFromPersoDB.statVIO == "Не проверен")
                            {
                                list_SZV_STAG_NO_proveren.Add(itemDataFromPersoDB);
                                list_SZV_STAG_NAZN_PENS_NO_proveren.Add(itemDataFromPersoDB);
                            }

                            //"СЗВ-СТАЖ" "Не принят"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ИСХОДНАЯ" && itemDataFromPersoDB.statVIO == "Не принят")
                            {
                                list_SZV_STAG_NO_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_NO_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ДОПОЛНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Не принят")
                            {
                                list_SZV_STAG_NO_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_ISXD_DOP_NO_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "НАЗНАЧЕНИЕ ПЕНСИИ" && itemDataFromPersoDB.statVIO == "Не принят")
                            {
                                list_SZV_STAG_NO_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_STAG_NAZN_PENS_NO_prinyato.Add(itemDataFromPersoDB);
                            }
                        }

                        //------------------------------------------------------------------------------------------
                        //"СЗВ-КОРР"
                        if (itemDataFromPersoDB.tip == "СЗВ-КОРР")
                        {
                            list_SZV_KORR.Add(itemDataFromPersoDB);

                            //"СЗВ-КОРР" "Принят"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "КОРРЕКТИРУЮЩАЯ" && itemDataFromPersoDB.statVIO == "Принят")
                            {
                                list_SZV_KORR_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_KORR_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОТМЕНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Принят")
                            {
                                list_SZV_KORR_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_OTMN_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОСОБАЯ" && itemDataFromPersoDB.statVIO == "Принят")
                            {
                                list_SZV_KORR_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_OSOB_prinyato.Add(itemDataFromPersoDB);
                            }

                            //"СЗВ-КОРР" "Принят частично"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "КОРРЕКТИРУЮЩАЯ" && itemDataFromPersoDB.statVIO == "Принят частично")
                            {
                                list_SZV_KORR_partial_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_KORR_partial_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОТМЕНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Принят частично")
                            {
                                list_SZV_KORR_partial_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_OTMN_partial_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОСОБАЯ" && itemDataFromPersoDB.statVIO == "Принят частично")
                            {
                                list_SZV_KORR_partial_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_OSOB_partial_prinyato.Add(itemDataFromPersoDB);
                            }

                            //"СЗВ-КОРР" "Не проверен"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "КОРРЕКТИРУЮЩАЯ" && itemDataFromPersoDB.statVIO == "Не проверен")
                            {
                                list_SZV_KORR_NO_proveren.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_KORR_NO_proveren.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОТМЕНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Не проверен")
                            {
                                list_SZV_KORR_NO_proveren.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_OTMN_NO_proveren.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОСОБАЯ" && itemDataFromPersoDB.statVIO == "Не проверен")
                            {
                                list_SZV_KORR_NO_proveren.Add(itemDataFromPersoDB);
                                list_SZV_KORR_OSOB_NO_proveren.Add(itemDataFromPersoDB);
                            }

                            //"СЗВ-КОРР" "Не принят"
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "КОРРЕКТИРУЮЩАЯ" && itemDataFromPersoDB.statVIO == "Не принят")
                            {
                                list_SZV_KORR_NO_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_KORR_NO_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОТМЕНЯЮЩАЯ" && itemDataFromPersoDB.statVIO == "Не принят")
                            {
                                list_SZV_KORR_NO_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_ISXD_OTMN_NO_prinyato.Add(itemDataFromPersoDB);
                            }
                            if (itemDataFromPersoDB.tipForm == "ИСХОДНАЯ" && itemDataFromPersoDB.tipSveden == "ОСОБАЯ" && itemDataFromPersoDB.statVIO == "Не принят")
                            {
                                list_SZV_KORR_NO_prinyato.Add(itemDataFromPersoDB);
                                list_SZV_KORR_OSOB_NO_prinyato.Add(itemDataFromPersoDB);
                            }
                        }

                        //------------------------------------------------------------------------------------------
                        //"СЗВ-ИСХ"
                        if (itemDataFromPersoDB.tip == "СЗВ-ИСХ")
                        {
                            list_SZV_ISX.Add(itemDataFromPersoDB);
                        }

                        //------------------------------------------------------------------------------------------
                        //"ОДВ-1"
                        if (itemDataFromPersoDB.tip == "ОДВ-1")
                        {
                            list_ODV_1.Add(itemDataFromPersoDB);
                        }

                        //------------------------------------------------------------------------------------------
                        //Ошибки

                        //"noName"
                        if ((itemDataFromPersoDB.tip != "СЗВ-СТАЖ" && itemDataFromPersoDB.tip != "СЗВ-КОРР" && itemDataFromPersoDB.tip != "СЗВ-ИСХ" && itemDataFromPersoDB.tip != "ОДВ-1") ||
                            (itemDataFromPersoDB.statVIO != "Принят" && itemDataFromPersoDB.statVIO != "Принят частично" && itemDataFromPersoDB.statVIO != "Не проверен" && itemDataFromPersoDB.statVIO != "Не принят") ||
                            //(itemDataFromPersoDB.statVIO != "Принят" && itemDataFromPersoDB.statVIO != "Принят частично" && itemDataFromPersoDB.statVIO != "Не проверен" && itemDataFromPersoDB.statVIO != "Не принят" && itemDataFromPersoDB.statVIO != "Не подавался") ||
                            (itemDataFromPersoDB.tipForm == "" && itemDataFromPersoDB.tipSveden == "") ||
                            itemDataFromPersoDB.curator == "")
                        {
                            list_noName.Add(itemDataFromPersoDB);
                        }

                        //"errorKvitanciya"
                        //if ((itemDataFromPersoDB.statusRec == "Зарегистрирован" || itemDataFromPersoDB.statusRec == "Отказ в обработке" || itemDataFromPersoDB.statusRec == "Отказ в регистрации" || 
                        //    itemDataFromPersoDB.statusRec == ""|| itemDataFromPersoDB.statusRec == "Квитанция не поступила" || itemDataFromPersoDB.statusRec == "Квитанция не поступила из ВИО") 
                        //    && itemDataFromPersoDB.statusRec != "Не принят")

                        //if (itemDataFromPersoDB.statusRec == "Зарегистрирован" || itemDataFromPersoDB.statusRec == "Отказ в обработке" || itemDataFromPersoDB.statusRec == "Отказ в регистрации" ||
                        //    itemDataFromPersoDB.statusRec == "" || itemDataFromPersoDB.statusRec == "Квитанция не поступила" || itemDataFromPersoDB.statusRec == "Квитанция не поступила из ВИО")
                        if (itemDataFromPersoDB.statusRec != "Обработан")
                        {
                            list_errorKvitanciya.Add(itemDataFromPersoDB);
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Количество выбранных записей \"list_SZV_STAG\": {0}", list_SZV_STAG.Count());
                    Console.WriteLine();
                    Console.WriteLine("Количество выбранных записей \"list_SZV_KORR\": {0}", list_SZV_KORR.Count());
                    Console.WriteLine();
                    Console.WriteLine("Количество выбранных записей \"list_SZV_ISX\": {0}", list_SZV_ISX.Count());
                    Console.WriteLine();
                    Console.WriteLine("Количество выбранных записей \"list_ODV_1\": {0}", list_ODV_1.Count());
                    Console.WriteLine();
                    Console.WriteLine("Количество выбранных записей \"list_noName\": {0}", list_noName.Count());
                    Console.WriteLine();
                    Console.WriteLine("Количество выбранных записей \"list_errorKvitanciya\": {0}", list_errorKvitanciya.Count());
                    Console.WriteLine();

                    //codZap + ";" + raion + ";" + regNum + ";" + nameStrah + ";" + tip + ";" + tipForm + ";" + tipSveden + ";" + year + ";" + priem + ";" + dataPriem + ";" + statVIO + ";" + dataVIO + ";"
                    //    + statusRec + ";" + kolZL + ";" + kolZlLGT + ";" + kolZl_PR_VIO + ";" + kolZl_NPR_VIO + ";" + shname + ";" + dateINS + ";" + timeINS + ";"
                    //    + dataUved + ";" + dataOtp + ";" + otmnFormAvailability + ";" + kategory + ";" + inn + ";" + dataPostPFR + ";" + dataSnyatPFR + ";" + dataPostRO + ";" + dataSnyatRO + ";"
                    //    + UP + ";" + curator + ";" + uniqZlSZVM + ";" + uniqZlSZVSTAG + ";";


                    //------------------------------------------------------------------------------------------
                    //Формируем файл статистики "Perso_СтатусКвитанции_Error_"
                    if (list_errorKvitanciya.Count() != 0)
                    {
                        //Создаем имя результирующего файла
                        string resultFile_errorKvitanc = IOoperations.katalogOut + @"\" + @"_0_СтатусКвитанции_Error_" + DateTime.Now.ToShortDateString() + ".csv";
                        //Проверяем на наличие файла отчета, если существует - удаляем
                        if (File.Exists(resultFile_errorKvitanc)) { IOoperations.FileDelete(resultFile_errorKvitanc); }

                        SelectDataFromPersoDB_AllForm.CreateExportFile(Program.zagolovokPersoSZVSTAG, list_errorKvitanciya, resultFile_errorKvitanc);
                    }

                    //------------------------------------------------------------------------------------------
                    //Формируем файл статистики "Perso_noName_"
                    if (list_noName.Count() != 0)
                    {
                        //Создаем имя результирующего файла
                        string resultFile_list_noName = IOoperations.katalogOut + @"\" + @"_0_Некорректное_состояние_" + DateTime.Now.ToShortDateString() + ".csv";
                        //Проверяем на наличие файла отчета, если существует - удаляем
                        if (File.Exists(resultFile_list_noName)) { IOoperations.FileDelete(resultFile_list_noName); }

                        SelectDataFromPersoDB_AllForm.CreateExportFile(Program.zagolovokPersoSZVSTAG, list_noName, resultFile_list_noName);
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
        public static string ConvertRegNom(string regNom)
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

        //------------------------------------------------------------------------------------------
        private static string ConvertRaion(string raion)
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

        //private static void WriteLogsErrorKvitanc(string resultFile_list_error, string zagolovokError, List<DataFromPersoDB> list_dataFromPersoDB, List<DataFromPersoDB> list_errorKvitanciya)
        //{
        //    foreach (var itemDataFromPersoDB in list_dataFromPersoDB)
        //    {
        //        //"Квитанция не поступила из ВИО"     "Обработан"
        //        if (
        //            (itemDataFromPersoDB.statusKvitanc == "Зарегистрирован" || itemDataFromPersoDB.statusKvitanc == "Отказ в обработке"
        //            || itemDataFromPersoDB.statusKvitanc == "Отказ в регистрации" || itemDataFromPersoDB.statusKvitanc == "") &&
        //            itemDataFromPersoDB.resultat != "Не принят"
        //            )
        //        {
        //            list_errorKvitanciya.Add(itemDataFromPersoDB);
        //        }
        //        else
        //        {
        //            continue;
        //        }
        //    }

        //    if (list_errorKvitanciya.Count() != 0)
        //    {
        //        //формируем результирующий файл статистики
        //        using (StreamWriter writer = new StreamWriter(resultFile_list_error, false, Encoding.GetEncoding(1251)))
        //        {
        //            writer.WriteLine(zagolovokError);

        //            int i = 0;

        //            foreach (var item in list_errorKvitanciya)
        //            {
        //                i++;
        //                writer.Write(i + ";");
        //                writer.WriteLine(item.ToStringPersoALL());
        //            }
        //        }
        //    }

        //}

        ////------------------------------------------------------------------------------------------       
        ////Формируем результирующий файл статистики
        //private static void WriteLogs(string resultFile, string zagolovok, List<DataFromPersoDB> listData)
        //{
        //    try
        //    {
        //        //формируем результирующий файл статистики
        //        using (StreamWriter writer = new StreamWriter(resultFile, false, Encoding.GetEncoding(1251)))
        //        {
        //            writer.WriteLine(zagolovok);

        //            foreach (var item in listData)
        //            {
        //                writer.WriteLine(item.ToStringPersoALL());
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



