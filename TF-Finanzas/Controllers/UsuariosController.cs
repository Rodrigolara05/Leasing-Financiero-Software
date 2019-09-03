using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TF_Finanzas.Authorization;
using TF_Finanzas.Constantes;
using TF_Finanzas.Models;

namespace TF_Finanzas.Controllers
{
    [UserLoggedOn]
    [AdminsOnly]
    public class UsuariosController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();
        

        // GET: Usuarios
        public ActionResult listado_clientes()
        {
            var d = from c in db.Usuarios.Include("Tipo_usuario")
                    where c.idTipoUsuario==1
                    select c;

            return View(d.ToList());
        }

        // GET: Usuarios
        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Tipo_usuario);
            return View(usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        // GET: Usuarios/Details/5
        public ActionResult Prestamos(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            var d = from c in db.Biens.Include("Usuario")
                    where c.idusuario == usuario.id
                    select c;
            
            return View(d.ToList());
        }
        public ActionResult Detalle_Cuota(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cuota cuota = db.cuotas.Find(id);
            if (cuota == null)
            {
                return HttpNotFound();
            }
            return View(cuota);
        }
        // GET: Usuarios/Details/5
        public ActionResult CuotasPagadas(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var d = from c in db.Deudas.Include("Periodo_gracia").Include("Bien")
                    where c.id_bien == id
                    select c;
            Deuda deuda = (Deuda)d.FirstOrDefault();

            var x = from s in db.cuotas.Include("Deuda")
                    where s.id_deuda == deuda.id && s.Pagado == true
                    select s;

            return View(x.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult CuotasPendientes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var d = from c in db.Deudas.Include("Periodo_gracia").Include("Bien")
                    where c.id_bien == id
                    select c;
            Deuda deuda = (Deuda)d.FirstOrDefault();

            var x = from s in db.cuotas.Include("Deuda")
                    where s.id_deuda == deuda.id && s.Pagado == false
                    select s;

            return View(x.ToList());
        }

        // GET: Usuarios/Create
        public ActionResult Create_Admin()
        {
            ViewBag.idTipoUsuario = new SelectList(db.Tipo_usuario, "id", "descripcion");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Admin([Bind(Include = "id,nombre,RUC,correo,contraseña,idTipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idTipoUsuario = new SelectList(db.Tipo_usuario, "id", "descripcion", usuario.idTipoUsuario);
            return View(usuario);
        }
        // GET: Usuarios/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,RUC,correo,contraseña")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.idTipoUsuario = 1;
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.idTipoUsuario = new SelectList(db.Tipo_usuario, "id", "descripcion", usuario.idTipoUsuario);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTipoUsuario = new SelectList(db.Tipo_usuario, "id", "descripcion", usuario.idTipoUsuario);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,RUC,correo,contraseña,idTipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idTipoUsuario = new SelectList(db.Tipo_usuario, "id", "descripcion", usuario.idTipoUsuario);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
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

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "id,nombre,RUC,correo,contraseña,idTipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var us = from u in db.Usuarios.Include("Tipo_usuario")
                         where u.correo == usuario.correo && u.contraseña == usuario.contraseña
                         select u;
                Usuario user2 = us.FirstOrDefault();
                if (user2 != null)
                {
                    Session[SessionName.User] = user2;
                    if (user2.idTipoUsuario == 1)
                        return RedirectToAction("Index_Cliente", "Home");
                    else
                    {
                        return RedirectToAction("Index_Admin", "Home");
                    }
                }
                else
                {
                    
                    ViewBag.ErrorMessage = "Invalido";
                }


            }
            return View(usuario);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
