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
using System.IO;
using System.Text.RegularExpressions;
using PagedList;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class VijestiController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index(int? page, int? OdabranaKategorija, string Naziv)
        {
            int id = OdabranaKategorija ?? 0;

            VijestiIndexVM model = new VijestiIndexVM();

            model.ListaKategorija = db.VijestiKategorijaDbSet.ToList();
            model.ListaKategorija.Insert(0, new Models.VijestiKategorija() { Id = 0, Naziv = "Odaberite kategoriju" });


            var u = db.VijestiDbSet.OrderByDescending(x => x.DatumObjave).ToList();
            //// lazy loading niuje ucitao kategorije
            //foreach (var item in u)
            //{
            //    item.UputstvaKategorije = db.UputstvaKategorijeDbSet.Where(v => v.Id == item.UputstvoKategorijaID).FirstOrDefault();
            //}

            if (id != 0)
                u = u.Where(i => i.VijestiKategorijaId == id).ToList();

            if (!string.IsNullOrEmpty(Naziv))
                u = u.Where(i => i.Naslov.Contains(Naziv)).ToList();


           
                foreach (var item in u)
                {
                    item.Opis = rijesisehtmla(item.Opis);
                }
       



            model.ListaRezultata = u.ToPagedList(page ?? 1, 10);

            return View(model);
        }
        public string rijesisehtmla(string input)
        {
          
            return Regex.Replace(Regex.Replace(input, @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


         VijestiDetaljiVM model = db.VijestiDbSet
         .Where(x => x.Id == id)
         .Select(f => new VijestiDetaljiVM
         {
             Id = f.Id,
             DatumIzmjene = f.DatumPosljednjeIzmjene ?? DateTime.MinValue,
             DatumObjave = f.DatumObjave,
             ImageUrl = f.ImageUrl,
             Naslov = f.Naslov,
             Opis = f.Opis,
             Vidljivo = f.Vidljivo

         }).Single();

            model.PrikazDatumObjave = model.DatumObjave.ToShortDateString();
            model.PrikazDatumIzmjene = model.DatumIzmjene.Year > 2015 ? model.DatumIzmjene.ToShortDateString() : "-";

            return View(model);
        }
        public ActionResult Dodaj()
        {
            VijestiDodajVM model = new VijestiDodajVM();
            model.ListaKategorija = db.VijestiKategorijaDbSet.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult Dodaj(VijestiDodajVM model)
        {
            if (ModelState.IsValid)
            {
                if ((model.ImageUpload != null && model.ImageUpload.ContentLength > 0)&&(model.ImageUpload.ContentType == "image/jpeg" || model.ImageUpload.ContentType == "image/png"))
                {


                    string putanjazabaze = FileUploader.UploadFile("Slike", model.ImageUpload);

                    Vijesti v = new Vijesti()
                    {
                        DatumObjave = DateTime.Now,
                        Naslov = model.Naziv,
                        Opis = model.Opis,
                        Vidljivo = model.Vidljivo,
                        VijestiKategorijaId = model.KategorijaId,
                        ImageUrl = putanjazabaze,
                        AltText = model.AltText
                        
                       

                    };

                    db.VijestiDbSet.Add(v);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                
            }
                else
                    ModelState.AddModelError("ImageUpload", "Podrzani formati su: jpg i png!");
            }
         

            model.ListaKategorija = db.VijestiKategorijaDbSet.ToList();
            return View(model);

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            if (db.VijestiDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }


            VijestiEditVM model = db.VijestiDbSet
          .Where(x => x.Id == id)
          .Select(f => new VijestiEditVM
          {
              Id = f.Id,
              Datum = f.DatumObjave,
              KategorijaId = f.VijestiKategorijaId,
              Naziv = f.Naslov,
              Opis = f.Opis,
              Vidljivo = f.Vidljivo,
              AltText = f.AltText,
              ImageUrl = f.ImageUrl,
              ListaKategorija = db.VijestiKategorijaDbSet.ToList(),

          }).Single();



            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(VijestiEditVM model)
        {
            if (ModelState.IsValid)
            {
                Vijesti v = db.VijestiDbSet.Find(model.Id);

                if (model.ImageUpload != null)
                {
                    /// ako je korisnik ucitao novu sliku izmjeni staru
                    if (v.ImageUrl == null)
                    {
                        v.ImageUrl = FileUploader.UploadFile("Slike", model.ImageUpload); ;

                    }
                    else if ((model.ImageUpload.ContentLength > 0) && (model.ImageUpload.ContentType == "image/jpeg" || model.ImageUpload.ContentType == "image/png"))
                    {

                        FileUploader.ObrisiFile(v.ImageUrl);
                        v.ImageUrl = FileUploader.UploadFile("Slike", model.ImageUpload); ;

                    }
                    else
                    {
                        ModelState.AddModelError("ImageUpload", "Podrzani formati su: jpg i png!");
                    }
                }

                v.Naslov = model.Naziv;
                v.Opis = model.Opis;
                v.Vidljivo = model.Vidljivo;
                v.VijestiKategorijaId = model.KategorijaId;
                v.DatumPosljednjeIzmjene = DateTime.Now;


                db.SaveChanges();

                return RedirectToAction("Index");
            }


            model.ListaKategorija = db.VijestiKategorijaDbSet.ToList();
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vijesti vijesti = db.VijestiDbSet.Find(id);
            if (vijesti == null)
            {
                return HttpNotFound();
            }
            return View(vijesti);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vijesti vijesti = db.VijestiDbSet.Find(id);
            db.VijestiDbSet.Remove(vijesti);
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
