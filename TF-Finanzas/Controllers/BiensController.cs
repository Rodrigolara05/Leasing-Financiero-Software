using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TF_Finanzas.Constantes;
using TF_Finanzas.Models;

namespace TF_Finanzas.Controllers
{
    public class BiensController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();

        // GET: Biens
        public ActionResult Index()
        {
            var biens = db.Biens.Include(b => b.Usuario);
            return View(biens.ToList());
        }

        // GET: Biens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bien bien = db.Biens.Find(id);
            if (bien == null)
            {
                return HttpNotFound();
            }
            return View(bien);
        }

        // GET: Biens/Create
        public ActionResult Create()
        {
            ViewBag.idusuario = new SelectList(db.Usuarios, "id", "nombre");
            return View();
        }

        // POST: Biens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,precio,idusuario")] Bien bien)
        {
            if (ModelState.IsValid)
            {
                db.Biens.Add(bien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idusuario = new SelectList(db.Usuarios, "id", "nombre", bien.idusuario);
            return View(bien);
        }

        // GET: Biens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bien bien = db.Biens.Find(id);
            if (bien == null)
            {
                return HttpNotFound();
            }
            ViewBag.idusuario = new SelectList(db.Usuarios, "id", "nombre", bien.idusuario);
            return View(bien);
        }

        // POST: Biens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,precio,idusuario")] Bien bien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idusuario = new SelectList(db.Usuarios, "id", "nombre", bien.idusuario);
            return View(bien);
        }

        // GET: Biens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bien bien = db.Biens.Find(id);
            if (bien == null)
            {
                return HttpNotFound();
            }
            return View(bien);
        }

        // POST: Biens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bien bien = db.Biens.Find(id);
            db.Biens.Remove(bien);
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
        // GET: Biens/Create
        public ActionResult Crear()
        {
            var lista_de_usuarios = from c in db.Usuarios
                                    where c.idTipoUsuario == 1
                                    select c ;
            ViewBag.idusuario = new SelectList(lista_de_usuarios.ToList(), "id", "nombre");
            //ViewBag.idusuario = ;
            return View();
        }

        // POST: Biens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "id,nombre,precio,idusuario")] Bien bien)
        {
            if (ModelState.IsValid)
            {
                db.Biens.Add(bien);
                db.SaveChanges();
                return RedirectToAction("Crear","Deudas");
            }

            ViewBag.idusuario = new SelectList(db.Usuarios, "id", "nombre", bien.idusuario);
            return View(bien);
        }

        // GET: Biens/Create
        public ActionResult Crear_Posible()
        {
            Usuario aux = (Usuario)Session[SessionName.User];
            List<Usuario> lista = new List<Usuario>();
            lista.Add(aux);
            ViewBag.idusuario = new SelectList(lista, "id", "nombre");
            return View();
        }

        // POST: Biens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear_Posible([Bind(Include = "id,nombre,precio,idusuario")] Bien bien)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Crear_Posible", "Deudas");
            }

            ViewBag.idusuario = new SelectList(db.Usuarios, "id", "nombre", bien.idusuario);
            return View(bien);
        }
    }
}
