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
    public class ResultsController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();

        // GET: Results
        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.Bien);
            return View(results.ToList());
        }
        // GET: Results
        public ActionResult Resultado()
        {
            var d = (from c in db.Results.Include("Bien")
                    select c).AsEnumerable();
            List<Result> lista = new List<Result>();

            Result res = (Result)d.Last();

            lista.Add(res);
            return View(lista);
        }
        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            ViewBag.Bien_id = new SelectList(db.Biens, "id", "nombre");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Bien_id,IGV,ValorVenta,MontoLeasing,Porcentaje_TEP,Numero_Cuotas,Total_Cuotas,Seguro_Riesgo,Intereses,Amortizacion_Capital,Seguro_contra_riesgo,Comision_periodica,Recompra,Desembolso_Total,TCEA_FlujoB,TCEA_FlujoN,VAN_FlujoB,VAN_FlujoN")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Bien_id = new SelectList(db.Biens, "id", "nombre", result.Bien_id);
            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.Bien_id = new SelectList(db.Biens, "id", "nombre", result.Bien_id);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Bien_id,IGV,ValorVenta,MontoLeasing,Porcentaje_TEP,Numero_Cuotas,Total_Cuotas,Seguro_Riesgo,Intereses,Amortizacion_Capital,Seguro_contra_riesgo,Comision_periodica,Recompra,Desembolso_Total,TCEA_FlujoB,TCEA_FlujoN,VAN_FlujoB,VAN_FlujoN")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Bien_id = new SelectList(db.Biens, "id", "nombre", result.Bien_id);
            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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
