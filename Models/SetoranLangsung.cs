using EcoWarga.Enums;

namespace EcoWarga.Models;

public class SetoranLangsung : LayananSampah
{
    public override string JenisLayanan => "Setoran Langsung";

    public SetoranLangsung(
        string idTransaksi,
        Nasabah nasabah,
        JenisSampah jenisSampah,
        double berat,
        DateTime tanggal,
        StatusLayanan status = StatusLayanan.Selesai)
        : base(idTransaksi, nasabah, jenisSampah, berat, tanggal, status)
    {
    }

    public override decimal HitungInsentif()
    {
        return (decimal)Berat * HargaDasarPerKg;
    }
}
