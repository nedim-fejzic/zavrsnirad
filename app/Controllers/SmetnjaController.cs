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
    public class SmetnjaController : Controller
    {

        private MojKontekst db = new MojKontekst();

        public ActionResult Index()
        {

            int korisnikid = (int)Session["logiran_korisnik"];
            Korisnik k = db.KorisnikDbSet.Find(korisnikid);

            if (k == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }



            SmetnjaIndexVM model = new SmetnjaIndexVM();
            model.ListaSmetnji = db.SmetnjeDbSet.Where(c => c.KorisnikId == korisnikid).ToList();


            return View(model);
        }

        public ActionResult Prijava()
        {
            int korisnikid = (int)Session["logiran_korisnik"];
            Korisnik k = db.KorisnikDbSet.Find(korisnikid);

            if (k == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }


            SmetnjaPrijavaVM model = new SmetnjaPrijavaVM();
            model.ListaAktivnihUsluga = db.AktivneUslugeDbSet.Where(c => c.KorisnikId == korisnikid).ToList();
            model.KorisnikId = korisnikid;
            model.ListaSmetnjaStatus = db.SmetnjeStatusDbSet.ToList();

            return View(model);

        }
        [HttpPost]
        public ActionResult Prijava(SmetnjaPrijavaVM model)
        {
            var sad = DateTime.Now;


            Smetnje s = new Smetnje();
            s.AktivnaUslugaId = model.AktivnaUslugaId;
            s.DatumUocavanja = model.DatumUocavanja;
            s.DatumZatvaranja = null;
            s.KorisnikId = model.KorisnikId;
            s.SmetnjeStatusId = 1;
            s.Opis = model.Opis;
            s.DatumOtvaranja = sad;

            // generisemo id 
            int ajdi = 1;
            var fg = db.SmetnjeDbSet.OrderByDescending(u => u.Id).FirstOrDefault();
            if (fg != null)
            {
                ajdi = fg.Id + 1;
            }


            s.BrojSmetnje = ajdi + "" + sad.ToString("HHmm");





            db.SmetnjeDbSet.Add(s);
            db.SaveChanges();


            TempData["Message"] = "Uspješno prijavljena smetnja pod brojem: <b>" + s.BrojSmetnje + "</b>";
            TempData["code"] = "info";


            return RedirectToAction("Index");

        }

        public ActionResult Detalji(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smetnje s = db.SmetnjeDbSet.Find(id);
            if (s == null)
            {
                return HttpNotFound();
            }

            SmetnjaDetaljiVM model = new SmetnjaDetaljiVM();

            model.DatumPodnosenja = s.DatumOtvaranja.ToString("dd-MM-yyyy");
            model.DatumUocavanja = s.DatumUocavanja?.ToString("dd/MM/yyyy");
            model.Id = s.Id;
            model.Sifra = s.BrojSmetnje;
            model.StatusSmetnje = s.SmetnjeStatus.Naziv;
            model.Opis = s.Opis;
            model.Korisnik = s.Korisnik;

            if (s.AktivnaUsluga != null)
            {
                model.Usluga = s.AktivnaUsluga.Paket.Naziv;

            }


            model.ListaOdgovora = db.SmetnjeOdgovori.Where(c => c.SmetnjaId == model.Id).ToList();


            return View(model);
        }
        public ActionResult Odgovor(SmetnjaDetaljiVM model)
        {

            Smetnje s = db.SmetnjeDbSet.Find(model.Id);
            if (s == null)
            {
                return HttpNotFound();
            }


            if (model.Odgovor == null || model.Odgovor == "")
            {
                TempData["Message"] = "Da bi odgovorili na smetnju, morate unijeti poruku!";
                TempData["code"] = "error";

                ModelState.AddModelError("Odgovor", "Odgovor mora sadrzavti poruku!");
                return RedirectToAction("Detalji", new { id = model.Id });
            }

            SmetnjeOdgovori o = new SmetnjeOdgovori();

            o.SmetnjaId = model.Id;
            o.Poruka = model.Odgovor;

            o.KorisnikId = (int)Session["logiran_korisnik"];
            o.Datum = DateTime.Now;

            db.SmetnjeOdgovori.Add(o);
            db.SaveChanges();
            return RedirectToAction("Detalji", new { id = model.Id });

        }
    }
}