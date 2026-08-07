# EcoWarga — UAS Algoritma dan Pemrograman II

Project Console App C# untuk kasus **Sistem Bank Sampah Digital "EcoWarga"**.

## Fitur

- Tambah data nasabah.
- Catat setoran langsung.
- Catat penjemputan rumah.
- Tampilkan semua layanan melalui `List<LayananSampah>` secara polymorphic.
- Cari transaksi berdasarkan ID transaksi atau ID nasabah.
- Ubah status layanan.
- Simpan dan muat data dari file.
- Logging aktivitas dan error.
- Laporan jumlah transaksi, total berat, total insentif, dan total poin.
- Validasi input dan custom exception.

## Konsep C# yang dipakai

- Encapsulation + constructor validation.
- Inheritance.
- Abstraction.
- Polymorphism.
- Multiple interfaces.
- Enum.
- Exception handling.
- Custom exception.
- File I/O.
- Logging.

## Struktur Project

```text
EcoWarga_UAS_AP2/
├── EcoWarga.csproj
├── Program.cs
├── Enums/
│   ├── JenisSampah.cs
│   └── StatusLayanan.cs
├── Models/
│   ├── Nasabah.cs
│   ├── LayananSampah.cs
│   ├── SetoranLangsung.cs
│   └── PenjemputanRumah.cs
├── Interfaces/
│   ├── IValidasiData.cs
│   ├── IPersistensiData.cs
│   └── ILaporan.cs
├── Exceptions/
│   ├── BeratTidakValidException.cs
│   └── MinimumPenjemputanException.cs
├── Helpers/
│   ├── InputHelper.cs
│   ├── SampahHelper.cs
│   └── FormatHelper.cs
├── Services/
│   ├── EcoWargaManager.cs
│   └── Logger.cs
├── data/
└── docs/
    ├── Diagram_Kelas.md
    ├── Laporan_Ringkas.md
    ├── Tutorial_Lengkap.md
    ├── Skenario_Pengujian.md
    └── Script_Video_Presentasi.md
```

## Menjalankan

```bash
dotnet restore
dotnet run
```

Project menargetkan `.NET 8`. Jika komputer hanya memiliki .NET 10, ubah:

```xml
<TargetFramework>net8.0</TargetFramework>
```

menjadi:

```xml
<TargetFramework>net10.0</TargetFramework>
```

atau install .NET 8 runtime/SDK.

## File Data

Saat aplikasi dijalankan, file dibuat di folder output aplikasi:

```text
bin/Debug/net8.0/data/
```

File yang digunakan:

- `data_nasabah.txt`
- `data_transaksi.txt`
- `log_aplikasi.txt`

```text
UAS_AP2_Karyawan_25306622096_ReihanAgam.zip
```
