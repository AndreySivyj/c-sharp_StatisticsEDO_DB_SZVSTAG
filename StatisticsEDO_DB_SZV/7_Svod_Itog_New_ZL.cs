﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace StatisticsEDO_DB_SZV
{
    class Svod_Itog_New
    {
        //BPI
        public string prinyato_Strah_BPI;
        public string prinyato_ZL_BPI;
        public string partial_prinyato_Strah_BPI;
        public string partial_prinyato_ZL_BPI;
        public string NO_proveren_Strah_BPI;
        public string NO_proveren_ZL_BPI;
        public string NO_prinyato_Strah_BPI;
        public string NO_prinyato_ZL_BPI;
        public int Summ_ZL_BPI
        {
            get
            {
                return Convert.ToInt32(prinyato_ZL_BPI) + Convert.ToInt32(partial_prinyato_ZL_BPI) + Convert.ToInt32(NO_proveren_ZL_BPI) + Convert.ToInt32(NO_prinyato_ZL_BPI);
            }
        }

        //BPI_Centr
        public string prinyato_Strah_BPI_Centr;
        public string prinyato_ZL_BPI_Centr;
        public string partial_prinyato_Strah_BPI_Centr;
        public string partial_prinyato_ZL_BPI_Centr;
        public string NO_proveren_Strah_BPI_Centr;
        public string NO_proveren_ZL_BPI_Centr;
        public string NO_prinyato_Strah_BPI_Centr;
        public string NO_prinyato_ZL_BPI_Centr;

        public int Summ_ZL_BPI_Centr
        {
            get
            {
                return Convert.ToInt32(prinyato_ZL_BPI_Centr) + Convert.ToInt32(partial_prinyato_ZL_BPI_Centr) + Convert.ToInt32(NO_proveren_ZL_BPI_Centr) + Convert.ToInt32(NO_prinyato_ZL_BPI_Centr);
            }
        }

        //Raion
        public string prinyato_Strah_Raion;
        public string prinyato_ZL_Raion;
        public string partial_prinyato_Strah_Raion;
        public string partial_prinyato_ZL_Raion;
        public string NO_proveren_Strah_Raion;
        public string NO_proveren_ZL_Raion;
        public string NO_prinyato_Strah_Raion;
        public string NO_prinyato_ZL_Raion;
        public int Summ_ZL_Raion
        {
            get
            {
                return Convert.ToInt32(prinyato_ZL_Raion) + Convert.ToInt32(partial_prinyato_ZL_Raion) + Convert.ToInt32(NO_proveren_ZL_Raion) + Convert.ToInt32(NO_prinyato_ZL_Raion);
            }
        }



        public Svod_Itog_New(
            string prinyato_Strah_BPI = "", string prinyato_ZL_BPI = "", string partial_prinyato_Strah_BPI = "", string partial_prinyato_ZL_BPI = "", string NO_proveren_Strah_BPI = "", string NO_proveren_ZL_BPI = "", string NO_prinyato_Strah_BPI = "", string NO_prinyato_ZL_BPI = "",
            string prinyato_Strah_BPI_Centr = "", string prinyato_ZL_BPI_Centr = "", string partial_prinyato_Strah_BPI_Centr = "", string partial_prinyato_ZL_BPI_Centr = "", string NO_proveren_Strah_BPI_Centr = "", string NO_proveren_ZL_BPI_Centr = "", string NO_prinyato_Strah_BPI_Centr = "", string NO_prinyato_ZL_BPI_Centr = "",
            string prinyato_Strah_Raion = "", string prinyato_ZL_Raion = "", string partial_prinyato_Strah_Raion = "", string partial_prinyato_ZL_Raion = "", string NO_proveren_Strah_Raion = "", string NO_proveren_ZL_Raion = "", string NO_prinyato_Strah_Raion = "", string NO_prinyato_ZL_Raion = ""
            )
        {
            //BPI
            this.prinyato_Strah_BPI = prinyato_Strah_BPI;
            this.prinyato_ZL_BPI = prinyato_ZL_BPI;
            this.partial_prinyato_Strah_BPI = partial_prinyato_Strah_BPI;
            this.partial_prinyato_ZL_BPI = partial_prinyato_ZL_BPI;
            this.NO_proveren_Strah_BPI = NO_proveren_Strah_BPI;
            this.NO_proveren_ZL_BPI = NO_proveren_ZL_BPI;
            this.NO_prinyato_Strah_BPI = NO_prinyato_Strah_BPI;
            this.NO_prinyato_ZL_BPI = NO_prinyato_ZL_BPI;

            //BPI_Centr
            this.prinyato_Strah_BPI_Centr = prinyato_Strah_BPI_Centr;
            this.prinyato_ZL_BPI_Centr = prinyato_ZL_BPI_Centr;
            this.partial_prinyato_Strah_BPI_Centr = partial_prinyato_Strah_BPI_Centr;
            this.partial_prinyato_ZL_BPI_Centr = partial_prinyato_ZL_BPI_Centr;
            this.NO_proveren_Strah_BPI_Centr = NO_proveren_Strah_BPI_Centr;
            this.NO_proveren_ZL_BPI_Centr = NO_proveren_ZL_BPI_Centr;
            this.NO_prinyato_Strah_BPI_Centr = NO_prinyato_Strah_BPI_Centr;
            this.NO_prinyato_ZL_BPI_Centr = NO_prinyato_ZL_BPI_Centr;

            //Raion
            this.prinyato_Strah_Raion = prinyato_Strah_Raion;
            this.prinyato_ZL_Raion = prinyato_ZL_Raion;
            this.partial_prinyato_Strah_Raion = partial_prinyato_Strah_Raion;
            this.partial_prinyato_ZL_Raion = partial_prinyato_ZL_Raion;
            this.NO_proveren_Strah_Raion = NO_proveren_Strah_Raion;
            this.NO_proveren_ZL_Raion = NO_proveren_ZL_Raion;
            this.NO_prinyato_Strah_Raion = NO_prinyato_Strah_Raion;
            this.NO_prinyato_ZL_Raion = NO_prinyato_ZL_Raion;
        }

        //public override string ToString()
        //{
        //    return prinyato_Strah_BPI + ";" + prinyato_ZL_BPI + ";" + partial_prinyato_Strah_BPI + ";" + partial_prinyato_ZL_BPI + ";" + NO_proveren_Strah_BPI + ";" + NO_proveren_ZL_BPI + ";" + NO_prinyato_Strah_BPI + ";" + NO_prinyato_ZL_BPI + ";"
        //         + prinyato_Strah_BPI_Centr + ";" + prinyato_ZL_BPI_Centr + ";" + partial_prinyato_Strah_BPI_Centr + ";" + partial_prinyato_ZL_BPI_Centr + ";" + NO_proveren_Strah_BPI_Centr + ";" + NO_proveren_ZL_BPI_Centr + ";" + NO_prinyato_Strah_BPI_Centr + ";" + NO_prinyato_ZL_BPI_Centr + ";"
        //         + prinyato_Strah_Raion + ";" + prinyato_ZL_Raion + ";" + partial_prinyato_Strah_Raion + ";" + partial_prinyato_ZL_Raion + ";" + NO_proveren_Strah_Raion + ";" + NO_proveren_ZL_Raion + ";" + NO_prinyato_Strah_Raion + ";" + NO_prinyato_ZL_Raion + ";";
        //}

        public override string ToString()
        {
            return prinyato_Strah_BPI + ";" + partial_prinyato_Strah_BPI + ";" + NO_proveren_Strah_BPI + ";" + NO_prinyato_Strah_BPI + ";" + Summ_ZL_BPI + ";"
                 + prinyato_Strah_BPI_Centr + ";" + partial_prinyato_Strah_BPI_Centr + ";" + NO_proveren_Strah_BPI_Centr + ";" + NO_prinyato_Strah_BPI_Centr + ";" + Summ_ZL_BPI_Centr + ";"
                 + prinyato_Strah_Raion + ";" + partial_prinyato_Strah_Raion + ";" + NO_proveren_Strah_Raion + ";" + NO_prinyato_Strah_Raion + ";" + Summ_ZL_Raion + ";";
        }
    }

    static class CreateSvod_Itog_New
    {
        public static Dictionary<string, Svod_Itog_New> dictionarySvod_Itog = new Dictionary<string, Svod_Itog_New>();    //Коллекция сводных данных из реестра Perso (TKey - Raion)

        public static void CreateSvod(Dictionary<string, DataFromPersoDB> dictionaryUnikRegNomPersoALL)
        {

            try
            {
                //Очищаем коллекции
                dictionarySvod_Itog.Clear();



                //Перебираем все значения по порядку для формирования сводной информации
                foreach (var itemUnikRegNomPersoALL in SelectDataForResultFile.dictionaryUnikRegNomPersoALL)
                {
                    if (itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG == "0" && itemUnikRegNomPersoALL.Value.otmnFormAvailability == "Да")
                    {
                        continue;
                    }
                    else
                    {
                        //------------------------------------------------------------------------------------------
                        // "ПК БПИ" + "Принят"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Принят" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_Strah_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_Strah_BPI) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_ZL_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_ZL_BPI) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0", "0", "0", "0", "0",
                                                        "0", "0", "0", "0", "0", "0", "0", "0",
                                                        "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }

                        // "ПК БПИ" + "Принят частично"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Принят частично" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре                        
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_Strah_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_Strah_BPI) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_ZL_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_ZL_BPI) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0"));

                            }
                        }

                        // "ПК БПИ" + "Не проверен"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Не проверен" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_Strah_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_Strah_BPI) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_ZL_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_ZL_BPI) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }

                        // "ПК БПИ" + "Не принят"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Не принят" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_Strah_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_Strah_BPI) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_ZL_BPI = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_ZL_BPI) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG,
                                                       "0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }



                        //------------------------------------------------------------------------------------------
                        // "ПК БПИ_Центр" + "Принят"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Принят" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ_Центр")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_Strah_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_Strah_BPI_Centr) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_ZL_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_ZL_BPI_Centr) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                        "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0", "0", "0", "0", "0",
                                                        "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }

                        // "ПК БПИ_Центр" + "Принят частично"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Принят частично" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ_Центр")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре                        
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_Strah_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_Strah_BPI_Centr) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_ZL_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_ZL_BPI_Centr) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                        "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0", "0", "0",
                                                        "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }

                        // "ПК БПИ_Центр" + "Не проверен"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Не проверен" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ_Центр")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_Strah_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_Strah_BPI_Centr) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_ZL_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_ZL_BPI_Centr) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }

                        // "ПК БПИ_Центр" + "Не принят"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Не принят" && itemUnikRegNomPersoALL.Value.specChanged == "ПК БПИ_Центр")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_Strah_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_Strah_BPI_Centr) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_ZL_BPI_Centr = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_ZL_BPI_Centr) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG,
                                                       "0", "0", "0", "0", "0", "0", "0", "0"));
                            }
                        }



                        //------------------------------------------------------------------------------------------
                        // "Специалист" + "Принят"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Принят" && itemUnikRegNomPersoALL.Value.specChanged == "Специалист")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_Strah_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_Strah_Raion) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_ZL_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].prinyato_ZL_Raion) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0",
                                                       "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0", "0", "0", "0", "0"));
                            }
                        }

                        // "Специалист" + "Принят частично"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Принят частично" && itemUnikRegNomPersoALL.Value.specChanged == "Специалист")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре                        
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_Strah_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_Strah_Raion) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_ZL_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].partial_prinyato_ZL_Raion) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "0", "0", "0", "0", "0", "0",
                                                       "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0", "0", "0"));
                            }
                        }

                        // "Специалист" + "Не проверен"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Не проверен" && itemUnikRegNomPersoALL.Value.specChanged == "Специалист")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_Strah_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_Strah_Raion) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_ZL_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_proveren_ZL_Raion) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                      "0", "0", "0", "0", "0", "0", "0", "0",
                                                      "0", "0", "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG, "0", "0"));
                            }
                        }

                        // "Специалист" + "Не принят"
                        if (itemUnikRegNomPersoALL.Value.statVIO == "Не принят" && itemUnikRegNomPersoALL.Value.specChanged == "Специалист")
                        {
                            //Добавляем выбранные данные в коллекцию
                            Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                            if (dictionarySvod_Itog.TryGetValue(itemUnikRegNomPersoALL.Value.raion, out tmpDataFrom_Svod_Itog))
                            {
                                //есть район в словаре
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_Strah_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_Strah_Raion) + 1).ToString();
                                dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_ZL_Raion = (Convert.ToInt32(dictionarySvod_Itog[itemUnikRegNomPersoALL.Value.raion].NO_prinyato_ZL_Raion) + Convert.ToInt32(itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG)).ToString();
                            }
                            else
                            {
                                //нет района в словаре
                                dictionarySvod_Itog.Add(itemUnikRegNomPersoALL.Value.raion,
                                    new Svod_Itog_New("0", "0", "0", "0", "0", "0", "0", "0",
                                                      "0", "0", "0", "0", "0", "0", "0", "0",
                                                      "0", "0", "0", "0", "0", "0", "1", itemUnikRegNomPersoALL.Value.uniqZlSZVSTAG));
                            }
                        }
                    }
                }

                //Формируем файл статистики "Perso_СтатусКвитанции_Error_"
                
                string zagolovok_Svod_Itog =
                    "БПИ_принятоСтрах" + ";" + "БПИ_частично_принятоСтрах" + ";" + "БПИ_Не_проверенСтрах" + ";" + "БПИ_Не_принятоСтрах" + ";" + "БПИ_КолЗЛ" + ";"

                    + "БПИ_Центр_принятоСтрах" + ";" + "БПИ_Центр_частично_принятоСтрах" + ";" + "БПИ_Центр_Не_проверенСтрах" + ";" + "БПИ_Центр_Не_принятоСтрах" + ";" + "БПИ_Центр_КолЗЛ" + ";"

                    + "Район_принятоСтрах" + ";" + "Район_частично_принятоСтрах" + ";" + "Район_Не_проверенСтрах" + ";" + "Район_Не_принятоСтрах" + ";" + "Район_КолЗЛ" + ";";

                string resultFile_Svod_Itog = IOoperations.katalogOut + @"\" + @"_3_СЗВ-СТАЖ_УникРегНомера_Свод_" + DateTime.Now.ToShortDateString() + ".csv";

                WriteLogsSvod_Itog_New(resultFile_Svod_Itog, zagolovok_Svod_Itog, dictionarySvod_Itog);
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

        private static string ConvertRaion(string raion)
        {
            try
            {
                if (raion.Count() == 1)
                {
                    return "042-00" + raion;
                }
                else
                {
                    return "042-0" + raion;
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

        private static void WriteLogsSvod_Itog_New(string resultFile_Svod_Itog, string zagolovok_Svod_Itog, Dictionary<string, Svod_Itog_New> dictionarySvod_Itog)
        {
            //Проверяем на наличие файла отчета, если существует - удаляем
            if (File.Exists(resultFile_Svod_Itog))
            {
                IOoperations.FileDelete(resultFile_Svod_Itog);
            }

            //формируем результирующий файл статистики
            using (StreamWriter writer = new StreamWriter(resultFile_Svod_Itog, false, Encoding.GetEncoding(1251)))
            {
                writer.WriteLine(zagolovok_Svod_Itog);

                for (int raion = 1; raion < 35; raion++)
                {
                    Svod_Itog_New tmpDataFrom_Svod_Itog = new Svod_Itog_New();
                    if (dictionarySvod_Itog.TryGetValue(ConvertRaion(raion.ToString()), out tmpDataFrom_Svod_Itog))
                    {
                        //есть район в словаре
                        writer.WriteLine(dictionarySvod_Itog[ConvertRaion(raion.ToString())].ToString());
                    }
                    else
                    {
                        //нет района в словаре
                        writer.WriteLine("0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" 
                                        + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" 
                                        + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" );
                    }
                }
            }
        }

    }
}
