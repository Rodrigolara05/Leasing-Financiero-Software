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
    public class cuotasController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();

        // GET: cuotas
        public ActionResult Index()
        {
            var cuotas = db.cuotas.Include(c => c.Deuda);
            return View(cuotas.ToList());
        }
        // GET: cuotas/Details/5
        public ActionResult Detalle_pendiente(int? id)
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
        // GET: cuotas/Details/5
        public ActionResult Details(int? id)
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

        // GET: cuotas/Create
        public ActionResult Create()
        {
            ViewBag.id_deuda = new SelectList(db.Deudas, "id", "id");
            return View();
        }

        // POST: cuotas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_deuda,saldo_inicial,interes,monto_cuota,amortizacion,seguro_riesgo,recompra,Comision,saldo_final,depreciacion,ahorro_tributario,IGV,f_bruto,f_igv,f_neto,PeriodoGracia,Pagado,Fecha_De_Pago")] cuota cuota)
        {
            if (ModelState.IsValid)
            {
                db.cuotas.Add(cuota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_deuda = new SelectList(db.Deudas, "id", "id", cuota.id_deuda);
            return View(cuota);
        }

        // GET: cuotas/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.id_deuda = new SelectList(db.Deudas, "id", "id", cuota.id_deuda);
            return View(cuota);
        }

        // POST: cuotas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_deuda,saldo_inicial,interes,monto_cuota,amortizacion,seguro_riesgo,recompra,Comision,saldo_final,depreciacion,ahorro_tributario,IGV,f_bruto,f_igv,f_neto,PeriodoGracia,Pagado,Fecha_De_Pago")] cuota cuota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_deuda = new SelectList(db.Deudas, "id", "id", cuota.id_deuda);
            return View(cuota);
        }

        // GET: cuotas/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: cuotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cuota cuota = db.cuotas.Find(id);
            db.cuotas.Remove(cuota);
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


        // GET: cuotas
        public ActionResult cuotas_pagadas()
        {
            Usuario usuario = (Usuario)Session[SessionName.User];
            var b = from c in db.Biens
                    where c.idusuario == usuario.id
                    select c;
            if (b.Count() != 0)
            {
                Bien bien = b.FirstOrDefault();

                var d = from c in db.Deudas
                        where c.id_bien == bien.id
                        select c;
                if (d.Count() != 0)
                {
                    Deuda deuda = d.FirstOrDefault();

                    var aux = from c in db.cuotas.Include("Deuda")
                              where c.Pagado == true && c.id_deuda == deuda.id
                              select c;

                    return View(aux.ToList());
                }
                else
                {
                    return RedirectToAction("Index_Cliente", "Home");
                }
            }
            return RedirectToAction("Index_Cliente", "Home");
        }
        // GET: cuotas
        public ActionResult cuotas_pendientes()
        {
            Usuario usuario = (Usuario)Session[SessionName.User];
            var b = from c in db.Biens
                    where c.idusuario == usuario.id
                    select c;
            if (b.Count() != 0)
            {
                Bien bien = b.FirstOrDefault();

                var d = from c in db.Deudas.Include("Bien")
                        where c.id_bien == bien.id
                        select c;
                if (d.Count() != 0)
                {
                    Deuda deuda = d.FirstOrDefault();

                    var aux = from c in db.cuotas.Include("Deuda")
                              where c.Pagado == false && c.id_deuda == deuda.id
                              select c;

                    return View(aux.ToList());
                }
                else
                {
                    return RedirectToAction("Index_Cliente", "Home");
                }
            }
            return RedirectToAction("Index_Cliente", "Home");
        }


    }
}
