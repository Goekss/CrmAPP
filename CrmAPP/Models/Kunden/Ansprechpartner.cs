using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace CrmAPP.Models.Kunden
{
    public class Ansprechpartner
    {
        public int Id { get; set; }

        [DisplayName("Adı Soyadı")]
        [StringLength(25)]
        public string Name { get; set; }

        [DisplayName("Telefon")]
        [StringLength(40)]
        public string Tel { get; set; }

        [DisplayName("E-Mail")]
        [StringLength(40, MinimumLength = 5)]
        public string Mail { get; set; }

        [DisplayName("Filiale")]
        public string Filiale { get; set; }

        // Hangi müşteriye (KundenDaten) ait olduğu:
        public int KundenDatenID { get; set; }
        public virtual KundenDaten KundenDaten { get; set; }
    }
}