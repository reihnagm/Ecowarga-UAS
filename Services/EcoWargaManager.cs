using System.Globalization;
using EcoWarga.Enums;
using EcoWarga.Exceptions;
using EcoWarga.Helpers;
using EcoWarga.Interfaces;
using EcoWarga.Models;

namespace EcoWarga.Services;

public class EcoWargaManager : IValidasiData, IPersistensiData, ILaporan
{
    private readonly List<Nasabah> _nasabah = new();
    private readonly List<LayananSampah> _layanan = new();

    private readonly string _dataFolder;
    private readonly string _nasabahFile;
    private readonly string _transaksiFile;
    private readonly Logger _logger;

    public EcoWargaManager()
    {
        // Mengambil lokasi folder tempat project dijalankan.
        string projectFolder = Directory.GetCurrentDirectory();

        // Menentukan folder penyimpanan data.
        _dataFolder = Path.Combine(projectFolder, "data");

        // Menentukan lokasi file nasabah.
        _nasabahFile = Path.Combine(
            _dataFolder,
            "data_nasabah.txt"
        );

        // Menentukan lokasi file transaksi.
        _transaksiFile = Path.Combine(
            _dataFolder,
            "data_transaksi.txt"
        );

        // Menentukan lokasi file log aplikasi.
        _logger = new Logger(
            Path.Combine(
                _dataFolder,
                "log_aplikasi.txt"
            )
        );

        // Membuat folder data jika belum tersedia.
        Directory.CreateDirectory(_dataFolder);
    }


    public void Jalankan()
    {
        MuatDataSaatStartup();
        bool berjalan = true;

        while (berjalan)
        {
            try
            {
                TampilkanMenu();
                Console.Write("Pilih menu: ");
                string pilihan = (Console.ReadLine() ?? string.Empty).Trim();

                Console.Clear();

                switch (pilihan)
                {
                    case "1":
                        TambahNasabah();
                        break;
                    case "2":
                        CatatSetoranLangsung();
                        break;
                    case "3":
                        CatatPenjemputanRumah();
                        break;
                    case "4":
                        TampilkanSemuaLayanan();
                        break;
                    case "5":
                        CariTransaksi();
                        break;
                    case "6":
                        UbahStatusLayanan();
                        break;
                    case "7":
                        SimpanData();
                        break;
                    case "8":
                        MuatData();
                        break;
                    case "9":
                        TampilkanLaporanRingkas();
                        break;
                    case "10":
                        TampilkanNasabah();
                        break;
                    case "0":
                        SimpanData();
                        berjalan = false;
                        _logger.Tulis("INFO", "Aplikasi ditutup dengan aman.");
                        Console.WriteLine("Data telah disimpan. Sampai jumpa!");
                        break;
                    default:
                        InputHelper.TulisError("Menu tidak tersedia.");
                        break;
                }
            }
            catch (Exception ex)
            {
                InputHelper.TulisError($"Terjadi kesalahan tak terduga: {ex.Message}");
                _logger.Tulis("ERROR", $"Unhandled: {ex}");
            }

            if (berjalan)
            {
                InputHelper.Tunggu();
                Console.Clear();
            }
        }
    }

    private void TampilkanMenu()
    {
        Console.WriteLine(new string('=', 72));
        Console.WriteLine("        ECOWARGA - SISTEM BANK SAMPAH DIGITAL");
        Console.WriteLine(new string('=', 72));
        Console.WriteLine("1.  Tambah data nasabah");
        Console.WriteLine("2.  Catat setoran langsung");
        Console.WriteLine("3.  Catat penjemputan rumah");
        Console.WriteLine("4.  Tampilkan seluruh layanan");
        Console.WriteLine("5.  Cari transaksi (ID transaksi / ID nasabah)");
        Console.WriteLine("6.  Ubah status layanan");
        Console.WriteLine("7.  Simpan data ke file");
        Console.WriteLine("8.  Baca / muat kembali data dari file");
        Console.WriteLine("9.  Tampilkan laporan ringkas");
        Console.WriteLine("10. Tampilkan data nasabah");
        Console.WriteLine("0.  Keluar");
        Console.WriteLine(new string('-', 72));
        Console.WriteLine($"Nasabah: {_nasabah.Count} | Transaksi: {_layanan.Count}");
        Console.WriteLine(new string('-', 72));
    }

    private void TambahNasabah()
    {
        InputHelper.TulisJudul("Tambah Data Nasabah");

        string id = InputHelper.BacaTeksWajib("ID Nasabah : ");

        if (_nasabah.Any(n => n.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
        {
            InputHelper.TulisError("ID nasabah sudah digunakan.");
            return;
        }

        string nama = InputHelper.BacaTeksWajib("Nama       : ");
        string alamat = InputHelper.BacaTeksWajib("Alamat     : ");

        try
        {
            var nasabah = new Nasabah(id, nama, alamat);
            ValidasiNasabah(nasabah);
            _nasabah.Add(nasabah);

            SimpanData();
            _logger.Tulis("INFO", $"Nasabah ditambahkan: {nasabah.Id} - {nasabah.Nama}");
            InputHelper.TulisSukses("Nasabah berhasil ditambahkan.");
        }
        catch (ArgumentException ex)
        {
            InputHelper.TulisError(ex.Message);
            _logger.Tulis("ERROR", $"Gagal tambah nasabah: {ex.Message}");
        }
    }

    private void CatatSetoranLangsung()
    {
        InputHelper.TulisJudul("Catat Setoran Langsung");

        Nasabah? nasabah = PilihNasabah();
        if (nasabah is null)
        {
            return;
        }

        string idTransaksi = InputHelper.BacaTeksWajib("ID Transaksi : ");

        try
        {
            ValidasiTransaksiId(idTransaksi);
            JenisSampah jenis = InputHelper.PilihJenisSampah();
            double berat = InputHelper.BacaBeratPositif("Berat (kg)   : ");
            ValidasiBerat(berat);

            LayananSampah transaksi = new SetoranLangsung(
                idTransaksi,
                nasabah,
                jenis,
                berat,
                DateTime.Now
            );

            _layanan.Add(transaksi);
            SimpanData();
            _logger.Tulis("INFO", $"Setoran langsung berhasil: {idTransaksi}");

            Console.WriteLine();
            transaksi.TampilkanRingkasan();
            InputHelper.TulisSukses("\nSetoran langsung berhasil dicatat.");
        }
        catch (BeratTidakValidException ex)
        {
            InputHelper.TulisError(ex.Message);
            _logger.Tulis("ERROR", $"Berat tidak valid pada setoran langsung: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            InputHelper.TulisError(ex.Message);
            _logger.Tulis("ERROR", $"Validasi transaksi gagal: {ex.Message}");
        }
    }

    private void CatatPenjemputanRumah()
    {
        InputHelper.TulisJudul("Catat Penjemputan Rumah");

        Nasabah? nasabah = PilihNasabah();
        if (nasabah is null)
        {
            return;
        }

        string idTransaksi = InputHelper.BacaTeksWajib("ID Transaksi : ");

        try
        {
            ValidasiTransaksiId(idTransaksi);
            JenisSampah jenis = InputHelper.PilihJenisSampah();
            double berat = InputHelper.BacaBeratPositif("Berat (kg)   : ");
            ValidasiBerat(berat);

            LayananSampah transaksi = new PenjemputanRumah(
                idTransaksi,
                nasabah,
                jenis,
                berat,
                DateTime.Now
            );

            _layanan.Add(transaksi);
            SimpanData();
            _logger.Tulis("INFO", $"Penjemputan rumah berhasil: {idTransaksi}");

            Console.WriteLine();
            transaksi.TampilkanRingkasan();
            InputHelper.TulisSukses("\nPermintaan penjemputan berhasil dicatat.");
        }
        catch (MinimumPenjemputanException ex)
        {
            InputHelper.TulisError(ex.Message);
            _logger.Tulis("ERROR", $"Minimum penjemputan tidak terpenuhi: {ex.Message}");
            Console.WriteLine("Program tetap berjalan dan kembali ke menu setelah ini.");
        }
        catch (BeratTidakValidException ex)
        {
            InputHelper.TulisError(ex.Message);
            _logger.Tulis("ERROR", $"Berat tidak valid pada penjemputan: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            InputHelper.TulisError(ex.Message);
            _logger.Tulis("ERROR", $"Validasi transaksi gagal: {ex.Message}");
        }
    }

    private Nasabah? PilihNasabah()
    {
        if (_nasabah.Count == 0)
        {
            InputHelper.TulisError("Belum ada nasabah. Tambahkan nasabah terlebih dahulu melalui menu 1.");
            return null;
        }

        Console.WriteLine("Daftar Nasabah:");
        foreach (var nasabah in _nasabah.OrderBy(n => n.Id))
        {
            Console.WriteLine($"- {nasabah.Id} | {nasabah.Nama}");
        }

        string idNasabah = InputHelper.BacaTeksWajib("\nMasukkan ID Nasabah: ");
        Nasabah? ditemukan = _nasabah.FirstOrDefault(
            n => n.Id.Equals(idNasabah, StringComparison.OrdinalIgnoreCase)
        );

        if (ditemukan is null)
        {
            InputHelper.TulisError("Nasabah tidak ditemukan.");
        }

        return ditemukan;
    }

    private void TampilkanNasabah()
    {
        InputHelper.TulisJudul("Data Nasabah");

        if (_nasabah.Count == 0)
        {
            Console.WriteLine("Belum ada data nasabah.");
            return;
        }

        foreach (var nasabah in _nasabah.OrderBy(n => n.Id))
        {
            Console.WriteLine(nasabah);
        }
    }

    private void TampilkanSemuaLayanan()
    {
        InputHelper.TulisJudul("Seluruh Layanan - Polymorphic Collection");

        if (_layanan.Count == 0)
        {
            Console.WriteLine("Belum ada transaksi.");
            return;
        }

        // Seluruh objek diproses melalui referensi LayananSampah.
        // Implementasi HitungInsentif/TampilkanRingkasan yang benar dipilih secara polymorphic.
        foreach (LayananSampah layanan in _layanan.OrderBy(l => l.Tanggal))
        {
            layanan.TampilkanRingkasan();
            Console.WriteLine(new string('-', 72));
        }
    }

    private void CariTransaksi()
    {
        InputHelper.TulisJudul("Cari Transaksi");
        string kataKunci = InputHelper.BacaTeksWajib("Masukkan ID transaksi atau ID nasabah: ");

        List<LayananSampah> hasil = _layanan
            .Where(l =>
                l.IdTransaksi.Equals(kataKunci, StringComparison.OrdinalIgnoreCase) ||
                l.Nasabah.Id.Equals(kataKunci, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (hasil.Count == 0)
        {
            Console.WriteLine("Transaksi tidak ditemukan.");
            return;
        }

        Console.WriteLine($"Ditemukan {hasil.Count} transaksi:\n");
        foreach (LayananSampah layanan in hasil)
        {
            layanan.TampilkanRingkasan();
            Console.WriteLine(new string('-', 72));
        }
    }

    private void UbahStatusLayanan()
    {
        InputHelper.TulisJudul("Ubah Status Layanan");
        string idTransaksi = InputHelper.BacaTeksWajib("ID Transaksi: ");

        LayananSampah? transaksi = _layanan.FirstOrDefault(
            l => l.IdTransaksi.Equals(idTransaksi, StringComparison.OrdinalIgnoreCase)
        );

        if (transaksi is null)
        {
            InputHelper.TulisError("Transaksi tidak ditemukan.");
            return;
        }

        Console.WriteLine($"Status saat ini: {transaksi.Status}");
        StatusLayanan statusBaru = InputHelper.PilihStatusLayanan();
        transaksi.UbahStatus(statusBaru);

        SimpanData();
        _logger.Tulis("INFO", $"Status {idTransaksi} diubah menjadi {statusBaru}.");
        InputHelper.TulisSukses($"Status berhasil diubah menjadi {statusBaru}.");
    }

    public void ValidasiNasabah(Nasabah nasabah)
    {
        if (string.IsNullOrWhiteSpace(nasabah.Id) ||
            string.IsNullOrWhiteSpace(nasabah.Nama) ||
            string.IsNullOrWhiteSpace(nasabah.Alamat))
        {
            throw new ArgumentException("ID, nama, dan alamat nasabah tidak boleh kosong.");
        }
    }

    public void ValidasiTransaksiId(string idTransaksi)
    {
        if (string.IsNullOrWhiteSpace(idTransaksi))
        {
            throw new ArgumentException("ID transaksi tidak boleh kosong.");
        }

        if (_layanan.Any(l => l.IdTransaksi.Equals(idTransaksi, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("ID transaksi sudah digunakan. Gunakan ID lain.");
        }
    }

    public void ValidasiBerat(double berat)
    {
        if (berat <= 0)
        {
            throw new BeratTidakValidException("Berat harus lebih dari 0 kg.");
        }
    }

    public void SimpanData()
    {
        bool berhasil = false;

        try
        {
            Directory.CreateDirectory(_dataFolder);

            var barisNasabah = new List<string>
            {
                "IdNasabah|Nama|Alamat"
            };

            barisNasabah.AddRange(
                _nasabah.Select(n => string.Join('|',
                    BersihkanField(n.Id),
                    BersihkanField(n.Nama),
                    BersihkanField(n.Alamat)))
            );

            File.WriteAllLines(_nasabahFile, barisNasabah);

            var barisTransaksi = new List<string>
            {
                "Tipe|IdTransaksi|IdNasabah|Nama|Alamat|JenisSampah|Berat|Tanggal|Status"
            };

            foreach (LayananSampah layanan in _layanan)
            {
                string tipe = layanan is PenjemputanRumah ? "PENJEMPUTAN" : "SETORAN";

                barisTransaksi.Add(string.Join('|',
                    tipe,
                    BersihkanField(layanan.IdTransaksi),
                    BersihkanField(layanan.Nasabah.Id),
                    BersihkanField(layanan.Nasabah.Nama),
                    BersihkanField(layanan.Nasabah.Alamat),
                    layanan.JenisSampah,
                    layanan.Berat.ToString(CultureInfo.InvariantCulture),
                    layanan.Tanggal.ToString("O", CultureInfo.InvariantCulture),
                    layanan.Status));
            }

            File.WriteAllLines(_transaksiFile, barisTransaksi);
            berhasil = true;
            _logger.Tulis("INFO", $"Data disimpan. Nasabah={_nasabah.Count}, Transaksi={_layanan.Count}");
        }
        catch (IOException ex)
        {
            InputHelper.TulisError($"Gagal menyimpan file: {ex.Message}");
            _logger.Tulis("ERROR", $"IOException saat simpan data: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            InputHelper.TulisError($"Tidak memiliki izin menulis file: {ex.Message}");
            _logger.Tulis("ERROR", $"UnauthorizedAccess saat simpan data: {ex.Message}");
        }
        finally
        {
            // finally dipakai agar blok ini selalu berjalan, baik proses simpan berhasil maupun gagal.
            if (berhasil)
            {
                Console.WriteLine("[File I/O] Data tersimpan ke folder data.");
            }
        }
    }

    public void MuatData()
    {
        try
        {
            Directory.CreateDirectory(_dataFolder);

            var nasabahBaru = new List<Nasabah>();
            var layananBaru = new List<LayananSampah>();

            if (File.Exists(_nasabahFile))
            {
                string[] baris = File.ReadAllLines(_nasabahFile);

                foreach (string line in baris.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] kolom = line.Split('|');
                    if (kolom.Length < 3)
                    {
                        _logger.Tulis("ERROR", $"Baris nasabah rusak dan dilewati: {line}");
                        continue;
                    }

                    var nasabah = new Nasabah(kolom[0], kolom[1], kolom[2]);

                    if (!nasabahBaru.Any(n => n.Id.Equals(nasabah.Id, StringComparison.OrdinalIgnoreCase)))
                    {
                        nasabahBaru.Add(nasabah);
                    }
                }
            }

            if (File.Exists(_transaksiFile))
            {
                string[] baris = File.ReadAllLines(_transaksiFile);

                foreach (string line in baris.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        string[] kolom = line.Split('|');
                        if (kolom.Length < 9)
                        {
                            throw new FormatException("Jumlah kolom transaksi tidak sesuai.");
                        }

                        string tipe = kolom[0];
                        string idTransaksi = kolom[1];
                        string idNasabah = kolom[2];
                        string nama = kolom[3];
                        string alamat = kolom[4];

                        if (!Enum.TryParse(kolom[5], true, out JenisSampah jenis))
                        {
                            throw new FormatException($"Jenis sampah '{kolom[5]}' tidak valid.");
                        }

                        double berat = double.Parse(kolom[6], CultureInfo.InvariantCulture);
                        DateTime tanggal = DateTime.Parse(kolom[7], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

                        if (!Enum.TryParse(kolom[8], true, out StatusLayanan status))
                        {
                            throw new FormatException($"Status '{kolom[8]}' tidak valid.");
                        }

                        Nasabah? nasabah = nasabahBaru.FirstOrDefault(
                            n => n.Id.Equals(idNasabah, StringComparison.OrdinalIgnoreCase)
                        );

                        if (nasabah is null)
                        {
                            nasabah = new Nasabah(idNasabah, nama, alamat);
                            nasabahBaru.Add(nasabah);
                        }

                        LayananSampah transaksi = tipe.Equals("PENJEMPUTAN", StringComparison.OrdinalIgnoreCase)
                            ? new PenjemputanRumah(idTransaksi, nasabah, jenis, berat, tanggal, status)
                            : new SetoranLangsung(idTransaksi, nasabah, jenis, berat, tanggal, status);

                        if (!layananBaru.Any(l => l.IdTransaksi.Equals(idTransaksi, StringComparison.OrdinalIgnoreCase)))
                        {
                            layananBaru.Add(transaksi);
                        }
                    }
                    catch (FormatException ex)
                    {
                        _logger.Tulis("ERROR", $"Format transaksi tidak valid: {ex.Message}. Baris: {line}");
                    }
                    catch (BeratTidakValidException ex)
                    {
                        _logger.Tulis("ERROR", $"Berat transaksi tidak valid saat load: {ex.Message}. Baris: {line}");
                    }
                    catch (MinimumPenjemputanException ex)
                    {
                        _logger.Tulis("ERROR", $"Minimum penjemputan tidak valid saat load: {ex.Message}. Baris: {line}");
                    }
                }
            }

            _nasabah.Clear();
            _nasabah.AddRange(nasabahBaru);
            _layanan.Clear();
            _layanan.AddRange(layananBaru);

            _logger.Tulis("INFO", $"Data dimuat. Nasabah={_nasabah.Count}, Transaksi={_layanan.Count}");
            InputHelper.TulisSukses($"Data berhasil dimuat: {_nasabah.Count} nasabah, {_layanan.Count} transaksi.");
        }
        catch (IOException ex)
        {
            InputHelper.TulisError($"Gagal membaca file: {ex.Message}");
            _logger.Tulis("ERROR", $"IOException saat muat data: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            InputHelper.TulisError($"Tidak memiliki izin membaca file: {ex.Message}");
            _logger.Tulis("ERROR", $"UnauthorizedAccess saat muat data: {ex.Message}");
        }
    }

    private void MuatDataSaatStartup()
    {
        try
        {
            Directory.CreateDirectory(_dataFolder);

            if (File.Exists(_nasabahFile) || File.Exists(_transaksiFile))
            {
                MuatData();
                Console.WriteLine("\nData sebelumnya otomatis dimuat.");
                Thread.Sleep(800);
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            _logger.Tulis("ERROR", $"Gagal load startup: {ex.Message}");
        }
    }

    public void TampilkanLaporanRingkas()
    {
        InputHelper.TulisJudul("Laporan Ringkas EcoWarga");

        int jumlahTransaksi = _layanan.Count;
        double totalBerat = _layanan.Sum(l => l.Berat);
        decimal totalInsentif = _layanan.Sum(l => l.HitungInsentif());
        int totalPoin = _layanan.Sum(l => l.HitungPoin());

        Console.WriteLine($"Jumlah Transaksi : {jumlahTransaksi}");
        Console.WriteLine($"Total Berat      : {totalBerat:0.##} kg");
        Console.WriteLine($"Total Insentif   : {FormatHelper.Rupiah(totalInsentif)}");
        Console.WriteLine($"Total Poin       : {totalPoin}");

        if (jumlahTransaksi > 0)
        {
            Console.WriteLine("\nRincian per Jenis Layanan:");
            Console.WriteLine($"- Setoran Langsung : {_layanan.Count(l => l is SetoranLangsung)}");
            Console.WriteLine($"- Penjemputan Rumah: {_layanan.Count(l => l is PenjemputanRumah)}");
        }
    }

    private static string BersihkanField(string value)
    {
        return value
            .Replace("|", "/")
            .Replace("\r", " ")
            .Replace("\n", " ")
            .Trim();
    }
}
