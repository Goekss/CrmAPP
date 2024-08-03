using CrmAPP.Models.Personal;
using CrmAPP.Models.Kunden;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmAPP.Models.Projekt
{
    public class Bericht
    {
        [Key]
        public int BerichtId { get; set; } // Raporun benzersiz kimliği

        [DisplayName("Titel")]
        [StringLength(100, ErrorMessage = "Die maximale Länge sollte 100 Zeichen betragen.")]
        public string Titel { get; set; } // Rapor başlığı

        [DisplayName("Datei Pfad")]
        public string DateiPfad { get; set; } // Dosyanın sunucudaki yolu

        [DisplayName("Datei Typ")]
        public string DateiTuru { get; set; } // Dosya türü (ör. PDF, DOCX, XLSX)

        [DisplayName("Version")]
        [StringLength(10, ErrorMessage = "Die maximale Länge sollte 10 Zeichen betragen.")]
        public string Version { get; set; } // Raporun versiyonu (ör. v1.0, v2.0)

        [DisplayName("Hochgeladen Am")]
        public DateTime? HochgeladenAm { get; set; } = DateTime.Now; // Varsayılan tarih

        [DisplayName("Bearbeitet Am")]
        public DateTime? BearbeitetAm { get; set; }

        [DisplayName("Gelöscht Am")]
        public DateTime? GeloschtAm { get; set; }

        // 🔹 Yükleyen kişi
        [DisplayName("Hochgeladen Von")]
        public int? HochgeladenVonId { get; set; }

        [ForeignKey("HochgeladenVonId")]
        public virtual PersonalAngaben HochgeladenVon { get; set; }

        // 🔹 Düzenleyen kişi
        [DisplayName("Bearbeitet Von")]
        public int? BearbeitetVonId { get; set; }

        [ForeignKey("BearbeitetVonId")]
        public virtual PersonalAngaben BearbeitetVon { get; set; }

        // 🔹 Silen kişi
        [DisplayName("Gelöscht Von")]
        public int? GeloschtVonId { get; set; }

        [ForeignKey("GeloschtVonId")]
        public virtual PersonalAngaben GeloschtVon { get; set; }

        // 🔹 Proje bağlantısı
        [DisplayName("Projekt")]
        [ForeignKey("Projekt")]
        public int PerProjID { get; set; }
        public virtual PersonalProjekte Projekt { get; set; }

        // 🔹 Müşteri bağlantısı
        [DisplayName("Kunde")]
        [ForeignKey("Kunde")]
        public int KundenDatenId { get; set; }
        public virtual KundenDaten Kunde { get; set; }
    }
}
