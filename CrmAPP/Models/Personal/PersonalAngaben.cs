using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CrmAPP.Models.Projekt;
using CrmAPP.Models.Kunden;
using System.Web.Mvc;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace CrmAPP.Models.Personal
{
    public class PersonalAngaben
    {
        public PersonalAngaben()
        {
            this.PersonalProjektes = new HashSet<PersonalProjekte>();
        }
        /*-HashSet<MitarbeiterProjekte>: Eine Sammlung, die in Many-to-many-Beziehungen verwendet wird,
        um viele Objekte einer Klasse zu speichern,die mit anderen Klasse in Beziehung stehen.
        Benzersiz bilgilerin girilmesini sağlar, tekrarı engeller.
        */
        /*Hinweis: 
          -Diese MitarbeiterProjekte Klasse stellt die Zwischentabelle dar, die die Beziehung zwischen zwei Tabellen herstellt.
          -Dieses Codefragment(parcasi) gibt an, dass PersonalDaten (ein Mitarbeiter) mehrere MitarbeiterProjects haben können.
         */
        [Key]//< weist darauf hin, dass die PersonalDatenID zu dieser Tabelle/Class gehört.
        public int PersonalDatenID { get; set; }

        [Required(ErrorMessage = "Vorname darf nicht leer sein")]
        [DisplayName("Vorname")] 
        [StringLength(30, ErrorMessage = "max Länge sollte 30 Zeichen sein")]
        public string Vorname { get; set; }


        [Required(ErrorMessage = "Nachname darf nicht leer sein")]
        [DisplayName("Nachname")] 
        [StringLength(30, ErrorMessage = "max Länge sollte 30 Zeichen sein")]
        public string Nachname { get; set; }


        [Required(ErrorMessage = "Bild darf nicht leer sein")]
        [DisplayName("Bild")]
        public string Bild { get; set; }


        [Required(ErrorMessage = "E-Mail darf nicht leer sein")]
        [DisplayName("E-Mail")]
        [StringLength(25, ErrorMessage = "max Länge sollte 25 Zeichen sein")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Passwort darf nicht leer sein")]
        [DisplayName("Passwort")]
        [StringLength(64, ErrorMessage = "Ihr Passwort sollte 64 Zeichen lang sein")]
        public string Passwort { get; set; }


        [Required(ErrorMessage = "Rufnummer darf nicht leer sein")]
        [DisplayName("Rufnummer")]
        [StringLength(20, ErrorMessage = "max Länge sollte 20 Zeichen sein")]
        [Phone(ErrorMessage = "ungültige Telefonnummer")]
        public string RufNummer { get; set; }


        [Required(ErrorMessage = "Abteilung darf nicht leer sein")]
        [DisplayName("Abteilung")]
        public string Abteilung { get; set; }


        [Required(ErrorMessage = "Aufgaben darf nicht leer sein")]
        [DisplayName("Aufgaben")]
        public string Aufgaben { get; set; }


        [Required(ErrorMessage = "Berechtigung darf nicht leer sein")]
        [DisplayName("Berechtigung")]
        public string Berechtigung { get; set; }



        [Required(ErrorMessage = "Rol darf nicht leer sein")]
        [DisplayName("Rol")]
        [StringLength(50, ErrorMessage = "max Länge sollte 10 Zeichen sein")]
        public string Rol { get; set; }


        [Required(ErrorMessage = "Hinweise darf nicht leer sein")]
        [DisplayName("Hinweise")]
        public string Hinweise { get; set; }

        public bool IsLockedOut { get; set; }
        public DateTime? LockoutTime { get; set; }

        public byte FailedLoginAttempts { get; set; } //Anzahl der fehlgeschlagenen Anmeldeversuche    



        public virtual ICollection<PersonalProjekte> PersonalProjektes { get; set; }
        public virtual ICollection<KundenDaten> KundenDatens { get; set; }

        

        /*Hinweis :
        ein Mitarbeiter kann mehr als ein Projekt haben und ein Projekt kann mehr als einen Mitarbeiter haben.
        hier verwenden wir viele zu viele - Viele-zu-viele Verknüpfung.*/
    }
}