 # Simple Pet Clinic API

 API sederhana untuk manajemen klinik hewan peliharaan, mencakup pendaftaran pengguna, manajemen hewan, reservasi layanan, rekam medis, grooming, dan transaksi.

 ## Fitur Utama

 - **Autentikasi & Otorisasi** – Register/Login menggunakan JWT, dengan 5 peran: Operator, Cashier, Doctor, Groomer, Customer.
 - **Manajemen Hewan Peliharaan** – Tambah, lihat profil, dan riwayat kesehatan hewan.
 - **Reservasi Layanan** – Buat reservasi untuk layanan medis/grooming, ubah status (Waiting, Done).
 - **Rekam Medis & Grooming** – Catat hasil pemeriksaan atau grooming berdasarkan reservasi.
 - **Produk & Layanan** – Kelola daftar produk/layanan klinik (kategori: Product, Medical, Grooming, PetHotel).
 - **Transaksi** – Pelanggan dapat membuat transaksi pembelian produk/layanan.
 - **Validasi** – Semua input divalidasi dengan FluentValidation.
 - **Global Exception Handling** – Respon error yang seragam.

 ## Teknologi

 - **.NET 10** (Web API minimal dengan Carter)
 - **Entity Framework Core** + **PostgreSQL**
 - **ASP.NET Core Identity** + **JWT Bearer**
 - **MediatR** (CQRS) + **FluentValidation**
 - **Swagger / OpenAPI** untuk dokumentasi

 ## Prasyarat

 - [.NET 10 SDK](https:dotnet.microsoft.com/en-us/download/dotnet/10.0)
 - PostgreSQL (atau sesuaikan connection string)
 - (Opsional) EF Core CLI: `dotnet tool install --global dotnet-ef`

 ## Instalasi & Menjalankan

 1. **Clone repositori**
    ```bash
    git clone https:github.com/user/simple-pet-clinic-api.git
    cd simple-pet-clinic-api
    ```

 2. **Atur connection string**
    Buka `appsettings.json` dan ubah `ConnectionStrings:DefaultConnection` sesuai server PostgreSQL Anda.

 3. **Jalankan migrasi database**
    ```bash
    dotnet ef database update
    ```
    *(Atau biarkan aplikasi membuat database secara otomatis, namun migrasi tetap disarankan.)*

 4. **Jalankan aplikasi**
    ```bash
    dotnet run
    ```
    Aplikasi akan berjalan di `https:localhost:5001` (atau sesuai konfigurasi).

 5. **Dokumentasi API**
    Buka `https:localhost:5001/swagger` untuk melihat daftar endpoint lengkap.

 ## Struktur Proyek (ringkas)

 ```
 Features/
  ├── Auth/            # Register, Login, JWT
  ├── Customer/        # Profil pelanggan
  ├── Pet/             # Hewan peliharaan
  ├── Reservation/     # Reservasi & update status
  ├── MedicalRecord/   # Rekam medis
  ├── GroomingRecord/  # Rekam grooming
  ├── Transaction/     # Pembelian
  └── Product/         # Produk & layanan
 Infrastructure/       # Behaviors, Exception handler
 Data/                 # DbContext, DbInitializer
 Models/               # Entity, DTO, Enum
 ```

 ## Endpoint Utama

 | Metode | Endpoint                        | Auth         | Deskripsi                              |
 |--------|---------------------------------|--------------|----------------------------------------|
 | POST   | `/api/auth/register`            | No           | Daftar pengguna baru                   |
 | POST   | `/api/auth/login`               | No           | Login, dapatkan token JWT              |
 | GET    | `/api/customer/me`              | Authenticated| Lihat profil sendiri                   |
 | POST   | `/api/pet/add`                  | Customer     | Tambah hewan peliharaan                |
 | GET    | `/api/pet`                      | Customer     | Lihat daftar hewan milik sendiri       |
 | GET    | `/api/pet/{id}`                 | Authenticated| Detail hewan                           |
 | POST   | `/api/reservation/create`       | Customer     | Buat reservasi                         |
 | GET    | `/api/reservation`              | Authenticated| Lihat reservasi (sesuai peran)        |
 | PUT    | `/api/reservation/{id}/status`  | Doctor/Groomer/Operator/Cashier | Ubah status reservasi   |
 | POST   | `/api/medical-record/create`    | Doctor/Operator | Buat rekam medis                    |
 | GET    | `/api/medical-record/pet/{id}`  | Authenticated| Lihat rekam medis hewan               |
 | POST   | `/api/grooming-record/create`   | Groomer/Operator | Buat rekam grooming                |
 | GET    | `/api/grooming-record/pet/{id}` | Authenticated| Lihat rekam grooming hewan            |
 | POST   | `/api/transaction/create`       | Customer     | Buat transaksi                         |
 | GET    | `/api/product`                  | Authenticated| Lihat semua produk/layanan             |
 | POST   | `/api/product/create`           | Operator     | Tambah produk baru                     |


# API Request Example

## 1. Register

**POST** `/api/auth/register`

```json
{
    "fullName": "Budi Santoso",
    "email": "budi@example.com",
    "password": "Rahasia123!",
    "role": "Customer"
}
```

| Parameter  | Tipe   | Deskripsi                       |
|------------|--------|----------------------------------|
| fullName   | string | Nama lengkap pengguna            |
| email      | string | Email (harus valid)              |
| password   | string | Minimal 6 karakter               |
| role       | string | Operator, Cashier, Doctor, Groomer, Customer |

## 2. Login

**POST** `/api/auth/login`

```json
{
    "email": "budi@example.com",
    "password": "Rahasia123!"
}
```

## 3. Tambah Hewan Peliharaan

**POST** `/api/pet/add`

```json
{
    "petName": "Milo",
    "species": "Kucing",
    "weight": 4.5,
    "diseaseHistory": "Alergi makanan"
}
```

| Parameter        | Tipe   | Deskripsi                 |
|------------------|--------|---------------------------|
| petName          | string | Nama hewan                |
| species          | string | Jenis hewan               |
| weight           | number | Berat (kg), > 0           |
| diseaseHistory   | string | Riwayat penyakit (opsional)|

## 4. Buat Reservasi

**POST** `/api/reservation/create`

```json
{
    "dateSchedule": "2026-07-01T10:00:00Z",
    "destinationService": "Medis",
    "petId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

| Parameter           | Tipe   | Deskripsi                              |
|---------------------|--------|----------------------------------------|
| dateSchedule        | string | ISO 8601 UTC, harus > waktu sekarang   |
| destinationService  | string | "Medis" atau "Grooming"                |
| petId               | GUID   | ID hewan yang dimiliki                 |

## 5. Ubah Status Reservasi

**PUT** `/api/reservation/{reservationId}/status`

```json
{
    "newStatus": "Done"
}
```

Nilai yang valid: `"Waiting"` atau `"Done"`.

## 6. Buat Rekam Medis

**POST** `/api/medical-record/create`

```json
{
    "medicaResults": "Kucing demam, diberikan infus.",
    "reservationId": "guid-reservasi"
}
```

## 7. Buat Rekam Grooming

**POST** `/api/grooming-record/create`

```json
{
    "groomingResults": "Mandi, potong kuku, bulu sehat.",
    "reservationId": "guid-reservasi"
}
```

## 8. Buat Transaksi

**POST** `/api/transaction/create`

```json
{
    "items": [
        {
            "productId": "guid-produk1",
            "quantity": 2
        },
        {
            "productId": "guid-produk2",
            "quantity": 1
        }
    ]
}
```

| Parameter   | Tipe     | Deskripsi              |
|-------------|----------|------------------------|
| productId   | GUID     | ID produk/layanan      |
| quantity    | integer  | Jumlah pembelian (>0)  |

## 9. Tambah Produk/Layanan Baru

**POST** `/api/product/create`

```json
{
    "itemName": "Vitamin Kucing",
    "category": "Product",
    "price": 45000.00,
    "stock": 50
}
```

Kategori yang valid: `"Product"`, `"Grooming"`, `"Medical"`, `"PetHotel"`.

---

**Catatan Umum:**
- Semua GUID menggunakan format standar `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`.
- Tanggal dalam format ISO 8601 UTC (contoh: `2026-07-01T10:00:00Z`).
- Endpoint yang memerlukan otorisasi harus menyertakan header `Authorization: Bearer <token>`.

 ## Akun Default

 Saat pertama kali dijalankan, sistem otomatis membuat akun admin:

 - **Email:** `admin@petclinic.com`
 - **Password:** `Admin_123!`
 - **Role:** Operator

 Gunakan akun ini untuk mengelola data awal (produk, pengguna, dll).

 ## Lisensi

 MIT – bebas digunakan untuk keperluan belajar maupun komersial.