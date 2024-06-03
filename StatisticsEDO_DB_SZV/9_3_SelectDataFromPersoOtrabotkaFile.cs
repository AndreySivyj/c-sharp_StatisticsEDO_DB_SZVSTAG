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
    #region считанная из файла информация

    public class DataFromPersoOtrabotkaFile
    {
        //string zagolovokError = "№ п/п" + ";" + "КодЗап" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";" + "ОтчМесяц" + ";" + "ОтчГод" + ";" + "Тип сведений" + ";" + "Дата представления" + ";"
        //                                        + "Категория" + ";" + "Дата постановки в ПФР" + ";" + "Дата постановки в РО" + ";" + "Дата снятия в РО" + ";" + "Результат проверки" + ";" + "Дата проверки" + ";"
        //                                        + "Количество ЗЛ в файле" + ";" + "ЗЛ принято" + ";" + "ЗЛ не принято" + ";"
        //                                        + "Статус квитанции" + ";" + "Специалист" + ";" + "Способ представления" + ";" + "Куратор" + ";" + "УП по данным УПФР" + ";"
        //                                        + "Дата направления уведомления страхователю" + ";" + "Контрольная дата для исправления (3 дня)" + ";" + "Исправлено (да|нет|не требуется)" + ";"
        //                                        + "Дата направления реестра в УПФР (в случае не исправления)" + ";" + "Дата исправления ошибки (после направления реестра УПФР)" + ";" + "Примечание" + ";"
        //                                        + "Результат контроля (руководитель)" + ";";

        public string codZap;
        public string raion;
        public string regNum;
        public string nameStrah;
        public string year;
        public string tip;
        public string tipForm;
        public string tipSveden;
        public string dataPredst;
        public string kategory;
        public string dataPostPFR;
        public string dataPostRO;
        public string dataSnyatRO;
        public string statVIO;
        public string dataVIO;
        public string kolZL;
        public string kolZlLGT;
        public string kolZL_PR_VIO;
        public string kolZL_NPR_VIO;
        public string statusKvitanc;
        public string spec;
        public string specChanged;
        public string kurator;
        public string UP;
        public string dataNaprUvedomlStrah;
        public string kontrDataIspravleniya;
        public string statusIspravleniya;
        public string dataNaprReestraPFR;
        public string dataIspravleniyaError;
        public string primechanie;
        public string resultatKontrolya;

        public DataFromPersoOtrabotkaFile(string codZap = "", string raion = "", string regNum = "", string nameStrah = "", string year = "",
                            string tip = "", string tipForm = "", string typeSved = "",
                            string dataPredst = "", string kategory = "", string dataPostPFR = "", string dataPostRO = "", string dataSnyatRO = "", string statVIO = "",
                             string dataVIO = "", string kolZL = "", string kolZlLGT = "", string kolZL_PR_VIO = "", string kolZL_NPR_VIO = "", string statusKvitanc = "",
                            string spec = "", string specChanged = "", string kurator = "", string UP = "", string dataNaprUvedomlStrah = "",
                            string kontrDataIspravleniya = "", string statusIspravleniya = "", string dataNaprReestraPFR = "", string dataIspravleniyaError = "",
                            string primechanie = "", string resultatKontrolya = "")
        {
            this.codZap = codZap;
            this.raion = raion;
            this.regNum = regNum;
            this.nameStrah = nameStrah;
            this.year = year;
            this.tip = tip;
            this.tipForm = tipForm;
            this.tipSveden = typeSved;
            this.dataPredst = dataPredst;
            this.kategory = kategory;
            this.dataPostPFR = dataPostPFR;
            this.dataPostRO = dataPostRO;
            this.dataSnyatRO = dataSnyatRO;
            this.statVIO = statVIO;
            this.dataVIO = dataVIO;
            this.kolZL = kolZL;
            this.kolZlLGT = kolZlLGT;
            this.kolZL_PR_VIO = kolZL_PR_VIO;
            this.kolZL_NPR_VIO = kolZL_NPR_VIO;
            this.statusKvitanc = statusKvitanc;
            this.spec = spec;
            this.specChanged = specChanged;
            this.kurator = kurator;
            this.UP = UP;
            this.dataNaprUvedomlStrah = dataNaprUvedomlStrah;
            this.kontrDataIspravleniya = kontrDataIspravleniya;
            this.statusIspravleniya = statusIspravleniya;
            this.dataNaprReestraPFR = dataNaprReestraPFR;
            this.dataIspravleniyaError = dataIspravleniyaError;
            this.primechanie = primechanie;
            this.resultatKontrolya = resultatKontrolya;
        }


        public override string ToString()
        {
            return codZap + ";" + raion + ";" + regNum + ";" + nameStrah + ";" + year + ";"
                        + tip + ";" + tipForm + ";" + tipSveden + ";" + dataPredst + ";" + kategory + ";" + dataPostPFR + ";"
                        + dataPostRO + ";" + dataSnyatRO + ";" + statVIO + ";" + dataVIO + ";" + kolZL + ";" + kolZlLGT + ";" + kolZL_PR_VIO + ";" + kolZL_NPR_VIO + ";" + statusKvitanc + ";"
                         + spec + ";" + specChanged + ";" + kurator + ";" + UP + ";" + dataNaprUvedomlStrah + ";" + kontrDataIspravleniya + ";" + statusIspravleniya + ";"
                         + dataNaprReestraPFR + ";" + dataIspravleniyaError + ";" + primechanie + ";" + resultatKontrolya + ";";
        }
    }

    #endregion

    //------------------------------------------------------------------------------------------
    #region Выбор данных из файла
    static class SelectDataFromPersoOtrabotkaFile
    {
        //Маска поиска файлов
        private static string fileSearchMask = "*.csv";

        public static Dictionary<string, DataFromPersoOtrabotkaFile> dictionaryPersoOtrabotkaOld = new Dictionary<string, DataFromPersoOtrabotkaFile>();    //Коллекция всех данных из реестра Perso_Отработка    

        //------------------------------------------------------------------------------------------
        //Открываем поток для чтения из файла и выбираем нужные позиции           
        private static void ReadAndParseTextFile(string openFile)
        {
            try
            {
                using (StreamReader reader = new StreamReader(openFile, Encoding.GetEncoding(1251)))
                {
                    while (!reader.EndOfStream)
                    {
                        string strTmp = reader.ReadLine();

                        char[] separator = { ';' };    //список разделителей в строке
                        string[] massiveStr = strTmp.Split(separator);     //создаем массив из строк между разделителями                      

                        //massiveStr.Count() == 20 &&
                        if (
                            massiveStr[0] != "№ п/п"
                            && massiveStr[0] != ""
                            && massiveStr[0] != " "
                            )
                        {
                            //Коллекция всех данных из файла
                            dictionaryPersoOtrabotkaOld.Add(massiveStr[1], new DataFromPersoOtrabotkaFile(massiveStr[1], massiveStr[2], massiveStr[3], massiveStr[4], massiveStr[5], massiveStr[6], massiveStr[7],
                                                        massiveStr[8], massiveStr[9], massiveStr[10], massiveStr[11], massiveStr[12], massiveStr[13], massiveStr[14], massiveStr[15],
                                                        massiveStr[16], massiveStr[17], massiveStr[18], massiveStr[19], massiveStr[20], massiveStr[21], massiveStr[22], massiveStr[23],
                                                        massiveStr[24], massiveStr[25], massiveStr[26], massiveStr[27], massiveStr[28], massiveStr[29], massiveStr[30], massiveStr[31]));
                        }
                        else
                        {
                            continue;
                        }
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
        /// <summary>
        /// Выбираем данные из файлов
        /// </summary>
        /// <param name="folderIn">Каталог с обрабатываемыми файлами</param>
        public static void ObrFileFromDirectory(string folderIn)
        {
            //Очищаем коллекции для данных из файлов
            dictionaryPersoOtrabotkaOld.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(folderIn);

            if (dirInfo.GetFiles(fileSearchMask).Count() == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет файлов \"Perso_Отработка\".");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {

                try
                {
                    //Console.WriteLine("Количество файлов \"Perso_Отработка\": " + dirInfo.GetFiles(fileSearchMask).Count());

                    //обрабатываем каждый файл по отдельности
                    foreach (FileInfo file in dirInfo.GetFiles(fileSearchMask))
                    {
                        //Открываем поток для чтения из файла и выбираем нужные позиции   
                        ReadAndParseTextFile(file.FullName);
                    }

                    Console.WriteLine("Количество выбранных строк: {0}", dictionaryPersoOtrabotkaOld.Count());
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
        //Формируем результирующий файл статистики с ошибками (файл \"Perso_Отработка\")
        public static void AddNewError()
        {
            foreach (var itemDataFromPersoDB in Program.listReestrSZVSTAG)
            {
                if (itemDataFromPersoDB.statVIO == "Принят частично" 
                    || itemDataFromPersoDB.statVIO == "Не принят" 
                    || itemDataFromPersoDB.statVIO == "Не подавался"
                    || itemDataFromPersoDB.statVIO == "")
                {

                    DataFromPersoOtrabotkaFile tmpData = new DataFromPersoOtrabotkaFile();
                    if (dictionaryPersoOtrabotkaOld.TryGetValue(itemDataFromPersoDB.codZap, out tmpData))
                    {
                        continue;
                    }
                    else
                    {
                        if (itemDataFromPersoDB.year == Program.otchYear || itemDataFromPersoDB.year == (Convert.ToInt32(Program.otchYear) + 1).ToString())
                        {
                            dictionaryPersoOtrabotkaOld.Add(itemDataFromPersoDB.codZap, new DataFromPersoOtrabotkaFile(
                                                                                                                        itemDataFromPersoDB.codZap, itemDataFromPersoDB.raion, itemDataFromPersoDB.regNum,
                                                                                                                        itemDataFromPersoDB.nameStrah, itemDataFromPersoDB.year,
                                                                                                                        itemDataFromPersoDB.tip, itemDataFromPersoDB.tipForm, itemDataFromPersoDB.tipSveden,
                                                                                                                        itemDataFromPersoDB.dataPriem, itemDataFromPersoDB.kategory,
                                                                                                                        itemDataFromPersoDB.dataPostPFR, itemDataFromPersoDB.dataPostRO, itemDataFromPersoDB.dataSnyatRO,
                                                                                                                        itemDataFromPersoDB.statVIO, itemDataFromPersoDB.dataVIO, itemDataFromPersoDB.kolZL, itemDataFromPersoDB.kolZlLGT,
                                                                                                                        itemDataFromPersoDB.kolZl_PR_VIO, itemDataFromPersoDB.kolZl_NPR_VIO, itemDataFromPersoDB.statusRec,
                                                                                                                        itemDataFromPersoDB.shname, itemDataFromPersoDB.specChanged, itemDataFromPersoDB.curator,
                                                                                                                        itemDataFromPersoDB.UP, itemDataFromPersoDB.dataUved, itemDataFromPersoDB.dataOtp));
                        }

                    }
                }
                else if (itemDataFromPersoDB.statVIO == "Не проверен" && itemDataFromPersoDB.statusRec == "Обработан")
                {
                    DataFromPersoOtrabotkaFile tmpData = new DataFromPersoOtrabotkaFile();
                    if (dictionaryPersoOtrabotkaOld.TryGetValue(itemDataFromPersoDB.codZap, out tmpData))
                    {
                        continue;
                    }
                    else
                    {
                        if (itemDataFromPersoDB.year == Program.otchYear || itemDataFromPersoDB.year == (Convert.ToInt32(Program.otchYear) + 1).ToString())
                        {
                            dictionaryPersoOtrabotkaOld.Add(itemDataFromPersoDB.codZap, new DataFromPersoOtrabotkaFile(
                                                                                            itemDataFromPersoDB.codZap, itemDataFromPersoDB.raion, itemDataFromPersoDB.regNum,
                                                                                            itemDataFromPersoDB.nameStrah, itemDataFromPersoDB.year,
                                                                                            itemDataFromPersoDB.tip, itemDataFromPersoDB.tipForm, itemDataFromPersoDB.tipSveden,
                                                                                            itemDataFromPersoDB.dataPriem, itemDataFromPersoDB.kategory,
                                                                                            itemDataFromPersoDB.dataPostPFR, itemDataFromPersoDB.dataPostRO, itemDataFromPersoDB.dataSnyatRO,
                                                                                            itemDataFromPersoDB.statVIO, itemDataFromPersoDB.dataVIO, itemDataFromPersoDB.kolZL, itemDataFromPersoDB.kolZlLGT,
                                                                                            itemDataFromPersoDB.kolZl_PR_VIO, itemDataFromPersoDB.kolZl_NPR_VIO, itemDataFromPersoDB.statusRec,
                                                                                            itemDataFromPersoDB.shname, itemDataFromPersoDB.specChanged, itemDataFromPersoDB.curator,
                                                                                            itemDataFromPersoDB.UP, itemDataFromPersoDB.dataUved, itemDataFromPersoDB.dataOtp));
                        }
                    }
                }
                else
                {
                    continue;
                }
            }


            //TODO: Закомментировал создание CSV-файла статистики "_Perso_Отработка_"
            ////формируем результирующий файл статистики
            //using (StreamWriter writer = new StreamWriter(resultFile, false, Encoding.GetEncoding(1251)))
            //{
            //    writer.WriteLine(zagolovok);

            //    int i = 0;

            //    foreach (var item in dictionaryPersoOtrabotkaOld)
            //    {
            //        i++;
            //        writer.Write(i + ";");
            //        writer.WriteLine(item.Value.ToString());
            //    }
            //}
        }




    }

    #endregion

}
