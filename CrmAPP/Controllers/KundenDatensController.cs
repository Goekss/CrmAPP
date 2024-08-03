using ClosedXML.Excel;
using CrmAPP.Models.DataContext;
using CrmAPP.Models.Kunden;
using CrmAPP.Models.Personal;
using CrmAPP.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;


namespace CrmAPP.Controllers
{
    [Authorize]
    public class KundenDatensController : BaseController
    {
        private new DBc_Context db = new DBc_Context();


        // GET: KundenDatens
        public ActionResult Index()
        {
            return View(db.KundenDatens.ToList());
        }

        public ActionResult Liste()
        {
            return View(db.KundenDatens.ToList());
        }

        //--CREATE--
        // hier gibt es 2 Operationen Get-Post
        // GET:  I) zuerst gibt es GET-Operation (Daten vom Server abrufen (Lesevorgang)
        public ActionResult Create()
        {
            var model = new KundenCreateViewModel
            {
                Kunde = new KundenDaten(),
                AnsprechpartnerList = new List<Ansprechpartner> { new Ansprechpartner() }
            };
            return View(model);
        }

        // POST: KundenDatens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KundenCreateViewModel model, HttpPostedFileBase Symbol)
        {
            if (model == null || model.Kunde == null)
                return View(model);

            // En az bir geçerli Ansprechpartner girilmiş mi kontrol et
            if (model.AnsprechpartnerList == null || !model.AnsprechpartnerList.Any(a => !string.IsNullOrWhiteSpace(a.Name)))
            {
                ModelState.AddModelError("", "Bitte fügen Sie mindestens eine:n Ansprechpartner:in hinzu.");
                return View(model);
            }

            // Resim yükleme işlemleri
            if (Symbol != null && Symbol.ContentLength > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(Symbol.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Symbol", "Nur Bilddateien sind erlaubt.(.jpg, .jpeg, .png).");
                    return View(model);
                }

                try
                {
                    var logosPath = Server.MapPath("~/Logos");
                    if (!Directory.Exists(logosPath))
                        Directory.CreateDirectory(logosPath);

                    var symbolName = Guid.NewGuid().ToString() + extension;
                    var path = Path.Combine(logosPath, symbolName);
                    Symbol.SaveAs(path);

                    model.Kunde.Logo = symbolName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Beim Laden des Logos ist ein Fehler aufgetreten: " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("Symbol", "Sie müssen eine Logodatei auswählen!");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                db.KundenDatens.Add(model.Kunde);
                db.SaveChanges();

                foreach (var partner in model.AnsprechpartnerList.Where(a => !string.IsNullOrWhiteSpace(a.Name)))
                {
                    partner.KundenDatenID = model.Kunde.KundenDatenID;
                    db.Ansprechpartners.Add(partner);
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }



        //// POST: II) als zwiete gibt es POST-Operationen Daten an den Server senden (Schreibvorgang/Hinzufügen)
        //[HttpPost] // Bu metot, yalnızca HTTP POST istekleriyle çalışır.
        //[ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma sağlamak için kullanılır.
        //public ActionResult Create(KundenDaten kundenDaten, HttpPostedFileBase Symbol)
        //{
        //    if (ModelState.IsValid) // Kullanıcıdan geçerli veri gelip gelmediğini kontrol eder.
        //    {
        //        if (Symbol != null && Symbol.ContentLength > 0) // Kullanıcı bir dosya yüklemiş mi kontrol edilir && Es wird überprüft, ob der Benutzer eine Datei hochgeladen hat
        //        {
        //            // Geçerli dosya uzantılarını tanımla && Definiere die gültigen Dateiendungen
        //            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //            var extension = Path.GetExtension(Symbol.FileName).ToLower(); // Dosyanın uzantısını al ve küçük harfe çevir && Hol dir die Dateiendung und wandle sie in Kleinbuchstaben um

        //            if (!allowedExtensions.Contains(extension)) // Dosyanın uzantısı geçerli değilse && Wenn die Dateiendung ungültig ist
        //            {
        //                ModelState.AddModelError("Symbol", "Nur Imagedateien sind erlaubt."); // Hata mesajı ekle: "Sadece resim dosyalarına izin verilir." && Wenn die Dateiendung ungültig ist, wird die Fehlermeldung angezeigt: 'Nur Bilddateien sind erlaubt.'"
        //                return View(kundenDaten); // Formu tekrar kullanıcıya göster && Zeige das Formular dem Benutzer erneut an.
        //            }

        //            try
        //            {
        //                // Benzersiz dosya adı oluştur (GUID ile) && Erstelle einen eindeutigen Dateinamen (mit GUID)
        //                var symbolName = Guid.NewGuid().ToString() + extension;

        //                // Sunucu üzerindeki "Logos" klasörüne dosyanın kaydedileceği yolu belirle && Bestimmen Sie den Pfad, in dem die Datei im Ordner "Logos" auf dem Server gespeichert wird.
        //                var path = Path.Combine(Server.MapPath("~/Logos"), symbolName);

        //                // Dosyayı belirlenen yola kaydet && Speichern Sie die Datei im angegebenen Pfad
        //                Symbol.SaveAs(path);

        //                // Kullanıcının müşteri verilerine yüklenen logo adını ekle && Fügen Sie den Dateinamen des hochgeladenen Logos zu den Kundendaten des Benutzers hinzu
        //                kundenDaten.Logo = symbolName;

        //                // Müşteri bilgilerini veritabanına kaydet && Speichern Sie die Kundendaten in der Datenbank
        //                db.KundenDatens.Add(kundenDaten);
        //                db.SaveChanges();

        //                // Kayıt işlemi başarılı olursa "Index" sayfasına yönlendir && Wenn der Vorgang erfolgreich ist, leiten Sie zur Seite "Index" weiter
        //                return RedirectToAction("Index");
        //            }
        //            catch (Exception ex) // Hata olursa yakala && Wenn ein Fehler auftritt, fange ihn ab
        //            {
        //                ModelState.AddModelError("", "Fehler beim Hochladen des Bildes: " + ex.Message);
        //                // Hata mesajını ModelState içine ekleyerek kullanıcıya göster.&& Zeigen Sie dem Benutzer die Fehlermeldung, indem Sie sie in das ModelState einfügen.
        //            }
        //        }
        //        else // Eğer dosya yüklenmemişse && Wenn keine Datei hochgeladen wurde
        //        {
        //            ModelState.AddModelError("Symbol", "Symbol darf nicht leer sein.");
        //            // "Dosya boş olamaz" hatası eklenir. && Fügen Sie den Fehler "Datei darf nicht leer sein" hinzu.
        //        }
        //    }

        //    return View(kundenDaten); // Eğer ModelState geçerli değilse, formu tekrar göster. && Wenn das ModelState ungültig ist, zeigen Sie das Formular erneut an.
        //}

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KundenDaten kundenDaten = db.KundenDatens.Find(id);
            if (kundenDaten == null)
            {
                return HttpNotFound();
            }
            return View(kundenDaten);
        }


        [Authorize(Roles = "Admin,Moderator")] // Admin ve Moderator rollerine sahip kullanıcılar bu metodu kullanabilir.&& Benutzer mit den Rollen Admin und Moderator können diese Methode verwenden.
                                               // GET: KundenDatens/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var kunde = db.KundenDatens.Find(id);
            if (kunde == null)
            {
                return HttpNotFound();
            }

            // İlgili Ansprechpartner’leri çek
            var ansprechpartnerList = db.Ansprechpartners
                .Where(a => a.KundenDatenID == kunde.KundenDatenID)
                .ToList();

            // ViewModel’i doldur
            var model = new CrmAPP.ViewModels.KundenEditViewModel
            {
                Kunde = kunde,
                AnsprechpartnerList = ansprechpartnerList
            };

            return View(model);
        }



        // Admin ve Moderator rollerine sahip kullanıcılar bu metodu kullanabilir.&& Benutzer mit den Rollen Admin und Moderator können diese Methode verwenden.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrmAPP.ViewModels.KundenEditViewModel model, HttpPostedFileBase Logo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vorliegendeKunde = db.KundenDatens.Find(model.Kunde.KundenDatenID);
                    if (vorliegendeKunde == null)
                    {
                        return HttpNotFound();
                    }

                    // Ana müşteri alanlarını güncelle
                    vorliegendeKunde.Unternehmen = model.Kunde.Unternehmen;
                    vorliegendeKunde.Vorname = model.Kunde.Vorname;
                    vorliegendeKunde.Nachname = model.Kunde.Nachname;
                    vorliegendeKunde.Email = model.Kunde.Email;
                    vorliegendeKunde.TelefonMobil = model.Kunde.TelefonMobil;
                    vorliegendeKunde.TelefonHaus = model.Kunde.TelefonHaus;
                    vorliegendeKunde.ProjektAuftrag = model.Kunde.ProjektAuftrag;
                    vorliegendeKunde.Website = model.Kunde.Website;
                    vorliegendeKunde.Adresse = model.Kunde.Adresse;
                    vorliegendeKunde.Hinweise = model.Kunde.Hinweise;

                    // Logo işlemleri
                    if (Logo != null && Logo.ContentLength > 0)
                    {
                        int maxFileSize = 50 * 1024 * 1024; // 50 MB
                        if (Logo.ContentLength > maxFileSize)
                        {
                            ModelState.AddModelError("Logo", "Die Datei ist zu groß. Maximal erlaubte Größe ist 50MB.");
                            return View(model);
                        }

                        var oldLogoPath = string.IsNullOrEmpty(vorliegendeKunde.Logo)
                            ? null
                            : Path.Combine(Server.MapPath("~/Logos"), vorliegendeKunde.Logo);

                        var extension = Path.GetExtension(Logo.FileName);
                        var fileName = Guid.NewGuid().ToString() + extension;

                        var filePath = Path.Combine(Server.MapPath("~/Logos/"), fileName);
                        Logo.SaveAs(filePath);

                        vorliegendeKunde.Logo = fileName;

                        if (!string.IsNullOrEmpty(oldLogoPath) && System.IO.File.Exists(oldLogoPath))
                        {
                            System.IO.File.Delete(oldLogoPath);
                        }
                    }

                    // Ansprechpartner güncellemesi
                    if (model.AnsprechpartnerList != null)
                    {
                        foreach (var partner in model.AnsprechpartnerList)
                        {
                            var dbPartner = db.Ansprechpartners.Find(partner.Id);
                            if (dbPartner != null)
                            {
                                dbPartner.Name = partner.Name;
                                dbPartner.Tel = partner.Tel;
                                dbPartner.Mail = partner.Mail;
                                dbPartner.Filiale = partner.Filiale;
                            }
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                // ModelState geçersizse aynı modeli geri döndür
                return View(model);
            }
            catch (HttpException ex)
            {
                if (ex.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
                {
                    ModelState.AddModelError("Bild", "Die Datei ist zu groß. Maximal erlaubte Größe ist 50MB.");
                    return View(model);
                }
                throw;
            }
        }



        // Admin ve Moderator rollerine sahip kullanıcılar bu metodu kullanabilir.&& Benutzer mit den Rollen Admin und Moderator können diese Methode verwenden.
        [Authorize(Roles = "Admin,Moderator")]
        // GET: KundenDatens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KundenDaten kundenDaten = db.KundenDatens.Find(id);
            if (kundenDaten == null)
            {
                return HttpNotFound();
            }
            return View(kundenDaten);
        }


        // Admin ve Moderator rollerine sahip kullanıcılar bu metodu kullanabilir.&& Benutzer mit den Rollen Admin und Moderator können diese Methode verwenden.
        [Authorize(Roles = "Admin,Moderator")]
        //POST: KundenDatens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KundenDaten kundenDaten = db.KundenDatens.Find(id);
            db.KundenDatens.Remove(kundenDaten);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //--------------- ANFANG: EXCEL-IMPORT-METHODEN FÜR KUNDEN ---------------

        // GET: /KundenDatens/Import
        // Zeigt die Seite für den Datei-Upload an.
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Import()
        {
            return View();
        }

        // POST: /KundenDatens/Import
        // Verarbeitet die hochgeladene Excel-Datei (EN NİHAİ, EN BASİT KURAL: Boşsa "Keine Daten" yaz, asla atlama)
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

            var yeniMusterilerListesi = new List<KundenDaten>();
            var olusanHatalar = new List<string>();
            int satirNumarasi = 0; // 0'dan başlatıyoruz çünkü başlık satırı da sayılabilir

            try
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    var worksheet = workbook.Worksheet(1);
                    if (worksheet.LastRowUsed() == null)
                    {
                        TempData["ErrorMessage"] = "Die Excel-Datei ist leer.";
                        return RedirectToAction("Import");
                    }

                    var headerRow = worksheet.Row(1);
                    var colIndeksleri = new Dictionary<string, int>();

                    foreach (var cell in headerRow.CellsUsed())
                    {
                        var headerText = cell.Value.ToString().Trim().ToLower();
                        if (!colIndeksleri.ContainsKey(headerText))
                        {
                            colIndeksleri.Add(headerText, cell.Address.ColumnNumber);
                        }
                    }

                    Func<string[], int?> findColumnIndex = (possibleHeaders) =>
                    {
                        foreach (var header in possibleHeaders)
                        {
                            if (colIndeksleri.ContainsKey(header)) return colIndeksleri[header];
                        }
                        return null;
                    };

                    var emailIndex = findColumnIndex(new[] { "email" });

                    // EN ÖNEMLİ DEĞİŞİKLİK: Artık "RowsUsed" değil, potansiyel olarak boş satırları da içeren aralığı okuyoruz.
                    var lastRowNumber = worksheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= lastRowNumber; i++) // 2. satırdan başla
                    {
                        satirNumarasi = i;
                        var row = worksheet.Row(i);

                        try
                        {
                            string email = emailIndex.HasValue ? row.Cell(emailIndex.Value).GetValue<string>() : "";

                            if (emailIndex.HasValue && !string.IsNullOrWhiteSpace(email) && db.KundenDatens.Any(k => k.Email == email))
                            {
                                olusanHatalar.Add($"Zeile {satirNumarasi}: Die E-Mail-Adresse '{email}' ist bereits registriert. Zeile übersprungen.");
                                continue; // Sadece e-posta tekrar ediyorsa atla. Başka hiçbir durumda atlama!
                            }

                            Func<string[], string> getValueOrDefault = (possibleHeaders) =>
                            {
                                var index = findColumnIndex(possibleHeaders);
                                if (index.HasValue)
                                {
                                    var cellValue = row.Cell(index.Value).GetValue<string>();
                                    return string.IsNullOrWhiteSpace(cellValue) ? "Keine Daten" : cellValue;
                                }
                                return "Keine Daten";
                            };

                            var musteri = new KundenDaten
                            {
                                // E-posta boşsa bile artık "Keine Daten" yazılacak.
                                Email = string.IsNullOrWhiteSpace(email) ? "Keine Daten" : email,
                                Unternehmen = getValueOrDefault(new[] { "unternehmen", "company" }),
                                Vorname = getValueOrDefault(new[] { "vorname", "first name" }),
                                Nachname = getValueOrDefault(new[] { "nachname", "last name" }),
                                TelefonMobil = getValueOrDefault(new[] { "telefonmobil", "mobile" }),
                                TelefonHaus = getValueOrDefault(new[] { "telefonhaus", "phone", "home" }),
                                Website = getValueOrDefault(new[] { "website" }),
                                Adresse = getValueOrDefault(new[] { "adresse", "address" }),
                                Logo = "",
                                ProjektAuftrag = "N/A",
                                Hinweise = "Aus Excel importiert"
                            };

                            // Ekstra kontrol: Eğer tüm alanlar "Keine Daten" ise bu tamamen boş bir satırdır, ekleme.
                            if (musteri.Unternehmen == "Keine Daten" && musteri.Email == "Keine Daten")
                            {
                                olusanHatalar.Add($"Zeile {satirNumarasi}: Zeile ist komplett leer und wurde übersprungen.");
                                continue;
                            }

                            yeniMusterilerListesi.Add(musteri);
                        }
                        catch (Exception ex)
                        {
                            olusanHatalar.Add($"Zeile {satirNumarasi}: Fehler beim Lesen der Daten. Details: {ex.Message}");
                        }
                    }
                }

                if (yeniMusterilerListesi.Any())
                {
                    db.KundenDatens.AddRange(yeniMusterilerListesi);
                    db.SaveChanges();
                }

                TempData["SuccessMessage"] = $"{yeniMusterilerListesi.Count} neue Kunden wurden erfolgreich importiert.";
                if (olusanHatalar.Any())
                {
                    TempData["ErrorList"] = olusanHatalar;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Beim Verarbeiten der Datei ist ein allgemeiner Fehler aufgetreten: " + ex.Message;
            }

            return RedirectToAction("Import");
        }

        //--------------- ENDE: EXCEL-IMPORT-METHODEN FÜR KUNDEN ---------------




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}