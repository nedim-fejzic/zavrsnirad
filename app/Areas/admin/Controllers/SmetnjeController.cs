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
    public class SmetnjeController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index(int? page, string Sifra, int? OdabraniKorisnik, int? OdabraniStatus)
        {
            int idKorisnik = OdabraniKorisnik ?? 0;
            int idStatus = OdabraniStatus ?? 0;

            SmetnjeIndexVM model = new SmetnjeIndexVM();

            model.BrojAktivnihSmetnji = db.SmetnjeDbSet.Where(c => c.SmetnjeStatusId == 1).Count().ToString();
            model.BrojSmetnji = db.SmetnjeDbSet.Count().ToString();

            model.ListaKorisnika = db.KorisnikDbSet.ToList();
            model.ListaKorisnika.Insert(0, new Models.Korisnik() { Id = 0, Ime = "Odaberite korisnika..." });

            model.ListaStatusa = db.SmetnjeStatusDbSet.ToList();
            model.ListaStatusa.Insert(0, new Models.SmetnjeStatus() { Id = 0, Naziv = "Odaberite status prijave...." });

            var u = db.SmetnjeDbSet.OrderByDescending(x => x.DatumOtvaranja ).ToList();
            u = u.OrderBy(x => x.SmetnjeStatusId).ToList();

            if (idKorisnik != 0)
                u = u.Where(i => i.KorisnikId == idKorisnik).ToList();

            if (idStatus != 0)
                u = u.Where(i => i.SmetnjeStatusId == idStatus).ToList();

            if (!string.IsNullOrEmpty(Sifra))
                u = u.Where(i => i.BrojSmetnje == Sifra).ToList();

            model.ListaRezultata = u.ToPagedList(page ?? 1, 10);

            return View(model);
        }
        public ActionResult Create()
        {
            SmetnjeDodajVM model = new SmetnjeDodajVM();
            model.ListaAktivnihUsluga = db.AktivneUslugeDbSet.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult Create(SmetnjeDodajVM model)
        {
            if (ModelState.IsValid)
            {
                var sad = DateTime.Now;
                Smetnje s = new Smetnje();
                s.AktivnaUslugaId = model.AktivnaUslugaId;
                s.DatumUocavanja = model.DatumUocavanja;
                s.DatumZatvaranja = null;
                s.KorisnikId = 1;
                s.SmetnjeStatusId = 1;
                s.Opis = model.Opis;
                s.DatumOtvaranja = sad;

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

            model.ListaAktivnihUsluga = db.AktivneUslugeDbSet.ToList();
            return View(model);
        }
        public ActionResult Details(int? id)
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

            SmetnjeDetaljiVM model = new SmetnjeDetaljiVM();

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
        public ActionResult Odgovor(SmetnjeDetaljiVM model)
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
                return RedirectToAction("Details", new { id = model.Id });
            }

            SmetnjeOdgovori o = new SmetnjeOdgovori();
            o.SmetnjaId = model.Id;
            o.Poruka = model.Odgovor;
            o.UposlenikId = Convert.ToInt32(Session["logiran_uposlenik"]);
            o.Datum = DateTime.Now;
            db.SmetnjeOdgovori.Add(o);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = model.Id });

        }
        public ActionResult Status(int id)
        {
            Smetnje s = db.SmetnjeDbSet.Find(id);
            if (s == null)
            {
                return HttpNotFound();
            }

            if (s.SmetnjeStatusId == 2)
            {
                TempData["Message"] = "Izabrana smetnja je već zatvorena!";
                TempData["code"] = "error";
            }
            else
            {
                s.SmetnjeStatusId = 2;
                s.DatumZatvaranja = DateTime.Now;

                s.UposlenikId = 1;
                db.SaveChanges();

                TempData["Message"] = "Uspješno ste zatvorili smetnju broj: <b>" + s.BrojSmetnje + "</b>";
                TempData["code"] = "info";
            }
            

            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smetnje smetnje = db.SmetnjeDbSet.Find(id);
            if (smetnje == null)
            {
                return HttpNotFound();
            }
            ViewBag.SmetnjeStatusId = new SelectList(db.SmetnjeStatusDbSet, "Id", "Naziv", smetnje.SmetnjeStatusId);
            return View(smetnje);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BrojSmetnje,AktivnaUslugaId,DatumUocavanja,SmetnjeStatusId,Opis")] Smetnje smetnje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smetnje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SmetnjeStatusId = new SelectList(db.SmetnjeStatusDbSet, "Id", "Naziv", smetnje.SmetnjeStatusId);
            return View(smetnje);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smetnje smetnje = db.SmetnjeDbSet.Find(id);
            if (smetnje == null)
            {
                return HttpNotFound();
            }
            return View(smetnje);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Smetnje smetnje = db.SmetnjeDbSet.Find(id);
            db.SmetnjeDbSet.Remove(smetnje);
            db.SaveChanges();
            return RedirectToAction("Index");
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
