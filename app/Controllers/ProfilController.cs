using app.Models;
using app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace app.Controllers
{
    [AuthKorisnik]
    public class ProfilController : Controller
    {
        private MojKontekst db = new MojKontekst();



        public ActionResult Index(int id)
        {

            var k = db.KorisnikDbSet.Find(id);

            ProfilIndexVM model = new ProfilIndexVM();
            model.Id = id;
            model.ImePrezime = k.Ime + " " + k.Prezime;
            model.JBMG = k.JMBG;
            model.Adresa = k.Adresa;
            model.Email = k.Email;
            model.Naselje = k.Naselje;
            model.Opcina = k.Opcina.NazivOpcine;
            model.Telefon = k.Telefon;

            model.ListaAktivnihUsluga = db.AktivneUslugeDbSet.Where(c => c.KorisnikId == k.Id && c.AktivnaUsluga == true).ToList();


            return View(model);
        }







        public ActionResult Lozinka(int id)
        {

            Korisnik k = db.KorisnikDbSet.Find(id);

            if (k == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            ProfilLozinkaVM model = new ProfilLozinkaVM();
            model.ImePrezime = k.Username;
            model.Id = k.Id;
            model.StariPassword = k.Password;


            return View(model);
        }

        [HttpPost]
        public ActionResult Lozinka(ProfilLozinkaVM model)
        {

            if (model.StaraLozinka != model.StariPassword)
            {
                ModelState.AddModelError("StaraLozinka", "Lozinka nije ispravna!");
            }


            if (!ModelState.IsValid)
            {

                return View(model);
            }


            Korisnik k = db.KorisnikDbSet.Find(model.Id);

            if (k == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            k.Password = model.NovaLozinka;
            db.SaveChanges();

            // old pass = new pass
            TempData["Message"] = "Uspješno ste promjenili lozinku!";
            TempData["code"] = "info";

            return View(model);
        }

    }
}