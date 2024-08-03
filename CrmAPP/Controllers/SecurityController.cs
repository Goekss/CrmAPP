using CrmAPP.Models.DataContext;
using CrmAPP.Models.Personal;
using CrmAPP.Services; // E-posta servisi 
using CrmAPP.ViewModels;
using System;
using System.Linq;
using System.Runtime.Caching; // MemoryCache için ekle
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CrmAPP.Controllers
{

    public class SecurityController : BaseController
    {
        // GET: Security
        private new DBc_Context db = new DBc_Context(); //Verbindungslink/Objekt für Datenbank

        [AllowAnonymous] // Erlauben Sie allen den Zugriff auf die Login-Seite
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {

            // 1️ MODELSTATE KONTROLÜ
            if (!ModelState.IsValid)
                return View(model);


            // 2️ ZÄHLER
            
            var person = db.PersonalAngabens.FirstOrDefault(p => p.Email == model.Email);    
            if (person == null)
            {
                ViewBag.Message = "Email oder Passwort falsch!";
                return View();
            }
            //if (model.RememberMe)
            //{
            //    // Doğrudan oturum aç!
            //    FormsAuthentication.SetAuthCookie(person.Email, true);
            //    // Session’a kullanıcı bilgilerini atayabilirsin
            //    Session[$"benutzerVorName_{person.Email}"] = person.Vorname;
            //    Session[$"benutzerNachName_{person.Email}"] = person.Nachname;
            //    // ...
            //    return RedirectToAction("Index", "Dashboard"); // 2FA yok!
            //}

            //  ist gesperrt?  -> sofort zurückkehren  
            if (person.IsLockedOut)
            {
                ViewBag.Message = "Ihr Konto ist gesperrt. Bitte kontaktieren Sie den Administrator.";
                return View();
            }


            // 3 Passwort Kontrolle
            var hashedPassword = HashPassword(model.Passwort);       
            if (person.Passwort != hashedPassword)
                

            {
                // 3.1 Passwort falsch:  Zähler erhöhen +1
                person.FailedLoginAttempts++;


                // 3.2 Wenn die Anzahl der fehlgeschlagenen Anmeldeversuche 3 erreicht, sperren Sie das Konto
                if (person.FailedLoginAttempts >= 3)
                {
                    person.IsLockedOut = true;
                    person.LockoutTime = DateTime.UtcNow;
                    db.SaveChanges(); // Änderungen in der Datenbank speichern


                    ViewBag.Message = "Ihr Konto wurde gesperrt. Bitte kontaktieren Sie den Administrator.";
                    return View(model);

                }

                // 3.3 übrig Versuche-Mesage
                db.SaveChanges(); // Änderungen in der Datenbank speichern

                int kalan = 3 - person.FailedLoginAttempts; // 2 oder 1 kann sein
                ViewBag.Message = $"E-mail oder Passwort ist leider falsch! Sie haben noch übrig {kalan} Versuch.";
                return View(model); // Validasyon hatası varsa view'a geri dön
            }

            // 4️  korrektes Passwort: den Zähler zurücksetzen - Reset
            person.FailedLoginAttempts = 0;
            db.SaveChanges();


           

            // 5 2FA code erstellen & Bestätigungsmail senden
            
            string code = new Random().Next(100000, 999999).ToString();
                Session["VerificationCode"] = code;
                Session["VerificationCodeExpiry"] = DateTime.Now.AddMinutes(5);
                Session["PendingUserEmail"] = person.Email;
                //Session["Remember Me"] = model.RememberMe; // ViewModel'den RememberMe değerini al ve Session'a ata

            var emailService = new EmailService();
                string htmlBody = $@"
                <div style='background:#f3f7fa; padding:30px; border-radius:12px; font-family:Arial, sans-serif;'>
                     <h2 style='color:#006d77; text-align:center;'>Anmeldebestätigungscode</h2>
                     <p style='font-size:16px; color:#333;'>Ihr Bestätigungscode lautet:</p>
                    <div style='font-size:32px; font-weight:bold; background:#e9ecef; padding:20px; border-radius:8px; text-align:center; color:#0a9396;'>{code}</div>
                     <p style='font-size:14px; color:#555; text-align:center; margin-top:20px;'>Der Code ist für 5 Minuten gültig.</p>
                 </div>
                ";
                await emailService.SendEmailAsync(person.Email, "Anmeldebestätigungscode", htmlBody);

                return RedirectToAction("VerifyCode");
            

       
        }


        // GET: Security/VerifyCode
        // Bu metod, onay kodu giriş ekranını kullanıcıya gösterir.
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VerifyCode()
        {
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyCode(CrmAPP.ViewModels.VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Validasyon hatası varsa view'a geri dön
            }

            var code = model.Code;
            var sessionCode = Session["VerificationCode"] as string;
            var expiry = Session["VerificationCodeExpiry"] as DateTime?;
            var email = Session["PendingUserEmail"] as string;

            if (sessionCode == null || expiry == null || DateTime.Now > expiry)
            {
                ViewBag.Message = "Der Code ist abgelaufen oder nicht gefunden. Bitte erneut anmelden.";
                return View(model);
            }

            if (code == sessionCode)
            {
                //var rememberMe = Session["RememberMe"] != null && (bool)Session["RememberMe"];
                FormsAuthentication.SetAuthCookie(email, false);
                var person = db.PersonalAngabens.FirstOrDefault(p => p.Email == email);
                if (person != null)
                {
                    // 📌 Session'a kullanıcı bilgilerini ata
                    Session[$"benutzerVorName_{email}"] = person.Vorname;
                    Session[$"benutzerNachName_{email}"] = person.Nachname;
                    Session[$"benutzerBild_{email}"] = "~/Images/" + (person.Bild ?? "default-avatar.png"); // 📌 ← BURASI EN ÖNEMLİ KISIM
                    Session[$"benutzerAbteilung_{email}"] = person.Abteilung;
                    Session[$"benutzerBerechtigung_{email}"] = person.Berechtigung;
                    Session[$"benutzerRol_{email}"] = person.Rol;
                }
                // Session temizliği
                Session.Remove("VerificationCode");
                Session.Remove("VerificationCodeExpiry");
                Session.Remove("PendingUserEmail");
                //Session.Remove("RememberMe");   

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Message = "Falscher Code!";
                return View(model);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendVerificationCode(string email)
        {
            string code = new Random().Next(100000, 999999).ToString();
            Session["VerificationCode"] = code;
            Session["VerificationCodeExpiry"] = DateTime.Now.AddMinutes(5);

            var emailService = new EmailService();
            await emailService.SendEmailAsync(email, "Anmeldebestätigungscode", $"Ihr Bestätigungscode lautet:<b>{code}</b>");

            ViewBag.Message = "Der Bestätigungscode wurde an Ihre E-Mail-Adresse gesendet!";
            return View("VerifyCode");
        }


        public ActionResult Logout()
        {
            var email = User.Identity.Name; // Kullanıcının e-posta adresini alıyoruz.

            // Kullanıcı bilgilerini Session'dan temizliyoruz.
            FormsAuthentication.SignOut(); // Kullanıcıyı oturumdan çıkartıyoruz. && Wir melden den Benutzer ab.

            Session.Clear();


            // Çerezleri temizliyoruz.
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1); // Çerezi geçersiz kılıyoruz.
                Response.Cookies.Add(authCookie); // Çerezi geri gönderiyoruz.
            }

            // Cache temizleme && Cache löschen
            Response.Cache.SetCacheability(HttpCacheability.NoCache);  // Caching'i devre dışı bırakıyoruz.&& Deaktivieren Sie das Caching.
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));  // Cache'den eski verileri almasını engelliyoruz.&& Wir verhindern, dass der Cache alte Daten abruft.
            Response.Cache.SetNoStore(); // Hiçbir veriyi saklamaması için cache'i siliyoruz.&& Wir löschen den Cache, um keine Daten zu speichern.

            return RedirectToAction("Login"); // Kullanıcıyı giriş sayfasına yönlendiriyoruz. && Wir leiten den Benutzer zur Anmeldeseite weiter.
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

//Hinweis;

// ---DİKKAT / NOTLAR---
// 1. Şifre kontrolü hash ile yapılmalı, plain text asla kullanılmamalı! (Güvenlik açığı!) + erledigt
// 2. Session'da deneme sayısını sadece email ile tutmak brute-force için yeterli koruma sağlamaz.  + 
// 3. Session anahtarlarını e-posta bazlı tutmak çoğu durumda gereksiz, tek kullanıcı varsa direkt Session["benutzerVorName"] gibi kullanabilirsin. +
// 4. Logout'ta tüm Session'ı temizlemek için Session.Clear() daha güvenli.+
// 5. ViewBag.Message yerine ModelState.AddModelError ile hata mesajı göstermek daha iyi bir pratik. - 
// 6. [HttpPost] Login metodunda ModelState.IsValid kontrolü eklenmeli (aksi halde validation atlanır).+
// 7. PersonalAngaben parametresi null gelebilir, null kontrolü eklenmeli.
// 8. Controller'da try-catch yok, database erişimi veya session işlemlerinde hata olursa exception fırlayabilir.
// 9. Auth cookie'sinin HttpOnly ve Secure flag'leri kontrol edilmeli (çerez güvenliği için).



/* CODE-ERSTELLEN & Onay kodu üretimi 
 * Kodu string olarak saklıyoruz çünkü int olarak saklarsak baştaki sıfır kaybolabilir.
 * Bu, kullanıcının girdiği kod ile sistemde tutulan kodun tam olarak eşleşmesini sağlar.
 * In dieser Weise wird ein 6-stelliger Code erzeugt, der Zahlen und Buchstaben enthalten kann.
 * Wenn wir int benutzen, geht die führende Null verloren; string bewahrt führende Nullen und unterstützt später auch Buchstaben-Codes.
 * Kullanıcı inputu: "023481"
 * int olarak alırsan: 23481
 * string olarak alırsan: "023481"(doğru eşleşir)
 * string code = "023481"; // Başında sıfır var
 * int codeInt = int.Parse(code); // 23481 olur, baştaki sıfır kaybolur
 */

