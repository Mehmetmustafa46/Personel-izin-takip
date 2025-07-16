using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace PersonelIzinTakip
{
    public class Database
    {
        private readonly string connectionString = "Server=DESKTOP-T0UT4UD\\MSSQLSERVER01;Database=PersonelIzinTakip;Trusted_Connection=True;TrustServerCertificate=True;";
        public List<Personel> GetPersoneller()
        {
            var personeller = new List<Personel>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Personeller", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personeller.Add(new Personel
                            {
                                Id = reader.GetInt32(0),
                                Ad = reader.GetString(1),
                                Soyad = reader.GetString(2),
                                SicilNo = reader.GetString(3),
                                Departman = reader.GetString(4),
                                Pozisyon = reader.GetString(5),
                                IseGirisTarihi = reader.GetDateTime(6),
                                KalanIzinGunu = reader.GetInt32(7),
                                Sifre = reader.IsDBNull(8) ? "1234" : reader.GetString(8)
                            });
                        }
                    }
                }
            }
            return personeller;
        }

        public void PersonelEkle(Personel personel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "INSERT INTO Personeller (Ad, Soyad, SicilNo, Departman, Pozisyon, IseGirisTarihi, KalanIzinGunu, Sifre) " +
                    "VALUES (@Ad, @Soyad, @SicilNo, @Departman, @Pozisyon, @IseGirisTarihi, @KalanIzinGunu, @Sifre)", connection))
                {
                    command.Parameters.AddWithValue("@Ad", personel.Ad);
                    command.Parameters.AddWithValue("@Soyad", personel.Soyad);
                    command.Parameters.AddWithValue("@SicilNo", personel.SicilNo);
                    command.Parameters.AddWithValue("@Departman", personel.Departman);
                    command.Parameters.AddWithValue("@Pozisyon", personel.Pozisyon);
                    command.Parameters.AddWithValue("@IseGirisTarihi", personel.IseGirisTarihi);
                    command.Parameters.AddWithValue("@KalanIzinGunu", personel.KalanIzinGunu);
                    command.Parameters.AddWithValue("@Sifre", personel.Sifre ?? "1234");
                    command.ExecuteNonQuery();
                }
            }
        }

        public void PersonelGuncelle(Personel personel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Personeller SET Ad=@Ad, Soyad=@Soyad, SicilNo=@SicilNo, Departman=@Departman, " +
                    "Pozisyon=@Pozisyon, IseGirisTarihi=@IseGirisTarihi, KalanIzinGunu=@KalanIzinGunu, Sifre=@Sifre " +
                    "WHERE Id=@Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", personel.Id);
                    command.Parameters.AddWithValue("@Ad", personel.Ad);
                    command.Parameters.AddWithValue("@Soyad", personel.Soyad);
                    command.Parameters.AddWithValue("@SicilNo", personel.SicilNo);
                    command.Parameters.AddWithValue("@Departman", personel.Departman);
                    command.Parameters.AddWithValue("@Pozisyon", personel.Pozisyon);
                    command.Parameters.AddWithValue("@IseGirisTarihi", personel.IseGirisTarihi);
                    command.Parameters.AddWithValue("@KalanIzinGunu", personel.KalanIzinGunu);
                    command.Parameters.AddWithValue("@Sifre", personel.Sifre ?? "1234");
                    command.ExecuteNonQuery();
                }
            }
        }

        public void PersonelSil(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Personeller WHERE Id=@Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Izin> GetIzinler()
        {
            var izinler = new List<Izin>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT i.*, p.Ad as PersonelAd, p.Soyad as PersonelSoyad FROM Izinler i " +
                    "INNER JOIN Personeller p ON i.PersonelId = p.Id", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            izinler.Add(new Izin
                            {
                                Id = reader.GetInt32(0),
                                PersonelId = reader.GetInt32(1),
                                BaslangicTarihi = reader.GetDateTime(2),
                                BitisTarihi = reader.GetDateTime(3),
                                IzinTuru = reader.GetString(4),
                                Aciklama = reader.GetString(5),
                                Durum = reader.GetString(6),
                                TalepTarihi = reader.GetDateTime(7),
                                OnaylayanKisi = reader.IsDBNull(8) ? null : reader.GetString(8),
                                OnayTarihi = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                                PersonelAd = reader.GetString(10),
                                PersonelSoyad = reader.GetString(11)
                            });
                        }
                    }
                }
            }
            return izinler;
        }

        public void IzinEkle(Izin izin)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "INSERT INTO Izinler (PersonelId, BaslangicTarihi, BitisTarihi, IzinTuru, Aciklama, Durum, TalepTarihi) " +
                    "VALUES (@PersonelId, @BaslangicTarihi, @BitisTarihi, @IzinTuru, @Aciklama, @Durum, @TalepTarihi)", connection))
                {
                    command.Parameters.AddWithValue("@PersonelId", izin.PersonelId);
                    command.Parameters.AddWithValue("@BaslangicTarihi", izin.BaslangicTarihi);
                    command.Parameters.AddWithValue("@BitisTarihi", izin.BitisTarihi);
                    command.Parameters.AddWithValue("@IzinTuru", izin.IzinTuru);
                    command.Parameters.AddWithValue("@Aciklama", izin.Aciklama);
                    command.Parameters.AddWithValue("@Durum", izin.Durum);
                    command.Parameters.AddWithValue("@TalepTarihi", izin.TalepTarihi);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void IzinGuncelle(Izin izin)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Izinler SET Durum=@Durum, OnaylayanKisi=@OnaylayanKisi, OnayTarihi=@OnayTarihi " +
                    "WHERE Id=@Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", izin.Id);
                    command.Parameters.AddWithValue("@Durum", izin.Durum);
                    command.Parameters.AddWithValue("@OnaylayanKisi", (object)izin.OnaylayanKisi ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OnayTarihi", (object)izin.OnayTarihi ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
} 