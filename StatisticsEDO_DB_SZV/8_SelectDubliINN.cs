using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace StatisticsEDO_DB_SZV
{
    #region считанная из файла информация

    public class SvodINN
    {

        public string regNum;
        public int countINN;

        public SvodINN(string regNum = "", int countINN = 0)
        {
            this.regNum = regNum;
            this.countINN = countINN;
        }


        public override string ToString()
        {
            return regNum + ";" + countINN + ";";
        }
    }

    #endregion

    static class SelectDubliINN
    {
        public static Dictionary<string, SvodINN> dictionarySvodINN = new Dictionary<string, SvodINN>();    //Создаем сводную по ИНН

        public static Dictionary<string, DataFromPersoDB> dictionaryDubliINNPerso = new Dictionary<string, DataFromPersoDB>();    //Коллекция "дублей" по полю ИНН из реестра "_УникРегНомерPerso_Общий_реестр_"        

        //Выбираем "дубли" из реестра "_УникРегНомерPerso_Общий_реестр_"
        public static void SelectDubli(Dictionary<string, DataFromPersoDB> dictionaryUnikRegNomPersoALL)
        {
            dictionaryDubliINNPerso.Clear();

            //формируем свод по кол-ву рег.Номеров для каждого ИНН
            foreach (var itemDictionaryUnikRegNomPersoALL in dictionaryUnikRegNomPersoALL)
            {
                //Добавляем выбранные данные в коллекцию
                SvodINN tmpDataSvodINN = new SvodINN();
                if (dictionarySvodINN.TryGetValue(itemDictionaryUnikRegNomPersoALL.Value.inn + itemDictionaryUnikRegNomPersoALL.Value.kpp, out tmpDataSvodINN))
                {
                    //TODO: добавил проверку на наличие ИНН!!!
                    if (itemDictionaryUnikRegNomPersoALL.Value.inn != "")
                    {
                        //есть рег.Номер в словаре
                        ++dictionarySvodINN[itemDictionaryUnikRegNomPersoALL.Value.inn + itemDictionaryUnikRegNomPersoALL.Value.kpp].countINN;
                    }

                }
                else
                {
                    //TODO: добавил проверку на наличие ИНН!!!
                    if (itemDictionaryUnikRegNomPersoALL.Value.inn != "")
                    {
                        //нет рег.Номера в словаре
                        dictionarySvodINN.Add(itemDictionaryUnikRegNomPersoALL.Value.inn + itemDictionaryUnikRegNomPersoALL.Value.kpp, new SvodINN(itemDictionaryUnikRegNomPersoALL.Value.regNum, 1));
                    }
                }
            }



            //Формируем коллекцию "дублей по ИНН" из реестра "_УникРегНомерPerso_Общий_реестр_"
            foreach (var itemDictionarySvodINN in dictionarySvodINN)
            {
                if (itemDictionarySvodINN.Value.countINN > 1)
                {
                    foreach (var item in dictionaryUnikRegNomPersoALL)
                    {
                        if (item.Value.inn + item.Value.kpp == itemDictionarySvodINN.Key)
                        {
                            dictionaryDubliINNPerso.Add(item.Value.regNum, item.Value);
                        }
                    }

                    //DataFromPersoDB tmpDataUnikRegNom = new DataFromPersoDB();
                    //if (dictionaryUnikRegNomPersoALL.TryGetValue(itemDictionarySvodINN.Value.regNum, out tmpDataUnikRegNom))
                    //{
                    //    //есть рег.Номер в словаре
                    //    dictionaryDubliINNPerso.Add(itemDictionarySvodINN.Value.regNum, dictionaryUnikRegNomPersoALL[itemDictionarySvodINN.Value.regNum]);
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Внимание! Ошибка наполнения файла \"Дубли_ИНН_реестр\" значениями");
                    //    continue;
                    //}   
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
