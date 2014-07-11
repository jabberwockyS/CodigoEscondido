using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesoroQR.Models.Tesoro;

namespace TesoroQR.Controllers
{
    [Authorize(Users = "Admin")]
    public class CircuitoController : Controller
    {
        private JuegoDBContext db = new JuegoDBContext();

        // GET: /Circuito/
        public ActionResult Index(int id)
        {
            Partida Partida = db.Partidas.Find(id);
            List<Circuito> circuitos = db.Circuitos.Where(x => x.Partida.PartidaID == id).ToList();

            //foreach(Circuito cir in circuitos)
            //{
            //    cir.Partida = db.Partidas.Single(x => x.PartidaID == id);
            //}


            Session["partidaid"] = id;
            return View(circuitos);
        }

        // GET: /Circuito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circuito circuito = db.Circuitos.Find(id);
            if (circuito == null)
            {
                return HttpNotFound();
            }
            return View(circuito);
        }

        // GET: /Circuito/Create
        public ActionResult Create()
        {
            return View();  
        }

        // POST: /Circuito/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CircuitoID,Nombre")] Circuito circuito)
        {

            if (ModelState.IsValid)
            {
                
                int partid = Convert.ToInt32(Session["partidaid"]);
               // db.Circuitos.Add(circuito);
                Partida part = db.Partidas.Find(partid);
                part.Circuito = new List<Circuito>();
                part.Circuito = db.Circuitos.Where(x => x.Partida.PartidaID == partid).ToList();
                part.Circuito.Add(circuito);
                db.SaveChanges();


                

                return RedirectToAction("Index", new { id = partid });
                
            }

            return View(circuito);
        }

        // GET: /Circuito/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circuito circuito = db.Circuitos.Find(id);
            if (circuito == null)
            {
                return HttpNotFound();
            }
            return View(circuito);
        }

        // POST: /Circuito/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CircuitoID,Nombre")] Circuito circuito)
        {
            if (ModelState.IsValid)
            {
                int partid = Convert.ToInt32(Session["partidaid"]);
                db.Entry(circuito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = partid });
            }
            return View(circuito);
        }

        // GET: /Circuito/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circuito circuito = db.Circuitos.Find(id);
            if (circuito == null)
            {
                return HttpNotFound();
            }
            return View(circuito);
        }

        // POST: /Circuito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Circuito circuito = db.Circuitos.Find(id);
            db.Circuitos.Remove(circuito);
            db.SaveChanges();

            int partid = Convert.ToInt32(Session["partidaid"]);
            return RedirectToAction("Index", new { id = partid });
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
