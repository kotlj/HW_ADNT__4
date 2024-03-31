using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HW_ANDT__4
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["FnVDataBaseConnectionString"].ConnectionString;
            var factory = DbProviderFactories.GetFactory("System.Data.Sqlclient");

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (var command = factory.CreateCommand())
                {
                    try
                    {
                        command.Connection = connection;

                        string choise = "";
                        while (true)
                        {
                            Console.WriteLine("Choose:\n1 - Out all\n2 - Names all\n3 - Colors all\n4 - Max cal\n5 - Min cal\n6 - Avg cal" +
                                "\n7 - Count vegetables\n8 - Count fruits\n9 - Veg and fruits by color\n10 - Count veg and fruits\n" +
                                "\n11 - Cal > num\n12 - Cal < num\n13 - Cal in range\n14 - show like yellow or orange color" +
                                "\n0 - Exit");
                            choise = Console.ReadLine();
                            if (choise == "0")
                            {
                                break;
                            }
                            else if (choise == "1")
                            {
                                command.CommandText = "SELECT * FROM FnV";
                            }
                            else if (choise == "2")
                            {
                                command.CommandText = "SELECT Name FROM FnV";
                            }
                            else if (choise == "3")
                            {
                                command.CommandText = "SELECT Color FROM FnV";
                            }
                            else if (choise == "4")
                            {
                                command.CommandText = "SELECT MAX(Caloricity) FROM FnV";
                            }
                            else if (choise == "5")
                            {
                                command.CommandText = "SELECT MIN(Caloricity) FROM FnV";
                            }
                            else if (choise == "6")
                            {
                                command.CommandText = "SELECT AVG(Caloricity) FROM FnV";
                            }
                            else if (choise == "7")
                            {
                                command.CommandText = "SELECT COUNT(*) FROM FnV\r\nWHERE FnV.Type LIKE 'Vegetable     '";
                            }
                            else if (choise == "8")
                            {
                                command.CommandText = "SELECT COUNT(*) FROM FnV\r\nWHERE FnV.Type LIKE 'Fruit         '";
                            }
                            else if (choise == "9")
                            {
                                Console.WriteLine("Enter color:\n");
                                string color = Console.ReadLine();
                                command.CommandText = $"SELECT COUNT(*) FROM FnV\r\nWHERE FnV.Color LIKE '{color}'";
                            }
                            else if (choise == "10")
                            {
                                command.CommandText = "SELECT Color, COUNT(*) FROM FnV\r\nGROUP BY Color";
                            }
                            else if (choise == "11")
                            {
                                Console.WriteLine("Enter cal top line:\n");
                                string cal = Console.ReadLine();
                                command.CommandText = $"SELECT * FROM FnV\r\nWHERE Caloricity < {cal}";
                            }
                            else if (choise == "12")
                            {
                                Console.WriteLine("Enter cal bottom line:\n");
                                string cal = Console.ReadLine();
                                command.CommandText = $"SELECT * FROM FnV\r\nWHERE Caloricity > {cal}";
                            }
                            else if (choise == "13")
                            {
                                Console.WriteLine("Enter cal bottom line:\n");
                                string cal_bot = Console.ReadLine();
                                Console.WriteLine("Enter cal top line:\n");
                                string cal_top = Console.ReadLine();
                                command.CommandText = $"SELECT * FROM FnV\r\nWHERE Caloricity < {cal_top} AND Caloricity > {cal_bot}";
                            }
                            else if (choise == "14")
                            {
                                command.CommandText = "SELECT * FROM FnV\r\nWHERE Color LIKE 'Yellow' OR Color LIKE 'RED'";
                            }
                            else
                            {
                                Console.WriteLine("ERROR: Invalid option!");
                            }
                            Stopwatch sw;
                            await connection.OpenAsync();
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                sw = Stopwatch.StartNew();
                                while (await reader.ReadAsync())
                                {
                                    Console.WriteLine($"ID: {reader["ID"]}\tName: {reader["Name"]}\tType: " +
                                        $"{reader["Type"]}\tColor: {reader["Color"]}\tCaloricity: {reader["Caloricity"]}");
                                }
                                sw.Stop();
                            }
                            Console.WriteLine($"Time to complite: {sw.Elapsed}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occured: {ex}");
                    }
                }
            }
        }
    }
}
