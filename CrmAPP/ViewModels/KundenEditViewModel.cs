using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrmAPP.Models.Kunden;      // KundenDaten için
using CrmAPP.Models.Personal;    // Ansprechpartner için

namespace CrmAPP.ViewModels
{
    public class KundenEditViewModel
    {
        public KundenDaten Kunde { get; set; }
        public List<Ansprechpartner> AnsprechpartnerList { get; set; }
    }
}