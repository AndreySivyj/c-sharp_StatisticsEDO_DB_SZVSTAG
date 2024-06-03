//TODO: !!! Закомментировал
/*
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
    public class CompareData
    {
        public string strnum;
        public string raion;
        public string insurer_reg_num;
        public string insurer_short_name;
        public string insurer_inn;
        public string insurer_kpp;
        //public string period;
        public string kurator;

        public CompareData(string strnum = "", string raion = "", string insurer_reg_num = "", string insurer_short_name = "",
                                string insurer_inn = "", string kpp = "", string kurator = "")
        {
            this.strnum = strnum;
            this.raion = raion;
            this.insurer_reg_num = insurer_reg_num;
            this.insurer_short_name = insurer_short_name;
            this.insurer_inn = insurer_inn;
            this.insurer_kpp = kpp;            
            this.kurator = kurator;

        }

        public override string ToString()
        {

            return strnum + ";" + raion + ";" + insurer_reg_num + ";" + insurer_short_name + ";"
                + insurer_inn + ";" + insurer_kpp + ";" + kurator + ";";

        }
    }

    static class Compare_SZVM_SZVSTAG
    {
        //private static SortedSet<string> sortedSet_regNum = new SortedSet<string>();     //Коллекция уник. регНомеров для сравнения

        private static Dictionary<string, CompareData> dictionary_CompareData_SZV_M = new Dictionary<string, CompareData>();     //Коллекция уник. данных СЗВ-М для сравнения
        private static Dictionary<string, CompareData> dictionary_CompareData_SZV_STAG = new Dictionary<string, CompareData>();  //Коллекция уник. данных СЗВ-СТАЖ для сравнения

        public static Dictionary<string, CompareData> dictionary_Uniq_snils_SZV_M = new Dictionary<string, CompareData>();      //Уник. СНИЛС в СЗВ-М
        public static Dictionary<string, CompareData> dictionary_Uniq_snils_SZV_STAG = new Dictionary<string, CompareData>();   //Уник. СНИЛС в СЗВ-СТАЖ

        //------------------------------------------------------------------------------------------
        public static void Compare()
        {
            try
            {
                //------------------------------------------------------------------------------------------
                //Наполняем коллекцию dictionary_CompareData_SZV_STAG
                foreach (var itemDataSTAG in Program.uniqSNILS_ISXD_STAG_no_OTMN)
                {
                    DataFromRKASVDB tmpData = new DataFromRKASVDB();
                    if (Program.dictionary_dataFromPKASVDB.TryGetValue(itemDataSTAG.Value.regNum, out tmpData))
                    {
                        //dictionary_CompareData_SZV_STAG.Add(tmpData.insurer_inn + "_" + tmpData.insurer_kpp + "_" + itemDataSTAG.Value.strnum,
                        //                            new CompareData(itemDataSTAG.Value.strnum, itemDataSTAG.Value.raion, itemDataSTAG.Value.regNum, tmpData.insurer_short_name,
                        //                                            tmpData.insurer_inn, tmpData.insurer_kpp, tmpData.kurator));
                        dictionary_CompareData_SZV_STAG[tmpData.insurer_inn + "_" + tmpData.insurer_kpp + "_" + itemDataSTAG.Value.strnum] =
                                                    new CompareData(itemDataSTAG.Value.strnum, itemDataSTAG.Value.raion, itemDataSTAG.Value.regNum, tmpData.insurer_short_name,
                                                                    tmpData.insurer_inn, tmpData.insurer_kpp, tmpData.kurator);

                        //sortedSet_regNum.Add(itemDataSTAG.Value.regNum);
                    }
                }

                if (dictionary_CompareData_SZV_STAG.Count != 0)
                {
                    //Создаем имя результирующего файла
                    string nameResultFile_CompareData_SZV_STAG = IOoperations.katalogOut + @"\" + @"_9_СЗВ-СТАЖ_СНИЛС_ИНН_КПП_" + DateTime.Now.ToShortDateString() + ".csv";

                    //Лучше использовать проверку наличия файла                
                    if (File.Exists(nameResultFile_CompareData_SZV_STAG)) { File.Delete(nameResultFile_CompareData_SZV_STAG); }

                    //Формируем заголовок                   
                    string zagolovok_Uniq_snils_SZV_STAG = "№ п/п" + ";" + "СНИЛС" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";"
                                                            + "ИНН" + ";" + "КПП" + ";"  + "Куратор" + ";";

                    //Формируем результирующий файл на основании данных из БД                     
                    Compare_SZVM_SZVSTAG.CreateExportFile(nameResultFile_CompareData_SZV_STAG, zagolovok_Uniq_snils_SZV_STAG, dictionary_CompareData_SZV_STAG);
                }



                //------------------------------------------------------------------------------------------
                //Наполняем коллекцию dictionary_CompareData_SZV_M
                foreach (var itemDataSZVM in SelectDataFromPersoDB_SZVM_v2.uniqSNILS_ISXD_SZV_M_no_OTMN)
                {
                    //dictionary_CompareData_SZV_M[Program.dictionary_dataFromPKASVDB[itemDataSZVM.Value.regNum].insurer_inn + "_" + Program.dictionary_dataFromPKASVDB[itemDataSZVM.Value.regNum].insurer_kpp + "_" + itemDataSZVM.Value.strnum] =
                    //                            new CompareData(
                    //                                            itemDataSZVM.Value.strnum, 
                    //                                            itemDataSZVM.Value.raion, 
                    //                                            itemDataSZVM.Value.regNum, 
                    //                                            Program.dictionary_dataFromPKASVDB[itemDataSZVM.Value.regNum].insurer_short_name,
                    //                                            Program.dictionary_dataFromPKASVDB[itemDataSZVM.Value.regNum].insurer_inn, 
                    //                                            Program.dictionary_dataFromPKASVDB[itemDataSZVM.Value.regNum].insurer_kpp,                                                                 
                    //                                            Program.dictionary_dataFromPKASVDB[itemDataSZVM.Value.regNum].kurator
                    //                                            );

                    //if (sortedSet_regNum.Contains(itemDataSZVM.Value.regNum))
                    //{
                    DataFromRKASVDB tmpData = new DataFromRKASVDB();
                    if (Program.dictionary_dataFromPKASVDB.TryGetValue(itemDataSZVM.Value.regNum, out tmpData))
                    {
                        //dictionary_CompareData_SZV_M.Add(tmpData.insurer_inn + tmpData.insurer_kpp + itemDataSZVM.strnum,
                        //                            new CompareData(itemDataSZVM.strnum, itemDataSZVM.raion, itemDataSZVM.regNum, tmpData.insurer_short_name,
                        //                                            tmpData.insurer_inn, tmpData.insurer_kpp, tmpData.kurator));

                        //есть не уникальные записи
                        dictionary_CompareData_SZV_M[tmpData.insurer_inn + "_" + tmpData.insurer_kpp + "_" + itemDataSZVM.Value.strnum] =
                                                    new CompareData(itemDataSZVM.Value.strnum, itemDataSZVM.Value.raion, itemDataSZVM.Value.regNum, tmpData.insurer_short_name,
                                                                    tmpData.insurer_inn, tmpData.insurer_kpp, tmpData.kurator);
                    }
                    //}


                }

                if (dictionary_CompareData_SZV_M.Count != 0)
                {
                    //Создаем имя результирующего файла
                    string nameResultFile_CompareData_SZV_M = IOoperations.katalogOut + @"\" + @"_9_СЗВ-М_СНИЛС_ИНН_КПП_" + DateTime.Now.ToShortDateString() + ".csv";

                    //Лучше использовать проверку наличия файла                
                    if (File.Exists(nameResultFile_CompareData_SZV_M)) { File.Delete(nameResultFile_CompareData_SZV_M); }

                    //Формируем заголовок                   
                    string zagolovok_Uniq_snils_SZV_M = "№ п/п" + ";" + "СНИЛС" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";"
                                                            + "ИНН" + ";" + "КПП" + ";" + "Куратор" + ";";

                    //Формируем результирующий файл на основании данных из БД                     
                    Compare_SZVM_SZVSTAG.CreateExportFile(nameResultFile_CompareData_SZV_M, zagolovok_Uniq_snils_SZV_M, dictionary_CompareData_SZV_M);
                }


                //------------------------------------------------------------------------------------------
                //Наполняем коллекцию dictionary_Uniq_snils_SZV_STAG (Уник. СНИЛС в СЗВ-СТАЖ)
                foreach (var itemCompareData_SZV_STAG in dictionary_CompareData_SZV_STAG)
                {
                    CompareData tmpData = new CompareData();
                    if (dictionary_CompareData_SZV_M.TryGetValue(itemCompareData_SZV_STAG.Key, out tmpData))
                    {
                        continue;
                    }
                    else
                    {
                        dictionary_Uniq_snils_SZV_STAG.Add(itemCompareData_SZV_STAG.Key, itemCompareData_SZV_STAG.Value);
                    }
                }


                //Наполняем коллекцию dictionary_Uniq_snils_SZV_M (Уник. СНИЛС в СЗВ-М)
                foreach (var itemCompareData_SZV_M in dictionary_CompareData_SZV_M)
                {
                    CompareData tmpData = new CompareData();
                    if (dictionary_CompareData_SZV_STAG.TryGetValue(itemCompareData_SZV_M.Key, out tmpData))
                    {
                        continue;
                    }
                    else
                    {
                        dictionary_Uniq_snils_SZV_M.Add(itemCompareData_SZV_M.Key, itemCompareData_SZV_M.Value);
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
        //Формируем результирующий файл статистики
        public static void CreateExportFile(string resultFile, string zagolovok, Dictionary<string, CompareData> dictionary_Compare)
        {
            //формируем результирующий файл статистики
            using (StreamWriter writer = new StreamWriter(resultFile, false, Encoding.GetEncoding(1251)))
            {
                writer.WriteLine(zagolovok);

                int i = 0;

                foreach (var item in dictionary_Compare)
                {
                    i++;
                    writer.Write(i + ";");
                    writer.WriteLine(item.Value.ToString());
                }
            }
        }

    }
}
*/