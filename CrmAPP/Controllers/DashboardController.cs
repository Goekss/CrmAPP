using CrmAPP.Models.DataContext;
using CrmAPP.ViewModels;            // KundeKontaktVM burada (ViewModels’te)
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace CrmAPP.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private new DBc_Context db = new DBc_Context();

        public ActionResult Index()
        {
            ViewBag.benutzerVorName = Session["benutzerVorName"];
            ViewBag.benutzerNachName = Session["benutzerNachName"];
            ViewBag.benutzerBild = Session["benutzerBild"];

            ViewBag.ProjektZahlen = db.PersonalProjektes.Count();

            // DÜZELTME: tek & yerine &&
            ViewBag.FertigteProjektZahlen = db.PersonalProjektes
                .Count(p => p.Fertigstellen == true && (p.Enddatum == null || p.Enddatum <= DateTime.Now));

            ViewBag.high = db.PersonalProjektes.Count(p => p.Priorität == "high");
            ViewBag.middle = db.PersonalProjektes.Count(p => p.Priorität == "middle");
            ViewBag.low = db.PersonalProjektes.Count(p => p.Priorität == "low");

            ViewBag.Laufendeprojekte = db.PersonalProjektes.Count(p => p.Fertigstellen == false);

            return View();
        }

        public ActionResult DasboardInfo()
        {
            // Random 5 Kunde + ilk Ansprechpartner (varsa)
            var kunden5 = db.KundenDatens
                .Include("AnsprechpartnerList")
                .OrderBy(r => Guid.NewGuid())
                .Take(5)
                .Select(k => new KundeKontaktVM
                {
                    Kunde = k.Unternehmen,
                    Kontaktperson = k.AnsprechpartnerList
                        .Select(a => a.Name + (a.Filiale != null ? " (" + a.Filiale + ")" : ""))
                        .FirstOrDefault()
                })
                .ToList();

            // Random 5 proje adı
            var projekte5 = db.PersonalProjektes
                .OrderBy(r => Guid.NewGuid())
                .Take(5)
                .Select(p => p.Projektname)
                .ToList();

            ViewBag.RandomKunden = kunden5;   // List<KundeKontaktVM>
            ViewBag.RandomProjekte = projekte5; // List<string>

            return View(); // View tarafında ViewBag üzerinden okuyacaksın
        }
    }
}
