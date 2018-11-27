using app.Areas.admin.ViewModels;
using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class OpremaController : Controller
    {

        private MojKontekst db = new MojKontekst();

        // GET: admin/Oprema
        public ActionResult Index(int? page, string device, string tip)
        {
            

            if (device== null)
                device = "";

           

            OpremaIndexVM model = new OpremaIndexVM();
            model.device = device;


            var res = db.UredjajDbSet.OrderByDescending(x => x.DatumNabavke).ToList();

            if (device != "")
                res = res.Where(i => i.Naziv.Contains(device) || i.SerijskiBroj.Contains(device)  ).ToList();



            if (tip != null && tip != "")
            {
                if (tip == "Z")
                {
                    res = res.Where(i => i.UUpotrebi == true).ToList();

                }
                else if(tip == "S")
                {
                    res = res.Where(i => i.UUpotrebi == false).ToList();
                }
            }


            model.ListaRezultata = res.ToPagedList(page ?? 1, 8);


            model.tip = tip;
            model.SlobodnoUredajaja = db.UredjajDbSet.Where(c => c.UUpotrebi == false).Count().ToString();
            model.ZauzetoUredjaja = db.UredjajDbSet.Where(c => c.UUpotrebi == true).Count().ToString();

            return View(model);
        }



        [HttpGet]
        public ActionResult Dodaj()
        {

            OpremaDodajVM model = new OpremaDodajVM();
            model.DatumNabavke = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        public ActionResult Dodaj(OpremaDodajVM model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.DatumString = model.DatumNabavke.ToString("dd-MM-yyyy");

            return RedirectToAction("SerijskiBrojevi", model);
        }

        

  

        [ActionName("SerijskiBrojevi"), HttpGet]
        public ActionResult SerijskiBrojeviGet(OpremaDodajVM model)
        {

            if (model.Naziv == "" || model.Naziv == null)
            {
                return RedirectToAction("Dodaj");
            }

            return View(model);

        }


        [ActionName("SerijskiBrojevi"), HttpPost]
        public ActionResult SerijskiBrojeviPost(OpremaDodajVM model)
        {
            // bug postoji kada se izabere datum gdje je broj dana veci od 12
            // u HttpGet metodi postoji objekat datum, ali na httpPost polje model.DatumNabavke postaje null
            // privremeno rijeseno da se datum konvertuje u string i prilikom inserta u bazu kreira tempDatum objekat


            int brojac = 0;

            for (int i = 0; i < model.Kolicina; i++)
            {

                model.ListaSerijskihBrojeva[i] = Regex.Replace(model.ListaSerijskihBrojeva[i], @"\s+", "");


                if (model.ListaSerijskihBrojeva[i] == "" || model.ListaSerijskihBrojeva[i] == null)
                {
                    ModelState.AddModelError("ListaSerijskihBrojeva[" + i + "]", "Serijski broj je obavezno polje!");
                    brojac++;
                }

                if (model.ListaSerijskihBrojeva[i].Length < 3)
                {
                    ModelState.AddModelError("ListaSerijskihBrojeva[" + i + "]", "Serijski broj mora sadržavati više od 3 znaka!");
                    brojac++;
                }
              
            }




            //if (model.ListaSerijskihBrojeva.GroupBy(n => n).Any(c => c.Count() > 0))
            //{
            //    brojac++;
            //}


            if (model.ListaSerijskihBrojeva.Distinct().Count() != model.ListaSerijskihBrojeva.Count())
            {
                brojac++;
            }



            if (brojac > 0)
            {
                ModelState.AddModelError(string.Empty,"Serijski brojevi moraju biti jedinstveni, imate duplikat serijskog broja!");
                return View(model);
            }

            // create
            var dt = DateTime.ParseExact(model.DatumString, "dd-MM-yyyy", CultureInfo.InvariantCulture);





            for (int i = 0; i < model.Kolicina; i++)
            {
                Uredjaj u = new Uredjaj()
                {
                    Naziv = model.Naziv,
                    Proizvodjac = model.Proizvodzac,
                    DatumNabavke = dt,
                    UUpotrebi = false,
                    Otpisan = false,
                    SerijskiBroj = model.ListaSerijskihBrojeva[i]

                };


                db.UredjajDbSet.Add(u);
                db.SaveChanges();

            }





            TempData["Message"] = "Uspješno je dodano: <b>" + model.Kolicina + "</b> uređaja!";
            TempData["code"] = "info";


            return RedirectToAction("Index");







        }



        [HttpGet]
        public ActionResult Uredi(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.UredjajDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            OpremaUrediVM model = db.UredjajDbSet
              .Where(x => x.Id == id)
              .Select(f => new OpremaUrediVM
              {
                  Id = f.Id,
               Proizvodzac = f.Proizvodjac,
                  DatumNabavke = f.DatumNabavke,
                  Naziv = f.Naziv,
                  SerijskiBroj = f.SerijskiBroj


              }).Single();

            model.DatumString = model.DatumNabavke.ToString("dd-MM-yyyy");


            return View(model);
        }

        [HttpPost]
        public ActionResult Uredi(OpremaUrediVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Uredjaj o = db.UredjajDbSet.Find(model.Id);

            if (o == null)
                return HttpNotFound();


            var dt = DateTime.ParseExact(model.DatumString, "dd-MM-yyyy", CultureInfo.InvariantCulture);



            o.DatumNabavke = dt;
            o.Naziv = model.Naziv;
            o.Proizvodjac = model.Proizvodzac;
            o.SerijskiBroj = model.SerijskiBroj;


            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Zaduzi(int id)
        {

            if (db.UredjajDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

        
            OpremaZaduziVM model = new OpremaZaduziVM();

            Uredjaj u  = db.UredjajDbSet.Where(f => f.Id == id).FirstOrDefault(); ;
            if (u.UUpotrebi == true)
            {
                return RedirectToAction("Razduzi", new { id = u.Id });
            }
           
            model.DatumZaduzenja = DateTime.Today;
            model.ListaKorisnika = db.KorisnikDbSet.ToList();
            model.OpremaId = id;
            model.device = u;

            return View(model);
        }


        [HttpPost]
        public ActionResult Zaduzi(OpremaZaduziVM model)
        {

            if (!ModelState.IsValid)
            {
                model.DatumZaduzenja = DateTime.Today;
                model.ListaKorisnika = db.KorisnikDbSet.ToList();
                Uredjaj u = db.UredjajDbSet.Where(f => f.Id == model.Id).FirstOrDefault(); ;
                model.OpremaId = model.Id;
                model.device = u;

                return View(model);
            }

            // zaduzi uredjaj
            UredjajZaduzenje uz = new UredjajZaduzenje()
            {
                KorisnikId = model.KorisnikId,
                DatumZaduzenja = model.DatumZaduzenja,
                UredjajId = model.OpremaId,
                Vracen = false
            };
            db.UredjajZaduzenjeDbSet.Add(uz);
            db.SaveChanges();


            // setuj u upotrebi na true
            Uredjaj o = db.UredjajDbSet.Find(model.OpremaId);
            o.UUpotrebi = true;
            db.SaveChanges();


            Korisnik k = db.KorisnikDbSet.Find(model.KorisnikId);
            TempData["Message"] = "Uspješno je zaduzen korisnik: <b>" + k.Ime + " " + k.Prezime + "</b>! </br></br> Korisnik je zadužio uređaj <b>" + o.Proizvodjac + " " + o.Naziv + "</b>";
            TempData["code"] = "info";

            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Razduzi(int id)
        {
            if (db.UredjajDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            OpremaRazduziVM model = new OpremaRazduziVM();
            Uredjaj u = db.UredjajDbSet.Where(f => f.Id == id).FirstOrDefault(); ;
            UredjajZaduzenje uz = db.UredjajZaduzenjeDbSet.Where(v => v.UredjajId == u.Id).FirstOrDefault();

            //model.device = db.UredjajDbSet.Where(f => f.Id == model.OpremaId).FirstOrDefault();

            model.OpremaId = u.Id;
            model.user = db.KorisnikDbSet.Where(r=>r.Id == uz.KorisnikId).FirstOrDefault();
            model.DatumZaduzenja = uz.DatumZaduzenja;
            model.device = u;

            return View(model);

        }

        [HttpPost]
        public ActionResult Razduzi(OpremaRazduziVM model)
        {
            Uredjaj u = db.UredjajDbSet.Where(f => f.Id == model.Id).FirstOrDefault(); ;
            UredjajZaduzenje uz = db.UredjajZaduzenjeDbSet.Where(v => v.UredjajId == u.Id).FirstOrDefault();

            if (model.DatumVracanja <= model.DatumZaduzenja)
            {
                ModelState.AddModelError("DatumVracanja", "Datum povratka mora biti veci od datuma predaje uredjaja!");

            }

            if (!ModelState.IsValid)
            {

                model.OpremaId = u.Id;
                model.user = db.KorisnikDbSet.Where(r => r.Id == uz.KorisnikId).FirstOrDefault();
                model.DatumZaduzenja = uz.DatumZaduzenja;
                model.device = u;

                return View(model);
            }


            Uredjaj o = db.UredjajDbSet.Find(model.Id);

            if (o == null)
                return HttpNotFound();

            uz.DatumPovratka = model.DatumVracanja;
            o.UUpotrebi = false;
            db.SaveChanges();

            model.user = db.KorisnikDbSet.Where(c => c.Id == uz.KorisnikId).FirstOrDefault();
            
            TempData["Message"] = "Uspješno je razduzen korisnik: <b>" + model.user.Ime + " " + model.user.Prezime + "</b>! </br></br> Korisnik je vratio uredjaj <b>" + o.Proizvodjac + " " + o.Naziv + "</b>";
            TempData["code"] = "info";

            return RedirectToAction("Index");
        }
    }
}


