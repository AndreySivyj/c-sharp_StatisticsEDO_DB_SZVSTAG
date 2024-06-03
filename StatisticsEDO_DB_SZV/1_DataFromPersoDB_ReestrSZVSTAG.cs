using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsEDO_DB_SZV
{
    public class DataFromPersoDB
    {
        public string codZap;       //0
        public string regNum;       //54
        public string tip;          //2
        public string tipForm;      //4
        public string tipSveden;    //5   
        public string year;         //6
        public string priem;        //57
        public string specChanged;
        public string dataPriem;    //8
        public string statVIO;      //58
        public string dataVIO;      //40
        public string statusRec;    //41
        public string kolZL;        //10
        public string kolZlLGT;     //61
        public string kolZl_PR_VIO; //59
        public string kolZl_NPR_VIO;//60
        public string shname;       //55
        public string dateINS;      //45
        public string timeINS;      //46        
        public string dataUved;     //48
        public string dataOtp;      //50

        public string otmnFormAvailability;
        public string nameStrah;
        public string kategory;
        public string inn;
        public string kpp;
        public string dataPostPFR;
        public string dataSnyatPFR;
        public string dataPostRO;
        public string dataSnyatRO;
        public string UP;
        public string curator;
        public string uniqZlSZVM;
        public string uniqZlSZVSTAG;
        public string raion;
        public string status_id;

        public DataFromPersoDB(string codZap = "", string raion = "", string regNum = "", string tip = "", string tipForm = "", string tipSveden = "", string year = "", string priem = "", string specChanged = "",
                            string dataPriem = "", string statVIO = "", string dataVIO = "", string statusRec = "", string kolZL = "", string kolZlLGT = "",
                            string kolZl_PR_VIO = "", string kolZl_NPR_VIO = "", string shname = "", string dateINS = "", string timeINS = "", string dataUved = "", string dataOtp = "",
                            string otmnFormAvailability = "", string nameStrah = "", string kategory = "", string inn = "", string dataPostPFR = "", string dataSnyatPFR = "", string dataPostRO = "", string dataSnyatRO = "",
                            string UP = "", string curator = "", string uniqZlSZVM = "0", string uniqZlSZVSTAG = "0", string kpp = "", string status_id = "")
        {
            this.codZap = codZap;
            this.regNum = regNum;

            this.tip = tip;
            this.tipForm = tipForm;
            this.tipSveden = tipSveden;
            this.year = year;
            this.priem = priem;



            //TODO: реализовать чтение из файла
            if (shname == "ПК БПИ")
            {
                this.specChanged = "ПК БПИ";
            }
            else if (shname == "Карасева Е.А." || shname == "Коваленко А.В." || shname == "Kovaleva N.A." || shname == "Сивый А.А." ||
                shname == "Верткова Т.Н." || shname == "Петракова Е.Н." || shname == "Бельская И.А." || shname == "Andronova I.V." ||
                shname == "Нестерова Е. В." || shname == "Кузьмина О. А." || shname == "Фролова С.Ф." || shname == "Конопелько М.А." ||
                shname == "Ананенко И. А." || shname == "Антощенко Ю.А." || shname == "Nikitina O.B.")
            {
                this.specChanged = "ПК БПИ_Центр";
            }
            else
            {
                this.specChanged = "Специалист";
            }



            this.dataPriem = dataPriem;
            this.statVIO = statVIO;
            this.dataVIO = dataVIO;
            this.statusRec = statusRec;
            this.kolZL = kolZL;
            this.kolZlLGT = kolZlLGT;
            this.kolZl_PR_VIO = kolZl_PR_VIO;
            this.kolZl_NPR_VIO = kolZl_NPR_VIO;
            this.shname = shname;
            this.dateINS = dateINS;
            this.timeINS = timeINS;
            this.dataUved = dataUved;
            this.dataOtp = dataOtp; //меняем ниже (+3 рабочих дня от dataUved)

            this.otmnFormAvailability = otmnFormAvailability;
            this.nameStrah = nameStrah;
            this.kategory = kategory;
            this.inn = inn;
            this.dataPostPFR = dataPostPFR;
            this.dataSnyatPFR = dataSnyatPFR;
            this.dataPostRO = dataPostRO;
            this.dataSnyatRO = dataSnyatRO;
            this.UP = UP;
            this.curator = curator;
            this.uniqZlSZVM = uniqZlSZVM;
            this.uniqZlSZVSTAG = uniqZlSZVSTAG;
            this.raion = raion;
            this.kpp = kpp;
            this.status_id = status_id;


            if (this.dataUved != "")
            {
                DateTime date = Convert.ToDateTime(dataUved);

                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    date = date.AddDays(4);
                    this.dataOtp = date.ToShortDateString();
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(3);
                    this.dataOtp = date.ToShortDateString();
                }
                else if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    date = date.AddDays(2);
                    this.dataOtp = date.ToShortDateString();
                }
                else if (date.DayOfWeek == DayOfWeek.Tuesday)
                {
                    date = date.AddDays(2);
                    this.dataOtp = date.ToShortDateString();
                }
                else if (date.DayOfWeek == DayOfWeek.Wednesday)
                {
                    date = date.AddDays(2);
                    this.dataOtp = date.ToShortDateString();
                }
                else if (date.DayOfWeek == DayOfWeek.Thursday)
                {
                    date = date.AddDays(4);
                    this.dataOtp = date.ToShortDateString();
                }
                else if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    date = date.AddDays(4);
                    this.dataOtp = date.ToShortDateString();
                }
                else
                {
                    this.dataOtp = "";
                }
            }
            else
            {
                this.dataOtp = "";
            }
        }


        //private string SverkaSummZL()
        //{
        //    return (Convert.ToInt32(this.kolZl_PR_VIO) - Convert.ToInt32(this.toutNewCountZL)).ToString();
        //}

        /*
        public static string zagolovokPersoSZVSTAG = "№ п/п" + ";" + "КодЗап" + ";" + "Район" + ";" + "РегНомер" + ";" + "Наименование" + ";" + "Тип формы" + ";" + "Тип ОДВ" + ";" + "Тип СЗВ" + ";"
                                                   + "ОтчГод" + ";" + "Специалист" + ";" + "Способ представления" + ";" + "Дата представления" + ";" + "Результат проверки" + ";" + "Дата проверки" + ";" + "Статус квитанции" + ";"
                                                   + "Количество ЗЛ в файле" + ";" + "Количество ЗЛ льготников" + ";" + "Количество ЗЛ принято" + ";" + "Количество ЗЛ не принято" + ";"
                                                   + "Дата записи в БД" + ";" + "Время записи в БД" + ";" + "Дата уведомления" + ";" + "Дата контроля" + ";" + "Признак наличия ОТМН форм" + ";"
                                                   + "Категория" + ";" + "ИНН" + ";" + "Дата постановки в ПФР" + ";" + "Дата снятия в ПФР" + ";" + "Дата постановки в РО" + ";" + "Дата снятия в РО" + ";"
                                                   + "УП по данным УПФР" + ";" + "Куратор" + ";"
                                                   + "Количество ЗЛ (уникальных СНИЛС) в представленной за год отчетности по форме СЗВ-М" + ";"
                                                   + "Количество ЗЛ (уникальных СНИЛС) в представленной за год отчетности по форме СЗВ-СТАЖ" + ";";

        */

        public override string ToString()
        {
            return codZap + ";" + raion + ";" + regNum + ";" + nameStrah + ";" + tip + ";" + tipForm + ";" + tipSveden + ";" + year + ";" + shname + ";" + specChanged + ";" + dataPriem + ";" + statVIO + ";" + dataVIO + ";"
                        + statusRec + ";" + kolZL + ";" + kolZlLGT + ";" + kolZl_PR_VIO + ";" + kolZl_NPR_VIO + ";" + dateINS + ";" + timeINS + ";"
                        + dataUved + ";" + dataOtp + ";" + otmnFormAvailability + ";" + kategory + ";" + inn + ";" + kpp + ";" + dataPostPFR + ";" + dataSnyatPFR + ";" + dataPostRO + ";" + dataSnyatRO + ";"
                        + UP + ";" + curator + ";" + uniqZlSZVM + ";" + uniqZlSZVSTAG + ";" + status_id + ";" + zlDifference();
        }

        private string zlDifference()
        {
            if (uniqZlSZVM != "" && uniqZlSZVSTAG != "")
            {
                return (Convert.ToInt32(uniqZlSZVM) - Convert.ToInt32(uniqZlSZVSTAG)).ToString();
            }
            else if (uniqZlSZVM != "" && uniqZlSZVSTAG == "")
            {
                return uniqZlSZVM;
            }
            else if (uniqZlSZVSTAG != "" && uniqZlSZVM == "")
            {
                return "-" + uniqZlSZVSTAG;
            }
            else
            {
                return "";
            }
        }
    }
}
