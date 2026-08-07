# Script Video Presentasi EcoWarga

Target durasi: sekitar 8–12 menit.

## 0:00–0:30 — Opening

> "Halo, saya [NAMA], NIM [NIM], dari kelas [KELAS]. Pada video ini saya akan mempresentasikan project UAS Algoritma dan Pemrograman II dengan studi kasus Sistem Bank Sampah Digital EcoWarga."

> "Aplikasi dibuat menggunakan C# Console App dan menerapkan encapsulation, inheritance, abstraction, polymorphism, multiple interfaces, enum, exception handling, File I/O, dan logging."

## 0:30–1:30 — Jelaskan Masalah dan Aturan Bisnis

Tampilkan README atau laporan.

> "EcoWarga digunakan untuk mencatat nasabah, setoran langsung, dan layanan penjemputan rumah. Setiap jenis sampah memiliki harga per kilogram. Setoran langsung mendapatkan insentif berat dikali harga dasar. Penjemputan rumah minimal 2 kilogram dan dipotong biaya layanan 5 ribu rupiah. Poin dihitung dari setiap seribu rupiah insentif menjadi 10 poin."

## 1:30–3:00 — Diagram Kelas

Tampilkan diagram.

> "Class utama model adalah LayananSampah yang bersifat abstract. Dua turunannya adalah SetoranLangsung dan PenjemputanRumah. LayananSampah berelasi dengan Nasabah dan menggunakan enum JenisSampah serta StatusLayanan. EcoWargaManager mengimplementasikan tiga interface yaitu IValidasiData, IPersistensiData, dan ILaporan."

## 3:00–5:30 — Tunjukkan Kode OOP

Urutan file:

1. `Nasabah.cs` → encapsulation + constructor validation.
2. `LayananSampah.cs` → abstract class + abstract method.
3. `SetoranLangsung.cs` → override rumus.
4. `PenjemputanRumah.cs` → override + minimum 2 kg + biaya Rp5.000.
5. `EcoWargaManager.cs` → `List<LayananSampah>` dan multiple interfaces.

Kalimat inti polymorphism:

> "Walaupun object aslinya bisa SetoranLangsung atau PenjemputanRumah, semua saya simpan di List LayananSampah. Saat HitungInsentif dipanggil, C# otomatis menggunakan override milik object sebenarnya."

## 5:30–6:30 — Exception + File I/O

Tunjukkan custom exception dan `InputHelper`.

> "Untuk input berat bukan angka, saya menangani FormatException. Untuk berat nol atau negatif saya menggunakan BeratTidakValidException. Untuk penjemputan di bawah dua kilogram saya menggunakan MinimumPenjemputanException. IOException ditangani ketika proses file, dan proses simpan memiliki blok finally."

Tunjukkan `Logger.cs` dan folder `data`.

## 6:30–9:30 — Demo Skenario Wajib

### Demo A: Setoran Langsung

1. Tambahkan N001 bila belum ada.
2. Transaksi TRX001.
3. Plastik 3 kg.
4. Tunjukkan hasil Rp10.500 dan 100 poin.

Ucapkan:

> "Tiga kilogram plastik dikali Rp3.500 menghasilkan Rp10.500. Poinnya floor 10.500 dibagi 1.000, yaitu 10, dikali 10 sehingga 100 poin."

### Demo B: Exception Penjemputan

1. Tambahkan N002 bila belum ada.
2. Masukkan penjemputan Logam 1.5 kg.
3. Tunjukkan `MinimumPenjemputanException`.
4. Tekankan program tidak crash.

### Demo C: Penjemputan Valid

1. N002.
2. TRX003.
3. Kertas 5 kg.
4. Tunjukkan Rp5.000 dan 50 poin.

Ucapkan:

> "Kertas 5 kilogram bernilai Rp10.000. Karena penjemputan dipotong biaya Rp5.000, insentif bersih menjadi Rp5.000 dan menghasilkan 50 poin."

### Demo D: Input Salah

Masukkan `abc`, lalu `-3`, lalu nilai valid.

Tunjukkan bahwa program meminta input ulang.

## 9:30–10:30 — Persistensi

1. Save.
2. Keluar.
3. Jalankan `dotnet run` lagi.
4. Tunjukkan data otomatis dibaca.
5. Tampilkan semua layanan.
6. Tunjukkan isi `data_transaksi.txt` dan `log_aplikasi.txt`.

> "Ini membuktikan data tidak hilang setelah aplikasi ditutup karena transaksi dimuat kembali dari file."

## 10:30–11:00 — Laporan

Pilih menu laporan.

> "Laporan menampilkan jumlah transaksi, total berat, total insentif, dan total poin dengan perhitungan langsung dari collection transaksi."

## 11:00–11:20 — Closing

> "Demikian implementasi EcoWarga. Seluruh konsep yang diminta digunakan pada alur aplikasi, bukan hanya dideklarasikan. Terima kasih."
