using CrmAPP.Models;
using CrmAPP.Models.DataContext;
using CrmAPP.Models.Personal;
using CrmAPP.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CrmAPP.Controllers
{
    [Authorize]
    public class MeinProfilController : BaseController
    {
        protected new DBc_Context db = new DBc_Context();

        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var personalData = db.PersonalAngabens.FirstOrDefault(p => p.Email == userName);

            if (personalData == null)
            {
                return HttpNotFound();
            }

            return View(personalData);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userName = User.Identity.GetUserName();
            var user = db.PersonalAngabens.FirstOrDefault(p => p.Email == userName);

            if (user == null)
            {
                TempData["Fehlermeldung"] = "Benutzer nicht gefunden.";
                return RedirectToAction("ChangePassword");
            }

            var aktuelleHash = HashPassword(model.CurrentPassword);
            if (user.Passwort != aktuelleHash)
            {
                TempData["PasswordCheckResult"] = "fehler";
                ModelState.AddModelError("", "Aktuelles Passwort ist falsch.");
                return View(model);
            }

            TempData["PasswordCheckResult"] = "erfolg";
            user.Passwort = HashPassword(model.NewPassword);

            try
            {
                db.SaveChanges();
                System.Web.Security.FormsAuthentication.SignOut();
                TempData["PasswortAenderungErfolg"] = "Das Passwort wurde erfolgreich geändert.";
                ViewBag.PasswortGeaendert = true;
                return View(model);
            }
            catch (Exception)
            {
                TempData["Fehlermeldung"] = "Ein Fehler ist aufgetreten, bitte versuchen Sie es erneut.";
                return View(model);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var personalData = db.PersonalAngabens.FirstOrDefault(p => p.PersonalDatenID == id);

            if (personalData == null)
            {
                return HttpNotFound();
            }

            return View(personalData);
        }

        public ActionResult Edit()
        {
            var userName = User.Identity.GetUserName();
            var personalData = db.PersonalAngabens.FirstOrDefault(p => p.Email == userName);

            if (personalData == null)
            {
                return HttpNotFound();
            }

            return View(personalData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonalAngaben model, HttpPostedFileBase BildUpload) 
        {
            // Kullanıcıyı bul
            var userName = User.Identity.GetUserName();
            var personalData = db.PersonalAngabens.FirstOrDefault(p => p.Email == userName);

            if (personalData == null)
            {
                return HttpNotFound();
            }

            // Sadece isim ve soyisim güncellenecek, diğer zorunlu alanları DB'den al
            // Modeli tekrar doldur, formdan gelmeyen alanlar için DB'deki değerleri kullan
            model.Bild = personalData.Bild;
            model.Email = personalData.Email;
            model.Passwort = personalData.Passwort;
            model.RufNummer = personalData.RufNummer;
            model.Abteilung = personalData.Abteilung;
            model.Aufgaben = personalData.Aufgaben;
            model.Berechtigung = personalData.Berechtigung;
            model.Rol = personalData.Rol;
            model.Hinweise = personalData.Hinweise;

            // --- PROFİL RESMİ YÜKLEME/EKLEME BLOKU ---
            if (BildUpload != null && BildUpload.ContentLength > 0)
            {
                int maxFileSize = 10 * 1024 * 1024; // 10 MB örnek
                if (BildUpload.ContentLength > maxFileSize)
                {
                    ModelState.AddModelError("Bild", "Die Datei ist zu groß. Maximal erlaubte Größe ist 10MB.");
                    return View(model);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(BildUpload.FileName).ToLower();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("Bild", "Nur Bilddateien (.jpg, .jpeg, .png, .gif) sind erlaubt.");
                    return View(model);
                }

                // Eski resmi sil
                var oldImageFileName = personalData.Bild;
                var oldImagePath = string.IsNullOrEmpty(oldImageFileName)
                    ? null
                    : Path.Combine(Server.MapPath("~/images/"), oldImageFileName);

                if (!string.IsNullOrEmpty(oldImageFileName) && System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);

                // Yeni resim ismini benzersiz yap
                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(Server.MapPath("~/images/"), fileName);
                BildUpload.SaveAs(filePath);

                personalData.Bild = fileName;
                model.Bild = fileName; // View’da göstermek için
                Session[$"benutzerBild_{User.Identity.Name}"] = "~/images/" + fileName; //in _Layout um Profibild zu aktualisierung
            }
            // --- PROFİL RESMİ BLOKU BİTTİ ---

            // Sadece Vorname ve Nachname validasyonu yap
            ModelState.Clear();
            TryValidateModel(model, nameof(PersonalAngaben));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Güncelleme işlemi
            personalData.Vorname = model.Vorname;
            personalData.Nachname = model.Nachname;

            try
            {
                db.SaveChanges();
                TempData["ProfilUpdateErfolg"] = "Ihr Profil wurde erfolgreich aktualisiert.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ein Fehler ist aufgetreten, bitte versuchen Sie es erneut.");
                return View(model);
            }
        }







        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
