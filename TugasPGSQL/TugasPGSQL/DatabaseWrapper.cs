using Npgsql;
using System;

namespace DatabaseWrapper
{
    class Database
    {
        // buat property untuk menyimpan informasi database yang dihubungkan. Nilai didapat dari constant di file env. Harus private agar tidak dapat diakses oleh public
        private string Host = "localhost";
        private string Port = "5432";
        private string Db = "Fasilkom";
        private string User = "postgres";
        private string Password = "sehatselalu11";

        private string Command;
        NpgsqlConnection Connection;
        public NpgsqlCommand Query;
        public Database()
        {
            string conn = $"Host={Host};Port={Port};Database={Db};Username={User};Password={Password}";
            this.Connection = new NpgsqlConnection(conn);
            try
            {
                this.Connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // mendapatkan error message dari Exception
            }
        }

        public void query(string command)
        {
            this.Command = command;
            this.Query = new NpgsqlCommand(this.Command, this.Connection);
        }

        public void executeCUD()
        {
            this.Query.Prepare();
            this.Query.ExecuteNonQuery();
            this.Query.Parameters.Clear();
        }

        public NpgsqlDataReader executeR()
        {
            //this.query(this.Command);
            return this.Query.ExecuteReader();
            this.Connection.Dispose();
            this.Connection.Close();
        }
    }
}
