using CrmAPP.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrmAPP.Controllers
{
    //in diesem feld wird nur das personal angezeigt, das im bereich der it-dienste tätig ist
    public class LiveSupportController : BaseController
    {
        private new DBc_Context db = new DBc_Context();
        // GET: LiveSupport

        public ActionResult LiveSupport()
        {
            var support= db.PersonalAngabens.Where(x => x.Abteilung == "IT-Service").ToList();
            return View(support.ToList());
        }

        public ActionResult TreeView()
        {
            return View();
        }

        public ActionResult Vista()
        
        {
            return View();
        }
    }
}