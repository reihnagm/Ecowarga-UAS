using EcoWarga.Services;

namespace EcoWarga;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "EcoWarga - Sistem Bank Sampah Digital";

        var aplikasi = new EcoWargaManager();
        aplikasi.Jalankan();
    }
}
