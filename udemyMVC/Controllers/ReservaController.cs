using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;

namespace udemyMVC.Controllers
{
    public class ReservaController : Controller
    {
        // GET: Reserva
        public ActionResult Index()
        {
            ListarLugar();
            using (var bd = new BDPasajeEntities())
            {
                var reserva = (from Viaje in bd.Viaje
                               join lugar in bd.Lugar
                               on Viaje.IIDLUGARORIGEN equals lugar.IIDLUGAR
                               join bus in bd.Bus
                               on Viaje.IIDBUS equals bus.IIDBUS
                               join lugarDes in bd.Lugar
                               on Viaje.IIDLUGARDESTINO equals lugarDes.IIDLUGAR
                               where Viaje.BHABILITADO == 1
                               select new ReservaCLS
                               {
                                   idviaje = Viaje.IIDVIAJE,
                                   nombreArchivo = Viaje.nombrefoto,
                                   foto = Viaje.FOTO,
                                   lugarOrigen = lugar.NOMBRE,
                                   lugarDestino = lugarDes.NOMBRE,
                                   nombreBus = bus.DESCRIPCION,
                                   asientos = (int)Viaje.NUMEROASIENTOSDISPONIBLES
                               }).ToList();
                return View(reserva);
            }               
        }
        public void ListarLugar()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from lugar in bd.Lugar
                         where lugar.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = lugar.NOMBRE,
                             Value = lugar.IIDLUGAR.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaLugar = lista;
            }
        }
    }
}