using DatabaseWrapper;
using Npgsql;

class User_Model
{
    private string Table = "users";
    private Database Db;

    public User_Model()
    {
        this.Db = new Database();
    }

    public void all()
    {
        this.Db.query($"SELECT * FROM {this.Table}");
        NpgsqlDataReader reader = this.Db.executeR();
        while (reader.Read())
        {
            int no = reader.GetInt32(0);
            string nama = reader.GetString(1);
            string nim = reader.GetString(2);
            string prodi = reader.GetString(3);
            int role_id = reader.GetInt32(4);
            
            if(role_id == 1)
            {
                continue;
            }
            Console.WriteLine($"{no}\t{nama}\t{nim}\t{prodi}");
        }
        reader.Close();
    }

    public void show(string keyword)
    {
        this.Db.query($"SELECT * FROM {this.Table} WHERE Nama ILIKE '%{keyword}%' OR NIM ILIKE '%{keyword}%' OR Prodi ILIKE '%{keyword}%'");
        NpgsqlDataReader reader = this.Db.executeR();
        while (reader.Read())
        {
            int no = reader.GetInt32(0);
            string nama = reader.GetString(1);
            string nim = reader.GetString(2);
            string prodi = reader.GetString(3);
            Console.WriteLine($"{no}\t{nama}\t{nim}\t{prodi}");
        }
        reader.Close();
    }

    public void store(string nama, string nim, string prodi)
    {
        this.Db.query($"INSERT INTO {this.Table}(Nama, NIM, Prodi) VALUES(@Nama, @NIM, @Prodi)");
        this.Db.Query.Parameters.AddWithValue("Nama", nama);
        this.Db.Query.Parameters.AddWithValue("NIM", nim);
        this.Db.Query.Parameters.AddWithValue("Prodi", prodi);
        this.Db.executeCUD();
        Console.WriteLine("Data berhasil ditambahkan");
    }

    public void update(int id, string nama, string nim, string prodi)
    {
        this.Db.query($"UPDATE users SET Nama=@Nama, NIM=@Nim, Prodi=@Prodi WHERE id=@id");
        this.Db.Query.Parameters.AddWithValue("id", id);
        this.Db.Query.Parameters.AddWithValue("Nama", nama);
        this.Db.Query.Parameters.AddWithValue("NIM", nim);
        this.Db.Query.Parameters.AddWithValue("Prodi", prodi);
        this.Db.executeCUD();
        Console.WriteLine("Data berhasil diubah");
    }

    public void destroy(int id)
    {
        this.Db.query($"DELETE FROM {this.Table} WHERE id = @id");
        this.Db.Query.Parameters.AddWithValue("id", id);
        this.Db.executeCUD();
        Console.WriteLine("Data berhasil dihapus");
    }

    public bool login(string nama_login, string nim_login)
    {
        this.Db.query($"SELECT * FROM users WHERE Nama = 'Christianus Yoga Wibisono' AND Role_id = 1");
        NpgsqlDataReader reader = this.Db.executeR();
        reader.Read();
        string nama = reader.GetString(1);
        string nim = reader.GetString(2);
        reader.Close();
        if(nama != nama_login && nim != nim_login)
        {
            return false;
        }
        return true;
    }
    
}
class Program
{
    static void Main(string[] args)
    {
        User_Model program = new User_Model();
        Console.WriteLine("=================================");
        Console.WriteLine("Sistem Pengelolaan Data Mahasiswa");
        Console.WriteLine("=================================");
        while (true)
        {
            Console.Write("Silahkan login terlebih dahulu:\nNama: ");
            string nama = Console.ReadLine();
            Console.Write("Password: ");
            string nim = Console.ReadLine();
            if(!program.login(nama, nim))
            {
                Console.WriteLine("Login gagal");
                break;
            }
            while (true)
            {
                Console.WriteLine("Pilihan menu:\n1. Create Data\n2. Show All Data\n3. Show Data by Keyword\n4. Update Data\n5. Delete Data\n6. Keluar");
                Console.Write("Masukkan menu: ");
                string decision = Console.ReadLine();
                if (decision == "1")
                {
                    Console.WriteLine("===========");
                    Console.WriteLine("Tambah Data");
                    Console.WriteLine("===========");
                    Console.Write("Masukkan nama: ");
                    string nama_baru = Console.ReadLine();
                    Console.Write("Masukkan NIM: ");
                    string nim_baru = Console.ReadLine();
                    Console.Write("Masukkan Prodi: ");
                    string prodi_baru = Console.ReadLine();
                    program.store(nama_baru, nim_baru, prodi_baru);
                    continue;
                }else if(decision == "2"){
                    Console.WriteLine("===========");
                    Console.WriteLine("Semua Data");
                    Console.WriteLine("===========");
                    Console.WriteLine("Nama\nNIM\nProdi");
                    program.all();
                    continue;
                }else if(decision == "3")
                {
                    Console.WriteLine("===========");
                    Console.WriteLine("Data Berdasarkan Keyword");
                    Console.WriteLine("===========");
                    Console.Write("Masukkan keyword: ");
                    string keyword = Console.ReadLine();
                    Console.WriteLine("Nama\nNIM\nProdi");
                    program.show(keyword);
                    continue;
                }else if(decision == "4")
                {
                    Console.WriteLine("===========");
                    Console.WriteLine("Update Data");
                    Console.WriteLine("===========");
                    Console.Write("Masukkan id: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Masukkan nama: ");
                    string nama_update = Console.ReadLine();
                    Console.Write("Masukkan NIM: ");
                    string nim_update = Console.ReadLine();
                    Console.Write("Masukkan Prodi: ");
                    string prodi_update = Console.ReadLine();
                    program.update(id, nama_update, nim_update, prodi_update);
                    continue;
                }
                else if(decision == "5")
                {
                    Console.WriteLine("===========");
                    Console.WriteLine("Update Data");
                    Console.WriteLine("===========");
                    Console.Write("Masukkan id: ");
                    int id_hapus = int.Parse(Console.ReadLine());
                    program.destroy(id_hapus);
                    continue;
                }
                else if(decision == "6")
                {
                    break;
                }
            }
        }
    }
}