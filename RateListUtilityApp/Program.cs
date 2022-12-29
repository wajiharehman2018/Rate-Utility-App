using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateListUtilityApp
{
    public class Program
    {
        static string selectedDB;
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Please pass arguments for filepath and database e.g. RateListUtilityApp.exe C:\\MyFolder\\MyFile.csv \"ER\".");
                Console.ReadKey();
                return;
            }
            //else
            //{
            //    foreach ( var a in args)
            //    {
            //        Console.WriteLine(a);
            //    }
            //    Console.ReadKey();
            //}

            string filePath = args[0];
            selectedDB = args[1];
            string conString; 
            switch (selectedDB)
            {
                case "ER":
                    Console.WriteLine("ICER HERE");
                    conString = ConfigurationManager.ConnectionStrings["ICER"].ConnectionString;
                    break;
                case "CC":
                    Console.WriteLine("CCMS HERE");
                    conString = ConfigurationManager.ConnectionStrings["CCMSCash"].ConnectionString;
                    break;
                default:
                    Console.WriteLine("Unidentified Database.");
                    return;
            }
            
            using(SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                Console.WriteLine("Connection Successful");
                AddToDatabase(con, filePath);
            }

            Console.ReadKey();
        }

        public static void AddToDatabase(SqlConnection con, string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line = reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        string[] rowData = line.Split(',');
                        SqlCommand command = new SqlCommand();
                        command.Connection = con;
                        command.CommandType = System.Data.CommandType.Text;
                        switch (selectedDB)
                        {
                            case "ER":
                                command.CommandText = "INSERT INTO ServiceRateList VALUES(" + rowData[0] + "," + rowData[1] + ",'" + rowData[2]
                           + "','" + rowData[3] + "'," + rowData[4] + "," + rowData[5] + ",'" + rowData[6] + "'," + rowData[7] + "," + rowData[8] + ")";
                                break;
                            case "CC":
                                command.CommandText = "INSERT INTO CCMSRateList VALUES(" + rowData[0] + "," + rowData[1] + ",'" + rowData[2]
                           + "','" + rowData[3] + "'," + rowData[4] + "," + rowData[5] + ",'" + rowData[6] + "'," + rowData[7] + "," + rowData[8] + ")";
                                break;
                            default:
                                Console.WriteLine("Unidentified Database.");
                                break;
                        }                       
                        command.ExecuteNonQuery();

                        //Console.WriteLine("0 : "+rowData[0]);
                        //Console.WriteLine("1 : " + rowData[1]);
                        //Console.WriteLine("2 : " + rowData[2]);
                        //Console.WriteLine("3 : " + rowData[3]);
                        //Console.WriteLine("4 : " + rowData[4]);
                        //Console.WriteLine("5 : " + rowData[5]);
                        //Console.WriteLine("6 : " + rowData[6]);
                        //Console.WriteLine("7 : " + rowData[7]);
                        //Console.WriteLine("8 : " + rowData[8]);
                    }
                }
                Console.WriteLine("Successfully Added to "+selectedDB+" Database");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
