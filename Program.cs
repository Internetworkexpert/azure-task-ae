using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace alwaysencrypted
{
    class Program
    {

        static void Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("sqlConnection");
            Console.Write("Enter a secret: ");
            string secret = Console.ReadLine();
            var conn = new SqlConnection(connectionString);
            string SQL = "INSERT INTO dbo.Secret(SafeData,UnsafeData) Values (@secret1,@secret2);";
            var cmdInsert = new SqlCommand(SQL, conn);
            var p1 = cmdInsert.CreateParameter();
            p1.ParameterName = "@secret1";
            p1.DbType = DbType.AnsiString;
            p1.Direction = ParameterDirection.Input;
            p1.Value = secret;
            p1.Size = 200;
            cmdInsert.Parameters.Add(p1);
            var p2 = cmdInsert.CreateParameter();
            p2.ParameterName = "@secret2";
            p2.DbType = DbType.AnsiString;
            p2.Direction = ParameterDirection.Input;
            p2.Value = secret;
            p2.Size = 200;
            cmdInsert.Parameters.Add(p2);

            var SQL2 = "SELECT SafeData FROM dbo.Secret;";
            var cmdSelect = new SqlCommand(SQL2, conn);
            conn.Open();
            try
            {
                cmdInsert.ExecuteNonQuery();
                var rdr = cmdSelect.ExecuteReader();
                Console.WriteLine("Here are your secrets for all to see: ");
                while (rdr.Read())
                {
                    Console.WriteLine(rdr["SafeData"]);
                }
            }
            finally
            {
                conn.Close();
            }

            Console.WriteLine("Done");
        }
    }
}
