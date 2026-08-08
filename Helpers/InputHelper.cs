using EcoWarga.Enums;
using EcoWarga.Exceptions;

namespace EcoWarga.Helpers;

public static class InputHelper
{
    public static string BacaTeksWajib(string label)
    {
        while (true)
        {
            Console.Write(label);
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            TulisError("Input tidak boleh kosong.");
        }
    }

    public static double BacaBeratPositif(string label)
    {
        while (true)
        {
            try
            {
                Console.Write(label);
                string input = (Console.ReadLine() ?? string.Empty).Trim().Replace(',', '.');

                // Sengaja menggunakan Parse agar FormatException benar-benar ditangani dengan try-catch.
                double berat = double.Parse(
                    input,
                    System.Globalization.CultureInfo.InvariantCulture
                );

                if (berat <= 0)
                {
                    throw new BeratTidakValidException("Berat harus lebih dari 0 kg.");
                }

                return berat;
            }
            catch (FormatException)
            {
                TulisError("Format berat tidak valid. Contoh input yang benar: 3 atau 1.5");
            }
            catch (BeratTidakValidException ex)
            {
                TulisError(ex.Message);
            }
        }
    }

    public static JenisSampah PilihJenisSampah()
    {
        while (true)
        {
            Console.WriteLine("\nJenis Sampah:");
            foreach (JenisSampah item in Enum.GetValues<JenisSampah>())
            {
                Console.WriteLine($"{(int)item}. {item} - {FormatHelper.Rupiah(SampahHelper.DapatkanHargaPerKg(item))}/kg");
            }

            Console.Write("Pilih jenis sampah [1-5]: ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (int.TryParse(input, out int pilihan) && Enum.IsDefined(typeof(JenisSampah), pilihan))
            {
                return (JenisSampah)pilihan;
            }

            TulisError("Pilihan jenis sampah tidak valid.");
        }
    }

    public static StatusLayanan? PilihStatusLayanan()
    {
        while (true)
        {
            Console.WriteLine("\nStatus Layanan:");

            foreach (StatusLayanan status in Enum.GetValues<StatusLayanan>())
            {
                Console.WriteLine($"{(int)status}. {status}");
            }

            Console.WriteLine("0. Kembali");

            Console.Write("Pilih status [0-4]: ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (!int.TryParse(input, out int pilihan))
            {
                TulisError("Input harus berupa angka.");
                continue;
            }

            if (pilihan == 0)
            {
                return null;
            }

            if (Enum.IsDefined(typeof(StatusLayanan), pilihan))
            {
                return (StatusLayanan)pilihan;
            }

            TulisError("Pilihan status tidak valid.");
        }
    }
    public static void TulisJudul(string judul)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 72));
        Console.WriteLine(judul.ToUpperInvariant());
        Console.WriteLine(new string('=', 72));
    }

    public static void TulisSukses(string pesan)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(pesan);
        Console.ResetColor();
    }

    public static void TulisError(string pesan)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {pesan}");
        Console.ResetColor();
    }

    public static void Tunggu()
    {
        Console.WriteLine("\nTekan ENTER untuk kembali ke menu...");
        Console.ReadLine();
    }
}
