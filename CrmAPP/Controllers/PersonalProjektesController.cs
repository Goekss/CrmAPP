using ClosedXML.Excel;
using CrmAPP.Models.DataContext;
using CrmAPP.Models.Kunden;
using CrmAPP.Models.Personal;
using CrmAPP.Models.Projekt;
using CsvHelper;
using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace CrmAPP.Controllers
{
    [Authorize]
    public class PersonalProjektesController : BaseController
    {
        private new DBc_Context db = new DBc_Context();

        // GET: PersonalProjektes
        public ActionResult Index()
        {
            var projektlisten = db.PersonalProjektes
                .Include(k => k.KundenDatens)
                .Include(p => p.PersonalAngabens)
                .Include(p => p.Ansprechpartner)
                .ToList();
            return View(projektlisten);
        }

        [HttpGet]
        public ActionResult Create()
        {
            FillViewBags(null, new List<int>(), new List<int>());
            return View(new PersonalProjekte());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonalProjekte model, int? SelectedKundenId, List<int> SelectedPersonalIds, List<int> SelectedAnsprechpartnerIds)
        {
            // Validation
            if (SelectedPersonalIds == null || SelectedPersonalIds.Count == 0)
                ModelState.AddModelError("", "Bitte wählen Sie mindestens einen Mitarbeiter aus.");

            if (!SelectedKundenId.HasValue || SelectedKundenId.Value == 0)
                ModelState.AddModelError("", "Bitte wählen Sie einen Kunden aus.");

            if (SelectedAnsprechpartnerIds == null || SelectedAnsprechpartnerIds.Count == 0)
                ModelState.AddModelError("", "Bitte wählen Sie mindestens einen Ansprechpartner aus.");

            if (!ModelState.IsValid)
            {
                FillViewBags(SelectedKundenId, SelectedPersonalIds, SelectedAnsprechpartnerIds);
                return View(model);
            }

            // Personelleri ekle
            model.PersonalAngabens = new List<PersonalAngaben>();
            if (SelectedPersonalIds != null)
            {
                foreach (var pid in SelectedPersonalIds)
                {
                    var personel = db.PersonalAngabens.Find(pid);
                    if (personel != null)
                        model.PersonalAngabens.Add(personel);
                }
            }

            // Müşteri ekle
            model.KundenDatens = new List<KundenDaten>();
            if (SelectedKundenId.HasValue)
            {
                var musteri = db.KundenDatens.Find(SelectedKundenId.Value);
                if (musteri != null)
                    model.KundenDatens.Add(musteri);
            }

            // Ansprechpartner ekle
            model.Ansprechpartner = new List<Ansprechpartner>();
            if (SelectedAnsprechpartnerIds != null)
            {
                foreach (var ansId in SelectedAnsprechpartnerIds)
                {
                    var ans = db.Ansprechpartners.Find(ansId);
                    if (ans != null)
                        model.Ansprechpartner.Add(ans);
                }
            }

            model.Startdatum = DateTime.Now;
            db.PersonalProjektes.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var personalProjekten = db.PersonalProjektes
                .Include(p => p.KundenDatens)
                .Include(p => p.Ansprechpartner)
                .Include(p => p.PersonalAngabens)
                .FirstOrDefault(p => p.PerProjID == id);

            if (personalProjekten == null)
                return HttpNotFound();

            return View(personalProjekten);
        }

        public ActionResult Edit(int id)
        {
            var projektobjektedit = db.PersonalProjektes
                .Include("KundenDatens")
                .Include("Ansprechpartner")
                .Include("PersonalAngabens")
                .FirstOrDefault(p => p.PerProjID == id);

            if (projektobjektedit == null)
                return HttpNotFound();

            int? selectedKundenId = projektobjektedit.KundenDatens.FirstOrDefault()?.KundenDatenID;
            List<int> selectedPersonalIds = projektobjektedit.PersonalAngabens.Select(a => a.PersonalDatenID).ToList();
            List<int> selectedAnsprechpartnerIds = projektobjektedit.Ansprechpartner.Select(a => a.Id).ToList();

            FillViewBags(selectedKundenId, selectedPersonalIds, selectedAnsprechpartnerIds);


            // Model'e seçili ID'leri de doldurursan View'da işin kolay olur
            projektobjektedit.SelectedPersonalIds = selectedPersonalIds; // Eğer modelde property varsa
            projektobjektedit.SelectedAnsprechpartnerIds = selectedAnsprechpartnerIds;

            return View(projektobjektedit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonalProjekte model, int? SelectedKundenId, List<int> SelectedPersonalIds, List<int> SelectedAnsprechpartnerIds)
        {
            var proDbObj = db.PersonalProjektes
                .Include("KundenDatens")
                .Include("Ansprechpartner")
                .Include("PersonalAngabens")
                .FirstOrDefault(p => p.PerProjID == model.PerProjID);

            if (proDbObj == null)
                return HttpNotFound();
            // Validation

            if (!ModelState.IsValid)
            {
                // --- DÜZELTME: Navigation property'leri elle doldur! ---
                if (SelectedKundenId.HasValue)
                    model.KundenDatens = new List<KundenDaten> { db.KundenDatens.Find(SelectedKundenId.Value) };
                else
                    model.KundenDatens = new List<KundenDaten>();

                if (SelectedAnsprechpartnerIds != null)
                    model.Ansprechpartner = db.Ansprechpartners.Where(a => SelectedAnsprechpartnerIds.Contains(a.Id)).ToList();
                else
                    model.Ansprechpartner = new List<Ansprechpartner>();

                if (SelectedPersonalIds != null)
                    model.PersonalAngabens = db.PersonalAngabens.Where(p => SelectedPersonalIds.Contains(p.PersonalDatenID)).ToList();
                else
                    model.PersonalAngabens = new List<PersonalAngaben>();

                FillViewBags(SelectedKundenId, SelectedPersonalIds, SelectedAnsprechpartnerIds);
                return View(model);
            }

            proDbObj.Erklärung = model.Erklärung;
            proDbObj.Projektname = model.Projektname;
            proDbObj.ProjektStatus = model.ProjektStatus;
            proDbObj.Priorität = model.Priorität;

            if (model.ProjektStatus == 100 && model.Fertigstellen)
            {
                proDbObj.Fertigstellen = true;
                proDbObj.Enddatum = DateTime.Now;
            }
            else
            {
                proDbObj.Fertigstellen = false;
                proDbObj.Enddatum = null;
            }

            // Müşteri ilişkisini güncelle
            proDbObj.KundenDatens.Clear();
            if (SelectedKundenId.HasValue)
            {
                var musteri = db.KundenDatens.Find(SelectedKundenId.Value);
                if (musteri != null)
                    proDbObj.KundenDatens.Add(musteri);
            }

            // Ansprechpartner ilişkisini güncelle
            proDbObj.Ansprechpartner.Clear();
            if (SelectedAnsprechpartnerIds != null)
            {
                foreach (var aid in SelectedAnsprechpartnerIds)
                {
                    var ans = db.Ansprechpartners.Find(aid);
                    if (ans != null)
                        proDbObj.Ansprechpartner.Add(ans);
                }
            }

            // Personal ilişkisini güncelle
            proDbObj.PersonalAngabens.Clear();
            if (SelectedPersonalIds != null)
            {
                foreach (var pid in SelectedPersonalIds)
                {
                    var personel = db.PersonalAngabens.Find(pid);
                    if (personel != null)
                        proDbObj.PersonalAngabens.Add(personel);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var personalProjekte = db.PersonalProjektes
                .Include("KundenDatens")
                .Include("Ansprechpartner")
                .Include("PersonalAngabens")
                .FirstOrDefault(p => p.PerProjID == id);

            if (personalProjekte == null)
                return HttpNotFound();

            return View(personalProjekte);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var personalProjekte = db.PersonalProjektes
                .Include("Ansprechpartner")
                .Include("PersonalAngabens")
                .Include("KundenDatens")
                .FirstOrDefault(p => p.PerProjID == id);

            if (personalProjekte == null)
                return HttpNotFound();

            personalProjekte.Ansprechpartner.Clear();
            personalProjekte.PersonalAngabens.Clear();
            personalProjekte.KundenDatens.Clear();

            db.PersonalProjektes.Remove(personalProjekte);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetAnsprechpartnerByKunde(int kundenId)
        {
            var ansprechpartnerListe = db.Ansprechpartners
                .Where(a => a.KundenDatenID == kundenId)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Tel,
                    a.Mail,
                    a.Filiale
                })
                .ToList();

            return Json(ansprechpartnerListe, JsonRequestBehavior.AllowGet);
        }


        // -------Import anfang--------

        // GET: /PersonalProjektes/Import
        // Proje import sayfasını gösterir.
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Import()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Import(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                TempData["ErrorMessage"] = "Bitte wählen Sie eine Datei aus.";
                return RedirectToAction("Import");
            }

            if (!excelFile.FileName.EndsWith(".xlsx"))
            {
                TempData["ErrorMessage"] = "Bitte laden Sie eine gültige Excel-Datei (.xlsx) hoch.";
                return RedirectToAction("Import");
            }

            var neueProjekte = new List<PersonalProjekte>();
            var fehlerListe = new List<string>();
            int zeile = 0;

            try
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    var sheet = workbook.Worksheet(1);
                    if (sheet.LastRowUsed() == null)
                    {
                        TempData["ErrorMessage"] = "Die Excel-Datei ist leer.";
                        return RedirectToAction("Import");
                    }

                    var headerRow = sheet.Row(1);

                    var targetFields = new[] {
                "projektname", "kunde", "erklärung", "priorität",
                "startdatum", "enddatum", "projektstatus", "personal"
            };

                    var spaltenIndex = new Dictionary<string, int>();

                    foreach (var cell in headerRow.CellsUsed())
                    {
                        var text = cell.Value.ToString().Trim().ToLower();
                        var fuzzy = targetFields
                            .Select(f => new { Feld = f, Score = FuzzySharp.Fuzz.Ratio(text, f) })
                            .OrderByDescending(f => f.Score)
                            .FirstOrDefault();

                        if (fuzzy != null && fuzzy.Score >= 85)
                            spaltenIndex[fuzzy.Feld] = cell.Address.ColumnNumber;
                    }

                    TempData["PreviewMapping"] = spaltenIndex.ToDictionary(kv => kv.Key, kv => headerRow.Cell(kv.Value).GetValue<string>());

                    if (!spaltenIndex.ContainsKey("projektname") || !spaltenIndex.ContainsKey("kunde"))
                    {
                        TempData["ErrorMessage"] = "Erforderliche Spalten fehlen (Projektname, Kunde).";
                        return RedirectToAction("Import");
                    }

                    int letzteZeile = sheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= letzteZeile; i++)
                    {
                        zeile = i;
                        var row = sheet.Row(i);
                        try
                        {
                            string projektname = row.Cell(spaltenIndex["projektname"]).GetValue<string>().Trim();
                            string kundeName = row.Cell(spaltenIndex["kunde"]).GetValue<string>().Trim();

                            if (string.IsNullOrWhiteSpace(projektname)) projektname = "Keine Daten";
                            if (string.IsNullOrWhiteSpace(kundeName)) kundeName = "Keine Daten";

                            if (string.IsNullOrWhiteSpace(projektname) || string.IsNullOrWhiteSpace(kundeName))
                            {
                                fehlerListe.Add($"Zeile {zeile}: Projektname oder Kundenname fehlen. Übersprungen.");
                                continue;
                            }

                            var kunde = db.KundenDatens.FirstOrDefault(k => k.Unternehmen.Equals(kundeName, StringComparison.OrdinalIgnoreCase));
                            if (kunde == null)
                            {
                                fehlerListe.Add($"Zeile {zeile}: Kunde '{kundeName}' nicht gefunden. Übersprungen.");
                                continue;
                            }

                            if (db.PersonalProjektes.Any(p => p.Projektname == projektname && p.KundenDatens.Any(k => k.KundenDatenID == kunde.KundenDatenID)))
                            {
                                fehlerListe.Add($"Zeile {zeile}: Projekt '{projektname}' für Kunde '{kundeName}' existiert bereits. Übersprungen.");
                                continue;
                            }

                            var projekt = new PersonalProjekte
                            {
                                Projektname = projektname,
                                Erklärung = row.Cell(spaltenIndex["erklärung"]).GetValue<string>() ?? "Keine Daten",
                                Priorität = row.Cell(spaltenIndex["priorität"]).GetValue<string>() ?? "Keine Daten",
                                Startdatum = DateTime.Now,
                                KundenDatens = new List<KundenDaten> { kunde }
                            };

                            if (spaltenIndex.ContainsKey("projektstatus"))
                            {
                                var statusStr = row.Cell(spaltenIndex["projektstatus"]).GetValue<string>().Replace("%", "").Trim();
                                if (int.TryParse(statusStr, out int status)) projekt.ProjektStatus = status;
                            }

                            if (spaltenIndex.ContainsKey("enddatum") && projekt.ProjektStatus == 100)
                            {
                                var endStr = row.Cell(spaltenIndex["enddatum"]).GetValue<string>();
                                if (DateTime.TryParse(endStr, out DateTime enddatum))
                                {
                                    projekt.Enddatum = enddatum;
                                    projekt.Fertigstellen = true;
                                }
                            }

                            if (spaltenIndex.ContainsKey("personal"))
                            {
                                var namen = row.Cell(spaltenIndex["personal"]).GetValue<string>();
                                if (!string.IsNullOrWhiteSpace(namen))
                                {
                                    var liste = namen.Split(',').Select(n => n.Trim()).ToList();
                                    projekt.PersonalAngabens = new List<PersonalAngaben>();
                                    foreach (var name in liste)
                                    {
                                        var p = db.PersonalAngabens.FirstOrDefault(x => (x.Vorname + " " + x.Nachname).Equals(name, StringComparison.OrdinalIgnoreCase));
                                        if (p != null) projekt.PersonalAngabens.Add(p);
                                        else fehlerListe.Add($"Zeile {zeile}: Personal '{name}' nicht gefunden.");
                                    }
                                }
                            }

                            neueProjekte.Add(projekt);
                        }
                        catch (Exception ex)
                        {
                            fehlerListe.Add($"Zeile {zeile}: Fehler beim Verarbeiten. {ex.Message}");
                        }
                    }

                    if (neueProjekte.Any())
                    {
                        db.PersonalProjektes.AddRange(neueProjekte);
                        db.SaveChanges();
                    }

                    TempData["SuccessMessage"] = $"{neueProjekte.Count} Projekte erfolgreich importiert.";
                    if (fehlerListe.Any()) TempData["ErrorList"] = fehlerListe;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Fehler beim Import: " + ex.Message;
            }

            return RedirectToAction("Import");
        }



        //-------Import ende--------






        public ActionResult Beenden(int id)
        {
            var projeobjekt3 = db.PersonalProjektes.Find(id);
            if (projeobjekt3 == null)
                return HttpNotFound();

            if (projeobjekt3.ProjektStatus != 100)
            {
                projeobjekt3.Fertigstellen = false;
                projeobjekt3.Enddatum = null;
            }
            else
            {
                projeobjekt3.Fertigstellen = true;
                projeobjekt3.Enddatum = DateTime.Now;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // KULLANIMI KOLAY OLSUN DİYE: VIEWBAG DOLDURMA FONKSİYONU
        private void FillViewBags(int? SelectedKundenId, List<int> SelectedPersonalIds, List<int> SelectedAnsprechpartnerIds)
        {
            var personInfo = db.PersonalAngabens.Select(p => new
            {
                PersonalDatenID = p.PersonalDatenID,
                vollname = p.Vorname + " " + p.Nachname
            }).ToList();

            var kundenInfo = db.KundenDatens.Select(k => new
            {
                k.KundenDatenID,
                k.Unternehmen
            }).ToList();

            ViewBag.PersonalDatenID = new SelectList(personInfo, "PersonalDatenID", "vollname", SelectedPersonalIds);
            ViewBag.KundenDatenID = new SelectList(kundenInfo, "KundenDatenID", "Unternehmen", SelectedKundenId);

            // GÜNCELLENEN KISIM! AnsprechpartnerList her zaman doldurulmalı!
            ViewBag.AnsprechpartnerList = (SelectedKundenId.HasValue && SelectedKundenId.Value != 0)
                ? db.Ansprechpartners.Where(a => a.KundenDatenID == SelectedKundenId.Value).ToList()
                : new List<Ansprechpartner>();

            ViewBag.SelectedPersonalIds = SelectedPersonalIds ?? new List<int>();
            ViewBag.SelectedKundenId = SelectedKundenId;
            ViewBag.SelectedAnsprechpartnerIds = SelectedAnsprechpartnerIds ?? new List<int>();
        }
    }
}