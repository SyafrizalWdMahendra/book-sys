# 🏢 System Room Booking (.NET 8 MVC + PostgreSQL)

Aplikasi Web Pemesanan Ruangan berbasis tanggal (*Date-Based Booking System*) yang dibangun menggunakan **ASP.NET Core 8 MVC** dan **PostgreSQL (Neon Cloud DB)**. 

Dibuat untuk memenuhi instruksi **Technical Test - Kalbe Nutritionals**.

---

## 🚀 Fitur Utama

1. **Autentikasi & Otorisasi:**
   - Login & Logout berbasis *Session Cookie*.
   - Keamanan Password Hashing (SHA256).
   - Akses kontrol berdasarkan ketersediaan hak pemesanan (User/Admin).

2. **Pengelolaan Booking Ruangan (CRUD & Filter):**
   - **Create:** Input booking berdasarkan ruangan, tanggal, dan catatan.
   - **Read:** Tabel daftar booking dilengkapi **Filter Berdasarkan Tanggal** dan **Filter Booking Saya**.
   - **Update & Delete:** Edit dan batalkan booking dengan proteksi *Ownership* (User hanya bisa mengubah/menghapus booking miliknya sendiri).

3. **Engine Validasi & Keamanan:**
   - **Mencegah Double-Booking:** Sistem otomatis menolak jika ruangan dipesan pada tanggal yang sama oleh pengguna lain.
   - **Mencegah Date Retroactive:** Validasi tanggal pemesanan tidak boleh di masa lalu.
   - **Proteksi Akses URL:** Mencegah manipulasi URL id booking milik pengguna lain.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core 8.0 MVC (C#)
- **Database:** PostgreSQL (Neon Cloud Database)
- **ORM:** Entity Framework Core 8 (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- **Frontend UI:** Razor Views (`.cshtml`), Bootstrap 5, FontAwesome / HTML5

---

## ⚙️ Cara Menjalankan Program (Setup & Run)

Database sudah terkonfigurasi secara *online* di **Neon PostgreSQL Cloud**, sehingga Anda **tidak perlu mengkonfigurasi database lokal**. Cukup jalankan langkah di bawah ini:

### 1. Prasyarat
- [**.NET 8.0 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0) telah ter-install di komputer Anda.

### 2. Run Aplikasi via Terminal/CLI
```bash
# 1. Clone repository ini
git clone https://github.com/SyafrizalWdMahendra/book-sys.git

# 2. Masuk ke folder project
cd RoomBookingApp

# 3. Restore dependencies & Run
dotnet restore
dotnet run
