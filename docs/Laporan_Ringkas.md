# LAPORAN RINGKAS UAS ALGORITMA DAN PEMROGRAMAN II

## Sistem Bank Sampah Digital "EcoWarga"

**Nama:** [ISI NAMA]  
**NIM:** [ISI NIM]  
**Kelas:** [ISI KELAS]  
**Mata Kuliah:** Algoritma dan Pemrograman II

---

## 1. Analisis Kasus

EcoWarga dibuat untuk mendigitalisasi proses bank sampah pada tingkat kelurahan. Sistem menangani data nasabah, setoran sampah langsung, penjemputan sampah rumah tangga, perhitungan insentif dan poin, perubahan status layanan, pencarian transaksi, penyimpanan data ke file, serta pembuatan laporan ringkas.

Aturan harga dasar per kilogram yang dipakai:

| Jenis Sampah | Harga/kg |
|---|---:|
| Plastik | Rp3.500 |
| Kertas | Rp2.000 |
| Logam | Rp8.000 |
| Kaca | Rp1.500 |
| Organik | Rp500 |

Rumus:

- Setoran langsung: `berat × harga dasar`.
- Penjemputan rumah: `max(0, berat × harga dasar - Rp5.000)` dengan minimum 2 kg.
- Poin: `floor(insentif / 1.000) × 10`.

## 2. Keputusan Desain dan Konsep OOP

Kelas `Nasabah` memakai encapsulation melalui property yang hanya dapat diubah dari dalam kelas dan constructor yang memvalidasi ID, nama, dan alamat.

`LayananSampah` dibuat sebagai abstract class karena semua transaksi memiliki data yang sama, tetapi cara menghitung insentif berbeda. Class ini memiliki abstract method `HitungInsentif()` dan property abstract `JenisLayanan`. `SetoranLangsung` dan `PenjemputanRumah` melakukan override sehingga perhitungan disesuaikan dengan aturan masing-masing.

Semua transaksi disimpan dalam `List<LayananSampah>`. Ketika program menampilkan transaksi atau menghitung laporan, objek diproses lewat referensi parent class. Hal ini membuat polymorphism benar-benar digunakan tanpa melakukan pengecekan tipe berulang untuk menentukan rumus insentif.

Class `EcoWargaManager` mengimplementasikan tiga interface: `IValidasiData`, `IPersistensiData`, dan `ILaporan`. Enum `JenisSampah` serta `StatusLayanan` dipakai langsung pada menu input dan output.

## 3. Exception Handling, File I/O, dan Logging

Dua custom exception digunakan:

1. `BeratTidakValidException` untuk berat kurang dari atau sama dengan 0.
2. `MinimumPenjemputanException` untuk penjemputan rumah dengan berat di bawah 2 kg.

`FormatException` ditangani saat input berat bukan angka dan ketika parsing file. `IOException` ditangani saat proses baca/tulis file. Blok `finally` digunakan pada proses penyimpanan untuk menunjukkan proses final yang tetap berjalan setelah operasi file selesai.

Data disimpan pada file:

- `data_nasabah.txt`
- `data_transaksi.txt`
- `log_aplikasi.txt`

Setiap transaksi yang berhasil langsung memicu penyimpanan data. Saat aplikasi dijalankan kembali, data lama otomatis dimuat apabila file tersedia.

## 4. Alur Program

1. Program membuat instance `EcoWargaManager`.
2. Data lama dimuat saat startup jika tersedia.
3. Pengguna memilih menu.
4. Seluruh input divalidasi.
5. Transaksi dibuat sebagai object child (`SetoranLangsung` atau `PenjemputanRumah`) namun disimpan sebagai `LayananSampah`.
6. Insentif dan poin dihitung sesuai implementasi polymorphic.
7. Data disimpan ke file dan aktivitas dicatat ke log.
8. Laporan mengambil jumlah transaksi, total berat, total insentif, serta total poin dari collection layanan.
9. Ketika menu keluar dipilih, data disimpan dan aplikasi ditutup dengan aman.

## 5. Hasil Pengujian Utama

- N001, Plastik 3 kg, setoran langsung → insentif Rp10.500 dan 100 poin.
- N002, Logam 1,5 kg, penjemputan → `MinimumPenjemputanException` ditangani dan program tetap berjalan.
- N002, Kertas 5 kg, penjemputan → nilai kotor Rp10.000, biaya Rp5.000, insentif Rp5.000 dan 50 poin.
- Input teks atau angka negatif untuk berat → ditolak dan diminta ulang.
- Setelah aplikasi ditutup dan dijalankan kembali → data dimuat dari file.

> Tambahkan screenshot hasil pengujian pada dokumen akhir sesuai kebutuhan pengumpulan.
