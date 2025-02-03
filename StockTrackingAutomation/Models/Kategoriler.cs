namespace StockTrackingAutomation.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Kategoriler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoriler()
        {
            this.Urunler = new HashSet<Urunler>();
        }

        public int KategoriId { get; set; }
        public string KategoriAd { get; set; }

        // Kategorilere ait ürünler koleksiyonu
        public virtual ICollection<Urunler> Urunler { get; set; }
    }
}
