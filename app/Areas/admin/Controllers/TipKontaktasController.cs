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

        // GET: admin/TipKontaktas
        public ActionResult Index()
        {
            return View(db.TipKontaktaDbSet.ToList());
        }

        // GET: admin/TipKontaktas/Details/5
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

        // GET: admin/TipKontaktas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/TipKontaktas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: admin/TipKontaktas/Edit/5
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

        // POST: admin/TipKontaktas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: admin/TipKontaktas/Delete/5
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

        // POST: admin/TipKontaktas/Delete/5
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
