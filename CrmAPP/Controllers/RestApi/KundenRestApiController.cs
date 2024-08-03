using CrmAPP.Models.DataContext;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

[RoutePrefix("api/kunden")]
public class KundenRestApiController : ApiController
{
    private readonly DBc_Context db = new DBc_Context();

             // Test için açık bırak
    [HttpGet, Route("")]
   
    public IHttpActionResult GetAll()
    {
        var list = db.KundenDatens
            .Select(k => new { k.KundenDatenID, k.Vorname, k.Email,k.Website,k.Unternehmen /* ihtiyacın kadar alan */ })
            .ToList();
        return Ok(list);
    }

    [AllowAnonymous]
    [HttpGet, Route("personal")]
    public IHttpActionResult GetAlls() 
    {
        var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var liste = db.PersonalAngabens.Select(p => new { p.PersonalDatenID, BildUrl = baseUrl + "/Images/" + p.Bild, p.Vorname, p.Nachname, p.Email, p.RufNummer,p.Berechtigung, p.Abteilung }).ToList();
        return Ok(liste);
    }

    [AllowAnonymous]
    [RoutePrefix("api/projekte")]
    public class PersonalProjektesApiController : ApiController
    {
        private DBc_Context db = new DBc_Context();

        // GET api/projekte
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllProjekte()
        {
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var projektelist = db.PersonalProjektes.Select(pr => new
            {
                pr.PerProjID,
                pr.Projektname,
                Kunden = pr.KundenDatens.Select(k => new
                {
                    k.KundenDatenID,
                    k.Unternehmen, 
                    LogoUrl=baseUrl + "/Logos/" + k.Logo,
                    k.Email
                }),
                pr.Priorität,
                pr.ProjektStatus,
                pr.Enddatum,
                pr.Startdatum,
                pr.Erklärung,
                Personal = pr.PersonalAngabens.Select(p => new
                {
                    p.PersonalDatenID,
                    Name = p.Vorname + " " + p.Nachname
                }),
                Ansprechpartner = pr.Ansprechpartner.Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Tel,
                    a.Mail
                })
            }).ToList();

            return Ok(projektelist);
        }

        // GET api/projekte/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetProjektById(int id)
        {
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var projekt = db.PersonalProjektes
                .Where(pr => pr.PerProjID == id)
                .Select(pr => new
                {
                    pr.PerProjID,
                    pr.Projektname,
                    Kunden = pr.KundenDatens.Select(k => new
                    {
                        k.KundenDatenID,
                        k.Unternehmen,
                        LogoUrl = baseUrl + "/Logos/" + k.Logo,
                        k.Email
                    }),
                    pr.Priorität,
                    pr.ProjektStatus,
                    pr.Enddatum,
                    pr.Startdatum,
                    pr.Erklärung,
                    Personal = pr.PersonalAngabens.Select(p => new
                    {
                        p.PersonalDatenID,
                        Name = p.Vorname + " " + p.Nachname
                    }),
                    Ansprechpartner = pr.Ansprechpartner.Select(a => new
                    {
                        a.Id,
                        a.Name,
                        a.Tel,
                        a.Mail
                    })
                })
                .FirstOrDefault();

            if (projekt == null)
                return NotFound();

            return Ok(projekt);
        }
    }

        [AllowAnonymous]
    [HttpGet, Route("{id:int}")]
    public IHttpActionResult GetById(int id)
    {
        var x = db.KundenDatens.Find(id);
        return x == null ? (IHttpActionResult)NotFound() : Ok(x);
    }

    
    //===== Wetter vorhersage Api ======

    [AllowAnonymous]
    [HttpGet, Route("weather/{city}")]
    public async Task<IHttpActionResult> GetWeather(string city)
    {
        string apiKey = "dd8cabfb783bca005023ec8489e166de";
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=tr";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode || json.Contains("city not found"))
                return BadRequest("Şehir bulunamadı veya API'den veri alınamadı.");
            return Ok(json);
        }
    }
}
