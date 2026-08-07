namespace EcoWarga.Models;

public class Nasabah
{
    private string _id;
    private string _nama;
    private string _alamat;

    public string Id
    {
        get => _id;
        private set => _id = ValidasiTeks(value, "ID nasabah");
    }

    public string Nama
    {
        get => _nama;
        private set => _nama = ValidasiTeks(value, "Nama nasabah");
    }

    public string Alamat
    {
        get => _alamat;
        private set => _alamat = ValidasiTeks(value, "Alamat nasabah");
    }

    public Nasabah(string id, string nama, string alamat)
    {
        _id = string.Empty;
        _nama = string.Empty;
        _alamat = string.Empty;

        Id = id;
        Nama = nama;
        Alamat = alamat;
    }

    private static string ValidasiTeks(string value, string namaField)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{namaField} tidak boleh kosong.");
        }

        return value.Trim();
    }

    public override string ToString()
    {
        return $"{Id} | {Nama} | {Alamat}";
    }
}
