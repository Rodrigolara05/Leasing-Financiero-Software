using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TF_Finanzas.Models;

namespace TF_Finanzas.Controllers
{
    public class Periodo_graciaController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();

        // GET: Periodo_gracia
        public ActionResult Index()
        {
            return View(db.Periodo_gracia.ToList());
        }

        // GET: Periodo_gracia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo_gracia periodo_gracia = db.Periodo_gracia.Find(id);
            if (periodo_gracia == null)
            {
                return HttpNotFound();
            }
            return View(periodo_gracia);
        }

        // GET: Periodo_gracia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Periodo_gracia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Descripcion,Tiempo")] Periodo_gracia periodo_gracia)
        {
            if (ModelState.IsValid)
            {
                db.Periodo_gracia.Add(periodo_gracia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(periodo_gracia);
        }

        // GET: Periodo_gracia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo_gracia periodo_gracia = db.Periodo_gracia.Find(id);
            if (periodo_gracia == null)
            {
                return HttpNotFound();
            }
            return View(periodo_gracia);
        }

        // POST: Periodo_gracia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Descripcion,Tiempo")] Periodo_gracia periodo_gracia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(periodo_gracia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(periodo_gracia);
        }

        // GET: Periodo_gracia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo_gracia periodo_gracia = db.Periodo_gracia.Find(id);
            if (periodo_gracia == null)
            {
                return HttpNotFound();
            }
            return View(periodo_gracia);
        }

        // POST: Periodo_gracia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Periodo_gracia periodo_gracia = db.Periodo_gracia.Find(id);
            db.Periodo_gracia.Remove(periodo_gracia);
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
