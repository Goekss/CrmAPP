using CrmAPP.Models.Kunden;
using CrmAPP.Models.Personal;
using CrmAPP.Models.Projekt;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CrmAPP.Models.DataContext
{
    public class DBc_Context: DbContext
    {
        //hier ist Verbindungslink(NAME) für die Datenbank.
        //1) Danach muss man enable-Migrations durchführen.
        //Danach wird sich die Datei MIGRATIONS(Configt.cs) hinzufügt automatisch von false auf true geändert.
        public DBc_Context() : base("Verbindung")
        {


        }
        public DbSet<PersonalAngaben> PersonalAngabens { get; set; }

        public DbSet<PersonalProjekte> PersonalProjektes { get; set; }
        
        public DbSet<KundenDaten> KundenDatens { get; set; }

        public DbSet<Ansprechpartner> Ansprechpartners { get; set; }

        public DbSet<Bericht> Berichte { get; set; }
    }
}