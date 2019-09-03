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
    public class DeudasController : Controller
    {
        private FinanzasEntitiesContext db = new FinanzasEntitiesContext();

        // GET: Deudas
        public ActionResult Index()
        {
            var deudas = db.Deudas.Include(d => d.Bien).Include(d => d.Periodo_gracia);
            return View(deudas.ToList());
        }

        // GET: Deudas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deuda deuda = db.Deudas.Find(id);
            if (deuda == null)
            {
                return HttpNotFound();
            }
            return View(deuda);
        }

        // GET: Deudas/Create
        public ActionResult Create()
        {
            ViewBag.id_bien = new SelectList(db.Biens, "id", "nombre");
            ViewBag.id_periodo_gracia = new SelectList(db.Periodo_gracia, "id", "Descripcion");
            return View();
        }

        // POST: Deudas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_bien,id_periodo_gracia,PG_Tiempo,nro_años,frecuencia,dias_x_año,TEA,IGV,i_renta,recompra,costo_notarial,costo_registral,tasacion,comision_de_estudio,comision_de_activacion,comision_periodica,seguro_riesgo,tasa_ks,tasa_wacc")] Deuda deuda)
        {
            if (ModelState.IsValid)
            {
                db.Deudas.Add(deuda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_bien = new SelectList(db.Biens, "id", "nombre", deuda.id_bien);
            ViewBag.id_periodo_gracia = new SelectList(db.Periodo_gracia, "id", "Descripcion", deuda.id_periodo_gracia);
            return View(deuda);
        }

        // GET: Deudas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deuda deuda = db.Deudas.Find(id);
            if (deuda == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_bien = new SelectList(db.Biens, "id", "nombre", deuda.id_bien);
            ViewBag.id_periodo_gracia = new SelectList(db.Periodo_gracia, "id", "Descripcion", deuda.id_periodo_gracia);
            return View(deuda);
        }

        // POST: Deudas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_bien,id_periodo_gracia,PG_Tiempo,nro_años,frecuencia,dias_x_año,TEA,IGV,i_renta,recompra,costo_notarial,costo_registral,tasacion,comision_de_estudio,comision_de_activacion,comision_periodica,seguro_riesgo,tasa_ks,tasa_wacc")] Deuda deuda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deuda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_bien = new SelectList(db.Biens, "id", "nombre", deuda.id_bien);
            ViewBag.id_periodo_gracia = new SelectList(db.Periodo_gracia, "id", "Descripcion", deuda.id_periodo_gracia);
            return View(deuda);
        }

        // GET: Deudas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deuda deuda = db.Deudas.Find(id);
            if (deuda == null)
            {
                return HttpNotFound();
            }
            return View(deuda);
        }

        // POST: Deudas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deuda deuda = db.Deudas.Find(id);
            db.Deudas.Remove(deuda);
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


        List<cuota> Calcular_Cuotas(Deuda deuda)
        {
            Result resultado = new Result();
            List<cuota> lista = new List<cuota>();
            //Valores por defecto para la suma
            double Intereses = 0, Amortizacion = 0;
            double Total_seguro = 0, Total_comision = 0, Total_recompra = 0;
            double TEP = 0;
            double exponente = deuda.frecuencia / deuda.dias_x_año;
            Intereses = 0;
            Amortizacion = 0;
            Total_seguro = 0;
            Total_comision = 0;
            Total_recompra = 0;
            double TEA = deuda.TEA / 100;
            double Impuesto_renta = deuda.i_renta / 100;
            double Recompra = deuda.recompra / 100;
            double Porcentaje_seguro_riesgo = deuda.seguro_riesgo / 100;
            double Ks = deuda.tasa_ks / 100;
            double WACC = deuda.tasa_wacc / 100;

            //Precio de venta deuda.Bien.precio
            //Calcular Datos de Salida
            var precio = from c in db.Biens
                         where c.id == deuda.id_bien
                         select c;
            Bien aux = precio.FirstOrDefault();
            double IGV = ((double)aux.precio / (1 + (deuda.IGV))) * (deuda.IGV);
            double VV = (double)aux.precio - IGV;

            double Monto = VV + (double)(deuda.costo_notarial + deuda.costo_registral + deuda.tasacion + deuda.comision_de_estudio + deuda.comision_de_activacion);
            TEP = Math.Pow((1 + TEA), ((deuda.frecuencia * 1.0) / (deuda.dias_x_año * 1.0))) - 1;

            int Nro_cuotas_por_año = deuda.dias_x_año / deuda.frecuencia;
            int Total_cuotas = Nro_cuotas_por_año * deuda.nro_años;
            double Seguro_riesgo = Porcentaje_seguro_riesgo * (double)aux.precio / Nro_cuotas_por_año;


            double Inte, Comision_periodica;
            double SaldoI, cuota, Amort, recomp, SaldoF, depre = 0;
            double ahorroT, i_g_v, Flujo_B, Flujo_I, Flujo_N;

            List<double> Flujo_Bruto_Lista = new List<double>();
            List<double> Flujo_Neto_Lista = new List<double>();
            List<double> Flujo_Bruto_Lista_VAN = new List<double>();
            List<double> Flujo_Neto_Lista_VAN = new List<double>();
            bool PG;
            int PG_Actual = 4;

            SaldoI = Monto;
            Inte = SaldoI * TEP;
            depre = (VV / Total_cuotas);
            Flujo_B = Monto;
            Flujo_I = 0;
            Flujo_N = Monto;
            Seguro_riesgo = Seguro_riesgo * -1;
            Comision_periodica = (double)deuda.comision_periodica * -1;
            depre = depre * -1;
            cuota = 0.00;
            Flujo_Bruto_Lista.Add(Flujo_B);
            Flujo_Neto_Lista.Add(Flujo_N);
            switch (deuda.id_periodo_gracia)
            {
                case 2://Total
                    PG_Actual = 2;
                    break;

                case 3://Parcial
                    PG_Actual = 3;
                    break;

                case 4:
                    PG_Actual = 4;
                    break;
                default:
                    break;
            }
            //Hacer las cuotas
            for (int i = 1; i <= Total_cuotas; i++)
            {
                cuota objCuota = new cuota();
                Inte = SaldoI * TEP;
                Inte = Inte * -1;
                if (i >= (deuda.PG_Tiempo+1))
                {
                    PG_Actual = 4;
                }

                if (PG_Actual == 2 || PG_Actual == 3)
                {
                    PG = true;
                }
                else
                {
                    PG = false;
                }

                if (PG == true)
                {
                    Amort = 0;
                }
                else
                {
                    Amort = SaldoI / (Total_cuotas - i + 1);
                }
                Amort = Amort * -1;

                if (PG_Actual == 2)
                    cuota = 0;
                else
                {
                    if (PG_Actual == 3)
                        cuota = Inte;
                    else
                        cuota = Inte + Amort;
                }

                if (i == Total_cuotas)
                    recomp = Recompra * VV;
                else
                    recomp = 0;
                recomp *= -1;
                if (PG_Actual == 2)
                    SaldoF = SaldoI - Inte;
                else
                    SaldoF = SaldoI + Amort;

                ahorroT = (Inte + Seguro_riesgo + Comision_periodica + depre) * Impuesto_renta;
                i_g_v = (cuota + Seguro_riesgo + Comision_periodica + recomp) * deuda.IGV;

                Flujo_B = cuota + Seguro_riesgo + Comision_periodica + recomp;
                Flujo_I = Flujo_B + i_g_v;
                Flujo_N = Flujo_B - ahorroT;
                Intereses = Intereses + Inte;
                Amortizacion = Amortizacion + Amort;
                Total_seguro = Total_seguro + Seguro_riesgo;
                Total_comision = Total_comision + Comision_periodica;
                Total_recompra = Total_recompra + recomp;
                objCuota.id_deuda = deuda.id;
                objCuota.saldo_inicial = (decimal)SaldoI;
                objCuota.interes = (decimal)Inte;
                objCuota.monto_cuota = (decimal)cuota;
                objCuota.amortizacion = (decimal)Amort;
                objCuota.seguro_riesgo = (decimal)Seguro_riesgo;
                objCuota.recompra = (decimal)recomp;
                objCuota.Comision = (decimal)Comision_periodica;
                objCuota.saldo_final = (decimal)SaldoF;
                objCuota.depreciacion = (decimal)depre;
                objCuota.ahorro_tributario = (decimal)ahorroT;
                objCuota.IGV = (decimal)i_g_v;
                objCuota.f_bruto = (decimal)Flujo_B;
                objCuota.f_igv = (decimal)Flujo_I;
                objCuota.f_neto = (decimal)Flujo_N;
                objCuota.PeriodoGracia = PG;
                objCuota.Pagado = false;
                objCuota.Fecha_De_Pago = DateTime.Today.AddMonths(i);
                lista.Add(objCuota);
                SaldoI = SaldoF;

                Flujo_Bruto_Lista.Add(Flujo_B);
                Flujo_Neto_Lista.Add(Flujo_N);
                Flujo_Bruto_Lista_VAN.Add(Flujo_B);
                Flujo_Neto_Lista_VAN.Add(Flujo_N);
            }
            int index =(int)deuda.id_bien;
            resultado.Bien_id = index;
            resultado.IGV = (decimal)IGV;
            resultado.ValorVenta = (decimal)VV;
            resultado.MontoLeasing = (decimal)Monto;
            resultado.Porcentaje_TEP = TEP;
            resultado.Numero_Cuotas = Nro_cuotas_por_año;
            resultado.Total_Cuotas = Total_cuotas;
            resultado.Seguro_Riesgo = (decimal)Seguro_riesgo;
            resultado.Intereses = (decimal)Intereses;
            resultado.Amortizacion_Capital = (decimal)Amortizacion;
            resultado.Seguro_contra_riesgo = (decimal)Total_seguro;
            resultado.Comision_periodica = (decimal)Total_comision;
            resultado.Recompra = (decimal)Total_recompra;
            resultado.Desembolso_Total = (decimal)(Intereses + Amortizacion + Total_seguro + Total_comision + Total_recompra);

            double tir_FB = Excel.FinancialFunctions.Financial.Irr(Flujo_Bruto_Lista, 1/100);
            tir_FB = 1 + tir_FB;
            double TCEA_FlujoB = Math.Pow(tir_FB, ((deuda.dias_x_año * 1.0) / (deuda.frecuencia * 1.0)));
            TCEA_FlujoB = TCEA_FlujoB - 1;
            resultado.TCEA_FlujoB = TCEA_FlujoB*100;

            double tir_FN = Excel.FinancialFunctions.Financial.Irr(Flujo_Neto_Lista, 1 / 100);
            tir_FN = 1 + tir_FN;
            double TCEA_FlujoN = Math.Pow(tir_FN, ((deuda.dias_x_año * 1.0) / (deuda.frecuencia * 1.0)));
            TCEA_FlujoN = TCEA_FlujoN - 1;
            resultado.TCEA_FlujoN = TCEA_FlujoN * 100;

            double exp= ((deuda.frecuencia * 1.0) / (deuda.dias_x_año * 1.0));
            double n1=Math.Pow((1+Ks),exp);
            n1 = n1 - 1;
            double VAN_FlujoB = Excel.FinancialFunctions.Financial.Npv(n1, Flujo_Bruto_Lista_VAN);
            VAN_FlujoB = VAN_FlujoB + Flujo_Bruto_Lista[0];
            resultado.VAN_FlujoB = (decimal)VAN_FlujoB;

            double exp2 = ((deuda.frecuencia * 1.0) / (deuda.dias_x_año * 1.0));
            double n2 = Math.Pow((1 + WACC), exp2);
            n2 = n2 - 1;
            double VAN_FlujoN = Excel.FinancialFunctions.Financial.Npv(n2, Flujo_Neto_Lista_VAN);
            VAN_FlujoN = VAN_FlujoN + Flujo_Neto_Lista[0];
            resultado.VAN_FlujoN = (decimal)VAN_FlujoN ;

            
            
            db.Results.Add(resultado);
            db.SaveChanges();
            return lista;
        }

        // GET: Deudas/Create
        public ActionResult Crear()
        {
            ViewBag.id_bien = new SelectList(db.Biens, "id", "nombre");
            ViewBag.id_periodo_gracia = new SelectList(db.Periodo_gracia, "id", "Descripcion");
            return View();
        }

        // POST: Deudas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "id,id_bien,id_periodo_gracia,PG_Tiempo,nro_años,frecuencia,dias_x_año,TEA,IGV,i_renta,recompra,costo_notarial,costo_registral,tasacion,comision_de_estudio,comision_de_activacion,comision_periodica,seguro_riesgo,tasa_ks,tasa_wacc")] Deuda deuda)
        {
            if (ModelState.IsValid)
            {
                deuda.IGV = 0.18;
                db.Deudas.Add(deuda);
                db.SaveChanges();
                List < cuota > lst = Calcular_Cuotas(deuda);
                db.cuotas.AddRange(lst);
                db.SaveChanges();
                return RedirectToAction("Resultado", "Results");
            }
            ViewBag.id_bien = new SelectList(db.Biens, "id", "nombre", deuda.id_bien);
            ViewBag.id_periodo_gracia = new SelectList(db.Periodo_gracia, "id", "Descripcion", deuda.id_periodo_gracia);
            return View(deuda);
        }
    }
}
