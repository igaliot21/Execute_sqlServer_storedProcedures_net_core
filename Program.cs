using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleAppNET_Core
{
    class Program
    {
        const string connectionString = "Data Source=192.168.1.21\\SQLEXPRESS;Initial Catalog=testDB1;User Id=sa; Password=ayanami;";
        static void Main(string[] args)
        {
            //nonquery();
            //scalar();
            reader();
        }
        static void nonquery() {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT_intoTestTable", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("txt","test2");
            cmd.Parameters.AddWithValue("dec",5.36);
            conn.Open();
            Console.WriteLine($"Rows affected: {cmd.ExecuteNonQuery()}");
            conn.Close();
        }
        static void scalar() {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GET_TestTableByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", 6);
            conn.Open();
            Console.WriteLine($"Row: {cmd.ExecuteScalar()}");
            conn.Close();
        }
        static void reader()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GET_AllRecordsTestTable", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
                Console.WriteLine($"Id: {dataReader.GetInt32(0)}, Text: {dataReader.GetString(1)}, Decimal: {dataReader.GetDecimal(2)}");
            }
            conn.Close();
        }
    }
}
