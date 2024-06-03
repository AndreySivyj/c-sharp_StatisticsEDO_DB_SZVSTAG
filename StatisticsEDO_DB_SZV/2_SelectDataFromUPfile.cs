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

    public class DataFromUPfile
    {
        public string regNumStrah;

        public string regNumUP;

        public DataFromUPfile(string regNumStrah = "", string regNumUP = "")
        {
            this.regNumStrah = regNumStrah;
            this.regNumUP = regNumUP;
        }


        public override string ToString()
        {
            return regNumStrah + ";" + regNumUP + ";";
        }
    }

    #endregion

    //------------------------------------------------------------------------------------------
    #region Выбор данных из файла
    static class SelectDataFromUPfile
    {
        //Маска поиска файлов
        private static string fileSearchMask = "*.csv";

        //public static List<DataFromUPfile> list_UP = new List<DataFromUPfile>();           //Коллекция данных из файлов с УП  

        public static Dictionary<string, DataFromUPfile> dictionary_UP = new Dictionary<string, DataFromUPfile>();           //Коллекция данных из файлов с УП

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
                        //DataFromPersoFile tmpDataFromFile;

                        //Очищаем объект для данных из строки для каждой итерации цикла                    

                        string strTmp = reader.ReadLine();

                        char[] separator = { ';', ',' };    //список разделителей в строке
                        string[] massiveStr = strTmp.Split(separator);     //создаем массив из строк между разделителями

                        if (
                            //massiveStr.Count() == 2 && massiveStr[0] != "" && massiveStr[0] != " "  && massiveStr[0].Count() == 14 && massiveStr[1].Count() == 14)  
                            massiveStr.Count() == 3 && massiveStr[0] != "" && massiveStr[0] != " " && massiveStr[0].Count() == 14)
                        {
                            //list_UP.Add(new DataFromUPfile(massiveStr[0], massiveStr[1]));
                            //dictionary_UP.Add(massiveStr[0], new DataFromUPfile(massiveStr[0], massiveStr[1]));
                            dictionary_UP[massiveStr[0]]= new DataFromUPfile(massiveStr[0], massiveStr[1]);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    //if (list_UP.Count() == 0)
                    if (dictionary_UP.Count() == 0)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Внимание! Нет информации об уполномоченных представителях.");
                        Console.ForegroundColor = ConsoleColor.Gray;
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
            //list_UP.Clear();
            dictionary_UP.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(folderIn);

            if (dirInfo.GetFiles(fileSearchMask).Count() == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет файлов для обработки.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {

                try
                {
                    //обрабатываем каждый файл по отдельности
                    foreach (FileInfo file in dirInfo.GetFiles(fileSearchMask))
                    {
                        //Открываем поток для чтения из файла и выбираем нужные позиции   
                        ReadAndParseTextFile(file.FullName);
                    }
                    
                    //Console.WriteLine();
                    //Console.WriteLine("Количество выбранных строк из файла(ов) с информацией об УП: {0} ", list_UP.Count());
                    Console.WriteLine("Количество выбранных строк из файла(ов) с информацией об УП: {0} ", dictionary_UP.Count());
                    //Console.WriteLine();

                    //try
                    //{
                    //    //Добавляем в файл данные                
                    //    using (StreamWriter writer = new StreamWriter("nameFile.csv", true, Encoding.GetEncoding(1251)))
                    //    {
                    //        writer.WriteLine("regNumStrah" + ";" + "regNumUP" + ";");

                    //        foreach (var item in dictionary_UP)
                    //        {
                    //            writer.WriteLine(item.Value.ToString());
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    IOoperations.WriteLogError(ex.ToString());

                    //    Console.WriteLine();
                    //    Console.ForegroundColor = ConsoleColor.Red;
                    //    Console.WriteLine(ex.Message);
                    //    Console.ForegroundColor = ConsoleColor.Gray;
                    //}
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

    #endregion

}
