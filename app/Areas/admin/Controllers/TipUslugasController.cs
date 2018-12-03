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
    public class TipUslugasController : Controller
    {
        private MojKontekst db = new MojKontekst();

        public ActionResult Index()
        {
            return View(db.TipUslugaDbSet.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipUsluga tipUsluga = db.TipUslugaDbSet.Find(id);
            if (tipUsluga == null)
            {
                return HttpNotFound();
            }
            return View(tipUsluga);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naziv")] TipUsluga tipUsluga)
        {
            if (ModelState.IsValid)
            {
                db.TipUslugaDbSet.Add(tipUsluga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipUsluga);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipUsluga tipUsluga = db.TipUslugaDbSet.Find(id);
            if (tipUsluga == null)
            {
                return HttpNotFound();
            }
            return View(tipUsluga);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] TipUsluga tipUsluga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipUsluga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipUsluga);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipUsluga tipUsluga = db.TipUslugaDbSet.Find(id);
            if (tipUsluga == null)
            {
                return HttpNotFound();
            }
            return View(tipUsluga);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipUsluga tipUsluga = db.TipUslugaDbSet.Find(id);
            db.TipUslugaDbSet.Remove(tipUsluga);
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
