using StockTrackingAutomation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

public class MusteriController : Controller
{
    private StockDbContext db = new StockDbContext();

    // Müşteriler Listesi
    public ActionResult Index()
    {
        var musteriler = db.Musteriler.ToList();

        // TempData'dan alınan mesajları sadece Müşteri sayfasında göster
        if (TempData["ErrorMessage"] != null)
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
        }
        if (TempData["SuccessMessage"] != null)
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
        }

        return View(musteriler);  // Listeyi View'a gönderiyoruz
    }

    // Yeni Müşteri Ekleme
    [HttpGet]
    public ActionResult Ekle()
    {
        return View();  // Yeni müşteri eklemek için form
    }

    [HttpPost]
    public ActionResult Ekle(Musteriler musteri)
    {
        if (ModelState.IsValid)  // Modelin doğruluğunu kontrol ediyoruz
        {
            db.Musteriler.Add(musteri);
            db.SaveChanges();  // Değişiklikleri kaydediyoruz
            return RedirectToAction("Index");  // Listeye yönlendiriyoruz
        }
        return View(musteri);  // Hata varsa formu tekrar gösteriyoruz
    }

    // Müşteri Silme (Onay için)
    [HttpGet]
    public ActionResult Sil(int id)
    {
        var musteri = db.Musteriler.Find(id);  // Müşteriyi buluyoruz
        if (musteri == null)
        {
            // Eğer müşteri bulunamazsa hata mesajı gösteriyoruz
            TempData["ErrorMessage"] = "Silinecek müşteri bulunamadı.";
            return RedirectToAction("Index");
        }

        // Müşterinin Satışlar tablosunda kullanılıp kullanılmadığını kontrol ediyoruz
        var satisVarMi = db.Satislar.Any(s => s.MusteriId == id);
        if (satisVarMi)
        {
            // Eğer müşteri satılmışsa, silinemez
            TempData["ErrorMessage"] = "Bu müşteri ile ilişkili satışlar olduğu için silinemez.";
            return RedirectToAction("Index");
        }

        return View(musteri);  // Müşteriyi silmek için onay sayfası gösteriyoruz
    }

    // Müşteri Silme - Onay (Post işlemi)
    [HttpPost, ActionName("Sil")]
    public ActionResult SilOnay(int id)
    {
        try
        {
            var musteri = db.Musteriler.Find(id);
            if (musteri != null)
            {
                // Müşteriyi silmeden önce, satışlar tablosunda yer alıp almadığını kontrol ettik
                var satisVarMi = db.Satislar.Any(s => s.MusteriId == id);
                if (satisVarMi)
                {
                    // Eğer müşteri ile ilişkili satış varsa, silme işlemi yapılmaz
                    TempData["ErrorMessage"] = "Bu müşteri ile ilişkili satışlar olduğu için silinemez.";
                    return RedirectToAction("Index");
                }

                // Satışla ilişkili değilse, müşteri kaydını sil
                db.Musteriler.Remove(musteri);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Müşteri başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Silinecek müşteri bulunamadı.";
            }
            return RedirectToAction("Index");  // Listeye yönlendiriyoruz
        }
        catch (Exception ex)
        {
            // Hata oluşursa, hata mesajını gösteriyoruz
            TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    // Müşteri Güncelleme (GET)
    [HttpGet]
    public ActionResult Guncelle(int id)
    {
        // Müşteri bilgilerini buluyoruz
        var musteri = db.Musteriler.Find(id);
        if (musteri == null)
        {
            return HttpNotFound();  // Müşteri bulunamazsa hata döndürüyoruz
        }
        return View(musteri);  // Güncelleme formunu gösteriyoruz
    }

    // Müşteri Güncelleme (POST)
    [HttpPost]
    public ActionResult Guncelle(Musteriler musteri)
    {
        if (ModelState.IsValid)  // Model doğrulaması başarılıysa
        {
            try
            {
                // Güncellenecek müşteriyi buluyoruz
                var mevcutMusteri = db.Musteriler.Find(musteri.MusteriId);
                if (mevcutMusteri == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek müşteri bulunamadı.";
                    return RedirectToAction("Index");
                }

                // Müşteri bilgilerini güncelliyoruz
                mevcutMusteri.Ad = musteri.Ad;
                mevcutMusteri.Soyad = musteri.Soyad;


                db.SaveChanges();  // Değişiklikleri kaydediyoruz
                TempData["SuccessMessage"] = "Müşteri başarıyla güncellendi.";
                return RedirectToAction("Index");  // Listeye yönlendiriyoruz
            }
            catch (Exception ex)
            {
                // Hata oluşursa, kullanıcıya hata mesajı gösteriyoruz
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Model geçerli değilse, formu tekrar gösteriyoruz
        return View(musteri);
    }

}