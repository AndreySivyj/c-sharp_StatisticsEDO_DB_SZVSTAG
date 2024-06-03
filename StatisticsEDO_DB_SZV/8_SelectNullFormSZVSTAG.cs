using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace StatisticsEDO_DB_SZV
{
    static class SelectNullFormSZV
    {
        public static Dictionary<string, DataFromPersoDB> dictionaryNullRegNomPerso = new Dictionary<string, DataFromPersoDB>();    //Коллекция "нулевиков" из реестра "_УникРегНомерPerso_Общий_реестр_"        

        //Выбираем "нулевиков" из реестра "_УникРегНомерPerso_Общий_реестр_"
        public static void SelectNullForm(Dictionary<string, DataFromPersoDB> dictionaryUnikRegNomPersoALL)
        {
            dictionaryNullRegNomPerso.Clear();

            foreach (var itemDictionaryUnikRegNomPersoALL in SelectDataForResultFile.dictionaryUnikRegNomPersoALL)
            {
                //if (itemDictionaryUnikRegNomPersoALL.Value.codZap== "844556")
                //{
                //    dictionaryNullRegNomPerso.Add(itemDictionaryUnikRegNomPersoALL.Value.regNum, itemDictionaryUnikRegNomPersoALL.Value);
                //}
                if (itemDictionaryUnikRegNomPersoALL.Value.kolZL == "0" &&
                    itemDictionaryUnikRegNomPersoALL.Value.kolZlLGT=="0"&&
                    itemDictionaryUnikRegNomPersoALL.Value.kolZl_NPR_VIO == "0"&&
                    itemDictionaryUnikRegNomPersoALL.Value.kolZl_PR_VIO == "0"&&
                    itemDictionaryUnikRegNomPersoALL.Value.uniqZlSZVSTAG == "0")
                {
                    dictionaryNullRegNomPerso.Add(itemDictionaryUnikRegNomPersoALL.Value.regNum, itemDictionaryUnikRegNomPersoALL.Value);
                }                
            }
                       

            

        }

        //------------------------------------------------------------------------------------------       
        //Формируем результирующий файл статистики
        public static void WriteLogs(string resultFile, string zagolovok, Dictionary<string, DataFromPersoDB> dictionary_perso)
        {
            //формируем результирующий файл статистики
            using (StreamWriter writer = new StreamWriter(resultFile, false, Encoding.GetEncoding(1251)))
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
    }
}
