using EcoWarga.Enums;
using EcoWarga.Exceptions;
using EcoWarga.Helpers;

namespace EcoWarga.Models;

public class PenjemputanRumah : LayananSampah
{
    public const decimal BiayaLayanan = 5000m;
    public const double MinimumBeratKg = 2.0;

    public override string JenisLayanan => "Penjemputan Rumah";

    public PenjemputanRumah(
        string idTransaksi,
        Nasabah nasabah,
        JenisSampah jenisSampah,
        double berat,
        DateTime tanggal,
        StatusLayanan status = StatusLayanan.Diajukan)
        : base(idTransaksi, nasabah, jenisSampah, berat, tanggal, status)
    {
        if (berat < MinimumBeratKg)
        {
            throw new MinimumPenjemputanException(
                $"Penjemputan rumah minimal {MinimumBeratKg:0.#} kg. Berat yang dimasukkan: {berat:0.##} kg."
            );
        }
    }

    public override decimal HitungInsentif()
    {
        decimal nilaiKotor = (decimal)Berat * HargaDasarPerKg;
        return Math.Max(0m, nilaiKotor - BiayaLayanan);
    }

    public override void TampilkanRingkasan()
    {
        base.TampilkanRingkasan();
        Console.WriteLine($"Biaya Layanan: {FormatHelper.Rupiah(BiayaLayanan)}");
    }
}
