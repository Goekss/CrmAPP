using System.Collections.Generic;
using CrmAPP.Models.Kunden;

namespace CrmAPP.ViewModels
{
    public class ProjektEditViewModel
    {
        // Projeye atanabilecek tüm Ansprechpartner'ler & die Ansprechpartnern, die dem Projekt zugewiesen werden können
        public List<Ansprechpartner> AnsprechpartnerList { get; set; }

        // Projeye şu anda atanmış Ansprechpartner Id'leri & die aktuell dem Projekt zugewiesenen Ansprechpartner-Ids
        public List<int> SelectedAnsprechpartnerIds { get; set; }

      
    }
}