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
using System.Text.RegularExpressions;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class FAQController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public string rijesisehtmla(string input)
        {

            return Regex.Replace(Regex.Replace(input, @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ");
        }


        // GET: admin/FAQ
        public ActionResult Index()
        {

            //FaqIndexVM model = new FaqIndexVM();

            //model.FaqRedovi = db.FAQDbSet
            // .Select(x => new FaqRed
            // {
            //     Id = x.Id,
            //     KategorijaId = x.FAQKategorijaId,
            //     NazivKategorije = x.FAQKategorija.Naziv,
            //     Odgovor = x.Odgovor,
            //     Pitanje = x.Pitanje,
            //     Vidljiv = x.Vidljiv

            // }).ToList();


            //foreach (var red in model.FaqRedovi)
            //{
            //    red.Odgovor = rijesisehtmla(red.Odgovor);


            //    if (red.Odgovor.Length > 20)
            //         red.Odgovor = red.Odgovor.Substring(0, 19) + "...";


            //}

            //model.ListaKategorija = db.FAQKategorijaDbSet.ToList();

            return RedirectToAction("Search");
        }

        // GET: admin/FAQ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ q = db.FAQDbSet.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }


            FaqDetaljiVM model = db.FAQDbSet
              .Where(x => x.Id == id)
              .Select(f => new FaqDetaljiVM
              {
                  Id = f.Id,
                  Pitanje = f.Pitanje,
                  Odgovor = f.Odgovor,
                  Vidljivo = f.Vidljiv?"DA":"NE",
                  Kategorija = f.FAQKategorija.Naziv

              }).Single();


            return View(model);
        }

        // GET: admin/FAQ/Create
        public ActionResult Dodaj()
        {
            FaqDodajVM model = new FaqDodajVM();
            model.ListaKategorija = db.FAQKategorijaDbSet.ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Dodaj(FaqDodajVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ListaKategorija = db.FAQKategorijaDbSet.ToList();
                return View(model);
            }

            FAQ o = new FAQ()
            {
                Odgovor = model.Odgovor,
                FAQKategorijaId = model.KategorijaId,
                Pitanje = model.Pitanje,
                Vidljiv = model.Vidljiv,
                DatumDodavanja = DateTime.Now
            };

            db.FAQDbSet.Add(o);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        /// //////////////////////////////////////////////////////////////////////////////////////////
        ///                      UREDI
        /// //////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.FAQDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            FaqEditVM model = db.FAQDbSet
              .Where(x => x.Id == id)
              .Select(f => new FaqEditVM
              {
                  Id = f.Id,
                  Pitanje = f.Pitanje,
                  Odgovor = f.Odgovor,
                  Vidljiv = f.Vidljiv,
                  KategorijaId = f.FAQKategorijaId,
                  ListaKategorija = db.FAQKategorijaDbSet.ToList()

              }).Single();




            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FaqEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ListaKategorija = db.FAQKategorijaDbSet.ToList();
                return View(model);
            }

            FAQ o = db.FAQDbSet.Find(model.Id);

            if (o == null)
                return HttpNotFound();

            o.Pitanje = model.Pitanje;
            o.Odgovor = model.Odgovor;
            o.Vidljiv = model.Vidljiv;
            o.FAQKategorijaId = model.KategorijaId;




            db.SaveChanges();

            return RedirectToAction("Index");

        }



        public ActionResult Search(int? page, int? OdabranaKategorija)
        {
            int id  = OdabranaKategorija ?? 0;




            FaqSearchVM model = new FaqSearchVM();
            model.ListaKategorija = db.FAQKategorijaDbSet.ToList();
            model.ListaKategorija.Insert(0, new Models.FAQKategorija() { Id = 0, Naziv = "Odaberite kategoriju" });


            var faqs = db.FAQDbSet.OrderByDescending(x=>x.DatumDodavanja).ToList();

            if (id!=0)
                faqs = faqs.Where(i => i.FAQKategorijaId == id).ToList();


           model.ListaRezultata = faqs.ToPagedList(page ?? 1, 10);

            foreach (var red in model.ListaRezultata)
            {
                red.Odgovor = rijesisehtmla(red.Odgovor);

                if (red.Pitanje.Length > 20)
                    red.Pitanje = red.Pitanje.Substring(0, 19) + "....";
                if (red.Odgovor.Length > 20)
                    red.Odgovor = red.Odgovor.Substring(0, 19) + "....";


            }

            return View(model);


        }





        // GET: admin/FAQ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ o = db.FAQDbSet.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            return View(o);
        }

        // POST: admin/FAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FAQ o = db.FAQDbSet.Find(id);
            db.FAQDbSet.Remove(o);
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
