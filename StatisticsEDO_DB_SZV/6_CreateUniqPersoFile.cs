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
    //------------------------------------------------------------------------------------------
    //На основании коллекции "уникальные регномера из реестра Perso" выбираем поэтапно из коллекций "данные из реестра Perso" (ИСХ, ДОП) статусы обработки
    #region Выбор данных из файла
    static class SelectDataForResultFile
    {
        public static Dictionary<string, DataFromPersoDB> dictionaryUnikRegNomPersoALL = new Dictionary<string, DataFromPersoDB>();    //Коллекция уникальных регНомеров из реестра Perso и статуса обработки сведений страхователя

        private static Dictionary<string, DataFromPersoDB> tmpDictionary_perso = new Dictionary<string, DataFromPersoDB>();             //Временная коллекция для поэтапного наполнения 

        //Коллекция уникальных регНомеров из реестра Perso, статус "09_НЕТ ДЕЯТЕЛЬНОСТИ"
        public static Dictionary<string, DataFromPersoDB> dictionaryUnikRegNomPerso_NFXD = new Dictionary<string, DataFromPersoDB>();

        public static List<DataFromPersoDB> listForSelectSposobPredstavleniya = new List<DataFromPersoDB>();

        //------------------------------------------------------------------------------------------        
        //На основании коллекции "уникальные регномера из реестра Perso" выбираем поэтапно из коллекций "данные из реестра Perso" статусы обработки        

        public static void SelectResultData()
        {            
            if (Program.persoUnikRegNomForSZV_STAG_Uniq_Reestr.Count() == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет данных для обработки.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {

                try
                {
                    //С учетом условия "itemListDataFromPersoDB.year==Program.otchYear"

                    //1. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ИСХОДНАЯ"   "Принят"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_prinyato);

                    //2. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ДОПОЛНЯЮЩАЯ"   "Принят"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_DOP_prinyato);

                    //3. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ИСХОДНАЯ"   "Принят частично"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_partial_prinyato);

                    //4. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ДОПОЛНЯЮЩАЯ"   "Принят частично"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_DOP_partial_prinyato);

                    //5. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ИСХОДНАЯ"   "Не проверен"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_NO_proveren);

                    //6. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ДОПОЛНЯЮЩАЯ"   "Не проверен"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_DOP_NO_proveren);

                    //7. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ИСХОДНАЯ"   "Не принят"                    
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_NO_prinyato);

                    //8. "СЗВ-СТАЖ" "ИСХОДНАЯ" "ДОПОЛНЯЮЩАЯ"   "Не принят"                    
                    //InsertInDictionaryCollection(Program.persoUnikRegNomForSZVM_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_DOP_NO_prinyato);
                    InsertInDictionaryCollection(Program.persoUnikRegNom_list_SZV_STAG, CreateDataFromPersoSelect.list_SZV_STAG_ISXD_DOP_NO_prinyato);



                    //9. Добавляем в общую коллекцию уникальные рег.номера со статусом "нет статуса" (остальные, в т.ч. ОТМН формы)
                    //foreach (var regNom in Program.persoUnikRegNomForSZVM_STAG)
                    foreach (var regNom in Program.persoUnikRegNom_list_SZV_STAG)
                    {
                        DataFromPersoDB tmpData = SelectDataFromListDataFromPersoDB(ConvertRegNom(regNom), dictionaryUnikRegNomPersoALL);
                        if (tmpData.regNum == "")
                        {
                            //TODO: Проверить!!! Добавил условие "itemListDataFromPersoDB.year==Program.otchYear"
                            if (tmpData.year == Program.otchYear || tmpData.year == (Convert.ToInt32(Program.otchYear) + 1).ToString())
                            {
                                tmpData.regNum = ConvertRegNom(regNom);
                                tmpData.statVIO = "нет статуса";

                                dictionaryUnikRegNomPersoALL.Add(ConvertRegNom(regNom), tmpData);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }



                    //10. Наполняем dictionaryUnikRegNomPersoALL верным способом представления сведений (на основании выборки из общего реестра всех документов СЗВ-СТАЖ)
                    SelectSposobPredstavleniya.SelectSposobPerdstavl();


                    /*
                    //11. Наполняем dictionaryUnikRegNomPersoALL верным количеством ЗЛ в СЗВ-М (запрос к БД Perso, с учетом отмененных снилс)                    
                    foreach (var itemCountZLPerso in Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVM)
                    {
                        DataFromPersoDB tmpDataFromPersoDB = new DataFromPersoDB();
                        if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.TryGetValue(itemCountZLPerso.Key, out tmpDataFromPersoDB))
                        {
                            SelectDataForResultFile.dictionaryUnikRegNomPersoALL[itemCountZLPerso.Key].uniqZlSZVM = itemCountZLPerso.Value.ToString();
                        }
                    }
                    */


                    //12. Наполняем dictionaryUnikRegNomPersoALL верным количеством ЗЛ в СЗВ-СТАЖ (запрос к БД Perso, с учетом отмененных снилс)                    
                    foreach (var itemCountZLPerso in Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVSTAG)
                    {
                        DataFromPersoDB tmpDataFromPersoDB = new DataFromPersoDB();
                        if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.TryGetValue(itemCountZLPerso.Key, out tmpDataFromPersoDB))
                        {
                            SelectDataForResultFile.dictionaryUnikRegNomPersoALL[itemCountZLPerso.Key].uniqZlSZVSTAG = itemCountZLPerso.Value.ToString();
                        }
                    }



                    //13. Ищем наличие OTMN формы и наполняем значениями коллекции "Program.dictionary_ReestrSZV_OTMN" коллекцию "dictionaryUnikRegNomPersoALL"
                    foreach (var itemReestrSZV_OTMN in Program.dictionary_ReestrSZV_OTMN)
                    {
                        DataFromPersoDB tmpDataFromPersoDB = new DataFromPersoDB();
                        if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.TryGetValue(itemReestrSZV_OTMN.Key, out tmpDataFromPersoDB))
                        {
                            SelectDataForResultFile.dictionaryUnikRegNomPersoALL[itemReestrSZV_OTMN.Key].otmnFormAvailability = itemReestrSZV_OTMN.Value.ToString();
                        }
                    }



                    //------------------------------------------------------------------------------------------
                    //14. Наполняем dictionaryUnikRegNomPersoALL значением поля "status_id"
                    //Наполняем данными из РК АСВ 

                    foreach (var itemDataFromPKASVDB in Program.dictionary_dataFromPKASVDB)
                    {

                        DataFromPersoDB tmpDataFromPersoDB = new DataFromPersoDB();
                        if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.TryGetValue(itemDataFromPKASVDB.Key, out tmpDataFromPersoDB))
                        {
                            SelectDataForResultFile.dictionaryUnikRegNomPersoALL[itemDataFromPKASVDB.Key].status_id = itemDataFromPKASVDB.Value.status_id;
                        }

                    }


                    //------------------------------------------------------------------------------------------
                    //15. Выбираем из dictionaryUnikRegNomPersoALL записи со значением поля "status_id" = "09_НЕТ ДЕЯТЕЛЬНОСТИ"
                    foreach (var item in dictionaryUnikRegNomPersoALL)
                    {
                        if (item.Value.status_id == "09_НЕТ ДЕЯТЕЛЬНОСТИ")
                        {
                            dictionaryUnikRegNomPerso_NFXD[item.Key] = item.Value;
                        }
                    }


                    
                    //16. Наполняем dictionaryUnikRegNomPersoALL верным количеством ЗЛ в СЗВ-M (запрос к БД Perso, с учетом отмененных снилс)                    
                    foreach (var itemCountZLPerso in Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVM)
                    {
                        DataFromPersoDB tmpDataFromPersoDB = new DataFromPersoDB();
                        if (SelectDataForResultFile.dictionaryUnikRegNomPersoALL.TryGetValue(itemCountZLPerso.Key, out tmpDataFromPersoDB))
                        {
                            SelectDataForResultFile.dictionaryUnikRegNomPersoALL[itemCountZLPerso.Key].uniqZlSZVM = itemCountZLPerso.Value.ToString();
                        }
                    }






                    //Формируем Excel-файл статистики "УникРегНомерPerso_Общий_реестр_"
                    //TODO: Закомментировал создание Excel-файла статистики "УникРегНомерPerso_Общий_реестр"
                    //CreateExcelFileUniqPerso.CreateNewExcelFile(dictionaryUnikRegNomPersoALL);                 

                }
                catch (Exception ex)
                {
                    IOoperations.WriteLogError(ex.ToString());

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                //Console.WriteLine();
                Console.WriteLine("Количество записей в файле \"_УникРегНомер_Общий_реестр_СЗВ-СТАЖ_\": {0}", dictionaryUnikRegNomPersoALL.Count());
                //Console.WriteLine();
            }
        }

        //------------------------------------------------------------------------------------------
        //Ищем данные в коллекциях из реестра Perso (по статусам обработки) и наполняем общий файл
        private static void InsertInDictionaryCollection(SortedSet<string> persoUnikRegNom, List<DataFromPersoDB> listDataFromPersoDB)
        {
            //очищаем временную коллекцию-словарь
            tmpDictionary_perso.Clear();

            //наполняем коллекцию-словарь
            foreach (DataFromPersoDB itemListDataFromPersoDB in listDataFromPersoDB)
            {
                DataFromPersoDB tmpData = new DataFromPersoDB();
                if (tmpDictionary_perso.TryGetValue(itemListDataFromPersoDB.regNum, out tmpData))
                {
                    continue;
                }
                else
                {
                    //TODO: Проверить!!! Добавил условие "itemListDataFromPersoDB.year==Program.otchYear"
                    if (itemListDataFromPersoDB.year == Program.otchYear)
                    {
                        tmpDictionary_perso.Add(itemListDataFromPersoDB.regNum, itemListDataFromPersoDB);

                        listForSelectSposobPredstavleniya.Add(itemListDataFromPersoDB);
                    }

                }
            }

            //Добавляем в общую коллекцию уникальные рег.номера с требуемым статусом
            foreach (var regNom in persoUnikRegNom)
            {
                DataFromPersoDB tmpData = SelectDataFromListDataFromPersoDB(ConvertRegNom(regNom), tmpDictionary_perso);
                if (tmpData.regNum == "")
                {
                    continue;
                }
                else
                {
                    DataFromPersoDB tmpData3 = new DataFromPersoDB();
                    if (dictionaryUnikRegNomPersoALL.TryGetValue(ConvertRegNom(regNom), out tmpData3))
                    {
                        continue;
                    }
                    else
                    {
                        dictionaryUnikRegNomPersoALL.Add(ConvertRegNom(regNom), tmpData);
                    }
                }
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
        //Выбираем данные из коллекции по регНомеру
        private static DataFromPersoDB SelectDataFromListDataFromPersoDB(string regNom, Dictionary<string, DataFromPersoDB> tmpDictionary_perso)
        {
            DataFromPersoDB tmpData = new DataFromPersoDB();
            if (tmpDictionary_perso.TryGetValue(regNom, out tmpData))
            {
                return tmpData;
            }
            else
            {
                return new DataFromPersoDB();
            }
        }



        //------------------------------------------------------------------------------------------       
        //Формируем результирующий файл статистики
        public static void CreateExportFile(string nameFile, string zagolovok, Dictionary<string, DataFromPersoDB> dictionary_perso)
        {
            try
            {
                //Добавляем в файл данные                
                using (StreamWriter writer = new StreamWriter(nameFile, true, Encoding.GetEncoding(1251)))
                {
                    writer.WriteLine(zagolovok);

                    int i = 0;

                    foreach (var item in dictionary_perso)
                    {
                        i++;
                        writer.Write(i + ";");
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
    }

    #endregion

}
