using CrmAPP.Models.DataContext;
using CrmAPP.Models.Personal;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace CrmAPP.Controllers
{
    public class TestimageController : BaseController
    {
       protected new DBc_Context db = new DBc_Context();
        // Veritabanına bağlanma / Verbindung zur Datenbank herstellen
       
        // GET: Testimage
        public ActionResult Index()
        {
            var personalAngabens = db.PersonalAngabens.ToList(); // Veritabanından tüm kişisel bilgileri alıyoruz / Wir holen alle Personalangaben aus der Datenbank.
            return View(personalAngabens);  // Verileri view'a gönderiyoruz / Wir senden die Daten an die View.
        }

        [HttpGet] // İlk ekleme GET metodu ile çalışacak / Die erste Hinzufügung funktioniert mit der GET-Methode
        public ActionResult BildCreate()
        {
            return View();  // Kullanıcıyı görüntü ekleme sayfasında tutuyoruz && Wir halten den Benutzer auf der Bildhinzufügungsseite
        }



        [HttpPost]
        [ValidateAntiForgeryToken] // Form verilerinin güvenliğini sağlamak için / Sicherstellen, dass das Formular sicher ist.
        public ActionResult BildCreate(PersonalAngaben img, HttpPostedFileBase bildDatei)
        {
            if (ModelState.IsValid)  // Model doğrulaması geçerli mi? / Ist das Modell gültig?
            {
                if (bildDatei != null && bildDatei.ContentLength > 0) // Eğer bir dosya yüklendiyse veya dosya boyutu sıfırdan büyükse / Wenn eine Datei hochgeladen wurde oder die Dateigröße größer als null ist.
                {
                    string fileName = Path.GetFileNameWithoutExtension(bildDatei.FileName);  // Dosya adını uzantısı olmadan alıyoruz / Wir holen den Dateinamen ohne Erweiterung.
                    string extension = Path.GetExtension(bildDatei.FileName);  // Dosyanın uzantısını alıyoruz / Wir holen die Dateierweiterung.
                    fileName = fileName + "_" + System.DateTime.Now.ToString("yymmssfff") + extension; // Dosyaadini benzersiz yapmak için tarih ekliyoruz && Wir fügen das Datum hinzu, um den Dateinamen einzigartig zu machen.
                    string path = Path.Combine(Server.MapPath("~/images/"), fileName);// Dosyanın kaydedileceği yolu oluşturuyoruz / Wir erstellen den Pfad, wo die Datei gespeichert wird.

                    // Klasör oluşturulmuş mu? Değilse oluştur && Ist der Ordner erstellt? Wenn nicht, erstellen
                    var folderPath = Server.MapPath("~/images/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);  // Klasörü oluşturuyoruz / Wir erstellen den Ordner.
                    }

                    
                    bildDatei.SaveAs(path);            // Dosyayı kaydet && Datei speichern
                    img.Bild = "/images/" + fileName; // Veritabanında saklamak için dosyanın yolunu atıyoruz / Wir speichern den Pfad der Datei in der Datenbank.
                    db.PersonalAngabens.Add(img);    // Yeni resmi veritabanına ekliyoruz / Wir fügen das Bild zur Datenbank hinzu.
                    db.SaveChanges();               // Değişiklikleri kaydediyoruz / Wir speichern die Änderungen.

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Bild", "Es wurde nicht hochgeladen");// Hata mesajı ekliyoruz, eğer dosya yüklenmediyse / Wir fügen eine Fehlermeldung hinzu, wenn keine Datei hochgeladen wurde.
                }
            }

            return View(img);// Eğer model geçerli değilse, kullanıcı yükleme sayfasında kalır ve JavaScript ile uyarılır / Wenn das Modell nicht gültig ist, bleibt der Benutzer auf der Upload-Seite und wird mit JavaScript benachrichtigt !!
        }
    }
}