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

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class PaketsController : Controller
    {
        private MojKontekst db = new MojKontekst();

        // GET: admin/Pakets
        public ActionResult Index()
        {
            PaketIndexVM model = new PaketIndexVM();

            model.PaketRedovi = db.PaketDbSet
             .Select(x => new PaketRed
             {
                 Id = x.Id,
                 CijenaSaPDV = x.CijenaSaPdv.ToString(),
                 DownSpeed = x.DownloadSpeed,
                 Flat = x.FlatRate?"DA":"NE",          
                 Naziv = x.Naziv,
                 SlikaPutanja = x.ImageUrl==null? "http://localhost:57805/Slike/404.jpg" : x.ImageUrl,
                 UpSpeed = x.UploadSpeed,
                 Kategorija = x.TipUsluga.Naziv,
                 KategorijaId = x.TipUslugaId
                    
             }).ToList();

            model.KategorijeL = db.TipUslugaDbSet.ToList();

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.PaketDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            PaketiDetailsVM model = db.PaketDbSet
              .Where(x => x.Id == id)
              .Select(f => new PaketiDetailsVM
              {
                  Id = f.Id,
                  AktivnaUsluga = f.AktivnaUsluga,
                  AltText = f.AltText,
                  CijenaBezPdv = f.CijenaBezPdv,
                  CijenaSaPdv = f.CijenaSaPdv,
                  DownloadSpeed= f.DownloadSpeed,
                  FlatRate = f.FlatRate?"DA":"NE",
                  ImageUrl = f.ImageUrl,
                  Naziv = f.Naziv,
                  UploadSpeed = f.UploadSpeed,TipUslugaId = f.TipUslugaId,
                  TipUsluge = f.TipUsluga.Naziv

              }).Single();


            return View(model);
        }

        // GET: admin/Pakets/Create
        public ActionResult Create()
        {
            PaketDodajVM model = new PaketDodajVM();
            model.ListaTipovaUsluga = db.TipUslugaDbSet.ToList();

            return View(model);
        }

       [HttpPost]
        public ActionResult Create(PaketDodajVM model)
        {
            if (ModelState.IsValid)
            {
                if ((model.ImageUpload != null && model.ImageUpload.ContentLength > 0) 
                    && (model.ImageUpload.ContentType == "image/jpeg" || model.ImageUpload.ContentType == "image/png"))
                { 

                    string putanjazabaze = FileUploader.UploadFile("Slike", model.ImageUpload);

                    Paket p = new Paket()
                    {
                        ImageUrl = putanjazabaze,
                        AltText = model.AltText,
                        AktivnaUsluga = model.AktivnaUsluga,
                        CijenaBezPdv = model.CijenaBezPdv,
                        CijenaSaPdv = model.CijenaSaPdv,
                        DownloadSpeed = model.DownloadSpeed,
                        FlatRate = model.FlatRate,
                        Naziv = model.Naziv,
                        UploadSpeed = model.UploadSpeed,
                        TipUslugaId = model.TipUslugaId
                    };


                    db.PaketDbSet.Add(p);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                    ModelState.AddModelError("ImageUpload", "Podrzani formati su: jpg i png!");
            }
           

            model.ListaTipovaUsluga = db.TipUslugaDbSet.ToList();
            return View(model);
        }

        // GET: admin/Pakets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paket paket = db.PaketDbSet.Find(id);
            if (paket == null)
            {
                return HttpNotFound();
            }

            PaketUrediVM model = db.PaketDbSet
                .Where(x => x.Id == id)
                .Select(f => new PaketUrediVM
                {
                    Id = f.Id,
                    AktivnaUsluga = f.AktivnaUsluga,
                    AltText = f.AltText,
                    CijenaBezPdv = f.CijenaBezPdv,
                    CijenaSaPdv = f.CijenaSaPdv,
                    DownloadSpeed = f.DownloadSpeed,
                    FlatRate = f.FlatRate,
                    ImageUrl = f.ImageUrl == null ? "http://localhost:57805/Slike/404.jpg" : f.ImageUrl,
                    Naziv = f.Naziv,
                    UploadSpeed = f.UploadSpeed,
                    TipUslugaId = f.TipUslugaId,
                    ListaTipovaUsluga = db.TipUslugaDbSet.ToList()

                }).Single();


            return View(model);
        }

        // POST: admin/Pakets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(PaketUrediVM model)
        {
            if (ModelState.IsValid)
            {
                Paket p = db.PaketDbSet.Find(model.Id);

               if (model.ImageUpload!= null )
                {
                    /// ako je korisnik ucitao novu sliku izmjeni staru
                    /// 

                    
                    if (p.ImageUrl == null) 
                    {
                        p.ImageUrl = FileUploader.UploadFile("Slike", model.ImageUpload); ;

                    }
                    else if((model.ImageUpload.ContentLength > 0) && (model.ImageUpload.ContentType == "image/jpeg" || model.ImageUpload.ContentType == "image/png"))
                    {

                        FileUploader.ObrisiFile(p.ImageUrl);
                        p.ImageUrl = FileUploader.UploadFile("Slike", model.ImageUpload); ;
                       
                    }
                    else
                    {
                        ModelState.AddModelError("ImageUpload", "Podrzani formati su: jpg i png!");
                    }
                }

                p.Naziv = model.Naziv;
                p.TipUslugaId = model.TipUslugaId;
                p.UploadSpeed = model.UploadSpeed;
                p.DownloadSpeed = model.DownloadSpeed;
                p.CijenaBezPdv = model.CijenaBezPdv;
                p.CijenaSaPdv = model.CijenaSaPdv;
                p.AltText = model.AltText;
                p.AktivnaUsluga = model.AktivnaUsluga;
                p.FlatRate = model.FlatRate;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
          

            model.ListaTipovaUsluga  = db.TipUslugaDbSet.ToList();
            return View(model);



        }

        // GET: admin/Pakets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paket paket = db.PaketDbSet.Find(id);
            if (paket == null)
            {
                return HttpNotFound();
            }
            return View(paket);
        }

        // POST: admin/Pakets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paket paket = db.PaketDbSet.Find(id);
            db.PaketDbSet.Remove(paket);
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
