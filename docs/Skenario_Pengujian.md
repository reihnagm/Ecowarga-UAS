# Skenario Pengujian Wajib EcoWarga

Gunakan bagian ini saat demo dan saat mengambil screenshot.

## Persiapan Data Nasabah

### Nasabah 1

- ID: `N001`
- Nama: `Budi`
- Alamat: `Jakarta`

### Nasabah 2

- ID: `N002`
- Nama: `Siti`
- Alamat: `Depok`

---

## Test 1 — Setoran Langsung Valid

Input:

- Nasabah: `N001`
- ID transaksi: `TRX001`
- Jenis: `Plastik`
- Berat: `3`

Perhitungan:

```text
3 kg × Rp3.500 = Rp10.500
floor(10.500 / 1.000) × 10
= floor(10,5) × 10
= 10 × 10
= 100 poin
```

Expected:

- Insentif: `Rp10.500`
- Poin: `100`
- Status: `Selesai`

Ambil screenshot hasil ini.

---

## Test 2 — Penjemputan Rumah Tidak Valid

Input:

- Nasabah: `N002`
- ID transaksi: `TRX002`
- Jenis: `Logam`
- Berat: `1.5`

Expected:

```text
MinimumPenjemputanException
Penjemputan rumah minimal 2 kg.
```

Program tidak boleh berhenti/crash dan harus kembali ke menu.

Ambil screenshot exception.

---

## Test 3 — Penjemputan Rumah Valid

Karena `TRX002` pada test gagal tidak disimpan, ID tersebut masih boleh dipakai. Namun untuk video lebih jelas gunakan `TRX003`.

Input:

- Nasabah: `N002`
- ID transaksi: `TRX003`
- Jenis: `Kertas`
- Berat: `5`

Perhitungan:

```text
5 kg × Rp2.000 = Rp10.000
Rp10.000 - Rp5.000 = Rp5.000
floor(5.000 / 1.000) × 10 = 50 poin
```

Expected:

- Insentif: `Rp5.000`
- Poin: `50`
- Status: `Diajukan`

Ambil screenshot.

---

## Test 4 — Input Berat Teks

Pada menu transaksi, masukkan:

```text
abc
```

Expected:

```text
[ERROR] Format berat tidak valid...
```

Lalu program meminta input berat lagi.

---

## Test 5 — Input Berat Negatif

Masukkan:

```text
-3
```

Expected:

```text
[ERROR] Berat harus lebih dari 0 kg.
```

Program meminta input ulang.

---

## Test 6 — Simpan dan Baca Kembali File

1. Pastikan sudah ada `TRX001` dan `TRX003`.
2. Pilih menu `7` untuk simpan.
3. Keluar dengan menu `0`.
4. Jalankan lagi dengan `dotnet run`.
5. Program akan otomatis membaca data lama.
6. Pilih menu `4` untuk menunjukkan transaksi masih tersedia.
7. Pilih menu `8` bila ingin mendemonstrasikan baca file secara manual.

Ambil screenshot:

- proses simpan,
- isi folder/file data,
- aplikasi setelah restart,
- transaksi yang berhasil dimuat kembali.

---

## Test 7 — Pencarian

Cari dengan:

```text
TRX001
```

Lalu cari dengan:

```text
N002
```

Expected:

- `TRX001` menampilkan satu transaksi.
- `N002` menampilkan seluruh transaksi milik N002.

---

## Test 8 — Ubah Status

1. Pilih menu `6`.
2. Masukkan `TRX003`.
3. Ubah status dari `Diajukan` menjadi `Diproses`, lalu bisa diuji lagi ke `Selesai`.
4. Tampilkan transaksi untuk membuktikan status tersimpan.

---

## Test 9 — Laporan Ringkas

Dengan data minimal `TRX001` dan `TRX003`, expected:

```text
Jumlah Transaksi : 2
Total Berat      : 8 kg
Total Insentif   : Rp15.500
Total Poin       : 150
```
