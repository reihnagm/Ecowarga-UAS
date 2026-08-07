using EcoWarga.Enums;
using EcoWarga.Exceptions;
using EcoWarga.Helpers;

namespace EcoWarga.Models;

public abstract class LayananSampah
{
    public string IdTransaksi { get; }
    public Nasabah Nasabah { get; }
    public JenisSampah JenisSampah { get; }
    public double Berat { get; }
    public DateTime Tanggal { get; }
    public StatusLayanan Status { get; private set; }

    public abstract string JenisLayanan { get; }

    protected LayananSampah(
        string idTransaksi,
        Nasabah nasabah,
        JenisSampah jenisSampah,
        double berat,
        DateTime tanggal,
        StatusLayanan status)
    {
        if (string.IsNullOrWhiteSpace(idTransaksi))
        {
            throw new ArgumentException("ID transaksi tidak boleh kosong.");
        }

        if (berat <= 0)
        {
            throw new BeratTidakValidException("Berat harus lebih dari 0 kg.");
        }

        IdTransaksi = idTransaksi.Trim();
        Nasabah = nasabah ?? throw new ArgumentNullException(nameof(nasabah));
        JenisSampah = jenisSampah;
        Berat = berat;
        Tanggal = tanggal;
        Status = status;
    }

    public decimal HargaDasarPerKg => SampahHelper.DapatkanHargaPerKg(JenisSampah);

    public abstract decimal HitungInsentif();

    public int HitungPoin()
    {
        decimal insentif = HitungInsentif();
        return (int)Math.Floor(insentif / 1000m) * 10;
    }

    public void UbahStatus(StatusLayanan statusBaru)
    {
        Status = statusBaru;
    }

    public virtual void TampilkanRingkasan()
    {
        Console.WriteLine($"ID Transaksi : {IdTransaksi}");
        Console.WriteLine($"Jenis Layanan: {JenisLayanan}");
        Console.WriteLine($"Nasabah      : {Nasabah.Id} - {Nasabah.Nama}");
        Console.WriteLine($"Jenis Sampah : {JenisSampah}");
        Console.WriteLine($"Berat        : {Berat:0.##} kg");
        Console.WriteLine($"Harga/kg     : {FormatHelper.Rupiah(HargaDasarPerKg)}");
        Console.WriteLine($"Insentif     : {FormatHelper.Rupiah(HitungInsentif())}");
        Console.WriteLine($"Poin         : {HitungPoin()}");
        Console.WriteLine($"Tanggal      : {Tanggal:dd-MM-yyyy HH:mm}");
        Console.WriteLine($"Status       : {Status}");
    }
}
