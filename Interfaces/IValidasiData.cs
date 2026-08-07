using EcoWarga.Models;

namespace EcoWarga.Interfaces;

public interface IValidasiData
{
    void ValidasiNasabah(Nasabah nasabah);
    void ValidasiTransaksiId(string idTransaksi);
    void ValidasiBerat(double berat);
}
