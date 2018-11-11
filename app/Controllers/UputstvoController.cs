using app.Models;
using app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace app.Controllers
{
    [AuthKorisnik]
    public class UputstvoController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index()
        {
            UputstvoIndexVM model = new UputstvoIndexVM();
            model.ListaKategorija = db.UputstvaKategorijeDbSet.ToList();
            model.ListaUputstva = db.UputstvaDbSet.Where(c => c.Vidljivo == true).ToList();

            return View(model);
        }

        public ActionResult GetPDF(string s)
        {

            if (!System.IO.File.Exists(HttpContext.Server.MapPath(s)))
            {
                return HttpNotFound();
            }

            return File(s, MediaTypeNames.Application.Pdf);
        }

       


    }
}