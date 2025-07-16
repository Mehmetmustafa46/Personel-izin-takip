using System;

namespace PersonelIzinTakip
{
    public class Izin
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string IzinTuru { get; set; } // Yıllık İzin, Hastalık İzni, Mazeret İzni vb.
        public string Aciklama { get; set; }
        public string Durum { get; set; } // Beklemede, Onaylandı, Reddedildi
        public DateTime TalepTarihi { get; set; }
        public string OnaylayanKisi { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string PersonelAd { get; set; }
        public string PersonelSoyad { get; set; }

        public int IzinGunuSayisi => (BitisTarihi - BaslangicTarihi).Days + 1;
    }
} 