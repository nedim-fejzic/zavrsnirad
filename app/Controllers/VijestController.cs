using app.Models;
using app.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace app.Controllers
{
    [AuthKorisnik]
    public class VijestController : Controller
    {
        private MojKontekst db = new MojKontekst();


        public ActionResult Index(int ? page)
        {
            VijestIndexVM model = new VijestIndexVM();

            var u = db.VijestiDbSet.OrderByDescending(x => x.DatumObjave).ToList();


            foreach (var item in u)
            {
                item.Opis = Regex.Replace(Regex.Replace(item.Opis, @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ");
                if (item.Opis.Length > 70)
                {
                    item.Opis = item.Opis.Substring(0, 70);
                    item.Opis += "...";
                }

                item.Opis = item.Opis.Replace("&scaron;", "š");
            }


            model.ListaRezultata = u.ToPagedList(page ?? 1, 20);

            return View(model);
        }

        public ActionResult Detalji(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var x = db.VijestiDbSet.Find(id);

            if (x == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }


            VijestDetaljiVM model = new VijestDetaljiVM();
            model.Id = x.Id;
            model.Kategorija = x.VijestiKategorija.Naziv;
            model.Naslov = x.Naslov;
            model.Opis = x.Opis;
            model.ImageUrl = x.ImageUrl;
            model.DatumObjave = x.DatumObjave.ToString("dd.MM.yyyy");

            return View(model);
        }
    }
}