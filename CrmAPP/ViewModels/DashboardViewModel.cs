using CrmAPP.Models.Kunden;
using CrmAPP.Models.Personal;
using CrmAPP.Models.Projekt;
using System;
using System.Collections.Generic;

namespace CrmAPP.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Kunden = new List<KundenDaten>();
            Ansprechpartner = new List<Ansprechpartner>();
            Projekte = new List<PersonalProjekte>();
        }

        public int ProjektZahlen { get; set; }
        public int FertigteProjektZahlen { get; set; }
        public int HighPriorityCount { get; set; }
        public int MiddlePriorityCount { get; set; }
        public int LowPriorityCount { get; set; }
        public int Laufendeprojekte { get; set; }

        // Müşteriler (random veya seçili)
        public List<KundenDaten> Kunden { get; set; }

        // Ansprechpartner bilgileri
        public List<Ansprechpartner> Ansprechpartner { get; set; }

        // Projeler
        public List<PersonalProjekte> Projekte { get; set; }
    }

    public class KundeKontaktVM
    {
        public string Kunde { get; set; }
        public string Kontaktperson { get; set; }
    }
}
