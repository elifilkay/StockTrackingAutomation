using System;
using System.Linq;
using System.Web.Mvc;
using StockTrackingAutomation.Models;

public class HomeController : Controller
{
    private StockDbContext db = new StockDbContext();

    public ActionResult Index()
    {
        try
        {
            // Veritabanına bağlantı testini basitçe yapmak için, herhangi bir tabloyu sorgulayalım.
            var urunler = db.Urunler.ToList(); // Urunler tablosundaki tüm kayıtları al

            if (urunler.Any())
            {
                ViewBag.Message = "Veritabanına başarıyla bağlantı kuruldu!";
            }
            else
            {
                ViewBag.Message = "Veritabanı boş, ancak bağlantı başarılı!";
            }

            // Toplam ürün, kategori ve müşteri sayısını al
            ViewBag.TotalProducts = db.Urunler.Count();
            ViewBag.TotalCategories = db.Kategoriler.Count();
            ViewBag.TotalCustomers = db.Musteriler.Count();
        }
        catch (Exception ex)
        {
            // Eğer bağlantı kurulamazsa, hata mesajını kullanıcıya ilet
            ViewBag.Message = "Veritabanı bağlantısı başarısız: " + ex.Message;
        }

        return View();
    }
}