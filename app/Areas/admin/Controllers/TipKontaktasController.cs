using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using app.Models;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class TipKontaktasController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index()
        {
            return View(db.TipKontaktaDbSet.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipKontakta tipKontakta = db.TipKontaktaDbSet.Find(id);
            if (tipKontakta == null)
            {
                return HttpNotFound();
            }
            return View(tipKontakta);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naziv")] TipKontakta tipKontakta)
        {
            if (ModelState.IsValid)
            {
                db.TipKontaktaDbSet.Add(tipKontakta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipKontakta);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipKontakta tipKontakta = db.TipKontaktaDbSet.Find(id);
            if (tipKontakta == null)
            {
                return HttpNotFound();
            }
            return View(tipKontakta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] TipKontakta tipKontakta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipKontakta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipKontakta);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipKontakta tipKontakta = db.TipKontaktaDbSet.Find(id);
            if (tipKontakta == null)
            {
                return HttpNotFound();
            }
            return View(tipKontakta);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipKontakta tipKontakta = db.TipKontaktaDbSet.Find(id);
            db.TipKontaktaDbSet.Remove(tipKontakta);
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
