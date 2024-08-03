using System;
using System.Linq;
using System.Web;
using CrmAPP.Models.Kunden;
using System.Web.Mvc;
using CrmAPP.Models.DataContext;

using System.IO;

namespace CrmAPP.Controllers
{
    public class TestlogoController : BaseController
    {
        private new DBc_Context db = new DBc_Context(); //Datenbank - Aufrufen


        // GET: Testlogo
        public ActionResult Index()
        {
            var kundenDatens = db.KundenDatens.ToList();

            return View(kundenDatens);
        }

        [HttpGet]
        public ActionResult LogoCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoCreate(KundenDaten logo, HttpPostedFileBase logoDatei)
        {
            if (!ModelState.IsValid)
            {
                if (logoDatei != null && logoDatei.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(logoDatei.FileName);
                    string extension = Path.GetExtension(logoDatei.FileName);
                    fileName = fileName + "_" + System.DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(Server.MapPath("~/logos/"), fileName);

                    // Klasör oluşturulmuş mu? Değilse oluştur
                    var folderPath = Server.MapPath("~/logos/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Dosyayı kaydet
                    logoDatei.SaveAs(path);
                    logo.Logo = "/logos/" + fileName;

                    db.KundenDatens.Add(logo);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Symbol", "Es wurde nicht hochgeladen");
                }
            }
            return View(logo);
        }


    }
}