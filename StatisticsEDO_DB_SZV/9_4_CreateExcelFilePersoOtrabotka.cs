using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;
//using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

namespace StatisticsEDO_DB_SZV
{
    static class CreateExcelFilePersoOtrabotka
    {
        //коллекция для считанных заголовков таблицы
        //public static List<string> list_table_zagolovki = new List<string>();
        //коллекция для считанных данных таблицы
        //public static List<List<string>> list_table = new List<List<string>>();

        //------------------------------------------------------------------------------------------
        //создаем Excel-файл и наполняем его данными
        public static void CreateNewExcelFile()
        {
            //экземпляр приложения
            Excel.Application excelApp = null;

            //экземпляр рабочей книги Excel
            Excel.Workbook workBook = null;

            //экземпляр листа Excel
            Excel.Worksheet workSheet = null;

            try
            {
                //Создаем экземпляр приложения
                excelApp = new Excel.Application();

                excelApp.ScreenUpdating = false;
                excelApp.EnableEvents = false;
                excelApp.Visible = false;


                //количество листов в рабочей книге
                excelApp.SheetsInNewWorkbook = 1;

                //Создаем экземпляр рабочей книги Excel
                workBook = excelApp.Workbooks.Add();

                //получаем первый лист документа (счет начинается с 1)                
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);

                //название листа (вкладки внизу)
                workSheet.Name = "Perso_Отработка";

                //------------------------------------------------------------------------------------------
                //Заполняем первую строку заголовками

                //for (int i = 1; i <= list_table_zagolovki.Count(); i++)
                //{
                //    workSheet.Cells[1, i] = list_table_zagolovki[i - 1];
                //}

                

                int i = 1;  //номер первой строки для заголовков

                workSheet.Cells[i, 1] = "№ п/п";
                workSheet.Cells[i, 2] = "КодЗап";
                workSheet.Cells[i, 3] = "Район";
                workSheet.Cells[i, 4] = "РегНомер";
                workSheet.Cells[i, 5] = "Наименование";                
                workSheet.Cells[i, 6] = "ОтчГод";
                workSheet.Cells[i, 7] = "Тип формы";
                workSheet.Cells[i, 8] = "Тип ОДВ";
                workSheet.Cells[i, 9] = "Тип СЗВ";
                workSheet.Cells[i, 10] = "Дата представления";
                workSheet.Cells[i, 11] = "Категория";
                workSheet.Cells[i, 12] = "Дата постановки в ПФР";
                workSheet.Cells[i, 13] = "Дата постановки в РО";
                workSheet.Cells[i, 14] = "Дата снятия в РО";
                workSheet.Cells[i, 15] = "Результат проверки";
                workSheet.Cells[i, 16] = "Дата проверки";
                workSheet.Cells[i, 17] = "Количество ЗЛ в файле";
                workSheet.Cells[i, 18] = "Количество ЗЛ (льгота)";
                workSheet.Cells[i, 19] = "ЗЛ принято";
                workSheet.Cells[i, 20] = "ЗЛ не принято";
                workSheet.Cells[i, 21] = "Статус квитанции";
                workSheet.Cells[i, 22] = "Специалист";
                workSheet.Cells[i, 23] = "Способ представления";
                workSheet.Cells[i, 24] = "Куратор";
                workSheet.Cells[i, 25] = "УП по данным УПФР";
                workSheet.Cells[i, 26] = "Дата направления уведомления страхователю";
                workSheet.Cells[i, 27] = "Контрольная дата для исправления (3 дня)";
                workSheet.Cells[i, 28] = "Исправлено (да|нет|не требуется)";
                workSheet.Cells[i, 29] = "Дата направления реестра в УПФР (в случае неисправления)";
                workSheet.Cells[i, 30] = "Дата исправления ошибки (после направления реестра УПФР)";
                workSheet.Cells[i, 31] = "Примечание";
                workSheet.Cells[i, 32] = "Результат контроля (руководитель)";

                //------------------------------------------------------------------------------------------
                //захватываем диапазон ячеек с заголовками
                //TODO: Внимание! При изменении количества ячеек с данными изменить значение диапазона
                Excel.Range rangeZagolovok = workSheet.Range[workSheet.Cells[i, 1], workSheet.Cells[i, 32]];
                //шрифт для захваченного диапазона
                rangeZagolovok.Cells.Font.Name = "Calibri";
                //размер шрифта для захваченного диапазона
                rangeZagolovok.Cells.Font.Size = 12;
                rangeZagolovok.Cells.Font.Bold = "true";
                rangeZagolovok.Cells.Font.Color = ColorTranslator.ToOle(Color.DarkBlue);

                //расставляем рамки со всех сторон
                rangeZagolovok.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeZagolovok.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeZagolovok.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeZagolovok.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeZagolovok.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;

                //устанавливаем цвет рамки
                rangeZagolovok.Borders.Color = ColorTranslator.ToOle(Color.DarkBlue);

                //задаем выравнивание в диапазоне
                rangeZagolovok.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rangeZagolovok.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //перенос текста по словам
                rangeZagolovok.WrapText = true;

                //авто ширина и авто высота
                rangeZagolovok.EntireColumn.AutoFit();
                rangeZagolovok.EntireRow.AutoFit();

                //автофильтр на заголовках                
                rangeZagolovok.AutoFilter(1, System.Reflection.Missing.Value, Excel.XlAutoFilterOperator.xlAnd, System.Reflection.Missing.Value, true);



                //------------------------------------------------------------------------------------------
                //Заполняем строки

                int n = 1;  //номер по порядку
                int j = 2;  //номер первой строки для данных
                foreach (var row in SelectDataFromPersoOtrabotkaFile.dictionaryPersoOtrabotkaOld)
                {                    
                    workSheet.Cells[j, 1] = n;
                    workSheet.Cells[j, 2] = row.Value.codZap;
                    workSheet.Cells[j, 3] = row.Value.raion;
                    workSheet.Cells[j, 4] = row.Value.regNum;
                    workSheet.Cells[j, 5] = row.Value.nameStrah;                    
                    workSheet.Cells[j, 6] = row.Value.year;
                    workSheet.Cells[j, 7] = row.Value.tip;
                    workSheet.Cells[j, 8] = row.Value.tipForm;
                    workSheet.Cells[j, 9] = row.Value.tipSveden;
                    workSheet.Cells[j, 10] = row.Value.dataPredst;
                    workSheet.Cells[j, 11] = row.Value.kategory;
                    workSheet.Cells[j, 12] = row.Value.dataPostPFR;
                    workSheet.Cells[j, 13] = row.Value.dataPostRO;
                    workSheet.Cells[j, 14] = row.Value.dataSnyatRO;
                    workSheet.Cells[j, 15] = row.Value.statVIO;
                    workSheet.Cells[j, 16] = row.Value.dataVIO;
                    workSheet.Cells[j, 17] = row.Value.kolZL;
                    workSheet.Cells[j, 18] = row.Value.kolZlLGT;
                    workSheet.Cells[j, 19] = row.Value.kolZL_PR_VIO;
                    workSheet.Cells[j, 20] = row.Value.kolZL_NPR_VIO;
                    workSheet.Cells[j, 21] = row.Value.statusKvitanc;
                    workSheet.Cells[j, 22] = row.Value.spec;
                    workSheet.Cells[j, 23] = row.Value.specChanged;
                    workSheet.Cells[j, 24] = row.Value.kurator;
                    workSheet.Cells[j, 25] = row.Value.UP;
                    workSheet.Cells[j, 26] = row.Value.dataNaprUvedomlStrah;
                    workSheet.Cells[j, 27] = row.Value.kontrDataIspravleniya;
                    workSheet.Cells[j, 28] = row.Value.statusIspravleniya;
                    workSheet.Cells[j, 29] = row.Value.dataNaprReestraPFR;
                    workSheet.Cells[j, 30] = row.Value.dataIspravleniyaError;
                    workSheet.Cells[j, 31] = row.Value.primechanie;
                    workSheet.Cells[j, 32] = row.Value.resultatKontrolya;

                    //for (int i = 1; i <= row.Value.Count(); i++)
                    //{
                    //    workSheet.Cells[j, i] = row[i - 1];
                    //}

                    n++;
                    j++;
                }


                //------------------------------------------------------------------------------------------
                //заполнение номера по порядку (замена на новые значения)
                //Excel.Range nomPP = workSheet.Range[workSheet.Cells[2, 1], workSheet.Cells[3, 1]];
                //Excel.Range destinationNomPP = workSheet.Range[workSheet.Cells[2, 1], workSheet.Cells[DataFromFile.list_table.Count() + 1, 1]];

                //nomPP.AutoFill(destinationNomPP, Excel.XlAutoFillType.xlFillDefault);


                //------------------------------------------------------------------------------------------
                //захватываем диапазон ячеек с данными
                //TODO: Внимание! При изменении количества ячеек с данными изменить значение диапазона
                Excel.Range rangeData = workSheet.Range[workSheet.Cells[2, 1], workSheet.Cells[SelectDataFromPersoOtrabotkaFile.dictionaryPersoOtrabotkaOld.Count() + 1, 32]];
                //шрифт для захваченного диапазона
                rangeData.Cells.Font.Name = "Calibri";
                //размер шрифта для захваченного диапазона
                rangeData.Cells.Font.Size = 12;

                //расставляем рамки со всех сторон
                rangeData.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeData.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeData.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeData.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeData.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;

                //устанавливаем цвет рамки
                //range2.Borders.Color = ColorTranslator.ToOle(Color.Red);

                //задаем выравнивание в диапазоне
                rangeData.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rangeData.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                

                //авто ширина и авто высота
                //TODO: Внимание! При изменении количества ячеек с данными изменить значение диапазона
                Excel.Range rangeData1 = workSheet.Range[workSheet.Cells[2, 1], workSheet.Cells[SelectDataFromPersoOtrabotkaFile.dictionaryPersoOtrabotkaOld.Count() + 1, 4]];
                rangeData1.EntireColumn.AutoFit();
                rangeData1.EntireRow.AutoFit();
                Excel.Range rangeData2 = workSheet.Range[workSheet.Cells[2, 6], workSheet.Cells[SelectDataFromPersoOtrabotkaFile.dictionaryPersoOtrabotkaOld.Count() + 1, 32]];
                rangeData2.EntireColumn.AutoFit();
                rangeData2.EntireRow.AutoFit();

                //Устанавливаем параметры для поля "Примечание"
                Excel.Range rangePrimechanie = workSheet.Range[workSheet.Cells[2,31], workSheet.Cells[SelectDataFromPersoOtrabotkaFile.dictionaryPersoOtrabotkaOld.Count() + 1, 31]];
                rangePrimechanie.ColumnWidth = 31;
                rangePrimechanie.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                rangePrimechanie.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //перенос текста по словам
                rangePrimechanie.WrapText = true;



                //закрепить область
                workSheet.Activate();
                workSheet.Application.ActiveWindow.FreezePanes = false;
                workSheet.Application.ActiveWindow.SplitRow = 1;
                workSheet.Application.ActiveWindow.SplitColumn = 0;
                workSheet.Application.ActiveWindow.FreezePanes = true; 

                try
                {
                    //Создаем результирующий каталог
                    if (!Directory.Exists(@"C:\_Out"))
                        Directory.CreateDirectory(@"C:\_Out");

                    workBook.Saved = true;

                    //Опасно использовать (не показываем предупреждения от Excel)
                    //excelApp.DisplayAlerts = false;

                    //Лучше использовать проверку наличия файла
                    string fileNameExcel = @"C:\_Out" + @"\" + @"_11_Perso_Отработка_ошибок_СЗВ-СТАЖ_" + DateTime.Now.ToShortDateString() + "_.xlsx";
                    if (File.Exists(fileNameExcel)) { File.Delete(fileNameExcel); }

                    excelApp.DefaultSaveFormat = Excel.XlFileFormat.xlHtml;
                    workBook.SaveAs(fileNameExcel,  //object Filename
                       Type.Missing,          //object FileFormat
                       Type.Missing,                       //object Password 
                       Type.Missing,                       //object WriteResPassword  
                       Type.Missing,                       //object ReadOnlyRecommended
                       Type.Missing,                       //object CreateBackup
                       Excel.XlSaveAsAccessMode.xlNoChange,//XlSaveAsAccessMode AccessMode
                       Type.Missing,                       //object ConflictResolution
                       Type.Missing,                       //object AddToMru 
                       Type.Missing,                       //object TextCodepage
                       Type.Missing,                       //object TextVisualLayout
                       Type.Missing);                      //object Local



                    //Опасно использовать (возвращаем предупреждения от Excel)
                    //excelApp.DisplayAlerts = true;

                    excelApp.Quit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    excelApp.Quit();
                }   
            }
            catch (Exception ex)
            {
                IOoperations.WriteLogError(ex.ToString());
            }
            finally
            {
                //корректно очищаем память от COM-объектов
                if (workSheet != null) Marshal.ReleaseComObject(workSheet);
                if (workBook != null) Marshal.ReleaseComObject(workBook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);

                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }






        }

    }
}
