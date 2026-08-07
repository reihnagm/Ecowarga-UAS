# Diagram Kelas EcoWarga

Diagram berikut bisa dibuka di editor Markdown yang mendukung Mermaid atau ditempel ke https://mermaid.live untuk dibuat menjadi gambar.

```mermaid
classDiagram
    class Nasabah {
        -string _id
        -string _nama
        -string _alamat
        +string Id
        +string Nama
        +string Alamat
        +Nasabah(id, nama, alamat)
    }

    class LayananSampah {
        <<abstract>>
        +string IdTransaksi
        +Nasabah Nasabah
        +JenisSampah JenisSampah
        +double Berat
        +DateTime Tanggal
        +StatusLayanan Status
        +string JenisLayanan*
        +decimal HitungInsentif()*
        +int HitungPoin()
        +void UbahStatus(status)
        +void TampilkanRingkasan()
    }

    class SetoranLangsung {
        +string JenisLayanan
        +decimal HitungInsentif()
    }

    class PenjemputanRumah {
        +decimal BiayaLayanan
        +double MinimumBeratKg
        +string JenisLayanan
        +decimal HitungInsentif()
        +void TampilkanRingkasan()
    }

    class IValidasiData {
        <<interface>>
        +ValidasiNasabah(nasabah)
        +ValidasiTransaksiId(idTransaksi)
        +ValidasiBerat(berat)
    }

    class IPersistensiData {
        <<interface>>
        +SimpanData()
        +MuatData()
    }

    class ILaporan {
        <<interface>>
        +TampilkanLaporanRingkas()
    }

    class EcoWargaManager {
        -List~Nasabah~ _nasabah
        -List~LayananSampah~ _layanan
        +Jalankan()
        +SimpanData()
        +MuatData()
        +TampilkanLaporanRingkas()
    }

    class JenisSampah {
        <<enumeration>>
        Plastik
        Kertas
        Logam
        Kaca
        Organik
    }

    class StatusLayanan {
        <<enumeration>>
        Diajukan
        Diproses
        Selesai
        Dibatalkan
    }

    class BeratTidakValidException {
        <<exception>>
    }

    class MinimumPenjemputanException {
        <<exception>>
    }

    LayananSampah <|-- SetoranLangsung
    LayananSampah <|-- PenjemputanRumah
    LayananSampah --> Nasabah
    LayananSampah --> JenisSampah
    LayananSampah --> StatusLayanan

    EcoWargaManager ..|> IValidasiData
    EcoWargaManager ..|> IPersistensiData
    EcoWargaManager ..|> ILaporan
    EcoWargaManager o-- Nasabah
    EcoWargaManager o-- LayananSampah

    Exception <|-- BeratTidakValidException
    Exception <|-- MinimumPenjemputanException
```

## Penjelasan Hubungan

1. `LayananSampah` adalah abstract class.
2. `SetoranLangsung` dan `PenjemputanRumah` mewarisi `LayananSampah`.
3. `HitungInsentif()` dioverride oleh masing-masing child class.
4. `EcoWargaManager` menyimpan transaksi dalam `List<LayananSampah>` sehingga polymorphism benar-benar digunakan.
5. `EcoWargaManager` mengimplementasikan tiga interface sekaligus.
