using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrmAPP.Models.Kunden;      // KundenDaten için (gerekirse düzelt)
using CrmAPP.Models.Personal;    // Ansprechpartner için (gerekirse düzelt)

namespace CrmAPP.ViewModels
{
    public class KundenCreateViewModel
    {
        public KundenDaten Kunde { get; set; }
        public List<Ansprechpartner> AnsprechpartnerList { get; set; }
    }
}