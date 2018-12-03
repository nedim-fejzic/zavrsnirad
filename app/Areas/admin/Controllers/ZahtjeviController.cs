using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using app.Models;
using app.Areas.admin.ViewModels;
using PagedList;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class ZahtjeviController : Controller
    {
        private MojKontekst db = new MojKontekst();

        // GET: admin/Zahtjevs
        public ActionResult Index(int? page, int? OdabranaKategorija, string Naziv)
        {

            int id = OdabranaKategorija ?? 0;

            ZahtjevIndexVM model = new ZahtjevIndexVM();

            model.ListaKategorija = db.ZahtjevStatusDbSet.ToList();
            model.ListaKategorija.Insert(0, new Models.ZahtjevStatus() { Id = 0, Naziv = "Odaberite status zahtjeva..." });


            var u = db.ZahtjevDbSet.OrderByDescending(x => x.DatumPodnosenja).ToList();
          

            if (id != 0)
                u = u.Where(i => i.ZahtjevstatusId == id).ToList();

            if (!string.IsNullOrEmpty(Naziv))
                u = u.Where(i => i.Ime.Contains(Naziv) || i.Prezime.Contains(Naziv)).ToList();


            model.ListaRezultata = u.ToPagedList(page ?? 1, 10);

            return View(model);
        }

        public ActionResult IndexPromjena(int? page, int? OdabranaKategorija, string Naziv)
        {

            int id = OdabranaKategorija ?? 0;

            ZahtjevIndexPromjenaVM model = new ZahtjevIndexPromjenaVM();

            model.ListaKategorija = db.ZahtjevStatusDbSet.ToList();
            model.ListaKategorija.Insert(0, new Models.ZahtjevStatus() { Id = 0, Naziv = "Odaberite status zahtjeva..." });

            //Where(c=>c.ZatvorenZahtjev!=true)
            var u = db.ZahtjevPromjenaDbSet.OrderByDescending(x => x.Datum).ToList();


            if (id != 0)
                u = u.Where(i => i.ZahtjevstatusId == id).ToList();

            if (!string.IsNullOrEmpty(Naziv))
                u = u.Where(i => i.Korisnik.Ime.Contains(Naziv) || i.Korisnik.Prezime.Contains(Naziv)).ToList();


            model.ListaRezultata = u.ToPagedList(page ?? 1, 10);

            return View(model);
        }






        // GET: admin/Zahtjevs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.ZahtjevDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }


            if (TempData["error"] != null)
            {
                ViewBag.Message = TempData["error"].ToString();

            }



            ZahtjevDetaljiVM model = db.ZahtjevDbSet
            .Where(x => x.Id == id)
            .Select(f => new ZahtjevDetaljiVM
            {
                Id = f.Id,
                Broj = f.Broj,
                DatumZahtjeva = f.DatumPodnosenja.ToString(),
                Email = f.Email,
                Ime = f.Ime,
                Opcina = f.Opcina.NazivOpcine,
                Napomena = f.NapomenaKorisnika,
                Naselje = f.Naselje,
                Prezime = f.Prezime,
                Telefon = f.Telefon,
                TipKontakta = f.TipKontakta.Naziv,
                Ulica = f.NazivUlice,
                Komentar = f.Komentar,
                Status = f.ZahtjevStatus.Naziv,
                StatusId = f.ZahtjevStatus.Id,
                JMBG = f.JMBG,
                ListaTraznihUsluga = db.ZahtjevUslugeDbSet.Where(g => g.ZahtjevId == id).ToList()


            }).Single();







            return View(model);
        }

        // GET: admin/Zahtjevs/Create
        public ActionResult Create()
        {
            ZahtjevDodajVM model = new ZahtjevDodajVM();

            model.ListaPaketica = new List<ListaPaketaView> { };

            foreach (var item in db.PaketDbSet.ToList())
            {
                ListaPaketaView a = new ListaPaketaView
                {
                    Paket = item,
                    Izabran = false
                };
                model.ListaPaketica.Add(a);
            }


            model.ListaTipKontakta = db.TipKontaktaDbSet.ToList();
            model.ListaOpcina = db.OpcinaDbSet.ToList();

            //model.IzabraniPaketi = new List<int>();
            return View(model);
        }

       
        [HttpPost]
        public ActionResult Create(ZahtjevDodajVM model)
        {

            // ako nije nista izabravno u paketima dodaj error
            //@Html.ValidationMessageFor(model => Model.IzabraniPaketi, "", new { @class = "text-danger" })

            if (!ModelState.IsValid)
            {

                model.ListaPaketica = new List<ListaPaketaView> { };

                foreach (var item in db.PaketDbSet.ToList())
                {
                    ListaPaketaView a = new ListaPaketaView
                    {
                        Paket = item,
                        Izabran = false
                    };
                    model.ListaPaketica.Add(a);
                }


                if (model.IzabraniPaketi != null)
                {

                    foreach (var u in model.IzabraniPaketi)
                    {
                        foreach (var i in model.ListaPaketica)
                        {
                            if (u == i.Paket.Id)
                            {
                                i.Izabran = true;
                            }
                        }
                    }
                }




                model.ListaTipKontakta = db.TipKontaktaDbSet.ToList();
                model.ListaOpcina = db.OpcinaDbSet.ToList();
                
                return View(model);
            }

            //string a = "";
            //foreach (var item in model.PaketiciId)
            //{
            //     a = a + item.ToString() +" - " ;
            //}

            //IList<int> ee = model.IzabraniPaketi;



            //var odabrano = string.Join(",", model.IzabraniPaketi);
            //return Content("<script language='javascript' type='text/javascript'>alert('" + odabrano + " ');</script>");


            Zahtjev z = new Zahtjev()
            {   
                Broj = model.Broj,
                Email = model.Email,
                Ime = model.Ime,
                NapomenaKorisnika = model.NapomenaKorisnika,
                Telefon = model.Telefon,
                TipKontaktaId = model.TipKontaktaId,
                Naselje = model.Naselje,
                NazivUlice = model.NazivUlice,
                OpcinaId = model.OpcinaId,
                Prezime = model.Prezime,
                DatumPodnosenja = DateTime.Now,
                ZahtjevstatusId = 1,
                ZatvorenZahtjev = false,
                DatumZatvaranja = DateTime.Parse("2000-01-01"),
                Komentar = ""
            };

            db.ZahtjevDbSet.Add(z);
            db.SaveChanges();

            // napravljen zahtjev
            // dodamo izabrane usluge u novu tabelu na osnovu  id novog zahtjeva
            int tempBrojac = 0;

            int NoviZahtjevId = z.Id;
            foreach (var paketic in model.IzabraniPaketi)
            {
                ZahtjevUsluge zu = new ZahtjevUsluge()
                {
                    ZahtjevId = NoviZahtjevId,
                    PaketId = paketic
                };

                db.ZahtjevUslugeDbSet.Add(zu);
                db.SaveChanges();
                tempBrojac++;
            }

            int brojac2 = tempBrojac;


            return RedirectToAction("Index");
        }

        // GET: admin/Zahtjevs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zahtjev zahtjev = db.ZahtjevDbSet.Find(id);
            if (zahtjev == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PaketId = new SelectList(db.PaketDbSet, "Id", "Naziv", zahtjev.PaketId);
            ViewBag.TipKontaktaId = new SelectList(db.TipKontaktaDbSet, "Id", "Naziv", zahtjev.TipKontaktaId);
            return View(zahtjev);
        }

        
        [HttpPost]
        public ActionResult Napomena(ZahtjevDetaljiVM model)
        {
            Zahtjev zahtjev = db.ZahtjevDbSet.Find(model.Id);
            if (zahtjev == null)
            {
                return HttpNotFound();
            }

            Zahtjev o = db.ZahtjevDbSet.Find(model.Id);

            if (o == null)
                return HttpNotFound();

            o.Komentar = model.Komentar;

            db.SaveChanges();
            return RedirectToAction("Details", model);

        }




        public ActionResult PromjeniStatus(int ZahtjevId, int id2)
        {
            Zahtjev z = db.ZahtjevDbSet.Find(ZahtjevId);

            if (z == null)
                return HttpNotFound();

            z.ZahtjevstatusId = id2;
            db.SaveChanges();






            TempData["code"] = "info";
            TempData["Message"] = "Promijenjen je status zahtjeva u:  <b>" + db.ZahtjevStatusDbSet.Find(id2).Naziv + "</b>";


           

            return RedirectToAction("Details", "Zahtjevi", new { id = ZahtjevId, area = "Admin" });


        }





        public ActionResult Aktivacija(int ZahtjevId)
        {


            // proslijedimo zahtjev
            // iz zahtjeva preuzmemo listu usluga koje je korisnik trazio da aktivira
            // svaku uslugu aktiviramo

            Zahtjev z = db.ZahtjevDbSet.Find(ZahtjevId);
            Korisnik k = new Korisnik();


            if (z.JMBG != null)
            {

               k = db.KorisnikDbSet.Where(f => f.JMBG == z.JMBG).First();
                
            }
            else
            {
                TempData["code"] = "error";
                TempData["Message"] = "Niste generisali korisnika ili u zahtjevu nije preciziran JBMG korisnika.";


                return RedirectToAction("Details", "Zahtjevi", new { id = ZahtjevId, area = "Admin" });
            }



            // provjeri jesu li vec aktivirane usluge
            if (db.AktivneUslugeDbSet.Where(c => c.ZahtjevId == z.Id).ToList().Count > 0) {

                TempData["code"] = "error";
                TempData["Message"] = "Vec su aktivirane usluge po zahtjevu -  <b>" + z.Id + "</b>";

                return RedirectToAction("Details", "Zahtjevi", new { id = ZahtjevId, area = "Admin" });

            }
            






            List<ZahtjevUsluge> ListaTrazenihUsluga = db.ZahtjevUslugeDbSet.Where(f => f.ZahtjevId == ZahtjevId).ToList();

            foreach (var item in ListaTrazenihUsluga)
            {
                AktivneUsluge a = new AktivneUsluge() {
                    ZahtjevId = z.Id,
                    AdresaInstalacije = z.NazivUlice + " " + z.Broj,
                    DatumInstalacije = DateTime.Now,
                    DatumAktivacije = DateTime.Now,
                    PaketId = item.PaketId,
                    KorisnikId = k.Id     ,
                    AktivnaUsluga = true
                };

                db.AktivneUslugeDbSet.Add(a);
                db.SaveChanges();


            }

            TempData["code"] = "info";
            TempData["Message"] = "Aktivirano je  <b>" + ListaTrazenihUsluga.Count + "</b> usluga.";


            return RedirectToAction("Details", "Zahtjevi", new { id = ZahtjevId, area = "Admin" });



        }








        [HttpGet]
        public ActionResult Promjena(int id)
        {

            ZahtjevPromjena z = db.ZahtjevPromjenaDbSet.Find(id);


            ZahtjevPromjenaVM model = new ZahtjevPromjenaVM();

            model.ZahtjevId = id;
            model.Napomena = z.Napomena;
            model.Datum = z.Datum;
            model.AktivnaUslugaId = z.AktivneUslugeId;
            model.AktivnaUsluga = z.AktivneUsluge;

            model.NovaUslugaId = z.PaketId??0;
            model.PaketNovi = db.PaketDbSet.Find(z.PaketId);
          

            return View(model);

        }

        [HttpPost]
        public ActionResult Promjena(ZahtjevPromjenaVM model)
        {   

            AktivneUsluge a = db.AktivneUslugeDbSet.Find(model.AktivnaUslugaId);

            if (a == null)
                return HttpNotFound();


            a.AktivnaUsluga = false;
            a.DatumZatvaranja = DateTime.Now;
            db.SaveChanges();



            // korisnik id
            Korisnik k = db.KorisnikDbSet.Find(a.KorisnikId);
            if (k == null)
            {
                return HttpNotFound();
            }



            AktivneUsluge nova = new AktivneUsluge()
            {
                KorisnikId = k.Id,
                AdresaInstalacije = k.Adresa,
                AktivnaUsluga = true,
                DatumAktivacije = DateTime.Now,
                DatumInstalacije = DateTime.Now,
                PaketId = model.NovaUslugaId

            };

            db.AktivneUslugeDbSet.Add(nova);
            db.SaveChanges();

            ZahtjevPromjena z = db.ZahtjevPromjenaDbSet.Find(model.ZahtjevId);
            z.DatumZatvaranja = DateTime.Now;
            z.ZatvorenZahtjev = true;
            z.ZahtjevstatusId = 3;
            db.SaveChanges();



            Paket stara = db.PaketDbSet.Find(db.AktivneUslugeDbSet.Find(model.AktivnaUslugaId).PaketId);
            Paket novaa = db.PaketDbSet.Find(model.NovaUslugaId);


            TempData["code"] = "info";
            TempData["Message"] = "Iskljucili ste uslugu: <b>" + stara.TipUsluga.Naziv + " - " + stara.Naziv +"</b> <br><br>" + "Uspjesno ste aktivirali uslugu: <b>" + novaa.TipUsluga.Naziv + " - " + novaa.Naziv + "</b>" ;




            return RedirectToAction("IndexPromjena", "Zahtjevi", new { area = "Admin" });


        }



        [HttpGet]
        public ActionResult Iskljuci(int id)
        {
            ZahtjevPromjena z = db.ZahtjevPromjenaDbSet.Find(id);

            
            ZahtjevIskljuciVM model = new ZahtjevIskljuciVM();

            model.ZahtjevId = id;
            model.Napomena = z.Napomena;
            model.Datum = z.Datum;
            model.AktivnaUslugaId = z.AktivneUslugeId;
            model.AktivnaUsluga = z.AktivneUsluge;
            model.Razlog = z.Razlog;

            return View(model);


        }
        [HttpPost]
        public ActionResult Iskljuci(ZahtjevIskljuciVM model)
        {
            AktivneUsluge a = db.AktivneUslugeDbSet.Find(model.AktivnaUslugaId);

            if (a == null) return HttpNotFound();

            a.AktivnaUsluga = false;
            a.DatumZatvaranja = DateTime.Now;
            db.SaveChanges();

            ZahtjevPromjena z = db.ZahtjevPromjenaDbSet.Find(model.ZahtjevId);
            z.DatumZatvaranja = DateTime.Now;
            z.ZatvorenZahtjev = true;
            z.ZahtjevstatusId = 3;
            db.SaveChanges();


            TempData["code"] = "info";
            TempData["Message"] = "Iskljucili ste uslugu: <b>" + a.Paket.TipUsluga.Naziv + " - " + a.Paket.Naziv + "</b> <br><br>";

            return RedirectToAction("IndexPromjena", "Zahtjevi", new { area = "Admin" });
        }



























        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
