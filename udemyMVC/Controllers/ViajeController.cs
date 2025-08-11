using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;
using udemyMVC.Filtros;

namespace udemyMVC.Controllers
{
    [Acceder]
    public class ViajeController : Controller
    {
        // GET: Viaje
        public ActionResult Index()
        {
            List<ViajeCLS> listaViaje = null;
            listarCombos();
            using (var bd = new BDPasajeEntities())
            {
                listaViaje = (from viaje in bd.Viaje
                              join lugarO in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarO.IIDLUGAR
                              join lugarD in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarD.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              where viaje.BHABILITADO==1
                              select new ViajeCLS
                              {
                                  iidViaje = viaje.IIDVIAJE,
                                  nombreBus = bus.PLACA,
                                  nombreLugarOrigen = lugarO.NOMBRE,
                                  nombreLugarDestino = lugarD.NOMBRE

                              }).ToList();

            }
                return View(listaViaje);
        }
        public ActionResult Agregar()
        {
            listarCombos();
            return View();
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
        public void ListarBus()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from bus in bd.Bus
                         where bus.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = bus.PLACA,
                             Value = bus.IIDBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaBus = lista;
            }
        }
        public void listarCombos()
        {
            ListarBus();
            ListarLugar();
        }

        public ActionResult Filtro(int? idLugarDestino)
        {
            List<ViajeCLS> listaViaje = new List<ViajeCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (idLugarDestino == null)
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarO in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarO.IIDLUGAR
                                  join lugarD in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarD.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  select new ViajeCLS
                                  {
                                      iidViaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarO.NOMBRE,
                                      nombreLugarDestino = lugarD.NOMBRE

                                  }).ToList();
                }
                else
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarO in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarO.IIDLUGAR
                                  join lugarD in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarD.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  && viaje.IIDLUGARDESTINO == idLugarDestino
                                  select new ViajeCLS
                                  {
                                      iidViaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarO.NOMBRE,
                                      nombreLugarDestino = lugarD.NOMBRE

                                  }).ToList();
                }

            }
            return PartialView("_TableViaje", listaViaje);
        }

        public string Guardar(ViajeCLS oViajeCLS, HttpPostedFileBase Foto, int titulo, string nomFoto)
        {
            string resp = "";
            try {
                if (!ModelState.IsValid || (Foto == null && titulo == -1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();

                    if (Foto == null && titulo == -1)
                    {
                        oViajeCLS.mensaje = "La foto es obligatoria";
                        resp += "<ul><li> Debe ingresar una foto</li></ul>";
                    }
                    resp += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        resp += "<li class='list-group-item'>" + item + "</li>";
                    }
                    resp += "</ul>";
                }
                else {
                    byte[] fotoBD = null;
                    if (Foto != null)
                    {
                        BinaryReader lector = new BinaryReader(Foto.InputStream);
                        fotoBD = lector.ReadBytes((int)Foto.ContentLength);
                    }
                    using (var bd = new BDPasajeEntities())
                    {
                        if (titulo == -1)
                        {
                            Viaje oViaje = new Viaje();
                            oViaje.IIDBUS = oViajeCLS.idBus;
                            oViaje.IIDLUGARDESTINO = oViajeCLS.idDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCLS.idOrigen;
                            oViaje.PRECIO = oViajeCLS.precio;
                            oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientos;
                            oViaje.FOTO = fotoBD;
                            oViaje.nombrefoto = nomFoto;
                            oViaje.BHABILITADO = 1;
                            bd.Viaje.Add(oViaje);
                            resp = bd.SaveChanges().ToString();
                            if (resp == "0") { resp = ""; }
                        }
                        else
                        {
                            Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == titulo).First();
                            oViaje.IIDBUS = oViajeCLS.idBus;
                            oViaje.IIDLUGARDESTINO = oViajeCLS.idDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCLS.idOrigen;
                            oViaje.PRECIO = oViajeCLS.precio;
                            oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientos;
                            if (Foto != null)
                            {
                                oViaje.FOTO = fotoBD;
                                oViaje.nombrefoto = nomFoto;
                            }
                            resp = bd.SaveChanges().ToString();
                        }
                    }
                }
            } catch (Exception e) { resp = ""; }
            return resp;
        }


        public JsonResult RecuperaInfo(int idViaje)
        {
            ViajeCLS oViajeCLS = new ViajeCLS();
            using (var bd = new BDPasajeEntities())
            {
                Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                oViajeCLS.iidViaje = oViaje.IIDVIAJE;
                oViajeCLS.idBus = (int)oViaje.IIDBUS;
                oViajeCLS.idDestino = (int)oViaje.IIDLUGARDESTINO;
                oViajeCLS.idOrigen = (int)oViaje.IIDLUGARORIGEN;
                oViajeCLS.precio = (decimal)oViaje.PRECIO;
                oViajeCLS.fechaViajeCadena = oViaje.FECHAVIAJE!=null?
                 ((DateTime)oViaje.FECHAVIAJE).ToString("yyyy-MM-dd"):"";
                oViajeCLS.numeroAsientos = (int)oViaje.NUMEROASIENTOSDISPONIBLES;
                oViajeCLS.nombreFoto = oViaje.nombrefoto;
                oViajeCLS.extension = Path.GetExtension(oViaje.nombrefoto);
                oViajeCLS.fotoCadena = Convert.ToBase64String(oViaje.FOTO);
            }
            return Json(oViajeCLS, JsonRequestBehavior.AllowGet);
        }

        public int Eliminar(int idViaje)
        {
            int res = 0;
            try {
                using (var bd = new BDPasajeEntities())
                {
                    Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                    oViaje.BHABILITADO = 0;
                    res = bd.SaveChanges();
                }
            } catch (Exception e) { }
            return res;
        }


    }
}