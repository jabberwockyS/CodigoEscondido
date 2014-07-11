using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoroQR.Models.Tesoro;

namespace TesoroQR.Controllers
{
     [Authorize]
    public class InformesController : Controller
    {
        TesoroRepository repo = new TesoroRepository();
        //
        // GET: /Informes/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ListarPartidas()
        {
            return View(repo.Partidas());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ListarJugadores(int partidaID)
        {
            Session["partidaID"] = partidaID;
            return View(repo.UsuariosPorPartida(partidaID));
        }



        [Authorize(Roles = "Admin")]
        public ActionResult ListarAvancePorJugador(int jugadorID)
        { 
            int partidaID = Convert.ToInt32( Session["partidaID"]);



            return View(repo.ListarAvancePorJugador(jugadorID, partidaID));
        }

       

        public ActionResult VerMiAvance(string jugadorName)
        {
            Partida partida = repo.PartidaHoy();
            Usuario jugador = repo.JugadorPorNombre(jugadorName);
            return View(repo.ListarAvancePorJugador(jugador.UsuarioID, partida.PartidaID));
        }

	}
}