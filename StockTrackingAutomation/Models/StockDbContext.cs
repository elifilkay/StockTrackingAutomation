using System.Data.Entity;

namespace StockTrackingAutomation.Models
{
    public class StockDbContext : DbContext
    {
        // StockDbContext'e bağlanmak için connectionString kullanılır.
        public StockDbContext() : base("name=StockTrackingDBEntities") // Web.config'teki connectionString ismi
        {
        }

        // Tablo ile ilişkili DbSet'ler
        public DbSet<Urunler> Urunler { get; set; }
        public DbSet<Musteriler> Musteriler { get; set; }
        public DbSet<Satislar> Satislar { get; set; }
        public DbSet<Kategoriler> Kategoriler { get; set; }

    }
}
