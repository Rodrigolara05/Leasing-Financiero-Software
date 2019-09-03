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
    public class Tipo_usuarioController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();

        // GET: Tipo_usuario
        public ActionResult Index()
        {
            return View(db.Tipo_usuario.ToList());
        }

        // GET: Tipo_usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
            if (tipo_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tipo_usuario);
        }

        // GET: Tipo_usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tipo_usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] Tipo_usuario tipo_usuario)
        {
            if (ModelState.IsValid)
            {
                db.Tipo_usuario.Add(tipo_usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_usuario);
        }

        // GET: Tipo_usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
            if (tipo_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tipo_usuario);
        }

        // POST: Tipo_usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] Tipo_usuario tipo_usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_usuario);
        }

        // GET: Tipo_usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
            if (tipo_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tipo_usuario);
        }

        // POST: Tipo_usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
            db.Tipo_usuario.Remove(tipo_usuario);
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
