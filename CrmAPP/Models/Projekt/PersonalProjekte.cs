using CrmAPP.Models.Personal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CrmAPP.Models.Kunden;

namespace CrmAPP.Models.Projekt
{
    public class PersonalProjekte
    {     
        
        //Sorgt dafür, dass einzigartige Informationen eingegeben werden und Duplikate verhindert werden.
        //Benzersiz bilgilerin girilmesini sağlar, tekrarı engeller.
        public PersonalProjekte()
        {
            this.PersonalAngabens = new HashSet<PersonalAngaben>();
            this.KundenDatens = new HashSet<KundenDaten>();
            this.Ansprechpartner = new HashSet<Ansprechpartner>(); 
            this.Berichte = new HashSet<Bericht>();
            SelectedAnsprechpartnerIds = new List<int>();
            SelectedPersonalIds = new List<int>();
        }

       




        [Key]//< weist darauf hin, dass die PersonalDatenID zu dieser Tabelle/Class gehört.
        public int PerProjID { get; set; }

        //
        [DisplayName("Projektname")] //Benennungen
        [StringLength(30, ErrorMessage = "max Länge sollte 30 Zeichen sein")]
        public string Projektname { get; set; }

        //
        [DisplayName("Inhalt")] //Benennungen
        public string Erklärung { get; set; }

        //
        [DisplayName("Start-Datum")] //Benennungen
        public DateTime Startdatum { get; set; }

        //
        [DisplayName("Priorität")]
        public string Priorität { get; set; } //Priorität-Reigenfonlge von Projekten

        //
        [DisplayName("Abschluss in %")]
        public int ProjektStatus { get; set; } // diagrammförmige Ansicht  Balken-Rund Diagram (int ) und momentaner  Prozentwert in %

        //
        [DisplayName("End-Datum")] //Benennungen
        public DateTime? Enddatum { get; set; }

        //
        public bool Fertigstellen { get; set; } // Projekt abgeschlossen oder nicht JA / NEIN



        public virtual ICollection<PersonalAngaben> PersonalAngabens { get; set; }
        public virtual ICollection<KundenDaten> KundenDatens { get; set; }

        public virtual ICollection<Ansprechpartner> Ansprechpartner { get; set; }  // Veritabanı ilişkisi


        public virtual ICollection<Bericht> Berichte { get; set; } // Bir projenin birden fazla raporu olabilir.


        public int? KundenDatenID { get; set; } // Tek müşteri için
        public List<int> SelectedAnsprechpartnerIds { get; set; }
         public List<int> SelectedPersonalIds { get; set; } // <-- EKLENDİ! (View/Controller için GEREKLİ)
        /*Hinweis :
        ein Mitarbeiter kann mehr als ein Projekt haben und/oder ein Projekt kann mehr als einen Mitarbeiter haben.
        hier verwenden wir viele zu viele - Viele-zu-viele Verknüpfung Verbindungen
        Diese Struktur zeigt an, dass MitarbeiterAngaben mehr als ein MitarbeiterProjekt haben werden.
        .*/
    }
}