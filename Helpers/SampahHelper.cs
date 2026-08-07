using EcoWarga.Enums;

namespace EcoWarga.Helpers;

public static class SampahHelper
{
    public static decimal DapatkanHargaPerKg(JenisSampah jenisSampah)
    {
        return jenisSampah switch
        {
            JenisSampah.Plastik => 3500m,
            JenisSampah.Kertas => 2000m,
            JenisSampah.Logam => 8000m,
            JenisSampah.Kaca => 1500m,
            JenisSampah.Organik => 500m,
            _ => throw new ArgumentOutOfRangeException(nameof(jenisSampah), "Jenis sampah tidak dikenali.")
        };
    }
}
