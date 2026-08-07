# Tutorial Lengkap Membuat dan Menjelaskan EcoWarga

Dokumen ini disusun supaya mudah dipakai sebagai panduan pengerjaan sekaligus bahan rekaman video presentasi.

## 1. Membuat Project

Buka Terminal / Command Prompt:

```bash
dotnet new console -n EcoWarga
cd EcoWarga
```

Project dari paket ini sudah dibuatkan, sehingga jika memakai folder yang diberikan cukup masuk ke folder tersebut.

Cek .NET:

```bash
dotnet --version
```

Project menargetkan `.NET 8`.

## 2. Memahami Struktur

Urutan penjelasan yang disarankan:

1. `Enums/` — pilihan jenis sampah dan status.
2. `Exceptions/` — dua custom exception.
3. `Interfaces/` — kontrak validasi, persistensi, laporan.
4. `Models/Nasabah.cs` — encapsulation dan constructor.
5. `Models/LayananSampah.cs` — abstract class.
6. `SetoranLangsung.cs` dan `PenjemputanRumah.cs` — inheritance + override.
7. `Services/EcoWargaManager.cs` — menu, polymorphic collection, File I/O.
8. `Services/Logger.cs` — logging.
9. `Program.cs` — entry point aplikasi.

## 3. Encapsulation

Buka `Models/Nasabah.cs`.

Poin yang dijelaskan:

- Field `_id`, `_nama`, `_alamat` bersifat private.
- Property memiliki `private set`.
- Data hanya diset melalui constructor.
- Constructor memvalidasi agar ID, nama, alamat tidak kosong.

Kalimat video:

> "Di class Nasabah saya menerapkan encapsulation. Data internal disimpan pada field private dan perubahan property dibatasi melalui private setter. Validasi dilakukan ketika object dibuat lewat constructor."

## 4. Abstraction + Inheritance

Buka `Models/LayananSampah.cs`.

Poin:

- Class diberi keyword `abstract`.
- Berisi data umum semua layanan.
- `HitungInsentif()` adalah abstract method.
- `JenisLayanan` adalah abstract property.

Lalu buka:

- `SetoranLangsung.cs`
- `PenjemputanRumah.cs`

Keduanya menggunakan `: LayananSampah`.

Kalimat video:

> "LayananSampah saya jadikan abstract class karena tidak dibuat langsung sebagai transaksi. Class ini menjadi template bagi setoran langsung dan penjemputan rumah."

## 5. Polymorphism

Buka `EcoWargaManager.cs` dan tunjukkan:

```csharp
private readonly List<LayananSampah> _layanan = new();
```

Kemudian tunjukkan loop:

```csharp
foreach (LayananSampah layanan in _layanan)
{
    layanan.TampilkanRingkasan();
}
```

Penjelasan:

- Collection menyimpan child object sebagai parent reference.
- `HitungInsentif()` yang dieksekusi otomatis mengikuti tipe object sebenarnya.
- Tidak perlu `if` berulang untuk memilih rumus.

## 6. Multiple Interfaces

Tunjukkan deklarasi:

```csharp
public class EcoWargaManager : IValidasiData, IPersistensiData, ILaporan
```

Interface yang digunakan:

- `IValidasiData`
- `IPersistensiData`
- `ILaporan`

Ini lebih dari syarat minimum dua interface.

## 7. Enum

Tunjukkan:

```csharp
public enum JenisSampah
public enum StatusLayanan
```

Jelaskan enum tidak hanya dideklarasikan, tetapi dipakai di:

- property model,
- input menu,
- output transaksi,
- file data.

## 8. Aturan Bisnis

Harga per kg berada pada `Helpers/SampahHelper.cs`.

Setoran langsung:

```csharp
return (decimal)Berat * HargaDasarPerKg;
```

Penjemputan rumah:

```csharp
decimal nilaiKotor = (decimal)Berat * HargaDasarPerKg;
return Math.Max(0m, nilaiKotor - BiayaLayanan);
```

Poin pada parent class:

```csharp
return (int)Math.Floor(insentif / 1000m) * 10;
```

## 9. Custom Exception

Dua exception:

- `BeratTidakValidException`
- `MinimumPenjemputanException`

Untuk demo paling bagus gunakan penjemputan 1.5 kg agar `MinimumPenjemputanException` muncul tetapi aplikasi tidak crash.

## 10. FormatException

Tunjukkan `InputHelper.BacaBeratPositif()`.

Program menggunakan `double.Parse()` dalam `try-catch`. Jika user menulis `abc`, `FormatException` ditangkap lalu user diminta mengisi ulang.

## 11. IOException dan finally

Tunjukkan method `SimpanData()`.

Ada:

```csharp
catch (IOException ex)
```

serta:

```csharp
finally
```

Jelaskan `finally` selalu dijalankan setelah try/catch selesai.

## 12. File I/O

Data tersimpan di folder runtime:

```text
bin/Debug/net8.0/data/
```

File:

```text
data_nasabah.txt
data_transaksi.txt
log_aplikasi.txt
```

Untuk bukti persistensi:

1. Tambah transaksi.
2. Pilih save.
3. Keluar.
4. Run ulang.
5. Data akan otomatis dimuat.
6. Tampilkan seluruh layanan.

## 13. Logging

Buka `log_aplikasi.txt`.

Contoh log:

```text
[2026-08-07 16:00:00] [INFO] Setoran langsung berhasil: TRX001
[2026-08-07 16:02:10] [ERROR] Minimum penjemputan tidak terpenuhi: ...
```

Logger mencatat aktivitas penting dan error dengan timestamp.

## 14. Cara Menjalankan

```bash
dotnet restore
dotnet run
```

Jika terjadi error karena runtime seperti komputer hanya punya `.NET 10`, ubah file `EcoWarga.csproj`:

```xml
<TargetFramework>net10.0</TargetFramework>
```

Lalu:

```bash
dotnet clean
dotnet run
```

## 15. Checklist Sebelum Rekam Video

- Pastikan project dapat `dotnet run` tanpa error.
- Hapus data lama jika ingin demo dari kondisi kosong.
- Siapkan N001 dan N002.
- Siapkan urutan skenario test.
- Buka diagram kelas.
- Buka beberapa file kode penting di editor.
- Buka folder data agar mudah menunjukkan File I/O.
- Pastikan wajah terlihat sesuai instruksi ujian.
- Rekam suara cukup jelas.
- Jangan hanya membaca kode; jelaskan alasan desain.

## 16. Checklist Pengumpulan

- Folder project lengkap.
- Diagram kelas.
- Laporan ringkas maksimal 3 halaman setelah diformat.
- Screenshot test valid.
- Screenshot exception.
- Screenshot save/load file.
- Link cloud/GitHub bila digunakan.
- Link video presentasi.
- ZIP dengan pola nama yang diminta dosen.
