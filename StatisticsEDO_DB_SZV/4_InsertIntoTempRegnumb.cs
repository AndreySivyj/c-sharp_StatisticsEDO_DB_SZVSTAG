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
    class InsertIntoTempRegnumb
    {
        //------------------------------------------------------------------------------------------        
        //Выбираем данные из БД Perso
        public static void DeleteTempValueInBD()
        {
            //Подключаемся к БД и выполняем запрос
            using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=PERSDB;UID=regusr;PWD=password;"))
            {
                try
                {
                    //открываем соединение
                    connection.Open();

                    //------------------------------------------------------------------------------------------
                    DB2Command deleteCommand1 = connection.CreateCommand();
                    deleteCommand1.CommandText = "commit";

                    //Устанавливаем значение таймаута
                    deleteCommand1.CommandTimeout = 570;

                    int rowAffected1 = deleteCommand1.ExecuteNonQuery();
                    //Console.WriteLine("deleteCommand1 {0} строк.", rowAffected1);

                    //------------------------------------------------------------------------------------------
                    DB2Command deleteCommand2 = connection.CreateCommand();
                    deleteCommand2.CommandText = "TRUNCATE table TEMP.REGNUMB immediate";

                    //Устанавливаем значение таймаута
                    deleteCommand2.CommandTimeout = 570;

                    int rowAffected2 = deleteCommand2.ExecuteNonQuery();
                    //Console.WriteLine("deleteCommand1 {0} строк.", rowAffected2);

                    //------------------------------------------------------------------------------------------
                    DB2Command deleteCommand3 = connection.CreateCommand();
                    deleteCommand3.CommandText = "commit";

                    //Устанавливаем значение таймаута
                    deleteCommand3.CommandTimeout = 570;

                    int rowAffected3 = deleteCommand3.ExecuteNonQuery();
                    //Console.WriteLine("deleteCommand3 {0} строк.", rowAffected3);

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

        //------------------------------------------------------------------------------------------        
        //Вставляем строки
        async public static void Insert(string query)
        {
            //Подключаемся к БД и выполняем запрос
            using (DB2Connection connection = new DB2Connection("Server=1.1.1.1:50000;Database=PERSDB;UID=regusr;PWD=password;"))
            {
                try
                {
                    //открываем соединение
                    await connection.OpenAsync();

                    DB2Command insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = query;

                    //Устанавливаем значение таймаута
                    insertCommand.CommandTimeout = 570;

                    int rowAffected = await insertCommand.ExecuteNonQueryAsync();   //возвращаем количество вставленных строк
                    //Console.WriteLine("Insert {0} строк.", rowAffected);
                    
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
}
