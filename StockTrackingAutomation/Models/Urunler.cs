namespace StockTrackingAutomation.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Urunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urunler()
        {
            this.Satislar = new HashSet<Satislar>();
        }

        public int UrunId { get; set; }
        public string UrunAd { get; set; }

        // Nullable KategoriId: Bu alan isteðe baðlý
        public int? KategoriId { get; set; }

        public decimal? Fiyat { get; set; }
        public string Marka { get; set; }
        public int? Stok { get; set; }

        // Kategori iliþkisinin opsiyonel olduðunu belirtiyoruz
        public virtual Kategoriler Kategoriler { get; set; }
        public virtual ICollection<Satislar> Satislar { get; set; }
    }
}
