using CrmAPP.Models.DataContext;
using CrmAPP.Models.Personal;
using CrmAPP.Models.Projekt;
using CrmAPP.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrmAPP.Controllers
{
    [Authorize]
    public class BerichtsController : BaseController
    {
        #region Constants
        private const int MaxFileSizeBytes = 50 * 1024 * 1024; // 50MB
        private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
        private const string StorageFolderVirtual = "~/Bericht_Dokumentation";
        #endregion

        #region Helper Methods
        private string GetStorageRoot()
        {
            var root = Server.MapPath(StorageFolderVirtual);
            Directory.CreateDirectory(root);
            return root;
        }

        private static string SafeFileName(string name)
        {
            return Path.GetFileName(name ?? string.Empty);
        }

        private string GetContentTypeByExtension(string ext)
        {
            ext = (ext ?? "").ToLowerInvariant();
            switch (ext)
            {
                case ".pdf": return "application/pdf";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                default: return "application/octet-stream";
            }
        }

        private void FillLists(BerichtCreateEditViewModel vm, bool filterProjectsBySelectedCustomer)
        {
            // Müşteri listesi her zaman dolu
            vm.KundenListe = db.KundenDatens
                .AsNoTracking()
                .Select(k => new SelectListItem
                {
                    Value = k.KundenDatenID.ToString(),
                    Text = string.IsNullOrEmpty(k.Unternehmen)
                        ? (k.Vorname + " " + k.Nachname)
                        : k.Unternehmen
                }).ToList();

            // Proje listesi: müşteri seçilmişse filtrele, yoksa boş bırak
            if (filterProjectsBySelectedCustomer && vm.KundenDatenId > 0)
            {
                vm.ProjektListe = db.PersonalProjektes
                    .AsNoTracking()
                    .Where(p => p.KundenDatens.Any(k => k.KundenDatenID == vm.KundenDatenId))
                    .Select(p => new SelectListItem
                    {
                        Value = p.PerProjID.ToString(),
                        Text = p.Projektname
                    }).ToList();
            }
            else
            {
                vm.ProjektListe = new List<SelectListItem>();
            }
        }
        #endregion

        #region CRUD Operations


        public ActionResult Index(string searchString, int? kundenFilter, int? projektFilter, string sortOrder)
        {
            // 1. ADIM: Filtre ve sıralama parametrelerini ViewBag'e en başta ata.
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitelSortParm = string.IsNullOrEmpty(sortOrder) ? "titel_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CurrentFilter = searchString;
            ViewBag.KundenFilter = kundenFilter;
            ViewBag.ProjektFilter = projektFilter;

            // 2. ADIM: Dropdown listelerini try-catch bloğundan ÖNCE veya en başında doldur.
            // Bu sayede sorguda bir hata olsa bile filtre dropdown'ları boş kalmaz.
            try
            {
                // Müşteri listesini doldur (Veritabanından çek)
                var kundenList = db.KundenDatens.AsNoTracking().OrderBy(k => k.Unternehmen).ThenBy(k => k.Vorname).ToList();
                ViewBag.KundenSelectList = kundenList.Select(k => new SelectListItem
                {
                    Value = k.KundenDatenID.ToString(),
                    Text = string.IsNullOrEmpty(k.Unternehmen) ? $"{k.Vorname} {k.Nachname}" : k.Unternehmen
                }).ToList();

                // EKSİK OLAN KISIM: Proje listesini doldur
                // Not: Proje tablonuzun adının 'Projekte', ID alanının 'PerProjID' ve isim alanının 'Projektname' olduğunu varsaydım.
                // Kendi modelinize göre bu isimleri kontrol edip gerekirse değiştirin.
                var projektList = db.PersonalProjektes.AsNoTracking().OrderBy(p => p.Projektname).ToList();
                ViewBag.ProjektSelectList = projektList.Select(p => new SelectListItem
                {
                    Value = p.PerProjID.ToString(),
                    Text = p.Projektname
                }).ToList();
            }
            catch (Exception ex)
            {
                // Eğer listeler doldurulurken bir hata olursa, sayfada hata oluşmaması için boş listeler ata.
                ViewBag.Error = "Fehler beim Laden der Filterlisten: " + ex.Message;
                ViewBag.KundenSelectList = new List<SelectListItem>();
                ViewBag.ProjektSelectList = new List<SelectListItem>();
                return View(new List<Bericht>()); // Hata durumunda boş modelle View'ı döndür.
            }


            try
            {
                // 3. ADIM: Ana rapor sorgusunu oluştur.
                var query = db.Berichte
                    .AsNoTracking()
                    .Include(b => b.Projekt)
                    .Include(b => b.Kunde)
                    .Include(b => b.HochgeladenVon)
                    .Where(b => b.GeloschtAm == null);

                // Arama (Search) - Null kontrollü daha güvenli hale getirildi.
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(b =>
                        (b.Titel != null && b.Titel.Contains(searchString)) ||
                        (b.Kunde != null && b.Kunde.Unternehmen != null && b.Kunde.Unternehmen.Contains(searchString)) ||
                        (b.Kunde != null && b.Kunde.Vorname != null && b.Kunde.Vorname.Contains(searchString)) ||
                        (b.Kunde != null && b.Kunde.Nachname != null && b.Kunde.Nachname.Contains(searchString)) ||
                        (b.Projekt != null && b.Projekt.Projektname != null && b.Projekt.Projektname.Contains(searchString)));
                }

                // Müşteriye göre filtreleme
                if (kundenFilter.HasValue)
                {
                    query = query.Where(b => b.KundenDatenId == kundenFilter.Value);
                }

                // Projeye göre filtreleme
                if (projektFilter.HasValue)
                {
                    query = query.Where(b => b.PerProjID == projektFilter.Value);
                }

                // Sıralama
                switch (sortOrder)
                {
                    case "titel_desc":
                        query = query.OrderByDescending(b => b.Titel);
                        break;
                    case "Date":
                        query = query.OrderBy(b => b.HochgeladenAm);
                        break;
                    case "date_desc":
                        query = query.OrderByDescending(b => b.HochgeladenAm);
                        break;
                    default:
                        query = query.OrderByDescending(b => b.HochgeladenAm);
                        break;
                }

                var list = query.ToList();

                // 4. ADIM: Silinen raporlar sorgusunu HATA VERMEYECEK şekilde düzelt.
                // ?. (null-conditional) ve ?? (null-coalescing) operatörleri kullanıldı.
                ViewBag.DeletedReports = db.Berichte
                    .AsNoTracking()
                    .Where(b => b.GeloschtAm != null)
                    .ToList() // Önce listeye çekip sonra Select yapmak daha güvenli olabilir.
                    .Select(b => new
                    {
                        b.BerichtId,
                        b.Titel,
                        b.GeloschtAm,
                        b.GeloschtVonId,
                        DeletedBy = db.PersonalAngabens.FirstOrDefault(p => p.PersonalDatenID == b.GeloschtVonId)?.Vorname ?? "Unbekannt"
                    })
                    .ToList();

                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Fehler beim Laden der Berichte: " + ex.Message;
                // Sorguda bir hata olsa bile, sayfa çökmesin. Boş bir liste gönder.
                // Filtre listeleri zaten yukarıda doldurulduğu için görünmeye devam edecek.
                return View(new List<Bericht>());
            }
        }







        // GET: Berichts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                var currentId = this.CurrentPersonalId;


            try
            {
                var bericht = db.Berichte
                    .AsNoTracking()
                    .Include(b => b.Projekt)
                    .Include(b => b.Kunde)
                    
                    .Include(b => b.HochgeladenVon)
                    .FirstOrDefault(b => b.BerichtId == id && b.GeloschtAm == null);

                if (bericht == null)
                    return HttpNotFound();

                return View(bericht);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Fehler beim Laden des Berichts: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Berichts/Create
        public ActionResult Create()
        {
            var vm = new BerichtCreateEditViewModel();
            FillLists(vm, filterProjectsBySelectedCustomer: true);
            return View(vm);
        }

        // POST: Berichts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BerichtCreateEditViewModel vm)
        {
            // Seçilen müşteriye göre proje listesini filtrele
            FillLists(vm, filterProjectsBySelectedCustomer: true);

            if (vm.Datei == null || vm.Datei.ContentLength <= 0)
                ModelState.AddModelError("Datei", "Bitte eine Datei wählen.");

            if (vm.Datei != null && vm.Datei.ContentLength > MaxFileSizeBytes)
                ModelState.AddModelError("Datei", "Die Datei ist zu groß (max 50MB).");

            string ext = null;
            if (vm.Datei != null)
            {
                ext = Path.GetExtension(vm.Datei.FileName)?.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
                    ModelState.AddModelError("Datei", "Nur .pdf, .doc, .docx, .xls, .xlsx erlaubt.");

                var allowedMimeTypes = new[]
                {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

                var mimeType = vm.Datei.ContentType;
                if (!allowedMimeTypes.Contains(mimeType))
                    ModelState.AddModelError("Datei", "Ungültiger Datei-Typ (MIME).");
            }

            // Güvenlik: Seçilen proje, seçilen müşteriye ait mi?
            if (vm.PerProjID > 0 && vm.KundenDatenId > 0)
            {
                var belongs = db.PersonalProjektes.Any(p =>
                    p.PerProjID == vm.PerProjID &&
                    p.KundenDatens.Any(k => k.KundenDatenID == vm.KundenDatenId)
                );

                if (!belongs)
                    ModelState.AddModelError("PerProjID", "Seçilen proje, seçilen müşteriye ait değil.");
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var storageRoot = GetStorageRoot();
                var fileName = Guid.NewGuid().ToString("N") + ext;
                var filePath = Path.Combine(storageRoot, fileName);
                vm.Datei.SaveAs(filePath);

                var currentId = this.CurrentPersonalId;

                var entity = new Bericht
                {
                    Titel = vm.Titel,
                    DateiPfad = fileName,
                    DateiTuru = ext?.TrimStart('.'),
                    Version = vm.Version,
                  
                    HochgeladenVonId = currentId,
                    PerProjID = vm.PerProjID,
                    KundenDatenId = vm.KundenDatenId
                };

                db.Berichte.Add(entity);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Bericht erfolgreich erstellt.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // 🧾 LOG DOSYASI OLUŞTURMA
                System.IO.File.AppendAllText(Server.MapPath("~/App_Data/error.log"),
                    $"{DateTime.Now} - [Create] Fehler: {ex}\n");

                ModelState.AddModelError("", "Fehler beim Erstellen: " + ex.Message);
                return View(vm);
            }
        }


        // GET: Berichts/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var b = db.Berichte.Find(id);
                if (b == null || b.GeloschtAm != null)
                    return HttpNotFound();

                var vm = new BerichtCreateEditViewModel
                {
                    BerichtId = b.BerichtId,
                    Titel = b.Titel,
                    Version = b.Version,
                    PerProjID = b.PerProjID,
                    KundenDatenId = b.KundenDatenId,
                    BestehendeDateiPfad = b.DateiPfad,
                    HochgeladenAm = b.HochgeladenAm // Nullable HochgeladenAm değeri burada da kullanılacak
                };

                FillLists(vm, filterProjectsBySelectedCustomer: true);
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Fehler beim Laden: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Berichts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit(BerichtCreateEditViewModel vm)
        {
            FillLists(vm, filterProjectsBySelectedCustomer: true);

            var entity = db.Berichte.Find(vm.BerichtId);
            if (entity == null || entity.GeloschtAm != null)
                return HttpNotFound();

            string ext = null;
            if (vm.Datei != null && vm.Datei.ContentLength > 0)
            {
                if (vm.Datei.ContentLength > MaxFileSizeBytes)
                    ModelState.AddModelError("Datei", "Die Datei ist zu groß (max 50MB).");

                ext = Path.GetExtension(vm.Datei.FileName)?.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
                    ModelState.AddModelError("Datei", "Nur .pdf, .doc, .docx, .xls, .xlsx erlaubt.");

                // ✅ MIME tipi kontrolü
                var allowedMimeTypes = new[] {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

                var mimeType = vm.Datei.ContentType;
                if (!allowedMimeTypes.Contains(mimeType))
                    ModelState.AddModelError("Datei", "Ungültiger Datei-Typ (MIME).");
            }

            // Güvenlik kontrolü
            if (vm.PerProjID > 0 && vm.KundenDatenId > 0)
            {
                var belongs = db.PersonalProjektes.Any(p =>
                    p.PerProjID == vm.PerProjID &&
                    p.KundenDatens.Any(k => k.KundenDatenID == vm.KundenDatenId)
                );

                if (!belongs)
                    ModelState.AddModelError("PerProjID", "ausgewähltes Projekt gehört nicht zu dieser Kunde .");
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var currentId = this.CurrentPersonalId;

                entity.Titel = vm.Titel;
                entity.Version = vm.Version;
                entity.PerProjID = vm.PerProjID;
                entity.KundenDatenId = vm.KundenDatenId;
                entity.BearbeitetAm = DateTime.Now;
                entity.BearbeitetVonId = currentId;

                // Değişiklik: HochgeladenAm nullable kontrolü ekledik
                //entity.HochgeladenAm = vm.HochgeladenAm ?? DateTime.Now; // Eğer null ise geçerli tarih atanır

                if (vm.Datei != null && vm.Datei.ContentLength > 0)
                {
                    var storageRoot = GetStorageRoot();
                    var newFileName = Guid.NewGuid().ToString("N") + ext;
                    var newPath = Path.Combine(storageRoot, newFileName);
                    vm.Datei.SaveAs(newPath);

                    // Eski dosyayı sil
                    if (!string.IsNullOrEmpty(entity.DateiPfad))
                    {
                        var oldName = SafeFileName(entity.DateiPfad);
                        var oldPath = Path.Combine(storageRoot, oldName);
                        if (System.IO.File.Exists(oldPath))
                        {
                            try { System.IO.File.Delete(oldPath); }
                            catch { /* ignore */ }
                        }
                    }

                    entity.DateiPfad = newFileName;
                    entity.DateiTuru = ext?.TrimStart('.');
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "Bericht erfolgreich aktualisiert.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Fehler beim Aktualisieren: " + ex.Message);
                return View(vm);
            }
        }


        // GET: Berichts/Delete/5 +
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.CurrentUser = db.PersonalAngabens
                 .FirstOrDefault(p => p.PersonalDatenID == this.CurrentPersonalId);

            try
            {
                var b = db.Berichte
                    .AsNoTracking()
                    .Include(x => x.Projekt)
                    .Include(x => x.Kunde)
                    .FirstOrDefault(x => x.BerichtId == id && x.GeloschtAm == null);

                if (b == null)
                    return HttpNotFound();

                return View(b);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Fehler beim Laden: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Berichts/Delete/5 ++
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var b = db.Berichte.Find(id);
                if (b == null || b.GeloschtAm != null)
                    return HttpNotFound();

                // Dosyayı sil
                try
                {
                    var storageRoot = GetStorageRoot();
                    var safeName = SafeFileName(b.DateiPfad);
                    var path = Path.Combine(storageRoot, safeName);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);

                        // Dosya silindiyse, veritabanı kaydını da sil
                        db.Berichte.Remove(b); // Hard delete: Veritabanındaki kaydı tamamen sil
                        db.SaveChanges(); // Değişiklikleri kaydet
                    }
                }
                catch (Exception fileEx)
                {
                    TempData["ErrorMessage"] = "Fehler beim Löschen der Datei: " + fileEx.Message;
                    return RedirectToAction("Index");
                }

                //// Soft delete: Silinme tarihini güncelle
                //var currentId = this.CurrentPersonalId;
                //b.GeloschtVonId = currentId;
                //b.GeloschtAm = DateTime.Now;  // Silinme tarihini ayarla
                //db.SaveChanges();  // Değişiklikleri kaydet

                TempData["SuccessMessage"] = "Bericht erfolgreich gelöscht.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Fehler beim Löschen: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        #endregion

        #region AJAX Methods

        // AJAX: Müşteriye göre projeler

        [HttpGet]
        public JsonResult GetProjekteByKunde(int kundenId)
        {
            try
            {
                // Debug için önce müşteri var mı kontrol edelim
                var kunde = db.KundenDatens.Find(kundenId);
                if (kunde == null)
                {
                    return Json(new { success = false, message = "Kunde nicht gefunden" }, JsonRequestBehavior.AllowGet);
                }

                // Projeleri kontrol edelim
                var proj = db.PersonalProjektes
                    .Where(p => p.KundenDatens.Any(k => k.KundenDatenID == kundenId))
                    .Select(p => new
                    {
                        value = p.PerProjID,
                        text = p.Projektname
                    })
                    .OrderBy(p => p.text)
                    .ToList();

                return Json(new
                {
                    success = true,
                    data = proj,
                    kundeInfo = $"{kunde.Vorname} {kunde.Nachname} - {kunde.Unternehmen}",
                    projectCount = proj.Count
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, stackTrace = ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region File Operations

        // Dosya indirme
        [HttpGet]
        public ActionResult Download(int id)
        {
            try
            {
                var b = db.Berichte.Find(id);
                if (b == null || b.GeloschtAm != null)
                    return HttpNotFound();

                var storageRoot = GetStorageRoot();
                var safeName = SafeFileName(b.DateiPfad);
                var path = Path.Combine(storageRoot, safeName);

                if (!System.IO.File.Exists(path))
                    return HttpNotFound("Datei nicht gefunden.");

                var contentType = GetContentTypeByExtension(Path.GetExtension(path));
                var downloadName = string.IsNullOrEmpty(b.Titel)
                    ? Path.GetFileName(path)
                    : $"{b.Titel}{Path.GetExtension(path)}";

                return File(path, contentType, downloadName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Fehler beim Download: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}