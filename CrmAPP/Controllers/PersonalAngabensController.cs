using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using ClosedXML.Excel;
using System.Web.Management;
using System.Web.Mvc;
using CrmAPP.Models.DataContext;
using CrmAPP.Models.Personal;


namespace CrmAPP.Controllers
{
    
    public class PersonalAngabensController : BaseController
    {
        private new DBc_Context db = new DBc_Context();


        [Authorize]
        //--INDEX--
        // GET: PersonalAngabens (Daten vom Server abrufen (Lesevorgang)
        public ActionResult Index()
        {
            return View(db.PersonalAngabens.ToList()); // Daten auflisten von Index
        }

        public ActionResult PersonalCard()
        {
            return View(db.PersonalAngabens.ToList());
        }



        
        //--CREATE--
        // hier gibt es 2 Operationen Get-Post
        // GET:  I) zuerst gibt es GET-Operation (Daten vom Server abrufen (Lesevorgang)
        public ActionResult Create()
        {
            return View();
        }

        //E-Mail kontrolle für doppeltte E-Mail-Adressen

        [HttpPost]
        public JsonResult CheckEmailExists(string email)
        {
            var emailExists = db.PersonalAngabens.Any(p => p.Email == email);
            return Json(!emailExists); // true dönerse email kullanılabilir
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


        // POST: II) als zwiete gibt es POST-Operationen Daten an den Server senden (Schreibvorgang/Hinzufügen)
        [Authorize(Roles = "Admin,Moderator")] //Nur Admin und Moderator können löschen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonalAngaben personalAngaben, HttpPostedFileBase Bild)
        {
            if (ModelState.IsValid)  // wenn es eine Dateneingabe gibt!
            {
                // 📌 Email kontrolle für doppelte E-Mail-Adressen
                var emailExists = db.PersonalAngabens.Any(p => p.Email == personalAngaben.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "⚠ Achtung! Diese E-Mail-Adresse ist bereits vergeben.");
                    return View(personalAngaben);
                }

                if (Bild != null && Bild.ContentLength > 0)
                {
                    // Dosya uzantısını kontrol et && Überprüfen Sie die Dateierweiterung
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(Bild.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Bild", "Nur Bilddateien sind erlaubt.");
                        return View(personalAngaben);
                    }

                    try
                    {
                        // dosya adı oluştur && Eindeutigen Dateinamen erstellen
                        var bild = Guid.NewGuid().ToString() + extension; // 
                        var path = Path.Combine(Server.MapPath("~/Images"), bild); // Dosya yolu && Dateipfad
                        Bild.SaveAs(path);
                        personalAngaben.Bild = bild;
                        personalAngaben.Passwort = HashPassword(personalAngaben.Passwort);
                        db.PersonalAngabens.Add(personalAngaben); // diese Datei hinzufügen
                        db.SaveChanges(); // abspeichern
                        return RedirectToAction("Index");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {
                                ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        ModelState.AddModelError("", "Fehler beim Hochladen des Bildes: Validierungsfehler!");
                    }
                    catch (Exception ex) // Fehlermeldung
                    {
                        ModelState.AddModelError("", "Fehler beim Hochladen des Bildes: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("Bild", "Bild darf nicht leer sein."); // wenn es keine Datei gibt
                }
            }

            return View(personalAngaben);
        }





        //--DETAILS--
        // GET: PersonalAngabens/Details (Daten vom Server abrufen (Lesevorgang)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAngaben personalAngaben = db.PersonalAngabens.Find(id);
            if (personalAngaben == null)
            {
                return HttpNotFound();
            }
            return View(personalAngaben);
        }





        // --EDIT 1--
        // hier gibt es 2 Operationen Get-Post
        // GET: Edit zuerst gibt es GET-Operation Daten vom Server abrufen (Lesevorgang)
        public ActionResult Edit(int? id) // zuerst finde die bearbeitende ID !
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // wenn es keine ID gibt ,zeige Fehlermeldung
            }
            PersonalAngaben personalAngaben = db.PersonalAngabens.Find(id);// finde die ID
            if (personalAngaben == null) // Id null ise hata döndür && Wenn die ID null ist, geben Sie einen Fehler zurück
            {
                return HttpNotFound();// zeige Fehlermeldung
            }
            return View(personalAngaben); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonalAngaben personalAngaben, HttpPostedFileBase Bild)
        {
            var existingPersonalAngaben = db.PersonalAngabens.AsNoTracking()
                                            .FirstOrDefault(p => p.PersonalDatenID == personalAngaben.PersonalDatenID);
            if (existingPersonalAngaben == null)
            {
                return HttpNotFound();
            }

            // --- ÖNCEKİ ÇÖZÜM ADIMLARI ---
            personalAngaben.Passwort = existingPersonalAngaben.Passwort;
            if (Bild == null)
            {
                personalAngaben.Bild = existingPersonalAngaben.Bild;
            }

            // --- YENİ HATA AYIKLAMA ADIMI ---
            // Eğer ModelState geçerli değilse, hataları ayıkla ve View'a gönder.
            if (!ModelState.IsValid)
            {
                // Hataları bir liste olarak topla
                var errorList = new List<string>();
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        // Sadece hata mesajı olanları listeye ekle
                        if (!string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            errorList.Add($"Alan Adı (Property): {modelStateKey} | Hata Mesajı: {error.ErrorMessage}");
                        }
                    }
                }

                // Hata listesini View'a göndermek için ViewBag kullan
                ViewBag.ValidationErrors = errorList;

                // View'a geri dönerken, resmin kaybolmaması için eski resmi tekrar ata
                personalAngaben.Bild = existingPersonalAngaben.Bild;
                return View(personalAngaben);
            }

            // --- GERİ KALAN KOD (ModelState geçerliyse çalışacak kısım) ---

            // E-posta kontrolü
            if (db.PersonalAngabens.Any(p => p.Email == personalAngaben.Email && p.PersonalDatenID != personalAngaben.PersonalDatenID))
            {
                ModelState.AddModelError("Email", "Diese E-Mail-Adresse ist bereits einem anderen Benutzer zugewiesen.");
                personalAngaben.Bild = existingPersonalAngaben.Bild;
                return View(personalAngaben);
            }

            // Yeni resim işlemleri
            if (Bild != null && Bild.ContentLength > 0)
            {
                if (!string.IsNullOrEmpty(existingPersonalAngaben.Bild))
                {
                    var oldImagePath = Path.Combine(Server.MapPath("~/Images"), existingPersonalAngaben.Bild);
                    if (System.IO.File.Exists(oldImagePath)) { System.IO.File.Delete(oldImagePath); }
                }
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(Bild.FileName);
                var newPath = Path.Combine(Server.MapPath("~/Images"), newFileName);
                Bild.SaveAs(newPath);
                personalAngaben.Bild = newFileName;
            }

            // Veritabanını güncelle
            db.Entry(personalAngaben).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //--LÖSCHEN--
        // hier gibt es 2 Operationen Get-Post
        //GET) zuerst gibt es GET-Operation (Daten vom Server abrufen (Lesevorgang)
        [Authorize(Roles ="Admin,Moderator")] //Nur Admin und Moderator können löschen
        public ActionResult Delete(int? id) // zuerst finde die löschende ID !
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAngaben personalAngaben = db.PersonalAngabens.Find(id);
            if (personalAngaben == null)
            {
                return HttpNotFound();
            }
            return View(personalAngaben);
        }


        [Authorize(Roles = "Admin,Moderator")]
        //POST: II) als zwiete gibt es POST-Operationen Daten an den Server senden (Schreibvorgang/Hinzufügen)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalAngaben personalAngaben = db.PersonalAngabens.Find(id);
            db.PersonalAngabens.Remove(personalAngaben); //dann  Lösche die gefundene ID!
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        ////--------------- ANFANG: AKTUALISIERTE EXCEL-IMPORT-METHODEN (DEUTSCH) ---------------

        //// GET: /PersonalAngabens/Import
        //// Zeigt die Seite für den Datei-Upload an.
        //[Authorize(Roles = "Admin")]
        //public ActionResult Import()
        //{
        //    return View();
        //}

        //// POST: /PersonalAngabens/Import
        //// Verarbeitet die hochgeladene Excel-Datei.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public ActionResult Import(HttpPostedFileBase excelFile)
        //{
        //    if (excelFile == null || excelFile.ContentLength == 0)
        //    {
        //        TempData["ErrorMessage"] = "Bitte wählen Sie eine Datei aus."; // <-- DEUTSCH
        //        return RedirectToAction("Import");
        //    }

        //    if (!excelFile.FileName.EndsWith(".xlsx"))
        //    {
        //        TempData["ErrorMessage"] = "Bitte laden Sie eine gültige Excel-Datei (.xlsx) hoch."; // <-- DEUTSCH
        //        return RedirectToAction("Import");
        //    }

        //    var yeniPersonellerListesi = new List<PersonalAngaben>();
        //    var olusanHatalar = new List<string>();
        //    int satirNumarasi = 1; // Beginnt bei 1, da die erste Zeile der Header ist.

        //    try
        //    {
        //        using (var workbook = new XLWorkbook(excelFile.InputStream))
        //        {
        //            var worksheet = workbook.Worksheet(1); // Erstes Arbeitsblatt der Excel-Datei.
        //            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Überspringt die Header-Zeile.

        //            foreach (var row in rows)
        //            {
        //                satirNumarasi++;
        //                try
        //                {
        //                    var email = row.Cell(3).GetValue<string>(); // Spalte C: E-Mail

        //                    // Wenn die E-Mail leer ist, die Zeile überspringen.
        //                    if (string.IsNullOrWhiteSpace(email))
        //                    {
        //                        olusanHatalar.Add($"Zeile {satirNumarasi}: Übersprungen, da das E-Mail-Feld leer ist."); // <-- DEUTSCH
        //                        continue;
        //                    }

        //                    // Prüfen, ob die E-Mail bereits in der Datenbank vorhanden ist.
        //                    if (db.PersonalAngabens.Any(p => p.Email == email))
        //                    {
        //                        olusanHatalar.Add($"Zeile {satirNumarasi}: Die E-Mail-Adresse '{email}' ist bereits registriert. Zeile übersprungen."); // <-- DEUTSCH
        //                        continue;
        //                    }

        //                    // Prüfen, ob das Passwort-Feld leer ist.
        //                    var parola = row.Cell(8).GetValue<string>(); // Spalte H: Passwort
        //                    if (string.IsNullOrWhiteSpace(parola))
        //                    {
        //                        olusanHatalar.Add($"Zeile {satirNumarasi}: Das Passwort-Feld darf nicht leer sein. Zeile übersprungen."); // <-- DEUTSCH
        //                        continue;
        //                    }

        //                    // Ein neues PersonalAngaben-Objekt für jede Zeile erstellen.
        //                    var personel = new PersonalAngaben
        //                    {
        //                        // ID wird nicht aus Excel gelesen, die DB weist sie automatisch zu.
        //                        Vorname = row.Cell(1).GetValue<string>(),      // Spalte A
        //                        Nachname = row.Cell(2).GetValue<string>(),    // Spalte B
        //                        Email = email,                                // Spalte C
        //                        RufNummer = row.Cell(4).GetValue<string>(),   // Spalte D
        //                        Abteilung = row.Cell(5).GetValue<string>(),   // Spalte E
        //                        Berechtigung = row.Cell(6).GetValue<string>(),// Spalte F
        //                        Rol = row.Cell(7).GetValue<string>(),         // Spalte G
        //                        Passwort = HashPassword(parola),              // Spalte H (wird gehasht)

        //                        // Standardwerte für [Required]-Felder, die nicht in Excel sind:
        //                        Bild = "", // Bild kann später hinzugefügt werden.
        //                        Aufgaben = "N/A", // Standardwert
        //                        Hinweise = "Aus Excel importiert", // <-- DEUTSCH

        //                        // Standard-Anfangswerte für neue Benutzer:
        //                        IsLockedOut = false,
        //                        LockoutTime = null,
        //                        FailedLoginAttempts = 0
        //                    };

        //                    yeniPersonellerListesi.Add(personel);
        //                }
        //                catch (Exception ex)
        //                {
        //                    olusanHatalar.Add($"Zeile {satirNumarasi}: Fehler beim Lesen der Daten. Details: {ex.Message}"); // <-- DEUTSCH
        //                }
        //            }
        //        }

        //        // Wenn es importierbare Mitarbeiter gibt, diese gesammelt in der DB speichern.
        //        if (yeniPersonellerListesi.Any())
        //        {
        //            db.Configuration.ValidateOnSaveEnabled = false;
        //            db.PersonalAngabens.AddRange(yeniPersonellerListesi);
        //            db.SaveChanges();
        //            db.Configuration.ValidateOnSaveEnabled = true;
        //        }

        //        TempData["SuccessMessage"] = $"{yeniPersonellerListesi.Count} neue Mitarbeiter wurden erfolgreich importiert."; // <-- DEUTSCH
        //        if (olusanHatalar.Any())
        //        {
        //            TempData["ErrorList"] = olusanHatalar;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "Beim Verarbeiten der Datei ist ein allgemeiner Fehler aufgetreten: " + ex.Message; // <-- DEUTSCH
        //    }

        //    return RedirectToAction("Import");
        //}

        ////--------------- ENDE: AKTUALISIERTE EXCEL-IMPORT-METHODEN (DEUTSCH) ---------------


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unlock(int id)
        {
            var gesperrterBenutzer = db.PersonalAngabens.Find(id);

            if (gesperrterBenutzer == null || !gesperrterBenutzer.IsLockedOut)
            {
                TempData["Message"] = "Der Benutzer ist nicht gesperrt oder existiert nicht.";
                return RedirectToAction("Details", new { id });
            }

            gesperrterBenutzer.IsLockedOut = false;
            gesperrterBenutzer.LockoutTime = null;
            gesperrterBenutzer.FailedLoginAttempts = 0;

            db.Entry(gesperrterBenutzer).Property(p => p.IsLockedOut).IsModified = true;
            db.Entry(gesperrterBenutzer).Property(p => p.LockoutTime).IsModified = true;
            db.Entry(gesperrterBenutzer).Property(p => p.FailedLoginAttempts).IsModified = true;

            try
            {
                // Sadece bu işlemde tüm model validasyonlarını devre dışı bırak
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                TempData["Message"] = "Der Benutzer wurde erfolgreich entsperrt.";
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var msgs = ex.EntityValidationErrors
                    .SelectMany(v => v.ValidationErrors)
                    .Select(v => $"{v.PropertyName}: {v.ErrorMessage}");

                var joined = string.Join("; ", msgs);
                TempData["Message"] = "Validierungsfehler: " + joined;
                System.Diagnostics.Debug.WriteLine(joined);
            }
            finally
            {
                // Diğer action’lar etkilenmesin diye tekrar aç
                db.Configuration.ValidateOnSaveEnabled = true;
            }

            return RedirectToAction("Details", new { id = gesperrterBenutzer.PersonalDatenID });
        }




    }
}
