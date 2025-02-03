using StockTrackingAutomation.Models;
using System.Data.Entity; // Include metodu için gerekli
using System.Linq;
using System.Web.Mvc;

public class SatisController : Controller
{
    private StockDbContext db = new StockDbContext();

    // Satışlar Listesi
    public ActionResult Index()
    {
        var satislar = db.Satislar
            .Include(s => s.Urunler) // Urun yerine Urunler kullanıldı
            .Include(s => s.Musteriler) // Musteri yerine Musteriler kullanıldı
            .Select(s => new SatisViewModel // ViewModel kullanıldı
            {
                SatisId = s.SatisId,
                MusteriAd = s.Musteriler.Ad + " " + s.Musteriler.Soyad,
                UrunAd = s.Urunler.UrunAd,
                Miktar = s.Miktar,
                ToplamTutar = s.Miktar * s.Urunler.Fiyat
            })
            .ToList();

        return View(satislar);
    }
    public ActionResult Ekle()
    {
        ViewBag.Musteriler = new SelectList(db.Musteriler, "MusteriId", "Ad");
        ViewBag.Urunler = new SelectList(db.Urunler, "UrunId", "UrunAd");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Ekle(Satislar satis)
    {
        if (ModelState.IsValid)
        {
            db.Satislar.Add(satis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(satis);
    }

}

// ViewModel tanımı
public class SatisViewModel
{
    public int SatisId { get; set; }
    public string MusteriAd { get; set; }
    public string UrunAd { get; set; }
    public int? Miktar { get; set; }
    public decimal? ToplamTutar { get; set; }
}

