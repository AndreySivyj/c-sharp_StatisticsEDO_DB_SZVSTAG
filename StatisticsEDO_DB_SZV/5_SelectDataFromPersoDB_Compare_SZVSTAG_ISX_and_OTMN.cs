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
    static class SelectDataFromPersoDB_Compare_SZVSTAG_ISX_and_OTMN
    {
        private static Dictionary<string, DataFromPersoDB_ISXDform> dictionary_uniqSNILS_ISXD_STAG = new Dictionary<string, DataFromPersoDB_ISXDform>();       //Коллекция данных
        private static Dictionary<string, DataFromPersoDB_OTMNform> dictionary_uniqSNILS_OTMN_STAG = new Dictionary<string, DataFromPersoDB_OTMNform>();       //Коллекция данных

        public static void Compare_SZVSTAG_ISX_and_OTMN()
        {
            //return raion + ";" + regNum + ";" + strnum + ";" + dateBeg + ";" + dateEnd + ";" + dateINS + ";" + timeINS + ";";
            //public static List<DataFromPersoDB_ISXDform> listReestrSZV_ISXD = new List<DataFromPersoDB_ISXDform>();     //Коллекция данных из БД Perso (реестр ИСХД форм)   

            //наполняем словарь dictionary_uniqSNILS_ISXD_STAG последним по Дате (Времени) регНом+СНИЛС
            foreach (DataFromPersoDB_ISXDform itemDataPerso in Program.listReestrSZV_ISXD)
            {
                //регНом+СНИЛС есть в словаре
                DataFromPersoDB_ISXDform tmpData = new DataFromPersoDB_ISXDform();
                if (dictionary_uniqSNILS_ISXD_STAG.TryGetValue(itemDataPerso.regNum + itemDataPerso.strnum, out tmpData))
                {
                    //сверяем даты импорта в БД (больше)
                    if (Convert.ToDateTime(itemDataPerso.dateINS) > Convert.ToDateTime(tmpData.dateINS))
                    {
                        dictionary_uniqSNILS_ISXD_STAG[itemDataPerso.regNum + itemDataPerso.strnum] = itemDataPerso;
                    }
                    //сверяем даты импорта в БД (равны)
                    else if (Convert.ToDateTime(itemDataPerso.dateINS) == Convert.ToDateTime(tmpData.dateINS))
                    {
                        //тогда сверяем время импорта в БД (больше)
                        if (Convert.ToDateTime(itemDataPerso.timeINS) > Convert.ToDateTime(tmpData.timeINS))
                        {
                            dictionary_uniqSNILS_ISXD_STAG[itemDataPerso.regNum + itemDataPerso.strnum] = itemDataPerso;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    dictionary_uniqSNILS_ISXD_STAG.Add(itemDataPerso.regNum + itemDataPerso.strnum, itemDataPerso);
                }
            }


            //return raion + ";" + regNum + ";" + strnum + ";" + dateINS + ";" + timeINS + ";";
            //public static List<DataFromPersoDB_OTMNform> listReestrSZV_OTMN = new List<DataFromPersoDB_OTMNform>();     //Коллекция данных из БД Perso (реестр ОТМН форм)

            //наполняем словарь dictionary_uniqSNILS_OTMN_STAG последним по Дате (Времени) регНом+СНИЛС
            foreach (DataFromPersoDB_OTMNform itemDataPerso in Program.listReestrSZV_OTMN)
            {
                //регНом+СНИЛС есть в словаре
                DataFromPersoDB_OTMNform tmpData = new DataFromPersoDB_OTMNform();
                if (dictionary_uniqSNILS_OTMN_STAG.TryGetValue(itemDataPerso.regNum + itemDataPerso.strnum, out tmpData))
                {
                    //сверяем даты импорта в БД (больше)
                    if (Convert.ToDateTime(itemDataPerso.dateINS) > Convert.ToDateTime(tmpData.dateINS))
                    {
                        dictionary_uniqSNILS_OTMN_STAG[itemDataPerso.regNum + itemDataPerso.strnum] = itemDataPerso;
                    }
                    //сверяем даты импорта в БД (равны)
                    else if (Convert.ToDateTime(itemDataPerso.dateINS) == Convert.ToDateTime(tmpData.dateINS))
                    {
                        //тогда сверяем время импорта в БД (больше)
                        if (Convert.ToDateTime(itemDataPerso.timeINS) > Convert.ToDateTime(tmpData.timeINS))
                        {
                            dictionary_uniqSNILS_OTMN_STAG[itemDataPerso.regNum + itemDataPerso.strnum] = itemDataPerso;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    dictionary_uniqSNILS_OTMN_STAG.Add(itemDataPerso.regNum + itemDataPerso.strnum, itemDataPerso);
                }
            }
            

            
            //Формируем реестр уникальных СНИЛС СЗВ-СТАЖ, СЗВ-КОРР с учетом отмененных форм
            foreach (var item_uniqSNILS_ISXD_STAG in dictionary_uniqSNILS_ISXD_STAG)
            {
                //регНом+СНИЛС есть в словаре
                DataFromPersoDB_OTMNform tmpData = new DataFromPersoDB_OTMNform();
                if (dictionary_uniqSNILS_OTMN_STAG.TryGetValue(item_uniqSNILS_ISXD_STAG.Key, out tmpData))
                {
                    //сверяем даты импорта в БД (больше)
                    if (Convert.ToDateTime(item_uniqSNILS_ISXD_STAG.Value.dateINS) > Convert.ToDateTime(tmpData.dateINS))
                    {
                        Program.uniqSNILS_ISXD_STAG_no_OTMN[item_uniqSNILS_ISXD_STAG.Key] = item_uniqSNILS_ISXD_STAG.Value;                        
                    }
                    //сверяем даты импорта в БД (равны)
                    else if (Convert.ToDateTime(item_uniqSNILS_ISXD_STAG.Value.dateINS) == Convert.ToDateTime(tmpData.dateINS))
                    {
                        //тогда сверяем время импорта в БД (больше)
                        if (Convert.ToDateTime(item_uniqSNILS_ISXD_STAG.Value.timeINS) > Convert.ToDateTime(tmpData.timeINS))
                        {
                            Program.uniqSNILS_ISXD_STAG_no_OTMN[item_uniqSNILS_ISXD_STAG.Key] = item_uniqSNILS_ISXD_STAG.Value;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    Program.uniqSNILS_ISXD_STAG_no_OTMN[item_uniqSNILS_ISXD_STAG.Key]= item_uniqSNILS_ISXD_STAG.Value;
                }
            }



            //Формируем сводные данные на основании реестра уникальных СНИЛС в СЗВ-СТАЖ с учетом отмененных форм
            foreach (var item in Program.uniqSNILS_ISXD_STAG_no_OTMN)
            {               
                //Добавляем выбранные данные в коллекцию
                int tmpData = 0;
                if (Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVSTAG.TryGetValue(item.Value.regNum, out tmpData))
                {
                    //есть рег.Номер в словаре
                    ++Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVSTAG[item.Value.regNum];                   
                }
                else
                {
                    //нет рег.Номера в словаре
                    Program.dictionary_svodDataFromPersoDB_UniqSNILS_SZVSTAG[item.Value.regNum] = 1;
                }
            }

        }

        //------------------------------------------------------------------------------------------        
        //Формируем результирующий файл на основании данных из БД
        public static void CreateExportFile(string zagolovok, Dictionary<string, DataFromPersoDB_ISXDform> uniqSNILS_ISXD_STAG_no_OTMN, string nameFile)
        {
            try
            {
                //Добавляем в файл данные                
                using (StreamWriter writer = new StreamWriter(nameFile, true, Encoding.GetEncoding(1251)))
                {
                    writer.WriteLine(zagolovok);

                    int i = 0;

                    foreach (var item in uniqSNILS_ISXD_STAG_no_OTMN)
                    {
                        i++;
                        writer.Write(i + ";");
                        writer.WriteLine(item.Value.ToStringNoStag());
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
}
