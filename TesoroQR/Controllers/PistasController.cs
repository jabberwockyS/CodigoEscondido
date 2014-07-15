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
    [Authorize]
    public class PistasController : Controller
    {

        JuegoDBContext db = new JuegoDBContext();

        TesoroRepository repo = new TesoroRepository();


        //
        // GET: /Pistas/

        [Authorize(Users = "Admin")]
        public ActionResult Index(int id)
        {
            Session["CircuitoID"] = id;
            return View(db.Pistas.Where(x => x.Circuito.CircuitoID == id).ToList());
        }


        public ActionResult VerPista(int Id)
        {

            String nombreUsuario = User.Identity.Name;
            Partida partida = db.Partidas.Single(x => x.Fecha.CompareTo(DateTime.Today) == 0);
            List<Circuito> circuitosList = db.Circuitos.Where(x => x.Partida.PartidaID == partida.PartidaID).ToList();
            Usuario usuario; //no lo cargo porque puede que ese usuario no exista
            Circuito queCircuito = QueCircuito(Id,circuitosList);
            int queOrden = QueOrden(Id);


            //guardo el usuario si es que ya no existe
            if (!db.Usuarios.Any(x => x.Nombre == nombreUsuario))
            {
                usuario = new Usuario() { Nombre = nombreUsuario, TipoUsuario = "jugador" };
                db.Usuarios.Add(usuario);
                //guardo el usuario
                db.SaveChanges();
            }
            else
            {
                usuario = db.Usuarios.Single(x => x.Nombre == nombreUsuario);
            }

            
                    Juego juegoNuevo;
                    //si la partida es la primera y no es el mismo dia(si es otro dia deberia solo registrar la partida)
                    if (!db.Juegos.Any(x => x.Jugador.Nombre == nombreUsuario && x.Partida.Fecha.CompareTo(DateTime.Today) == 0))
                    {

                        //agrego un el juego si es que no existe
                        juegoNuevo = new Juego() { HoraInicio = DateTime.Now, horaFin = DateTime.Now, Jugador = usuario, Partida = partida };
                        db.Juegos.Add(juegoNuevo);

                        //guardo la partida
                        db.SaveChanges();
                    }
                   
                    juegoNuevo = db.Juegos.Single(x=> x.Jugador.Nombre == nombreUsuario && x.Partida.Fecha.CompareTo(DateTime.Today)== 0);

            //aca controla que sea un codigo de inicio de camino
            if (Id == 1 || Id == 6 || Id == 11 || Id == 16)
            {
                
                    




                     //aca controlo si ya no hay un avance registrado, de lo contrario quiere decir que no debo registrar la pista 1 del camino
                    if (!db.Avances.Any(x => x.Circuito.CircuitoID == queCircuito.CircuitoID && x.Juego.JuegoID == juegoNuevo.JuegoID))
                    {

                        Juego juegoAvance = db.Juegos.Single(x => x.Jugador.Nombre == nombreUsuario && x.Partida.Fecha.CompareTo(DateTime.Today) == 0);
                        db.Avances.Add(new Avance() { UltimaPista = 1, Circuito = queCircuito, Juego = juegoAvance });
                        //guardo el avance
                        db.SaveChanges();
                        int orden = QueOrden(Id);
                        Pista pistaSalida = db.Pistas.Single(x => x.Circuito.CircuitoID == queCircuito.CircuitoID && x.orden == orden);
                        pistaSalida.Circuito = db.Circuitos.Single(x => x.Pistas.Any(y => y.PistaID == pistaSalida.PistaID));

                        return View(pistaSalida);
                    }
                    else
                    {
                        Pista pista = db.Pistas.Single(x => x.Circuito.CircuitoID == queCircuito.CircuitoID && x.orden == queOrden);
                        return RedirectToAction("PistaYaEncontrada", pista);
                    }
                    


            }
            //aca controla que sea un codigo de final de camino
            else if (Id == 5 || Id == 10 || Id == 15 || Id == 20)
            {
                Usuario jugador = db.Usuarios.Single(x => x.Nombre == nombreUsuario);
                Juego juego = db.Juegos.Single(x => x.Jugador.UsuarioID == jugador.UsuarioID && x.Partida.PartidaID == partida.PartidaID);
                if (db.Avances.Any(x => x.Juego.JuegoID == juego.JuegoID && x.Circuito.CircuitoID == queCircuito.CircuitoID))
                {
                    Avance avance = db.Avances.Single(x => x.Juego.JuegoID == juego.JuegoID && x.Circuito.CircuitoID == queCircuito.CircuitoID);
                    Pista pista = db.Pistas.Single(x => x.Circuito.CircuitoID == queCircuito.CircuitoID && x.orden == queOrden);


                    


                    if (queOrden == avance.UltimaPista + 1)
                    {

                        avance.UltimaPista = QueOrden(Id);
                        db.SaveChanges();


                        //voy a controlar si gano, es decir, termino todos los circuitos

                        if (repo.Termino(jugador,partida))
                        {
                            repo.RegistrarFinal(juego);
                            return View("Termino");
                        }
                        else
                        {

                            return RedirectToAction("Gano", pista);
                        }

                    }
                    else if (queOrden <= avance.UltimaPista)
                    {
                        return RedirectToAction("PistaYaEncontrada", pista);
                    }
                    else
                    {
                        return RedirectToAction("Adelantado", pista);
                    }
                }
                else
                {
                    return RedirectToAction("Index","Home",null);
                }



            }
            //ahora controlemos cualquiera de las otras pista que no son finales!
            else
            {

                Usuario jugador = db.Usuarios.Single(x => x.Nombre == nombreUsuario);
                Juego juego = db.Juegos.Single(x => x.Jugador.UsuarioID == jugador.UsuarioID && x.Partida.PartidaID == partida.PartidaID);
                if (db.Avances.Any(x => x.Juego.JuegoID == juego.JuegoID && x.Circuito.CircuitoID == queCircuito.CircuitoID))
                {
                    Avance avance = db.Avances.Single(x => x.Juego.JuegoID == juego.JuegoID && x.Circuito.CircuitoID == queCircuito.CircuitoID);

                    Pista pista = db.Pistas.Single(x => x.Circuito.CircuitoID == queCircuito.CircuitoID && x.orden == queOrden);


                    if (queOrden == avance.UltimaPista + 1)
                    {
                        avance.UltimaPista = queOrden;
                        db.SaveChanges();




                        return View(pista);
                    }
                    else if (queOrden <= avance.UltimaPista)
                    {
                        return RedirectToAction("PistaYaEncontrada", pista);
                    }
                    else
                    {
                        return RedirectToAction("Adelantado", pista);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }


            }




           
        }

        public ActionResult PistaYaEncontrada(Pista pista)
        {
            return View(pista);
        }

        public ActionResult Adelantado(Pista pista)
        {
            return View(pista);
        }

        private Circuito QueCircuito(int Id, List<Circuito> circList)
		{
            
			Circuito circ;
			if (Id > 15)
            {
                circ = circList[3];
            }
            else if (Id > 10)
            {
                
                circ = circList[2];
            }
            else if (Id > 5)
            {
               
                circ = circList[1];
            }
            else
                circ = circList[0];


            return circ;
		
		}

        private int QueOrden(int Id)
        {
            int orden = Id;
            if (Id > 15)
              orden = Id - 15;
            else if (Id > 10)
              orden = Id - 10;
            else if (Id > 5)
              orden = Id - 5;

            return orden;

        }
      

        public ActionResult Gano(Pista pista)
        {
            pista.Circuito = db.Circuitos.Single(x => x.Pistas.Any(y => y.PistaID == pista.PistaID));
                
            return View(pista);
        }

        public ActionResult Termino()
        {
            return View();
        }

        [Authorize(Users = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pista pista = db.Pistas.Find(id);
            if (pista == null)
            {
                return HttpNotFound();
            }
            return View(pista);
        }

        // GET: /Esta/Create
        [Authorize(Users = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Esta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Create([Bind(Include = "PistaID,Nombre,Descripcion,orden")] Pista pista)
        {
            if (ModelState.IsValid)
            {
                int circuitoID = Convert.ToInt32(Session["CircuitoID"]);
                pista.Circuito = db.Circuitos.Single(x => x.CircuitoID == circuitoID);
                db.Pistas.Add(pista);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = circuitoID });
            }

            return View(pista);
        }

        // GET: /Esta/Edit/5
        [Authorize(Users = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pista pista = db.Pistas.Find(id);
            if (pista == null)
            {
                return HttpNotFound();
            }
            return View(pista);
        }

        // POST: /Esta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Edit([Bind(Include = "PistaID,Nombre,Descripcion,orden")] Pista pista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pista).State = EntityState.Modified;
                db.SaveChanges();
                int circuitoID = Convert.ToInt32(Session["CircuitoID"]);
                return RedirectToAction("Index", new { id = circuitoID });
            }
            return View(pista);
        }

        // GET: /Esta/Delete/5
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pista pista = db.Pistas.Find(id);
            if (pista == null)
            {
                return HttpNotFound();
            }
            return View(pista);
        }

        // POST: /Esta/Delete/5
        [Authorize(Users = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pista pista = db.Pistas.Find(id);
            db.Pistas.Remove(pista);
            db.SaveChanges();
            int circuitoID = Convert.ToInt32(Session["CircuitoID"]);
            return RedirectToAction("Index", new { id = circuitoID });
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

	
