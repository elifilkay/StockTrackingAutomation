using StockTrackingAutomation.Models;
using System;
using System.Data.Entity; // Include metodu için gerekli
using System.Linq;
using System.Web.Mvc;

public class UrunController : Controller
{
    private StockDbContext db = new StockDbContext();

    // Ürünler Listesi
    public ActionResult Index()
    {
        var urunler = db.Urunler.Include(u => u.Kategoriler).ToList(); // Include metodu için lambda ifadesi kullanıldı
        return View(urunler);
    }

    // Yeni Ürün Ekleme
    [HttpGet]
    public ActionResult Ekle()
    {
        ViewBag.Kategoriler = new SelectList(db.Kategoriler, "KategoriId", "KategoriAd");
        return View();
    }

    [HttpPost]
    public ActionResult Ekle(Urunler urun)
    {
        if (ModelState.IsValid)
        {
            db.Urunler.Add(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Kategoriler = new SelectList(db.Kategoriler, "KategoriId", "KategoriAd", urun.KategoriId);
        return View(urun);
    }

    [HttpGet]
    public ActionResult Sil(int id)
    {
        var urun = db.Urunler.Find(id); // ID'yi kullanarak ürünü buluyoruz
        if (urun == null)
        {
            // Eğer ürün bulunamazsa kullanıcıya mesaj gösteriyoruz
            TempData["ErrorMessage"] = "Silinecek ürün bulunamadı.";
            return RedirectToAction("Index");
        }
        return View(urun); // Ürünü silmek için onay sayfası gösteriliyor
    }
    [HttpPost, ActionName("Sil")]
    public ActionResult SilOnay(int id)
    {
        try
        {
            var urun = db.Urunler.Find(id);
            if (urun != null)
            {
                // Ürünün Satışlar tablosunda kullanılıp kullanılmadığını kontrol et
                var satisVarMi = db.Satislar.Any(s => s.UrunId == id);
                if (satisVarMi)
                {
                    TempData["ErrorMessage"] = "Bu ürün satılmış olduğu için silinemez.";
                    return RedirectToAction("Index");
                }

                // Eğer ürün satışta kullanılmamışsa, sil
                db.Urunler.Remove(urun);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Silinecek ürün bulunamadı.";
            }
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    // Ürün Güncelleme
    [HttpGet]
    public ActionResult Guncelle(int id)
    {
        var urun = db.Urunler.Find(id);
        if (urun == null)
        {
            return HttpNotFound();
        }
        ViewBag.Kategoriler = new SelectList(db.Kategoriler, "KategoriId", "KategoriAd", urun.KategoriId);
        return View(urun);
    }

    [HttpPost]
    public ActionResult Guncelle(Urunler urun)
    {
        if (ModelState.IsValid)
        {
            db.Entry(urun).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Kategoriler = new SelectList(db.Kategoriler, "KategoriId", "KategoriAd", urun.KategoriId);
        return View(urun);
    }
}
