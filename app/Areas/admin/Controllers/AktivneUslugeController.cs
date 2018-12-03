using app.Areas.admin.ViewModels;
using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class AktivneUslugeController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index(int? page, int? ListaTipUsluga, int ? ListaPaketa, string ImePrezime)
        {

            int usluga = ListaPaketa ?? 0;
            int tip = ListaTipUsluga ?? 0;


            AktivneUslugeIndexVm model = new AktivneUslugeIndexVm();
            model.ListaPaketa = db.PaketDbSet.ToList();
            model.ListaPaketa.Insert(0, new Models.Paket() { Id = 0, Naziv = "Odaberite paket..." });

            model.ListaTipUsluga = db.TipUslugaDbSet.ToList();
            model.ListaTipUsluga.Insert(0, new Models.TipUsluga() { Id = 0, Naziv = "Odaberite tip usluge..." });


            var rez = db.AktivneUslugeDbSet.Where(v=>v.AktivnaUsluga==true).OrderByDescending(x => x.DatumInstalacije).ToList();

            if (tip != 0)
                rez = rez.Where(i => i.Paket.TipUslugaId == tip).ToList();

            if (usluga != 0)
                rez = rez.Where(i => i.PaketId == usluga).ToList();

            if (!string.IsNullOrEmpty(ImePrezime))
                rez = rez.Where(i => i.Korisnik.Ime.Contains(ImePrezime)).ToList();





            model.ListaRezultata = rez.ToPagedList(page ?? 1, 10);

            return View(model);

        }
        [HttpGet]
        public ActionResult Detalji(int id)
        {
            // korisnik  je id
            if (db.KorisnikDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            AktivneUslugeDetaljiVM model = new AktivneUslugeDetaljiVM();
            model.ListaAktivnihUsluga = db.AktivneUslugeDbSet.Where(c => c.KorisnikId == id && c.AktivnaUsluga == true).ToList();

            model.KorisnikId = id;
            model.Korisnik = db.KorisnikDbSet.Where(c => c.Id == id).FirstOrDefault();



            return View(model);
        }
        public ActionResult Iskljuci(int id)
        {
            //proslijedjen id usluge koju cemo iskljuciti
           
            AktivneUsluge q = db.AktivneUslugeDbSet.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            q.AktivnaUsluga = false;
            q.DatumZatvaranja = DateTime.Now;
            db.SaveChanges();

            TempData["code"] = "info";
            TempData["Message"] = "Zatvorili ste aktivnu uslugu:  <b>" + q.Paket.TipUsluga.Naziv + " - " + q.Paket.Naziv + "</b>";


            return RedirectToAction("Detalji", new {id = q.KorisnikId });
        }
        public ActionResult IskljuciSve(int id)
        {
            //proslijedjen id korisnika


            var l = db.AktivneUslugeDbSet.Where(c=>c.KorisnikId == id && c.AktivnaUsluga == true).ToList();
            if (l == null)
            {
                return HttpNotFound();
            }
            if (l.Count()<1)
            {
                TempData["code"] = "info";
                TempData["Message"] = "Korisnik nema aktivnih usluga!";
                return RedirectToAction("Detalji", new { id = id });

            }

            int brojac = 0;
            foreach (var item in l)
            {
                item.AktivnaUsluga = false;
                item.DatumZatvaranja = DateTime.Now;
                db.SaveChanges();
                brojac++;
            }

            TempData["code"] = "info";
            TempData["Message"] = "Zatvorili ste:  <b>" + brojac + "</b>" + " aktivnih usluga!" ;


            return RedirectToAction("Detalji", new { id = id });
        }
        [HttpGet]
        public ActionResult Aktiviraj(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik k = db.KorisnikDbSet.Find(id);
            if (k == null)
            {
                return HttpNotFound();
            }

            AktivneUslugeAktivirajVM model = new AktivneUslugeAktivirajVM();
            model.Korisnik = k.Ime + " " + k.Prezime;
            model.KorisnikId = k.Id;
            model.ListaPaketa = db.PaketDbSet.ToList().OrderBy(c=>c.TipUslugaId).ToList();
            model.BrojUsluga = db.AktivneUslugeDbSet.Where(c => c.KorisnikId == k.Id && c.AktivnaUsluga == true).Count();
            model.AdresaInstalacije = k.Adresa;

            return View(model);

        }
        [HttpPost]
        public ActionResult Aktiviraj(AktivneUslugeAktivirajVM model)
        {
            DateTime temp;
            if (model.DatumInstalacije != null)
            {
                if (DateTime.TryParse(model.DatumInstalacije.ToString(), out temp))
                {
                    if (model.DatumInstalacije.Value.Year < 2018)
                    {
                        ModelState.AddModelError("DatumInstalacije", "Datum instalacije nije ispravan!");

                    }
                }
                else
                {
                    ModelState.AddModelError("DatumInstalacije", "Datum instalacije nije ispravan!");

                }
            }
            if (!ModelState.IsValid)
            {
                Korisnik vv = db.KorisnikDbSet.Find(model.KorisnikId);
                model.Korisnik = vv.Ime + " " + vv.Prezime;
                model.ListaPaketa = db.PaketDbSet.ToList().OrderBy(c => c.TipUslugaId).ToList();
                model.BrojUsluga = db.AktivneUslugeDbSet.Where(c => c.KorisnikId == model.KorisnikId && c.AktivnaUsluga == true).Count();
                return View(model);
            }
            Korisnik k = db.KorisnikDbSet.Find(model.KorisnikId);
            AktivneUsluge a = new AktivneUsluge()
            {
               KorisnikId = k.Id,
               AdresaInstalacije = k.Adresa,
               AktivnaUsluga = true,
               DatumAktivacije = DateTime.Now,
               DatumInstalacije = DateTime.Now,
               PaketId = model.PaketId
               
            };
            db.AktivneUslugeDbSet.Add(a);
            db.SaveChanges();

            return RedirectToAction("Detalji", new {id = k.Id });
        }
        [HttpGet]
        public ActionResult NovaAktivacija()
        {
            AktivneUslugeNovaAktivacijaVM model = new AktivneUslugeNovaAktivacijaVM();
            model.ListaPaketa = db.PaketDbSet.ToList().OrderBy(c => c.TipUslugaId).ToList();
            model.ListaKorisnika = db.KorisnikDbSet.OrderBy(c => c.Ime).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult NovaAktivacija(AktivneUslugeNovaAktivacijaVM model)
        {
            DateTime temp;
            if (model.DatumInstalacije != null)
            {
                if (DateTime.TryParse(model.DatumInstalacije.ToString(), out temp))
                {
                    if (model.DatumInstalacije.Value.Year < 2018)
                    {
                        ModelState.AddModelError("DatumInstalacije", "Datum instalacije nije ispravan!");

                    }
                }
                else
                {
                    ModelState.AddModelError("DatumInstalacije", "Datum instalacije nije ispravan!");

                }
            }
            if (!ModelState.IsValid)
            {
                model.ListaPaketa = db.PaketDbSet.ToList().OrderBy(c => c.TipUslugaId).ToList();
                model.ListaKorisnika = db.KorisnikDbSet.OrderBy(c => c.Ime).ToList();
                return View(model);
            }

            Korisnik k = db.KorisnikDbSet.Find(model.KorisnikId);

            AktivneUsluge a = new AktivneUsluge()
            {
                KorisnikId = k.Id,
                AdresaInstalacije = k.Adresa,
                AktivnaUsluga = true,
                DatumAktivacije = DateTime.Now,
                DatumInstalacije = DateTime.Now,
                PaketId = model.PaketId

            };

            db.AktivneUslugeDbSet.Add(a);
            db.SaveChanges();

            return RedirectToAction("Detalji", new { id = k.Id });
        }
    }
}