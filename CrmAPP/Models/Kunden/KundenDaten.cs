using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CrmAPP.Models.Projekt;
using CrmAPP.Models.Personal;



namespace CrmAPP.Models.Kunden
{
    public class KundenDaten
    {
        public KundenDaten()
        {
            this.PersonalProjektes = new HashSet<PersonalProjekte>();
            this.AnsprechpartnerList = new List<Ansprechpartner>();
        }



        [Key]
        public int KundenDatenID { get; set; }

        [DisplayName("Unternehmen")] //Benennungen
        [StringLength(30, ErrorMessage = "max Länge sollte 30 Zeichen sein")]
        public string Unternehmen { get; set; }

        //
        [DisplayName("Vorname")] //Benennungen
        [StringLength(30, ErrorMessage = "max Länge sollte 30 Zeichen sein")]
        public string Vorname { get; set; }
        //
        [DisplayName("Nachname")]
        [StringLength(25, ErrorMessage = "max Länge sollte 25 Zeichen sein")]
        public string Nachname { get; set; }
        //
        [DisplayName("Logo")]
        public string Logo { get; set; }
        //
        [DisplayName("E-Mail")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "Die Adresse sollte zwischen 5 und 40 Zeichen lang sein")]
        public string Email { get; set; }
        //
        [DisplayName("Telefon Mobile")]
        [StringLength(40, ErrorMessage = "max Länge sollte 40 Zeichen sein")]
        public string TelefonMobil { get; set; }

        [DisplayName("Telefon Home ")]
        [StringLength(40, ErrorMessage = "max Länge sollte 40 Zeichen sein")]
        public string TelefonHaus { get; set; }
        //
        [DisplayName("Projekt/Auftrag")]
        [StringLength(40, ErrorMessage = "max Länge sollte 40 Zeichen sein")]
        public string ProjektAuftrag { get; set; }
        //
        [DisplayName("Website")]
        [StringLength(25, ErrorMessage = "max Länge sollte 25 Zeichen sein")]
        public string Website { get; set; }
        //
        [DisplayName("Adresse")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Die Adresse sollte zwischen 5 und 100 Zeichen lang sein")]
        public string Adresse { get; set; }
        //
        [DisplayName("Hinweise")]
        public string Hinweise { get; set; }


        public virtual ICollection<PersonalProjekte> PersonalProjektes { get; set; }  //One-to-many
        public virtual List<Ansprechpartner> AnsprechpartnerList { get; set; }





    }


}