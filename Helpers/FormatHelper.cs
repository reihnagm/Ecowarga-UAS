using System.Globalization;

namespace EcoWarga.Helpers;

public static class FormatHelper
{
    private static readonly CultureInfo Indonesia = new("id-ID");

    public static string Rupiah(decimal nilai)
    {
        return nilai.ToString("C0", Indonesia);
    }
}
