using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;

namespace StatisticsEDO_DB_SZV
{
    class Program
    {
        private static DateTime start;

        public static string raionS = "001";
        public static string raionPo = "034";

        public static string otchYear = "0";
        public static string p_date_priem_st = "0";
        public static string p_date_priem_fn = "0";

        public static List<DataFromPersoDB> listReestrSZVSTAG = new List<DataFromPersoDB>();                        //Коллекция данных из БД Perso (реестр СЗВ-СТАЖ)

        public static List<DataFromPersoDB_OTMNform> listReestrSZV_OTMN = new List<DataFromPersoDB_OTMNform>();     //Коллекция данных из БД Perso (реестр ОТМН форм)
        public static Dictionary<string, string> dictionary_ReestrSZV_OTMN = new Dictionary<string, string>();      //Коллекция данных из БД Perso (реестр ОТМН форм)

        public static List<DataFromPersoDB_ISXDform> listReestrSZV_ISXD = new List<DataFromPersoDB_ISXDform>();     //Коллекция данных из БД Perso (реестр ИСХД форм)        

        //public static List<DataFromPersoDB_SZVM> listReestrSZV_SZVM = new List<DataFromPersoDB_SZVM>();             //Коллекция данных из БД Perso (реестр СЗВ-М)

        public static SortedSet<string> persoUnikRegNomForPKASV = new SortedSet<string>();                          //Коллекция уникальных регномеров из реестра Perso (формат - 42000000000)
        public static SortedSet<string> persoUnikRegNom_list_SZV_STAG = new SortedSet<string>();                                  //Коллекция уникальных регномеров из реестра Perso (формат - 42000000000)
        public static SortedSet<string> persoUnikRegNomForSZV_STAG_Uniq_Reestr = new SortedSet<string>();                    //Коллекция уникальных регномеров из реестра Perso (формат - 42000000000)
        public static SortedSet<string> reestr350regNumForInsert = new SortedSet<string>();

        public static Dictionary<string, DataFromRKASVDB> dictionary_dataFromPKASVDB = new Dictionary<string, DataFromRKASVDB>();       //Коллекция данных из БД РК АСВ

        public static string zagolovokPersoSZVSTAG = "№ п/п" + ";" + "КодЗап" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";" + "Тип формы" + ";" + "Тип ОДВ" + ";" + "Тип СЗВ" + ";"
                                                        + "ОтчГод" + ";" + "Специалист" + ";" + "Способ представления" + ";" + "Дата представления" + ";" + "Результат проверки" + ";" + "Дата проверки" + ";" + "Статус квитанции" + ";"
                                                        + "Количество ЗЛ в файле" + ";" + "Количество ЗЛ льготников" + ";" + "Количество ЗЛ принято" + ";" + "Количество ЗЛ не принято" + ";"
                                                        + "Дата записи в БД" + ";" + "Время записи в БД" + ";" + "Дата уведомления" + ";" + "Дата контроля" + ";" + "Признак наличия ОТМН форм" + ";"
                                                        + "Категория" + ";" + "ИНН" + ";" + "КПП" + ";" + "Дата постановки в ПФР" + ";" + "Дата снятия в ПФР" + ";" + "Дата постановки в РО" + ";" + "Дата снятия в РО" + ";"
                                                        + "УП по данным УПФР" + ";" + "Куратор" + ";"
                                                        + "Количество ЗЛ (уникальных СНИЛС) в представленной за год отчетности по форме СЗВ-М" + ";"
                                                        + "Количество ЗЛ (уникальных СНИЛС) в представленной за год отчетности по форме СЗВ-СТАЖ" + ";"
                                                        + "Статус" + ";" + "Разница ЗЛ (СЗВ-М - СЗВ-СТАЖ)" + ";";

        public static Dictionary<string, int> dictionary_svodDataFromPersoDB_UniqSNILS_SZVM = new Dictionary<string, int>();            //коллекция сводных данных по СЗВ-М

        public static Dictionary<string, int> dictionary_svodDataFromPersoDB_UniqSNILS_SZVSTAG = new Dictionary<string, int>();         //коллекция сводных данных по СЗВ-СТАЖ

        //public static SortedSet<string> sortedSet_dataFromPersoDB_UniqSNILS_ALL = new SortedSet<string>();                              //Коллекция уникальных СНИЛС (сверка СЗВ-СТАЖ и СЗВ-М)


        public static Dictionary<string, DataFromPersoDB_ISXDform> uniqSNILS_ISXD_STAG_no_OTMN = new Dictionary<string, DataFromPersoDB_ISXDform>();       //Коллекция уник СНИЛС из БД Perso (реестр ИСХД форм) без ОТМН форм

        public static NameValueCollection allAppSettings = ConfigurationManager.AppSettings;              //формируем массив настроек приложения

        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(125, 55);  //Устанавливаем размер окна консоли            

                //NameValueCollection allAppSettings = ConfigurationManager.AppSettings;              //формируем массив настроек приложения                        
                string destinationfolderName = allAppSettings["destinationfolderName"];             //каталог назначения  

                //время начала обработки
                start = DateTime.Now;

                //Создаем каталоги по умолчанию
                IOoperations.BasicDirectoryAndFileCreate();



                //------------------------------------------------------------------------------------------
                //0. Вводим параметры

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 117));
                Console.WriteLine("Введите необходимые параметры:");
                Console.WriteLine(new string('-', 117));
                Console.ForegroundColor = ConsoleColor.Gray;


                //Program.otchYear = (DateTime.Now.Year - 1).ToString();
                Program.otchYear = allAppSettings["otchYear"];      //отчГод из файла конфигурации

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Введите данные \"Отчетный период - год\" (по умолчанию - {0}): ", Program.otchYear);
                Console.ForegroundColor = ConsoleColor.Gray;
                string tmp_p_god = Console.ReadLine();
                if (tmp_p_god != "")
                {
                    Program.otchYear = tmp_p_god;
                }

                Console.WriteLine();
                Program.p_date_priem_st = "01.01.2018";
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Введите данные \"Дата приема с\" (по умолчанию - 01.01.2018): ");
                Console.ForegroundColor = ConsoleColor.Gray;
                string tmpReadLine = Console.ReadLine();
                if (tmpReadLine != "")
                {
                    Program.p_date_priem_st = tmpReadLine;
                }

                Console.WriteLine();
                Program.p_date_priem_fn = DateTime.Now.ToShortDateString();  //текущая системная дата
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Введите данные \"Дата приема по\" (по умолчанию - текущая системная дата): ");
                Console.ForegroundColor = ConsoleColor.Gray;
                tmpReadLine = Console.ReadLine();
                Console.WriteLine();
                if (tmpReadLine != "")
                {
                    Program.p_date_priem_fn = tmpReadLine;
                }

                //TODO: сделать (при необходимости) формирование списка отчетных периодов для запроса
                ////string p_godPersoSTAG = p_god + "," + (Convert.ToInt32(p_god) + 1).ToString();

                //string p_godPersoSTAG = p_god;

                //string p_godTMP = p_god;

                //int yearNow = DateTime.Now.Year;

                //while (yearNow != Convert.ToInt32(p_godTMP))
                //{
                //    p_godTMP = (Convert.ToInt32(p_godTMP) + 1).ToString();

                //    p_godPersoSTAG = p_godPersoSTAG + "," + p_godTMP;
                //}



                //------------------------------------------------------------------------------------------
                //1. Обработка файлов в каталоге \""_In_Status_ID"\"

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 107));
                Console.WriteLine("Обработка файлов в каталоге \"_In_Status_ID\", пожалуйста ждите...");
                Console.WriteLine(new string('-', 107));
                Console.ForegroundColor = ConsoleColor.Gray;

                SelectDataFromStatusIDfile.ObrFileFromDirectory(IOoperations.katalogInStatusID);
                //Console.WriteLine();
                //Console.WriteLine("Количество выбранных записей (каталог \"_In_Status_ID\"): {0}", SelectDataFromStatusIDfile.dictionaryStatusID.Count());

                //Console.WriteLine();



                //------------------------------------------------------------------------------------------
                //1. Обработка файлов в каталоге \"_In_UP\"

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 117));
                Console.WriteLine("Обработка файлов в каталоге \"_In_UP\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                Console.WriteLine(new string('-', 117));
                Console.ForegroundColor = ConsoleColor.Gray;

                SelectDataFromUPfile.ObrFileFromDirectory(IOoperations.katalogInUP);
                //Console.WriteLine();
                //Console.WriteLine("Количество выбранных записей об уполномоченных представителях: {0}", SelectDataFromUPfile.list_UP.Count());

                //Console.WriteLine();



                //------------------------------------------------------------------------------------------
                //2. Выбираем данные по кураторам (каталог \"_In_Curators\")

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 117));
                Console.WriteLine("Обработка файлов в каталоге \"_In_Curators\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                Console.WriteLine(new string('-', 117));
                Console.ForegroundColor = ConsoleColor.Gray;

                SelectDataFromCuratorsFile.ObrFileFromDirectory(IOoperations.katalogInCurators);
                //Console.WriteLine();
                //Console.WriteLine("Количество выбранных записей о кураторах (каталог \"_In_Curators\"): {0}", SelectDataFromCuratorsFile.dictionary_Curators.Count());

                //Console.WriteLine();


                //TODO: Закомментировал "кураторы частично"
                ////------------------------------------------------------------------------------------------
                ////3. Выбираем данные по кураторам (каталог \"_In_Curators_partial\")

                //Console.ForegroundColor = ConsoleColor.Cyan;
                //Console.WriteLine(new string('-', 117));
                //Console.WriteLine("Обработка файлов в каталоге \"_In_Curators_partial\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                //Console.WriteLine(new string('-', 117));
                //Console.ForegroundColor = ConsoleColor.Gray;

                //SelectDataFromCuratorsFilePartial.ObrFileFromDirectory(IOoperations.katalogInCuratorsPartial);
                //Console.WriteLine();
                //Console.WriteLine("Количество выбранных записей о кураторах (каталог \"_In_Curators_partial\"): {0}", SelectDataFromCuratorsFilePartial.dictionary_CuratorsPartial.Count());

                //Console.WriteLine();

                //------------------------------------------------------------------------------------------
                //4. Выбор данных из БД Perso

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 117));
                Console.WriteLine("Выбор данных из БД Perso (общий реестр СЗВ-СТАЖ), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                Console.WriteLine(new string('-', 117));
                Console.ForegroundColor = ConsoleColor.Gray;

                //Выбираем данные из БД Perso
                SelectDataFromPersoDB_AllForm.SelectDataFromPerso(DatabaseQueries.CreateQueryPersoReestrSZVSTAG(Program.raionS, Program.raionPo, Program.p_date_priem_st, Program.p_date_priem_fn, Program.otchYear));



                //------------------------------------------------------------------------------------------
                //5. Выбираем данные из РК АСВ            

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 117));
                Console.WriteLine("Выбираем данные из РК АСВ, пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                Console.WriteLine(new string('-', 117));
                Console.ForegroundColor = ConsoleColor.Gray;

                //Наполняем коллекцию уникальными значениями регНомеров из БД ПК Perso
                foreach (var itemDataFromPersoDB in Program.listReestrSZVSTAG)
                {
                    //Коллекция уникальных регномеров из реестра Perso
                    persoUnikRegNomForPKASV.Add(SelectDataFromRKASVDB.ConvertRegNomForSelect(itemDataFromPersoDB.regNum));
                }

                if (persoUnikRegNomForPKASV.Count != 0)
                {
                    //Выбираем данные из РК АСВ
                    SelectDataFromRKASVDB.SelectDataFromRKASV(DatabaseQueries.CreatequeryRKASV());

                    //Импортируем в коллекцию Program.listReestrSZVSTAG данные из РК АСВ
                    SelectDataFromRKASVDB.ImportDataFromRKASV();

                    string nameResultFile_SelectFromASV = IOoperations.katalogOut + @"\" + @"_9_SelectFromASV_" + DateTime.Now.ToShortDateString() + ".csv";
                    if (File.Exists(nameResultFile_SelectFromASV)) { File.Delete(nameResultFile_SelectFromASV); }

                    //Формируем заголовок                   
                    string zagolovok = "raion" + ";" + "insurer_reg_num" + ";" + "name" + ";" + "insurer_reg_start_date" + ";" + "insurer_reg_finish_date" + ";" + "INSURER_REG_DATE_RO" + ";" + "INSURER_UNREG_DATE_RO" + ";" + "category_code" + ";" + "insurer_inn" + ";" + "reg_start_code" + ";" + "reg_finish_code" + ";" + "kpp" + ";" + "status_id" + ";" + "kurator" + ";";

                    //Формируем результирующий файл на основании данных из БД
                    //TODO: НЕ закомментировал создание файла статистики "SelectFromASV"
                    SelectDataFromRKASVDB.CreateExportFile(zagolovok, Program.dictionary_dataFromPKASVDB, nameResultFile_SelectFromASV);



                    //------------------------------------------------------------------------------------------
                    //6. Формируем результирующий файл "_PersoReestrSZVSTAG_" на основании данных из БД

                    //Создаем имя результирующего файла
                    string nameResultFile_PersoReestrSZVSTAG = IOoperations.katalogOut + @"\" + @"_1_СЗВ-СТАЖ_Общий_реестр_" + DateTime.Now.ToShortDateString() + ".csv";
                    if (File.Exists(nameResultFile_PersoReestrSZVSTAG)) { File.Delete(nameResultFile_PersoReestrSZVSTAG); }

                    //Формируем результирующий файл на основании данных из БД                    
                    SelectDataFromPersoDB_AllForm.CreateExportFile(Program.zagolovokPersoSZVSTAG, Program.listReestrSZVSTAG, nameResultFile_PersoReestrSZVSTAG);



                    //------------------------------------------------------------------------------------------
                    //7. Создаем необходимые коллекции из реестра "Program.listReestrSZVSTAG"
                    CreateDataFromPersoSelect.CreateListCollections();


                    //------------------------------------------------------------------------------------------
                    //8. Наполняем коллекцию уникальными значениями регНомеров из БД ПК Perso
                    foreach (var itemDataFromPersoDB in CreateDataFromPersoSelect.list_SZV_STAG)
                    {
                        if (itemDataFromPersoDB.year == Program.otchYear || itemDataFromPersoDB.year == (Convert.ToInt32(Program.otchYear) + 1).ToString())
                        {
                            //Коллекция уникальных регномеров из реестра Perso для "CreateUniqPersoFile" (создание реестра уникальных регНомеров из БД Perso)
                            persoUnikRegNom_list_SZV_STAG.Add(SelectDataFromRKASVDB.ConvertRegNomForSelect(itemDataFromPersoDB.regNum));
                        }
                    }



                    if (persoUnikRegNom_list_SZV_STAG.Count != 0)
                    {
                        //Очищаем временную таблицу
                        //"TRUNCATE table TEMP.REGNUMB immediate"
                        InsertIntoTempRegnumb.DeleteTempValueInBD();

                        //int countMassiveRegNum = persoUnikRegNom_list_SZV_STAG.Count();



                        //Вставка массива регНомеров во временную таблицу
                        //insert into temp.regnumb(regnumb) values(42001000010),(42001000027);
                        foreach (var item in persoUnikRegNom_list_SZV_STAG)
                        {
                            //--countMassiveRegNum;
                            if (reestr350regNumForInsert.Count == 350)
                            {
                                InsertIntoTempRegnumb.Insert(DatabaseQueries.CreateInsertQuery());
                                reestr350regNumForInsert.Clear();

                                reestr350regNumForInsert.Add(item);
                            }
                            //else if (countMassiveRegNum == 0 && reestr350regNumForInsert.Count!=0)
                            //{
                            //    InsertIntoTempRegnumb.Insert(DatabaseQueries.CreateInsertQuery());
                            //    reestr350regNumForInsert.Clear();
                            //}
                            else
                            {
                                reestr350regNumForInsert.Add(item);
                            }
                        }

                        if (reestr350regNumForInsert.Count != 0)
                        {
                            InsertIntoTempRegnumb.Insert(DatabaseQueries.CreateInsertQuery());
                            reestr350regNumForInsert.Clear();
                        }



                        //------------------------------------------------------------------------------------------
                        //9. Выбор данных из БД Perso (СЗВ-СТАЖ, СЗВ-КОРР ИСХД формы)_v2
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Выбор данных из БД Perso (СЗВ-СТАЖ, СЗВ-КОРР ИСХД формы), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        //Выбираем данные из БД Perso
                        SelectDataFromPersoDB_ISXDform.SelectDataFromPersoDB(DatabaseQueries.CreateQueryPersoReestrISXD());



                        //------------------------------------------------------------------------------------------
                        //10. Выбор данных из БД Perso (СЗВ-СТАЖ ОТМН формы)

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Выбор данных из БД Perso (СЗВ, ОТМН формы), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        //Выбираем данные из БД Perso
                        SelectDataFromPersoDB_OTMNform.SelectDataFromPerso(DatabaseQueries.CreateQueryPersoReestrOTMN());



                        //------------------------------------------------------------------------------------------
                        //11. Формируем реестр уникальных СНИЛС в СЗВ-СТАЖ, СЗВ-КОРР с учетом отмененных форм
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Формируем реестр уникальных СНИЛС в СЗВ-СТАЖ, СЗВ-КОРР с учетом отмененных форм, пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        //Формируем реестр уникальных СНИЛС СЗВ-СТАЖ, СЗВ-КОРР с учетом отмененных форм
                        SelectDataFromPersoDB_Compare_SZVSTAG_ISX_and_OTMN.Compare_SZVSTAG_ISX_and_OTMN();

                        Console.WriteLine("Количество выбранных строк из БД Perso (уникальных СНИЛС в СЗВ-СТАЖ, СЗВ-КОРР с учетом отмененных форм): {0} ", uniqSNILS_ISXD_STAG_no_OTMN.Count());
                        //Console.WriteLine();

                        if (Program.uniqSNILS_ISXD_STAG_no_OTMN.Count != 0)
                        {
                            string zagolovokUniqSNILS_ISXD_STAG_no_OTMN = "№ п/п" + ";" + "Район" + ";" + "РегНомер" + ";" + "СНИЛС" + ";"
                                                            + "Дата записи в БД" + ";" + "Время записи в БД";

                            string nameResultFile_UniqSNILS_ISXD_STAG_no_OTMN = IOoperations.katalogOut + @"\" + @"_1_СЗВ-СТАЖ_УникСНИЛС_без_ОТМН_" + DateTime.Now.ToShortDateString() + ".csv";
                            if (File.Exists(nameResultFile_UniqSNILS_ISXD_STAG_no_OTMN)) { File.Delete(nameResultFile_UniqSNILS_ISXD_STAG_no_OTMN); }

                            //Формируем результирующий файл
                            SelectDataFromPersoDB_Compare_SZVSTAG_ISX_and_OTMN.CreateExportFile(zagolovokUniqSNILS_ISXD_STAG_no_OTMN, Program.uniqSNILS_ISXD_STAG_no_OTMN, nameResultFile_UniqSNILS_ISXD_STAG_no_OTMN);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Нет данных.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }



                        //------------------------------------------------------------------------------------------
                        //12. Наполняем коллекцию уникальными значениями регНомеров из listReestrSZV_ISXD
                        foreach (var item in Program.listReestrSZV_ISXD)
                        {
                            //Коллекция уникальных регномеров из реестра Perso для "CreateUniqPersoFile" (создание реестра уникальных регНомеров из БД Perso)
                            Program.persoUnikRegNomForSZV_STAG_Uniq_Reestr.Add(SelectDataFromRKASVDB.ConvertRegNomForSelect(item.regNum));
                        }




                        //------------------------------------------------------------------------------------------
                        //00. Выбираем данные по СЗВ-М
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Выбор данных из БД Perso (СЗВ-М), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;


                        //00.1. Выбираем данные из БД Perso ISX                           
                        SelectDataFromPersoDB_SZVM.SelectDataFromPerso_ISXD(DatabaseQueries.CreateQueryPersoSZVM_ISX());

                        //00.2. Выбираем данные из БД Perso OTMN и сравниваем ИСХД и ОТМН формы, оставляя уник. СНИЛС
                        SelectDataFromPersoDB_SZVM.SelectDataFromPerso_OTMN(DatabaseQueries.CreateQueryPersoSZVM_OTMN());

                        //00.3 Сверяем полученные реестры и убираем отмененные формы 
                        //и формируем массив Program.hashSet_SZVM_UniqSNILS - уникальных KEY (INN + SNILS + KPP + otchMonth) для сверки с СЗВ-СТАЖ
                        SelectDataFromPersoDB_SZVM.Compare_SZV_M_ISX_and_OTMN();

                        //00.4 Формируем результирующий файл на основании данных из БД                    
                        string nameResultFile_Compare_SZV_M_ISX_and_OTMN = IOoperations.katalogOut + @"\" + @"_1_СЗВ-М_УникСНИЛС_без_ОТМН_" + DateTime.Now.ToShortDateString() + ".csv"; //Имя файла
                        if (File.Exists(nameResultFile_Compare_SZV_M_ISX_and_OTMN)) { File.Delete(nameResultFile_Compare_SZV_M_ISX_and_OTMN); }

                        if (SelectDataFromPersoDB_SZVM.uniqSNILS_SZV_M.Count() != 0)
                        {
                            //Формируем результирующий файл
                            SelectDataFromPersoDB_SZVM.CreateExportFile(nameResultFile_Compare_SZV_M_ISX_and_OTMN, SelectDataFromPersoDB_SZVM.zagolovokPersoSZV_short, SelectDataFromPersoDB_SZVM.uniqSNILS_SZV_M);
                        }

                        //00.5 Формируем сводную информацию по ЗЛ в СЗВ-М




                        //------------------------------------------------------------------------------------------
                        //13. На основании коллекции "уникальные регномера из реестра Perso" выбираем поэтапно из коллекций "данные из реестра Perso" статусы обработки                        
                        //и создаем файл "_УникРегНомер_Общий_реестр_СЗВ-СТАЖ_"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Формируем сводный реестр уникальных регномеров из БД Perso (со статусом приема), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;


                        //На основании коллекции выбираем данные
                        SelectDataForResultFile.SelectResultData();

                        //Формируем общий CSV-файл статистики "УникРегНомер_Общий_реестр_СЗВ-СТАЖ_"
                        if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.Count != 0)
                        {
                            //имя файла
                            string resultFile_dictionaryUnikRegNomPersoALL = IOoperations.katalogOut + @"\" + @"_3_СЗВ-СТАЖ_УникРегНомера_Общий_реестр_" + Program.otchYear + @"_" + DateTime.Now.ToShortDateString() + "_.csv";

                            //Проверяем на наличие файла отчета, если существует - удаляем
                            if (File.Exists(resultFile_dictionaryUnikRegNomPersoALL)) { File.Delete(resultFile_dictionaryUnikRegNomPersoALL); }

                            //Формируем результирующий файл
                            SelectDataForResultFile.CreateExportFile(resultFile_dictionaryUnikRegNomPersoALL, Program.zagolovokPersoSZVSTAG, SelectDataForResultFile.dictionaryUnikRegNomPersoALL);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Нет данных \"_3_СЗВ - СТАЖ_УникРегНомера_Общий_реестр_\".");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }



                        //Формируем общий CSV-файл статистики "_Статус_09_НЕТ ДЕЯТЕЛЬНОСТИ_"
                        if (SelectDataForResultFile.dictionaryUnikRegNomPerso_NFXD.Count != 0)
                        {
                            //имя файла
                            string resultFile_dictionaryUnikRegNomPerso_NFXD = IOoperations.katalogOut + @"\" + @"_3_СЗВ-СТАЖ_Статус_09_НЕТ ДЕЯТЕЛЬНОСТИ_" + Program.otchYear + @"_" + DateTime.Now.ToShortDateString() + "_.csv";

                            //Проверяем на наличие файла отчета, если существует - удаляем
                            if (File.Exists(resultFile_dictionaryUnikRegNomPerso_NFXD)) { File.Delete(resultFile_dictionaryUnikRegNomPerso_NFXD); }

                            //Формируем результирующий файл
                            SelectDataForResultFile.CreateExportFile(resultFile_dictionaryUnikRegNomPerso_NFXD, Program.zagolovokPersoSZVSTAG, SelectDataForResultFile.dictionaryUnikRegNomPerso_NFXD);

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Внимание! Есть страхователи со статусом \"НЕТ ДЕЯТЕЛЬНОСТИ\" .");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Нет страхователей со статусом \"НЕТ ДЕЯТЕЛЬНОСТИ\" .");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine();
                        }



                        //------------------------------------------------------------------------------------------
                        //14. Формируем сводную информацию на основании коллекции "_3_СЗВ-СТАЖ_УникРегНомера_Общий_реестр_" и создаем файл @"_УникРегНомерPerso_Свод_New"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Формируем сводную \"_3_СЗВ-СТАЖ_УникРегНомера_Свод_\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        CreateSvod_Itog_New.CreateSvod(SelectDataForResultFile.dictionaryUnikRegNomPersoALL);



                        //------------------------------------------------------------------------------------------
                        //15. Формируем реестр "нулевиков" из реестра "_УникРегНомерPerso_Общий_реестр_"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Формируем реестр \"нулевиков\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        SelectNullFormSZV.SelectNullForm(SelectDataForResultFile.dictionaryUnikRegNomPersoALL);


                        if (SelectNullFormSZV.dictionaryNullRegNomPerso.Count != 0)
                        {
                            //имя файла
                            string resultFile_dictionaryNullRegNomPerso = IOoperations.katalogOut + @"\" + @"_4_Нулевые формы_реестр_" + DateTime.Now.ToShortDateString() + "_.csv";

                            //Проверяем на наличие файла отчета, если существует - удаляем
                            if (File.Exists(resultFile_dictionaryNullRegNomPerso))
                            {
                                IOoperations.FileDelete(resultFile_dictionaryNullRegNomPerso);
                            }

                            //создаем общий файл
                            SelectNullFormSZV.WriteLogs(resultFile_dictionaryNullRegNomPerso, Program.zagolovokPersoSZVSTAG, SelectNullFormSZV.dictionaryNullRegNomPerso);

                            Console.WriteLine("Количество выбранных строк из БД Perso (\"нулевиков\"): {0} ", SelectNullFormSZV.dictionaryNullRegNomPerso.Count());
                        }
                        else
                        {
                            //Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Нет данных.");
                            //Console.ForegroundColor = ConsoleColor.Gray;
                        }



                        //------------------------------------------------------------------------------------------
                        //16. Формируем реестр "полностью отмененных" из реестра "_УникРегНомерPerso_Общий_реестр_"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Формируем реестр \"полностью отмененных\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        SelectOTMNFormSZV.SelectNullForm(SelectDataForResultFile.dictionaryUnikRegNomPersoALL);

                        if (SelectOTMNFormSZV.dictionaryOTMNRegNomPerso.Count != 0)
                        {
                            //имя файла
                            string resultFile_dictionary_OTMN_ALL_ZL = IOoperations.katalogOut + @"\" + @"_4_Полностью отменены_реестр_" + DateTime.Now.ToShortDateString() + "_.csv";

                            //Проверяем на наличие файла отчета, если существует - удаляем
                            if (File.Exists(resultFile_dictionary_OTMN_ALL_ZL))
                            {
                                IOoperations.FileDelete(resultFile_dictionary_OTMN_ALL_ZL);
                            }

                            //создаем общий файл
                            SelectOTMNFormSZV.WriteLogs(resultFile_dictionary_OTMN_ALL_ZL, Program.zagolovokPersoSZVSTAG, SelectOTMNFormSZV.dictionaryOTMNRegNomPerso);

                            Console.WriteLine("Количество выбранных строк из БД Perso (\"полностью отмененных\"): {0} ", SelectOTMNFormSZV.dictionaryOTMNRegNomPerso.Count());
                        }
                        else
                        {
                            //Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Нет данных.");
                            //Console.ForegroundColor = ConsoleColor.Gray;
                        }



                        //------------------------------------------------------------------------------------------
                        //17. Формируем реестр "дублей по ИНН" из реестра "_УникРегНомерPerso_Общий_реестр_"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Формируем реестр \"дублей по ИНН\", пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        SelectDubliINN.SelectDubli(SelectDataForResultFile.dictionaryUnikRegNomPersoALL);

                        if (SelectDubliINN.dictionaryDubliINNPerso.Count != 0)
                        {
                            //имя файла
                            string resultFile_dictionaryDubliINNPerso = IOoperations.katalogOut + @"\" + @"_4_Дубли_ИНН_реестр_" + DateTime.Now.ToShortDateString() + "_.csv";

                            //Проверяем на наличие файла отчета, если существует - удаляем
                            if (File.Exists(resultFile_dictionaryDubliINNPerso))
                            {
                                IOoperations.FileDelete(resultFile_dictionaryDubliINNPerso);
                            }

                            //создаем общий файл
                            SelectDubliINN.WriteLogs(resultFile_dictionaryDubliINNPerso, Program.zagolovokPersoSZVSTAG, SelectDubliINN.dictionaryDubliINNPerso);

                            Console.WriteLine("Количество выбранных строк из БД Perso (\"дублей по ИНН\"): {0} ", SelectDubliINN.dictionaryDubliINNPerso.Count());
                        }
                        else
                        {
                            //Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Нет данных.");
                            //Console.ForegroundColor = ConsoleColor.Gray;
                        }



                        //------------------------------------------------------------------------------------------
                        //18. Считываем данные в каталоге \"_In_PlanPriema\" для сверки с плановым показателемпо приему СЗВ-СТАЖ
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Считываем данные из плана по приему СЗВ-СТАЖ (каталог \"_In_PlanPriema\"), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        SelectDataFromPlanPriemaFile.ObrFileFromDirectory(IOoperations.katalogInPlanPriema);



                        //------------------------------------------------------------------------------------------
                        //19. Сравниваем реестры "dictionaryPlanPriema" и "dictionaryUnikRegNomPersoALL"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 117));
                        Console.WriteLine("Сравниваем данные из плана по приему СЗВ-СТАЖ и реестр Perso, пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 117));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        CompareReestrPersoAndPlanPriema.CompareReestr();


                        /*
                        //------------------------------------------------------------------------------------------
                        //20.1 Считываем данные предыдущей отработки реестра ошибок Perso (файлы \"Perso_Отработка\")
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(new string('-', 107));
                        Console.WriteLine("Считываем данные предыдущей отработки ошибок Perso (каталог \"Perso_Отработка\"), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
                        Console.WriteLine(new string('-', 107));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        SelectDataFromPersoOtrabotkaFile.ObrFileFromDirectory(IOoperations.katalogInPersoOtrabotkaOld);
                        */


                        //------------------------------------------------------------------------------------------
                        //20.2 Добавляем новые ошибки в реестр \"Perso_Отработка\"
                        SelectDataFromPersoOtrabotkaFile.AddNewError();



                        //TODO: !!! Закомментировал формирование Excel-файла статистики "Perso_Отработка"
                        //------------------------------------------------------------------------------------------
                        //20.3 Формируем Excel-файл статистики "Perso_Отработка"                        
                        //CreateExcelFilePersoOtrabotka.CreateNewExcelFile();











                    }

                }



                //вычисляем время затраченное на обработку
                TimeSpan stop = DateTime.Now - start;

                //Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('-', 117));
                Console.WriteLine("Обработка выполнилась за " + stop.TotalSeconds + " сек. ({0})", DateTime.Now.ToLongTimeString());
                Console.ForegroundColor = ConsoleColor.Gray;

                //Console.ReadKey();

                //Задержка экрана
                Thread.Sleep(TimeSpan.FromSeconds(5));



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
}

//TODO: !!! Закомментировал выборки по СЗВ-М
/*
//TODO: Заменяем способ формирования рееста

//------------------------------------------------------------------------------------------
//11. Выбор данных из БД Perso (свод по СЗВ-М)
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(new string('-', 117));
Console.WriteLine("Выбор данных из БД Perso (свод по СЗВ-М), пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
Console.WriteLine(new string('-', 117));
Console.ForegroundColor = ConsoleColor.Gray;

//11.1. Наполняем коллекцию уникальными значениями регНомеров из listReestrSZV_ISXD
foreach (var item in Program.listReestrSZV_ISXD)
{
    //Коллекция уникальных регномеров из реестра Perso для "CreateUniqPersoFile" (создание реестра уникальных регНомеров из БД Perso)
    Program.persoUnikRegNomForSZVM_STAG.Add(SelectDataFromRKASVDB.ConvertRegNomForSelect(item.regNum));
}


//11.2. Выбираем данные из БД Perso ISX                           
SelectDataFromPersoDB_SZVM_v2.SelectDataFromPerso_ISXD(DatabaseQueries.CreateQueryPersoReestrSZVM_ISX());


//Закомментировал
//if (SelectDataFromPersoDB_SZVM_v2.dictionary_uniqSNILS_ISXD_SZV_M.Count() != 0)
//{
//    //Создаем имя результирующего файла
//    string nameResultFile_SelectFromPerso_SZVM_snils = IOoperations.katalogOut + @"\" + @"_9_СЗВ-М_ИСХД_СНИЛС_и_периоды_" + DateTime.Now.ToShortDateString() + ".csv";

//    //Лучше использовать проверку наличия файла                
//    if (File.Exists(nameResultFile_SelectFromPerso_SZVM_snils)) { File.Delete(nameResultFile_SelectFromPerso_SZVM_snils); }

//    //Формируем заголовок               raion + ";" + regNum + ";" + strnum + ";" + otchYear + ";" + otchMonth + ";"    
//    string zagolovok_SelectFromPerso_SZVM = "raion" + ";" + "reg_num" + ";" + "strnum" + ";" + "otchYear" + ";" + "otchMonth" + ";";

//    //Формируем результирующий файл на основании данных из БД                     
//    SelectDataFromPersoDB_SZVM_v2.CreateExportFileSNILS(zagolovok_SelectFromPerso_SZVM, SelectDataFromPersoDB_SZVM_v2.dictionary_uniqSNILS_ISXD_SZV_M, nameResultFile_SelectFromPerso_SZVM_snils);
//}


//Закомментировал
//11.3. Выбираем данные из БД Perso OTMN и сравниваем ИСХД и ОТМН формы, оставляя уник. СНИЛС
SelectDataFromPersoDB_SZVM_v2.SelectDataFromPerso_OTMN(DatabaseQueries.CreateQueryPersoReestrSZVM_OTMN());


//if (SelectDataFromPersoDB_SZVM_v2.dictionary_uniqSNILS_OTMN_SZV_M.Count() != 0)
//{
//    //Создаем имя результирующего файла
//    string nameResultFile_SelectFromPerso_SZVM_snils = IOoperations.katalogOut + @"\" + @"_9_СЗВ-М_ОТМН_СНИЛС_и_периоды_" + DateTime.Now.ToShortDateString() + ".csv";

//    //Лучше использовать проверку наличия файла                
//    if (File.Exists(nameResultFile_SelectFromPerso_SZVM_snils)) { File.Delete(nameResultFile_SelectFromPerso_SZVM_snils); }

//    //Формируем заголовок               raion + ";" + regNum + ";" + strnum + ";" + otchYear + ";" + otchMonth + ";"    
//    string zagolovok_SelectFromPerso_SZVM = "raion" + ";" + "reg_num" + ";" + "strnum" + ";" + "otchYear" + ";" + "otchMonth" + ";";

//    //Формируем результирующий файл на основании данных из БД                     
//    SelectDataFromPersoDB_SZVM_v2.CreateExportFileSNILS(zagolovok_SelectFromPerso_SZVM, SelectDataFromPersoDB_SZVM_v2.dictionary_uniqSNILS_OTMN_SZV_M, nameResultFile_SelectFromPerso_SZVM_snils);
//}









//foreach (string regNumItem in Program.persoUnikRegNomForSZVM_STAG)
//{
//    //Выбираем данные из БД Perso                            
//    SelectDataFromPersoDB_SZVM.SelectDataFromPerso(regNumItem);                           
//}



if (SelectDataFromPersoDB_SZVM_v2.uniqSNILS_ISXD_SZV_M_no_OTMN.Count() != 0)
{
    //Создаем имя результирующего файла
    string nameResultFile_SelectFromPerso_SZVM_snils = IOoperations.katalogOut + @"\" + @"_2_СЗВ-М_УникСНИЛС_и_периоды_без_ОТМН_" + DateTime.Now.ToShortDateString() + ".csv";

    //Лучше использовать проверку наличия файла                
    if (File.Exists(nameResultFile_SelectFromPerso_SZVM_snils)) { File.Delete(nameResultFile_SelectFromPerso_SZVM_snils); }

    //Формируем заголовок               raion + ";" + regNum + ";" + strnum + ";" + otchYear + ";" + otchMonth + ";"    
    string zagolovok_SelectFromPerso_SZVM = "raion" + ";" + "reg_num" + ";" + "strnum" + ";" + "otchYear" + ";" + "otchMonth" + ";";

    //Формируем результирующий файл на основании данных из БД                     
    SelectDataFromPersoDB_SZVM_v2.CreateExportFileSNILS(zagolovok_SelectFromPerso_SZVM, SelectDataFromPersoDB_SZVM_v2.uniqSNILS_ISXD_SZV_M_no_OTMN, nameResultFile_SelectFromPerso_SZVM_snils);
}



Console.WriteLine("Количество выбранных строк из БД Perso (свод по СЗВ-М): {0} ", dictionary_svodDataFromPersoDB_UniqSNILS_SZVM.Count());
//Console.WriteLine();

//Формируем реестр
if (Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVM.Count != 0)
{
    //Создаем имя результирующего файла
    string nameResultFile_SelectFromPerso_SZVM = IOoperations.katalogOut + @"\" + @"_2_СЗВ-М_Свод_УникСНИЛС_без_ОТМН_" + DateTime.Now.ToShortDateString() + ".csv";

    //Лучше использовать проверку наличия файла                
    if (File.Exists(nameResultFile_SelectFromPerso_SZVM)) { File.Delete(nameResultFile_SelectFromPerso_SZVM); }

    //Формируем заголовок                   
    string zagolovok_SelectFromPerso_SZVM = "reg_num" + ";" + "countSNILS" + ";";

    //Формируем результирующий файл на основании данных из БД                     
    SelectDataFromPersoDB_SZVM_v2.CreateExportFile(zagolovok_SelectFromPerso_SZVM, dictionary_svodDataFromPersoDB_UniqSNILS_SZVM, nameResultFile_SelectFromPerso_SZVM);
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Нет данных.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

*/



//TODO: !!! Закомментировал
/*

//------------------------------------------------------------------------------------------
//21. Сравниваем коллекции СНИЛС в СЗВ-М и в СЗВ-СТАЖ и выбираем уникальных
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(new string('-', 107));
Console.WriteLine("Сравниваем коллекции СНИЛС в СЗВ-М и в СЗВ-СТАЖ и выбираем уникальных, пожалуйста ждите... ({0})", DateTime.Now.ToLongTimeString());
Console.WriteLine(new string('-', 107));
Console.ForegroundColor = ConsoleColor.Gray;

Compare_SZVM_SZVSTAG.Compare();

Console.WriteLine("Количество выбранных строк (\"уникальных в СЗВ-СТАЖ\"): {0} ", Compare_SZVM_SZVSTAG.dictionary_Uniq_snils_SZV_STAG.Count());
Console.WriteLine("Количество выбранных строк (\"уникальных в СЗВ-М\"): {0} ", Compare_SZVM_SZVSTAG.dictionary_Uniq_snils_SZV_M.Count());

//Формируем реестр "_12_УникСНИЛС_в_СЗВ-СТАЖ_"
if (Compare_SZVM_SZVSTAG.dictionary_Uniq_snils_SZV_STAG.Count != 0)
{
    //Создаем имя результирующего файла
    string nameResultFile_Uniq_snils_SZV_STAG = IOoperations.katalogOut + @"\" + @"_12_УникСНИЛС_в_СЗВ-СТАЖ_" + DateTime.Now.ToShortDateString() + ".csv";

    //Лучше использовать проверку наличия файла                
    if (File.Exists(nameResultFile_Uniq_snils_SZV_STAG)) { File.Delete(nameResultFile_Uniq_snils_SZV_STAG); }

    //Формируем заголовок                   
    string zagolovok_Uniq_snils_SZV_STAG = "№ п/п" + ";" + "СНИЛС" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";"
                                            + "ИНН" + ";" + "КПП" + ";" + "Куратор" + ";";

    //Формируем результирующий файл на основании данных из БД                     
    Compare_SZVM_SZVSTAG.CreateExportFile(nameResultFile_Uniq_snils_SZV_STAG, zagolovok_Uniq_snils_SZV_STAG, Compare_SZVM_SZVSTAG.dictionary_Uniq_snils_SZV_STAG);
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Нет уникальных в СЗВ-СТАЖ.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

//Формируем реестр "_12_УникСНИЛС_в_СЗВ-М_"
if (Compare_SZVM_SZVSTAG.dictionary_Uniq_snils_SZV_M.Count != 0)
{
    //Создаем имя результирующего файла
    string nameResultFile_Uniq_snils_SZV_M = IOoperations.katalogOut + @"\" + @"_12_УникСНИЛС_в_СЗВ-М_" + DateTime.Now.ToShortDateString() + ".csv";

    //Лучше использовать проверку наличия файла                
    if (File.Exists(nameResultFile_Uniq_snils_SZV_M)) { File.Delete(nameResultFile_Uniq_snils_SZV_M); }

    //Формируем заголовок                   
    string zagolovok_Uniq_snils_SZV_M = "№ п/п" + ";" + "СНИЛС" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";"
                                            + "ИНН" + ";" + "КПП" + ";" + "Куратор" + ";";

    //Формируем результирующий файл на основании данных из БД                     
    Compare_SZVM_SZVSTAG.CreateExportFile(nameResultFile_Uniq_snils_SZV_M, zagolovok_Uniq_snils_SZV_M, Compare_SZVM_SZVSTAG.dictionary_Uniq_snils_SZV_M);
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Нет уникальных в СЗВ-М.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

*/



/*                                                                   

//------------------------------------------------------------------------------------------
//TODO: Сделать сверку с МИЦ-файлом
//18. Сравниваем реестры "dictionarySpuspis" и "dictionaryUnikRegNomPersoALL"
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(new string('-', 107));
Console.WriteLine("Сравниваем данные из файла \"42_spuspis2a.csv\" и реестр Perso, пожалуйста ждите...");
Console.WriteLine(new string('-', 107));
Console.ForegroundColor = ConsoleColor.Gray;

CompareReestrPersoAndSpuspis.CompareReestr(SelectDataFromSpuspisFile.dictionaryDataSpuspisSvod, SelectDataForResultFile.dictionaryUnikRegNomPersoALL);

*/
