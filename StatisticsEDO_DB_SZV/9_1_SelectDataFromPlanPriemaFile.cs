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
    //#region считанная из файла информация

    //public class DataFromPlanPriemaFile
    //{
    //    //№ п/п	Код района	Рег. номер	Наименование	Дата постановки на учет в РО	Дата снятия с учета в РО	Категория	ИНН	"Количество ЗЛ, учтенных в подсистеме АИС ПФР 2 за предшествующий отчетный период по форме СЗВ-М"	"Предшественник ("старый" регистрационный номер)"

    //    public string nomPor;
    //    public string raion;
    //    public string regNum;
        
        
    //    public DataFromPlanPriemaFile(string nomPor = "", string raion = "", string regNum = "")
    //    {
    //        this.nomPor = nomPor;
    //        this.raion = raion;
    //        this.regNum = regNum;            
    //    }


    //    public override string ToString()
    //    {
    //        return nomPor + ";" + raion + ";" + regNum + ";";
            
    //    }
    //}

    //#endregion

    //------------------------------------------------------------------------------------------
    #region Выбор данных из файла
    static class SelectDataFromPlanPriemaFile
    {
        //Маска поиска файлов
        private static string fileSearchMask = "*.csv";

        public static Dictionary<string, string> dictionaryPlanPriema = new Dictionary<string, string>();    //Коллекция всех данных из реестра PlanPriema    

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
                        string strTmp_ALL = reader.ReadLine();

                        if (strTmp_ALL == "(\"\"старый\"\" регистрационный номер)\""|| strTmp_ALL ==""|| strTmp_ALL ==" ")
                        {
                            continue;
                        }
                        else
                        {
                            char[] separator = { ';' };    //список разделителей в строке
                            string[] massiveStr = strTmp_ALL.Split(separator);     //создаем массив из строк между разделителями                      

                            //massiveStr.Count() == 20 &&
                            if (
                                massiveStr[0] != "№ п/п"
                                && massiveStr[0] != ""
                                && massiveStr[0] != " "
                                && massiveStr[3] != "4"                                
                                )
                            {
                                //Коллекция всех данных из файла
                                //dictionaryPlanPriema.Add(massiveStr[2], new DataFromPlanPriemaFile(massiveStr[0], massiveStr[1], massiveStr[2], massiveStr[3], massiveStr[4], massiveStr[5], massiveStr[6],
                                //    massiveStr[7], massiveStr[8], massiveStr[9]));
                                dictionaryPlanPriema.Add(massiveStr[3], strTmp_ALL);
                            }
                            else
                            {
                                continue;
                            }
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
            dictionaryPlanPriema.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(folderIn);

            if (dirInfo.GetFiles(fileSearchMask).Count() == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет файлов в каталоге \"_In_PlanPriema\".");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {

                try
                {
                    //Console.WriteLine("Количество файлов в каталоге \"_In_PlanPriema\": " + dirInfo.GetFiles(fileSearchMask).Count());

                    //обрабатываем каждый файл по отдельности
                    foreach (FileInfo file in dirInfo.GetFiles(fileSearchMask))
                    {
                        if (file.Name == Program.otchYear + "_PlanSZVSTAG.csv")
                        {
                            //Открываем поток для чтения из файла и выбираем нужные позиции   
                            ReadAndParseTextFile(file.FullName);
                        }


                        //Открываем поток для чтения из файла и выбираем нужные позиции   
                        //ReadAndParseTextFile(file.FullName);
                    }

                    if (dictionaryPlanPriema.Count() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Внимание! Нет планового показателя в каталоге \"_In_PlanPriema\".");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine();
                    }

                    Console.WriteLine("Количество выбранных строк: {0}", dictionaryPlanPriema.Count());
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
        //Формируем результирующий файл статистики
        public static void WriteLogs(string resultFile, string zagolovok, Dictionary<string, string> dictionaryPlanPriema)
        {
            //формируем результирующий файл статистики
            using (StreamWriter writer = new StreamWriter(resultFile, false, Encoding.GetEncoding(1251)))
            {
                writer.WriteLine(zagolovok);

                foreach (var item in dictionaryPlanPriema)
                {
                    writer.WriteLine(item.Value.ToString());
                }
            }
        }
    }

    #endregion

}
