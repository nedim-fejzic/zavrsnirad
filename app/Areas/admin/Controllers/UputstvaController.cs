using app.Areas.admin.ViewModels;
using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net.Mime;
using System.IO;
using PagedList;
using System.Net;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class UputstvaController : Controller
    {

        private MojKontekst db = new MojKontekst();


        // GET: admin/Uputstva
        public ActionResult Index(int? page, int? OdabranaKategorija, string Naziv)
        {

            int id = OdabranaKategorija ?? 0;
          
            UputstvaIndexVM model = new UputstvaIndexVM();

            model.ListaKategorija = db.UputstvaKategorijeDbSet.ToList();
            model.ListaKategorija.Insert(0, new Models.UputstvaKategorije() { Id = 0, Naziv = "Odaberite kategoriju" });


            var u = db.UputstvaDbSet.OrderByDescending(x => x.DatumDodavanja).ToList();
            // lazy loading niuje ucitao kategorije
            foreach (var item in u)
            {
                item.UputstvaKategorije = db.UputstvaKategorijeDbSet.Where(v => v.Id == item.UputstvoKategorijaID).FirstOrDefault();
            }

            if (id != 0)
                u = u.Where(i => i.UputstvoKategorijaID == id).ToList();

            if (!string.IsNullOrEmpty(Naziv))
                u = u.Where(i => i.Naslov.Contains(Naziv)).ToList();


            model.ListaRezultata = u.ToPagedList(page ?? 1, 10);

            return View(model);
        }

        [HttpGet]
        public ActionResult Dodaj()
        {

            UputstvaDodajVM model = new UputstvaDodajVM();
            model.ListaKategorija = db.UputstvaKategorijeDbSet.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult Dodaj(UputstvaDodajVM model)
        {
            //Validiraj i ucitaj upustvo i napravai folder

            if (ModelState.IsValid)
            {
                if ((model.FileUpload != null && model.FileUpload.ContentLength > 0) && (model.FileUpload.ContentType == "application/pdf"))
                {

                    string putanjazabaze = FileUploader.UploadFile("PDF", model.FileUpload);

                    Uputstva u = new Uputstva()
                    {
                        DatumDodavanja = DateTime.Now,
                        Naslov = model.Naslov,
                        UputstvoKategorijaID = model.UputstvoKategorijaID,
                        Vidljivo = model.Vidljivo,
                        Putanja = putanjazabaze,
                        TipDokumenta = "pdf",
                        NazivDokumenta = model.FileUpload.FileName
                    };





                    db.UputstvaDbSet.Add(u);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                    ModelState.AddModelError("FileUpload", "Molimo izaberite uputstvo u PDF formatu!");
            }


            model.ListaKategorija = db.UputstvaKategorijeDbSet.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Uredi(int id)
        {
            if (db.UputstvaDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            UputstvoUrediVM model = db.UputstvaDbSet
          .Where(x => x.Id == id)
          .Select(f => new UputstvoUrediVM
          {
              Id = f.Id,
              DatumDodavanja = f.DatumDodavanja,
            TipDokumenta = f.TipDokumenta,
            Naslov = f.Naslov,
            NazivDokumenta = f.NazivDokumenta,
            Putanja = f.Putanja,
            Vidljivo = f.Vidljivo,
            ListaKategorija = db.UputstvaKategorijeDbSet.ToList(),
            UputstvoKategorijaID = f.UputstvoKategorijaID

          }).Single();

            return View(model);
        }
        [HttpPost]
        public ActionResult Uredi(UputstvoUrediVM model)
        {
            if (ModelState.IsValid)
            {
                Uputstva v = db.UputstvaDbSet.Find(model.Id);

                if (model.FileUpload != null)
                {
                    /// ako je korisnik ucitao novu sliku izmjeni staru
                    if (v.Putanja == null)
                    {
                        v.Putanja = FileUploader.UploadFile("PDF", model.FileUpload); ;
                        v.NazivDokumenta = model.FileUpload.FileName;

                    }
                    else if ((model.FileUpload.ContentLength > 0) && (model.FileUpload.ContentType == "application/pdf"))
                    {

                        FileUploader.ObrisiFile(v.Putanja);
                        v.Putanja = FileUploader.UploadFile("PDF", model.FileUpload); 
                        v.NazivDokumenta = model.FileUpload.FileName;

                    }
                    else
                    {
                        ModelState.AddModelError("FileUpload", "Molimo izaberite uputstvo u PDF formatu!");
                    }
                }



                v.DatumDodavanja = DateTime.Now;
                v.Naslov = model.Naslov;
                v.UputstvoKategorijaID = model.UputstvoKategorijaID;
                v.Vidljivo = model.Vidljivo;



                db.SaveChanges();

                return RedirectToAction("Index");
            }

            model.ListaKategorija = db.UputstvaKategorijeDbSet.ToList();
            return View(model);
        }


        [HttpGet]
        public ActionResult GetPDF(string s)
        {


            if (!System.IO.File.Exists(HttpContext.Server.MapPath(s)))
            {
                return HttpNotFound();
            }

            return File(s, MediaTypeNames.Application.Pdf);
        }

         public FileStreamResult DajPDF(string s)
        {
            FileStream fs = new FileStream(s, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }



        [HttpGet]
        public ActionResult Detalji(int id)
        {

            if (db.UputstvaDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            UputstvaDetaljiVM model = new UputstvaDetaljiVM();
            Uputstva u = db.UputstvaDbSet.Find(id);

            model.DatumDodavanja = u.DatumDodavanja.ToString("dd-MM-yyyy");
            model.Id = u.Id;
            model.Kategorija = db.UputstvaKategorijeDbSet.Find(u.UputstvoKategorijaID).Naziv;
            model.Naslov = u.Naslov;
            model.NazivDokumenta = u.Naslov;
            model.Vidljivo = u.Vidljivo ? "DA" : "NE";
            model.Putanja = u.Putanja;

            return View(model);
        }
    }
}