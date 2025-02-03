using StockTrackingAutomation.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace StockTrackingAutomation.Controllers
{
    public class KategoriController : Controller
    {
        private StockDbContext db = new StockDbContext();

        // Kategoriler Listesi
        public ActionResult Index()
        {
            var kategoriler = db.Kategoriler.ToList();
            return View(kategoriler);
        }

        // Yeni Kategori Ekleme
        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Kategoriler kategori)
        {
            if (ModelState.IsValid)
            {
                db.Kategoriler.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        public ActionResult Sil(int id)
        {
            try
            {
                // Kategoriyi bul
                var kategori = db.Kategoriler.Find(id);

                if (kategori == null)
                {
                    TempData["ErrorMessage"] = "Kategori bulunamadı.";
                    return RedirectToAction("Index");
                }

                // Kategoriye bağlı ürünleri kontrol et
                var urunler = db.Urunler.Where(u => u.KategoriId == id).ToList();

                if (urunler.Any()) // Eğer kategoriye bağlı ürün varsa
                {
                    // Kullanıcıya kategoriye bağlı ürünler olduğu için silme işlemi yapılamaz mesajı ver
                    TempData["ErrorMessage"] = "Bu kategoriye bağlı ürünler bulunduğundan dolayı silme işlemi yapılamaz.";
                }
                else
                {
                    // Kategoriyi sil
                    db.Kategoriler.Remove(kategori);
                    db.SaveChanges(); // Veritabanı değişikliklerini kaydediyoruz

                    // Başarı mesajı
                    TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Hata durumunda exception mesajı göster
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

     

            // Kategori Güncelleme
            [HttpGet]
        public ActionResult Guncelle(int id)
        {
            var kategori = db.Kategoriler.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        [HttpPost]
        public ActionResult Guncelle(Kategoriler kategori)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Kategoriler.Attach(kategori);
                    db.Entry(kategori).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Concurrency hatası için yönetim
                    var entry = ex.Entries.Single();
                    var clientValues = (Kategoriler)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The category was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Kategoriler)databaseEntry.ToObject();

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, click the Save button again.");

                        // Güncel verileri model üzerinde güncelleme
                        kategori.KategoriAd = databaseValues.KategoriAd;
                    }
                }
            }
            return View(kategori);
        }
    }
}
