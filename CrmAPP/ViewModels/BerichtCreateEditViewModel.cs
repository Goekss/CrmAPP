using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace CrmAPP.ViewModels
{
    public class BerichtCreateEditViewModel
    {
        public int? BerichtId { get; set; }

        [Display(Name = "Titel")]
        [StringLength(100)]
        public string Titel { get; set; }

        [Required(ErrorMessage = "Projekt auswählen")]
        public int PerProjID { get; set; }

        [Required(ErrorMessage = "Kunde auswählen")]
        public int KundenDatenId { get; set; }

        [Display(Name = "Version")]
        [StringLength(10)]
        public string Version { get; set; }

        // Upload edilen dosya (Create için zorunlu, Edit'te opsiyonel)
        public HttpPostedFileBase Datei { get; set; }

        // Edit sırasında mevcut dosyayı göstermek için
        public string BestehendeDateiPfad { get; set; }

        // HochgeladenAm nullable DateTime ekleyin
        [Display(Name = "Hochgeladen Am")]
        public DateTime? HochgeladenAm { get; set; }  // Nullable DateTime ekledik

        // Dropdown listeleri
        public IEnumerable<SelectListItem> ProjektListe { get; set; }
        public IEnumerable<SelectListItem> KundenListe { get; set; }
    }
}