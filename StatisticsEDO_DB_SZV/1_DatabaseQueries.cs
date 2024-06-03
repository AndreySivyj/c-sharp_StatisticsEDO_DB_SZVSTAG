using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsEDO_DB_SZV
{
    static class DatabaseQueries
    {
        //------------------------------------------------------------------------------------------
        //Вставка массива регНомеров во временную таблицу
        public static string CreateInsertQuery()
        {
            //две части текста запроса
            //insert into temp.regnumb(regnumb) values(42001000010),(42001000027);

            string part1 = @"insert into temp.regnumb(regnumb) values";

            //Текст запроса
            string queryPersoSZVM_ISX = part1;

            int tmpCount = Program.reestr350regNumForInsert.Count;

            foreach (var item in Program.reestr350regNumForInsert)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + "(" + item + "), ";
                }
                else
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + "(" + item + ")";
                }
            }            

            return queryPersoSZVM_ISX;
        }


        //------------------------------------------------------------------------------------------
        public static string CreateQueryPersoReestrSZVSTAG(string raionS, string raionPo, string p_date_priem_st, string p_date_priem_fn, string otchYear)
        {
            string queryPersoReestrSZVSTAG =
            @"select " +
            @"a.COD_ZAP, " +                            //0
            @"a.TIP, " +                                //1
            @"a.TIP_FORM, " +                           //2
            @"a.TIP_SVEDEN, " +                         //3
            @"a.GOD, " +                                //4
            @"a.D_PRIEM, " +                            //5
            @"a.DATE_VIO, " +                           //6
            @"a.STATUS_REC, " +                         //7
            @"a.COL_ZL, " +                             //8
            @"a.DATE_INS, " +                           //9
            @"a.TIME_INS, " +                           //10
            @"a.DATE_UVED, " +                          //11
            @"a.DATE_OTP, " +                           //12
            @"regnumb, " +                              //13
            @"u.shname, " +                             //14
            //@"s.region, " +                             //закомментировал
            @"CASE " +                                  //15
            @"WHEN bpi='T' or ks ='T' THEN 'БПИ' " +
            @"WHEN bpi='F' and ks ='F' and cod_uz <> -1 THEN 'Лично' " +
            @"WHEN cod_uz = -1 THEN 'По почте' " +
            @"END as priem, " +
            @"CASE " +                                  //16
            @"WHEN STVIO=0 THEN 'Не подавался' " +
            @"WHEN STVIO=1 THEN 'Не проверен' " +
            @"WHEN STVIO=2 THEN 'Принят' " +
            @"WHEN STVIO=3 THEN 'Не принят' " +
            @"WHEN STVIO=4 THEN 'Принят частично' " +
            @"END as statvio, " +
            @"value(col_vio_pr, 0) as col_pr_vio, " +   //17
            @"value(col_vio_np, 0) as col_npr_vio, " +  //18
            @"value(col_lgot, 0) as col_lgt " +         //19
            @"from PERS.ODV_1 a " +
            @"left join PERS.STRAH s on a.cod_org=s.cod_zap " +
            @"left join PERS.USER u on a.cod_user=u.kod " +
            @"left join (select count(strnum) col_vio_pr, a.cod_zap from PERS.WORKS_ODV w left join PERS.ODV_1 a on w.cod_szv = a.cod_zap where w.stvio=4 group by a.cod_zap) vp on vp.cod_zap = a.cod_zap " +
            @"left join (select count(strnum) col_vio_np, a.cod_zap from PERS.WORKS_ODV w left join PERS.ODV_1 a on w.cod_szv = a.cod_zap where w.stvio<>4 group by a.cod_zap) vn on vn.cod_zap = a.cod_zap " +
            @"left join (select count(strnum) col_lgot, a.cod_zap from PERS.WORKS_ODV w left join PERS.ODV_1 a on w.cod_szv = a.cod_zap " +
            @"left join (select cod_works as lgt from PERS.STAG_ODV where (tu<>'' or ou<>'' or stag in ('СЕЗОН','ПОЛЕ','ВОДОЛАЗ','ЛЕПРО') or visluga<>'') group by cod_works) s on s.lgt=w.cod_zap " +
            @"where s.lgt>0 group by a.cod_zap) lg on lg.cod_zap = a.cod_zap " +
            @"where s.region between " + raionS + @" and " + raionPo + @" and d_priem  between '" + p_date_priem_st + @"' and '" + p_date_priem_fn + @"' " +
            //@"and god=" + otchYear +
            @" order by s.region, regnumb";

            return queryPersoReestrSZVSTAG;
        }

        //------------------------------------------------------------------------------------------
        public static string CreatequeryRKASV()
        {
            //две части текста запроса к РК АСВ             
            string part1 =
                @"select a.insurer_reg_num, a.insurer_reg_start_date, a.insurer_reg_finish_date, a.insurer_short_name, a.insurer_last_name, a.insurer_first_name, a.insurer_middle_name, " +
                @"a.INSURER_REG_DATE_RO, a.INSURER_UNREG_DATE_RO, b.category_code, a.insurer_inn, d.ro_code, e.reg_start_code, r.reg_finish_code, a.insurer_kpp, a.INSURER_STATUS_ID " +
                @"from(select* FROM asv_insurer) a " +
                @"left join(select category_id, category_code from asv_category) b on a.category_id = b.category_id left join (select ro_id, ro_code from asv_ro) d on a.ro_id = d.ro_id " +
                @"left join (select reg_start_id, reg_start_code from asv_reg_start) e on a.reg_start_id = e.reg_start_id " +
                @"left join (select reg_finish_id, reg_finish_code from asv_reg_finish) r on a.reg_finish_id = r.reg_finish_id " +
                @"where a.insurer_reg_num in (";

            string part2 = @") order by a.insurer_reg_num";

            //Текст запроса
            string queryRKASV = part1;

            int tmpCount = Program.persoUnikRegNomForPKASV.Count;

            foreach (var item in Program.persoUnikRegNomForPKASV)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryRKASV = queryRKASV + item + ", ";
                }
                else
                {
                    queryRKASV = queryRKASV + item;
                }
            }

            queryRKASV = queryRKASV + part2;

            return queryRKASV;
        }

        //------------------------------------------------------------------------------------------
        public static string CreateQueryPersoReestrOTMN()
        {
            ////две части текста запроса     
            //string part1 = @"select o.cod_zap, s.regnumb, o.tip, o.tip_form, o.tip_sveden, o.D_PRIEM, o.STVIO, w.strnum, w.STVIO, w.KORPERIOD, w.KORGOD "
            //               + @"from pers.odv_1 o, pers.works_odv w, pers.strah s "
            //               + @"where w.cod_szv = o.cod_zap "
            //               + @"and s.regnumb in (";

            //string part2 = @") and o.cod_org = s.cod_zap and o.tip in ('СЗВ-СТАЖ', 'СЗВ-КОРР') "
            //               + @"and o.tip_sveden in ('ОТМЕНЯЮЩАЯ') "
            //               + @" and w.STVIO in (4,1) "
            //               + @"and w.KORGOD=" + Program.otchYear
            //               + @" order by s.regnumb, o.cod_zap, w.strnum";


            /*
            //две части текста запроса     
            string part1 = @"select distinct s.regnumb, w.strnum, o.DATE_INS, o.TIME_INS, w.REGNUMB "
                           + @"from pers.odv_1 o, pers.works_odv w, pers.stag_odv g, pers.strah s "
                           + @"where w.cod_szv = o.cod_zap "
                           + @"and o.cod_org = s.cod_zap "
                           + @"and w.korgod=" + Program.otchYear
                           + @" and o.tip in ('СЗВ-СТАЖ', 'СЗВ-КОРР') "
                           + @"and o.tip_sveden in ('ОТМЕНЯЮЩАЯ') "
                           + @"and s.regnumb in (";

            string part2 = @") and w.STVIO in (0,4,1) "
                           + @"and o.STATUS_REC <> '' "
                           + @"order by s.regnumb, w.strnum";






            //Текст запроса
            string queryPersoReestrOTMN = part1;

            int tmpCount = Program.persoUnikRegNom_list_SZV_STAG.Count;

            foreach (var item in Program.persoUnikRegNom_list_SZV_STAG)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoReestrOTMN = queryPersoReestrOTMN + item + ", ";
                }
                else
                {
                    queryPersoReestrOTMN = queryPersoReestrOTMN + item;
                }
            }

            queryPersoReestrOTMN = queryPersoReestrOTMN + part2;

            return queryPersoReestrOTMN;
            */




            string queryPersoReestrOTMN = @"select distinct s.regnumb, w.strnum, o.DATE_INS, o.TIME_INS, w.REGNUMB "
                           + @"from pers.odv_1 o, pers.works_odv w, pers.stag_odv g, pers.strah s "
                           + @"where w.cod_szv = o.cod_zap "
                           + @"and o.cod_org = s.cod_zap "
                           + @"and w.korgod=" + Program.otchYear
                           + @" and o.tip in ('СЗВ-СТАЖ', 'СЗВ-КОРР') "
                           + @"and o.tip_sveden in ('ОТМЕНЯЮЩАЯ') "
                           + @"and s.regnumb in (select distinct regnumb from temp.regnumb) "
                            + @"and w.STVIO in (0,4,1) "
                           + @"and o.STATUS_REC <> '' "
                           + @"order by s.regnumb, w.strnum";

            return queryPersoReestrOTMN;

        }

        //------------------------------------------------------------------------------------------
        //СЗВ-СТАЖ с периодами стажа по СНИЛС, все принятые или не проверенные с датой и временем ввода
        public static string CreateQueryPersoReestrISXD()
        {
            string p_godPersoSTAG = Program.otchYear;

            string p_godTMP = Program.otchYear;

            int yearNow = DateTime.Now.Year;

            while (yearNow != Convert.ToInt32(p_godTMP))
            {
                p_godTMP = (Convert.ToInt32(p_godTMP) + 1).ToString();

                p_godPersoSTAG = p_godPersoSTAG + "," + p_godTMP;
            }
            /*
            //две части текста запроса
            string part1 = @"select distinct s.regnumb, w.strnum, g.DATE_BEG, g.DATE_END, o.DATE_INS, o.TIME_INS, w.REGNUMB " +
                @"from pers.odv_1 o, pers.works_odv w, pers.stag_odv g, pers.strah s " +
                @"where " +
                @"w.cod_szv=o.cod_zap " +
                @"and g.cod_works=w.cod_zap " +
                @"and o.cod_org=s.cod_zap " +
                @"and o.god in (" + p_godPersoSTAG + ") " +
                //@"and o.tip in ('СЗВ-СТАЖ','СЗВ-КОРР') " +
                @"and o.tip in ('СЗВ-СТАЖ','СЗВ-КОРР') " +
                //@"and o.tip_sveden in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ','КОРРЕКТИРУЮЩАЯ') " +
                @"and o.tip_sveden in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ','КОРРЕКТИРУЮЩАЯ') " +
                @"and year(g.date_beg)=" + Program.otchYear + " " +
                @"and year(g.date_end)=" + Program.otchYear + " " +
                @"and s.regnumb in (";


            string part2 = @") and w.stvio in (0,4,1) " + //'Не проверен', 'Проверен, принят'
                @"and o.STATUS_REC <> '' " +
                @"order by s.regnumb, w.strnum";           

            //Текст запроса
            string queryPersoSTAG_ISX = part1;

            int tmpCount = Program.persoUnikRegNom_list_SZV_STAG.Count;

            foreach (var item in Program.persoUnikRegNom_list_SZV_STAG)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoSTAG_ISX = queryPersoSTAG_ISX + item + ", ";
                }
                else
                {
                    queryPersoSTAG_ISX = queryPersoSTAG_ISX + item;
                }
            }

            queryPersoSTAG_ISX = queryPersoSTAG_ISX + part2;

            return queryPersoSTAG_ISX;
            */



            string queryPersoSTAG_ISX = @"select distinct s.regnumb, w.strnum, g.DATE_BEG, g.DATE_END, o.DATE_INS, o.TIME_INS, w.REGNUMB " +
                @"from pers.odv_1 o, pers.works_odv w, pers.stag_odv g, pers.strah s " +
                @"where " +
                @"w.cod_szv=o.cod_zap " +
                @"and g.cod_works=w.cod_zap " +
                @"and o.cod_org=s.cod_zap " +
                @"and o.god in (" + p_godPersoSTAG + ") " +
                //@"and o.tip in ('СЗВ-СТАЖ','СЗВ-КОРР') " +
                @"and o.tip in ('СЗВ-СТАЖ','СЗВ-КОРР') " +
                //@"and o.tip_sveden in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ','КОРРЕКТИРУЮЩАЯ') " +
                @"and o.tip_sveden in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ','КОРРЕКТИРУЮЩАЯ') " +
                @"and year(g.date_beg)=" + Program.otchYear + " " +
                @"and year(g.date_end)=" + Program.otchYear + " " +
                @"and s.regnumb in (select distinct regnumb from temp.regnumb) " +
                @"and w.stvio in (0,4,1) " + //'Не проверен', 'Проверен, принят'
                @"and o.STATUS_REC <> '' " +
                @"order by s.regnumb, w.strnum";

            return queryPersoSTAG_ISX;

        }



        //------------------------------------------------------------------------------------------
        //Запрос по СЗВ-М, все принятые или не проверенные с датой и временем ввода        
        public static string CreateQueryPersoSZVM_ISX()
        {
            /*
            //две части текста запроса
            //string part1 = @"select distinct p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS, w.TIP_FORM " +
            string part1 = @"select distinct p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ') " +
            @"and p.regnumb in (";


            string part2 = @") and w.stvio in (0, 1, 4) " + //'Не проверен', 'Проверен, принят'
               @"and s.STATUS_REC <> '' " +
               @"order by p.regnumb, w.strnum, w.god, w.period";

            //Текст запроса
            string queryPersoSZVM_ISX = part1;

            int tmpCount = Program.persoUnikRegNom_list_SZV_STAG.Count;

            foreach (var item in Program.persoUnikRegNom_list_SZV_STAG)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + item + ", ";
                }
                else
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + item;
                }
            }

            queryPersoSZVM_ISX = queryPersoSZVM_ISX + part2;

            return queryPersoSZVM_ISX;
            */

            string queryPersoSZVM_ISX = @"select distinct p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ') " +
            @"and p.regnumb in (select distinct regnumb from temp.regnumb) "+
            @"and w.stvio in (0, 1, 4) " + //'Не проверен', 'Проверен, принят'
            @"and s.STATUS_REC <> '' " +
            @"order by p.regnumb, w.strnum, w.god, w.period";
            
            return queryPersoSZVM_ISX;
        }

        //------------------------------------------------------------------------------------------
        //Запрос по СЗВ-М, все принятые или не проверенные с датой и временем ввода        
        public static string CreateQueryPersoSZVM_OTMN()
        {
            /*
            //две части текста запроса
            string part1 = @"select distinct p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ОТМЕНЯЮЩАЯ') " +
            @"and p.regnumb in (";


            string part2 = @") and w.stvio in (0, 1, 4) " + //'Не проверен', 'Проверен, принят'
               @"and s.STATUS_REC <> '' " +
               @"order by p.regnumb, w.strnum, w.god, w.period";

            //Текст запроса
            string queryPersoSZVM = part1;

            int tmpCount = Program.persoUnikRegNom_list_SZV_STAG.Count;

            foreach (var item in Program.persoUnikRegNom_list_SZV_STAG)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoSZVM = queryPersoSZVM + item + ", ";
                }
                else
                {
                    queryPersoSZVM = queryPersoSZVM + item;
                }
            }

            queryPersoSZVM = queryPersoSZVM + part2;

            return queryPersoSZVM;
            */

            //две части текста запроса
            string queryPersoSZVM = @"select distinct p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ОТМЕНЯЮЩАЯ') " +
            @"and p.regnumb in (select distinct regnumb from temp.regnumb) "+
            @"and w.stvio in (0, 1, 4) " + //'Не проверен', 'Проверен, принят'
            @"and s.STATUS_REC <> '' " +
            @"order by p.regnumb, w.strnum, w.god, w.period";

            return queryPersoSZVM;
        }



















        /*
        //------------------------------------------------------------------------------------------
        //Запрос по СЗВ-М, все принятые или не проверенные с датой и временем ввода        
        public static string CreateQueryPersoReestrSZVM_ISX()
        {
            //две части текста запроса
            string part1 = @"select p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ') " +            
            @"and p.regnumb in (";            


            string part2 = @") and w.stvio in (1, 4)" + //'Не проверен', 'Проверен, принят'
               @" order by p.regnumb, w.strnum, w.god, w.period";

            //Текст запроса
            string queryPersoSZVM_ISX = part1;


            //int tmpCount = Program.persoUnikRegNomForSZVM_STAG.Count;

            //foreach (var item in Program.persoUnikRegNomForSZVM_STAG)


            int tmpCount = Program.persoUnikRegNomForSZVM_STAG.Count;

            foreach (var item in Program.persoUnikRegNomForSZVM_STAG)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + item + ", ";
                }
                else
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + item;
                }
            }

            queryPersoSZVM_ISX = queryPersoSZVM_ISX + part2;

            return queryPersoSZVM_ISX;
        }

        //------------------------------------------------------------------------------------------
        //Запрос по СЗВ-М, все принятые или не проверенные с датой и временем ввода        
        public static string CreateQueryPersoReestrSZVM_OTMN()
        {
            //две части текста запроса
            string part1 = @"select p.regnumb, w.strnum, w.god, w.period, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ОТМЕНЯЮЩАЯ') " +
            @"and p.regnumb in (";


            string part2 = @") and w.stvio in (1, 4)" + //'Не проверен', 'Проверен, принят'
               @" order by p.regnumb, w.strnum, w.god, w.period";

            //Текст запроса
            string queryPersoSZVM_ISX = part1;


            //int tmpCount = Program.persoUnikRegNomForSZVM_STAG.Count;

            //foreach (var item in Program.persoUnikRegNomForSZVM_STAG)



                int tmpCount = Program.persoUnikRegNomForSZVM_STAG.Count;

            foreach (var item in Program.persoUnikRegNomForSZVM_STAG)
            {
                --tmpCount;
                if (tmpCount != 0)
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + item + ", ";
                }
                else
                {
                    queryPersoSZVM_ISX = queryPersoSZVM_ISX + item;
                }
            }

            queryPersoSZVM_ISX = queryPersoSZVM_ISX + part2;

            return queryPersoSZVM_ISX;
        }

        //------------------------------------------------------------------------------------------
        //Запрос по СЗВ-М, все принятые или не проверенные с датой и временем ввода        
        public static string CreateQueryPersoReestrSZVM_ISX_1RegNum(string regNum)
        {
            string query = @"select distinct p.regnumb, w.strnum, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ') " +
            @"and p.regnumb=" + regNum + " " +
            @"and w.STVIO in (4,1) " +
            @"order by p.regnumb, w.strnum";

            return query;
        }

        //------------------------------------------------------------------------------------------
        //Запрос по СЗВ-М, все принятые или не проверенные с датой и временем ввода        
        public static string CreateQueryPersoReestrSZVM_OTMN_1RegNum(string regNum)
        {
            string query = @"select distinct p.regnumb, w.strnum, s.DATE_INS, s.TIME_INS " +
            @"from pers.WORKS_M w, pers.SZV_M s, pers.STRAH p " +
            @"where w.cod_szv = s.cod_zap " +
            @"and s.cod_org = p.cod_zap " +
            @"and s.GOD=" + Program.otchYear + " " +
            @"and w.tip_form in ('ИСХОДНАЯ','ДОПОЛНЯЮЩАЯ') " +
            @"and p.regnumb=" + regNum + " " +
            @"and w.STVIO in (4,1) " +
            @"order by p.regnumb, w.strnum";

            return query;
        }
        */
    }
}
