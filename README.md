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

 ## Akun Default

 Saat pertama kali dijalankan, sistem otomatis membuat akun admin:

 - **Email:** `admin@petclinic.com`
 - **Password:** `Admin_123!`
 - **Role:** Operator

 Gunakan akun ini untuk mengelola data awal (produk, pengguna, dll).

 ## Lisensi

 MIT – bebas digunakan untuk keperluan belajar maupun komersial.