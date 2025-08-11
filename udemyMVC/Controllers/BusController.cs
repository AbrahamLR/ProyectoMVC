using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;
using udemyMVC.Filtros;

namespace udemyMVC.Controllers
{
    [Acceder]
    public class BusController : Controller
    {
        // GET: Bus
        public ActionResult Index(BusCLS oBuscls)
        {
            listarCombos();
            List<BusCLS> listaRes = new List<BusCLS>();
            List<BusCLS> listabus = null;
            using (var bd = new BDPasajeEntities())
            {
                if (oBuscls.idBus == 0 && oBuscls.placa == null && oBuscls.idModelo == 0 && oBuscls.idSucursal == 0 && oBuscls.idTipoBuspublic == 0)
                {
                    listabus = (from bus in bd.Bus
                                join sucursal in bd.Sucursal
                                on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                                join tipobus in bd.TipoBus
                                on bus.IIDTIPOBUS equals tipobus.IIDTIPOBUS
                                join tipomodelo in bd.Modelo
                                on bus.IIDMODELO equals tipomodelo.IIDMODELO
                                where bus.BHABILITADO == 1
                                select new BusCLS
                                {
                                    idBus = bus.IIDBUS,
                                    placa = bus.PLACA,
                                    nombreModelo = tipomodelo.NOMBRE,
                                    nombreSucursal = sucursal.NOMBRE,
                                    nombreTipoBus = tipobus.NOMBRE,
                                    idModelo=tipomodelo.IIDMODELO,
                                    idSucursal=sucursal.IIDSUCURSAL,
                                   idTipoBuspublic=tipobus.IIDTIPOBUS

                                }).ToList();
                    listaRes = listabus;
                }
                else
                {
                    if (oBuscls.idBus != 0) {listabus= listabus.Where(p => p.idBus.ToString().Contains(oBuscls.idBus.ToString())).ToList(); }
                    if (oBuscls.placa != null) { listabus = listabus.Where(p => p.placa.Contains(oBuscls.placa)).ToList(); }
                    if (oBuscls.idModelo != 0) { listabus = listabus.Where(p => p.idModelo.ToString().Contains(oBuscls.idModelo.ToString())).ToList(); }
                    if (oBuscls.idSucursal != 0) { listabus = listabus.Where(p => p.idSucursal.ToString().Contains(oBuscls.idSucursal.ToString())).ToList(); }
                }
                listaRes = listabus;
            }
                return View(listaRes);
        }

        public ActionResult Agregar()
        {           
            listarCombos();
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(BusCLS oBusCls)
        {
            if (!ModelState.IsValid) { return View(oBusCls); }
            using (var bd = new BDPasajeEntities())
            {
                Bus oBus = new Bus();
                oBus.BHABILITADO = 1;
                oBus.IIDSUCURSAL = oBusCls.idSucursal;
                oBus.IIDTIPOBUS = oBusCls.idTipoBuspublic;
                oBus.PLACA = oBusCls.placa;
                oBus.FECHACOMPRA = oBusCls.fechaCompra;
                oBus.IIDMODELO = oBusCls.idModelo;
                oBus.NUMEROFILAS = oBusCls.numFilas;
                oBus.NUMEROCOLUMNAS = oBusCls.numColumnas;
                oBus.DESCRIPCION = oBusCls.descripcion;
                oBus.OBSERVACION = oBusCls.observacion;
                oBus.IIDMARCA = oBusCls.idMarca;
                bd.Bus.Add(oBus);
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            listarCombos();
            BusCLS obusCls = new BusCLS();
            using (var bd = new BDPasajeEntities())
            {
                Bus obus = bd.Bus.Where(p => p.IIDBUS.Equals(id)).First();
                obusCls.idBus = obus.IIDBUS;
                obusCls.idSucursal = (int)obus.IIDSUCURSAL;
                obusCls.idTipoBuspublic = (int)obus.IIDTIPOBUS;
                obusCls.placa = obus.PLACA;
                obusCls.fechaCompra = (DateTime)obus.FECHACOMPRA;
                obusCls.idModelo = (int)obus.IIDMODELO;
                obusCls.numFilas = (int)obus.NUMEROFILAS;
                obusCls.numColumnas = (int)obus.NUMEROCOLUMNAS;
                obusCls.idMarca = (int)obus.IIDMARCA;
                obusCls.observacion = obus.OBSERVACION;
                obusCls.descripcion = obus.DESCRIPCION;


            }
           
            return View(obusCls);
        }

        public void listaTipoBus()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.TipoBus
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaTipoBus = lista;
            }
        }

        public void listaModelo()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Modelo
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDMODELO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaModelo = lista;
            }
        }

        public void listaSucursal()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Sucursal
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDSUCURSAL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaSucursal = lista;
            }
        }

        public void listaMarca()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Marca
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDMARCA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaMarca = lista;
            }
        }

        public void listarCombos()
        {
            listaMarca();
            listaModelo();
            listaSucursal();
            listaTipoBus();
        }
    }
}