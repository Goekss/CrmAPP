using CrmAPP.Models.DataContext;
using CrmAPP.Models.Personal;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.Ajax.Utilities;

namespace CrmAPP.Controllers
{
    public class BaseController : Controller
    {
        protected DBc_Context db = new DBc_Context();

        // Yeni: oturum açmış kullanıcının ID'si ve model nesnesi
        protected int? CurrentPersonalId { get; private set; }
        protected PersonalAngaben CurrentPerson { get; private set; }

        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Sayfa cache'ini devre dışı bırak & Sayfa önbelleğini temizle  //Deaktivieren Sie den Seiten-Cache & Löschen Sie den Seiten-Cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetNoStore();

            // Varsayılanları sıfırla
            CurrentPersonalId = null;
            CurrentPerson = null;

            if (User?.Identity?.IsAuthenticated == true)
            {
                // Öncelikle claim içinden e-posta almayı dene, yoksa Identity.Name kullan
                var cp = User as ClaimsPrincipal;
                var email = cp?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                            ?? User.Identity.Name;

                if (!string.IsNullOrWhiteSpace(email))
                {
                    var normalizedEmail = email.Trim().ToLowerInvariant();

                    // Veritabanından kullanıcıyı al (okuma amaçlı AsNoTracking)
                    CurrentPerson = db.PersonalAngabens
                                      .AsNoTracking()
                                      .FirstOrDefault(p => p.Email.ToLower() == normalizedEmail);

                    if (CurrentPerson != null)
                    {
                        CurrentPersonalId = CurrentPerson.PersonalDatenID;

                        // ViewBag'e de koy (gerekirse viewlarda kullanılmak üzere)
                        ViewBag.benutzerVorname = CurrentPerson.Vorname ?? "Default Vorname";
                        ViewBag.benutzerNachname = CurrentPerson.Nachname ?? "Default Nachname";
                        ViewBag.benutzerAbteilung = CurrentPerson.Abteilung ?? "Default Abteilung";
                        ViewBag.benutzerBerechtigung = Session[$"benutzerBerechtigung_{normalizedEmail}"] ?? "Default Berechtigung";
                        ViewBag.benutzerBild = CurrentPerson.Bild ?? Session[$"benutzerBild_{normalizedEmail}"] ?? "~/Images/default-avatar.png";
                        ViewBag.benutzerId = CurrentPersonalId;
                    }
                    else
                    {
                        // Eğer veritabanında bulunamazsa eski Session tabanlı fallback kullanılabilir
                        ViewBag.benutzerVorname = Session[$"benutzerVorName_{email}"] ?? "Default Vorname";
                        ViewBag.benutzerNachname = Session[$"benutzerNachName_{email}"] ?? "Default Nachname";
                        ViewBag.benutzerAbteilung = Session[$"benutzerAbteilung_{email}"] ?? "Default Abteilung";
                        ViewBag.benutzerBerechtigung = Session[$"benutzerBerechtigung_{email}"] ?? "Default Berechtigung";
                        ViewBag.benutzerBild = Session[$"benutzerBild_{email}"] ?? "~/Images/default-avatar.png";
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}