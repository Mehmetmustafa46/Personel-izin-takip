using System;

namespace PersonelIzinTakip
{
    public class Personel
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? SicilNo { get; set; }
        public string? Departman { get; set; }
        public string? Pozisyon { get; set; }
        public DateTime IseGirisTarihi { get; set; }
        public int KalanIzinGunu { get; set; }
        public string? Sifre { get; set; }

        public string AdSoyad => $"{Ad} {Soyad}";
    }
} 