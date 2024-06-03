using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace StatisticsEDO_DB_SZV
{
    static class CompareReestrPersoAndPlanPriema
    {
        public static Dictionary<string, DataFromPersoDB> dictionaryUnikRegNomPersoCompare = new Dictionary<string, DataFromPersoDB>();             //Коллекция "Есть в Perso нет в Плане"

        public static Dictionary<string, string> dictionaryPlanPriemaCompare = new Dictionary<string, string>();    //Коллекция "Есть в Плане нет в Perso"

        public static void CompareReestr()
        {
            //очищаем коллекции
            dictionaryUnikRegNomPersoCompare.Clear();
            dictionaryPlanPriemaCompare.Clear();



            //------------------------------------------------------------------------------------------
            //наполняем коллекцию "Есть в Perso нет в Плане"
            foreach (var itemUnikRegNomPersoALL in SelectDataForResultFile.dictionaryUnikRegNomPersoALL)
            {
                string tmpDataPlanPriema = "";
                if (SelectDataFromPlanPriemaFile.dictionaryPlanPriema.TryGetValue(itemUnikRegNomPersoALL.Value.regNum, out tmpDataPlanPriema))
                {
                    continue;
                }
                else
                {
                    dictionaryUnikRegNomPersoCompare.Add(itemUnikRegNomPersoALL.Value.regNum, SelectDataForResultFile.dictionaryUnikRegNomPersoALL[itemUnikRegNomPersoALL.Value.regNum]);
                }
            }

            if (dictionaryUnikRegNomPersoCompare.Count() != 0)
            {
                //имя файла
                string resultFile_dictionaryUnikRegNomPersoCompare = IOoperations.katalogOut + @"\" + @"_10_Есть_в_Perso_нет_в_Плане_" + DateTime.Now.ToShortDateString() + "_.csv";

                //Проверяем на наличие файла отчета, если существует - удаляем
                if (File.Exists(resultFile_dictionaryUnikRegNomPersoCompare))
                {
                    IOoperations.FileDelete(resultFile_dictionaryUnikRegNomPersoCompare);
                }

                //создаем общий файл
                SelectDataForResultFile.CreateExportFile(resultFile_dictionaryUnikRegNomPersoCompare, Program.zagolovokPersoSZVSTAG, dictionaryUnikRegNomPersoCompare);

                Console.WriteLine("Количество записей в реестре \"Есть в Perso нет в Плане\" : {0}", dictionaryUnikRegNomPersoCompare.Count());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет данных.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }


            //------------------------------------------------------------------------------------------
            //наполняем коллекцию "Есть в Плане нет в Perso"
            foreach (var itemPlanPriema in SelectDataFromPlanPriemaFile.dictionaryPlanPriema)
            {
                DataFromPersoDB tmpDataUnikRegNomPerso = new DataFromPersoDB();
                if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.TryGetValue(itemPlanPriema.Key, out tmpDataUnikRegNomPerso))
                {
                    continue;
                }
                else
                {
                    dictionaryPlanPriemaCompare.Add(itemPlanPriema.Key, SelectDataFromPlanPriemaFile.dictionaryPlanPriema[itemPlanPriema.Key]);
                }
            }

            if (dictionaryPlanPriemaCompare.Count() != 0)
            {
                string zagolovok1 = "№ п/п" + ";" + "Район (МРУ)" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";"
                    + "Дата постановки в ПФР" + ";" + "Дата снятия в ПФР" + ";"
                    + "Дата постановки в РО" + ";" + "Дата снятия в РО" + ";"
                    + "Категория" + ";" + "ИНН" + ";" + "КПП" + ";"
                    + "Количество уникальных СНИЛС в СЗВ-М за 2019 год" + ";" 
                    + "Количество уникальных СНИЛС в СЗВ-СТАЖ за 2019 год(отчетность сдана)" + ";"
                    + "Ожидаемая дата представления СЗВ-СТАЖ за 2019 год" + ";";

                //имя файла
                string resultFile_dictionaryPlanPriemaCompare = IOoperations.katalogOut + @"\" + @"_10_Есть_в_Плане_нет_в_Perso_" + DateTime.Now.ToShortDateString() + "_.csv";

                //Проверяем на наличие файла отчета, если существует - удаляем
                if (File.Exists(resultFile_dictionaryPlanPriemaCompare))
                {
                    IOoperations.FileDelete(resultFile_dictionaryPlanPriemaCompare);
                }

                //создаем общий файл
                SelectDataFromPlanPriemaFile.WriteLogs(resultFile_dictionaryPlanPriemaCompare, zagolovok1, dictionaryPlanPriemaCompare);

                Console.WriteLine("Количество записей в реестре \"Есть в Плане нет в Perso\" : {0}", dictionaryPlanPriemaCompare.Count());
            }

        }
    }
}
