using app.Areas.admin.ViewModels;
using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class StartController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index()
        {
            StartIndexVM model = new StartIndexVM();
            model.ProdajaKorisnici = db.KorisnikDbSet.Count().ToString();
            model.ProdajaZahtjevi = db.ZahtjevDbSet.Where(c=>c.ZahtjevStatus.Id==1).Count().ToString();
            model.ProdajaRacuni = db.RacuniDbSet.Where(c=>c.Placen==false).Count().ToString();

            model.PodrskaSmetnje = db.SmetnjeDbSet.Where(c => c.SmetnjeStatusId  == 1).Count().ToString();
            model.PodrskaUputstva = db.UputstvaDbSet.Count().ToString();
            model.PodrskaLozinka ="";

            model.MenadzerVijesti = db.VijestiDbSet.Where(c => c.Vidljivo == true).Count().ToString();
            model.MenadzerFaq = db.FAQDbSet.Count().ToString();
            model.MenadzerOprema = db.UredjajDbSet.Where(c => c.UUpotrebi == false).Count().ToString();
            model.MenadzerPaketi = db.PaketDbSet.Count().ToString();
            model.MenadzerAktivneUsluge = db.AktivneUslugeDbSet.Count().ToString();
            return View(model);
        }
        public ActionResult Logout()
        {
            Session["logiran_korisnik"] = null;
            Session["imeprezime"] = null;
            Session["logiran_uposlenik"] = null;
            Session["role"] = null;
            //TempData["Message"] = "Uspješno ste se odjavili!";
            //TempData["code"] = "info";

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}