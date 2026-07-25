DROP TABLE IF EXISTS "Bookings";
DROP TABLE IF EXISTS "Rooms";
DROP TABLE IF EXISTS "Users";

CREATE TABLE "Users" (
    "UserId" SERIAL PRIMARY KEY,
    "Username" VARCHAR(50) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "FullName" VARCHAR(100) NOT NULL,
    "Role" VARCHAR(20) NOT NULL DEFAULT 'User',
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Rooms" (
    "RoomId" SERIAL PRIMARY KEY,
    "RoomName" VARCHAR(100) NOT NULL,
    "Capacity" INT NOT NULL,
    "Description" VARCHAR(255) NULL,
    "IsActive" BOOLEAN DEFAULT TRUE
);

CREATE TABLE "Bookings" (
    "BookingId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "RoomId" INT NOT NULL,
    "BookingDate" DATE NOT NULL,
    "Notes" VARCHAR(255) NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_Bookings_Users" FOREIGN KEY ("UserId") REFERENCES "Users"("UserId") ON DELETE CASCADE,
    CONSTRAINT "FK_Bookings_Rooms" FOREIGN KEY ("RoomId") REFERENCES "Rooms"("RoomId") ON DELETE CASCADE,
    CONSTRAINT "UQ_Room_BookingDate" UNIQUE ("RoomId", "BookingDate")
);

CREATE INDEX "IX_Bookings_BookingDate" ON "Bookings"("BookingDate");

INSERT INTO "Users" ("Username", "PasswordHash", "FullName", "Role") 
VALUES 
('admin', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA', 'Administrator System', 'Admin'),
('syafrizal', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA', 'Syafrizal Wd', 'User');

INSERT INTO "Rooms" ("RoomName", "Capacity", "Description") 
VALUES 
('Ruang Rapat Utama (Gedung A)', 20, 'Dilengkapi Proyektor, Sound System, dan AC'),
('Ruang Inovasi (Gedung B)', 10, 'Cocok untuk diskusi tim kecil / Standup'),
('Auditorium Utama', 100, 'Kapasitas besar untuk seminar & townhall meeting');

INSERT INTO "Bookings" ("UserId", "RoomId", "BookingDate", "Notes") 
VALUES 
(1, 1, CURRENT_DATE, 'Rapat Koordinasi Mingguan Manajemen'),
(2, 2, CURRENT_DATE + INTERVAL '1 day', 'Diskusi Sprint Planning Dev Team');