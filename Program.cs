using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace ConsoleAppNET_Core
{
    class Program
    {
        const string connectionString = "Data Source=192.168.1.21\\SQLEXPRESS;Initial Catalog=testDB1;User Id=sa; Password=ayanami;MultipleActiveResultSets=True;";
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            //nonquery(conn);
            //scalar(conn);
            //reader(conn);
            retrieveFileDataBase(conn, insertFileDataBase(conn));
            conn.Close();

        }
        static void nonquery(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("INSERT_intoTestTable", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("txt", "test2");
            cmd.Parameters.AddWithValue("dec", 5.36);
            Console.WriteLine($"Rows affected: {cmd.ExecuteNonQuery()}");
        }
        static void scalar(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("GET_TestTableByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", 6);
            Console.WriteLine($"Row: {cmd.ExecuteScalar()}");
        }
        static void reader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("GET_AllRecordsTestTable", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine($"Id: {dataReader.GetInt32(0)}, Text: {dataReader.GetString(1)}, Decimal: {dataReader.GetDecimal(2)}");
            }
        }
        static string insertFileDataBase(SqlConnection conn) {
            string filePath = Console.ReadLine();
            var filename = filePath.Split('\\');
            var ext = filename[filename.Length - 1].Split('.');
            var stream = File.Open(filePath, FileMode.Open);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            SqlCommand cmd = new SqlCommand("INSERT_intoTestFileTable", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("file", memoryStream.ToArray());
            cmd.Parameters.AddWithValue("ext", ext[1]);
            return cmd.ExecuteScalar().ToString();
        }
        static void retrieveFileDataBase(SqlConnection conn, string fileId){
            SqlCommand cmd = new SqlCommand("GET_fromTestFileTable", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("fileId", fileId);
            SqlDataReader reader = cmd.ExecuteReader();
            string ext = string.Empty;
            byte[] bt = null;
            while (reader.Read()) {
                bt = (byte[])reader.GetValue(0);
                ext = reader.GetString(1);
            }
            Console.WriteLine($"lenght of the byte array: {bt.Length}");
            Console.WriteLine($"extension:  {ext}");
        }
        
    }
}
