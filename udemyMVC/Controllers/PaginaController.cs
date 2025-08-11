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
    public class PaginaController : Controller
    {
        // GET: Pagina
        public ActionResult Index()
        {
            List<PaginaCLS> listaPag = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaPag = (from pagina in bd.Pagina
                            where pagina.BHABILITADO == 1
                            select new PaginaCLS
                            {
                                idPagina = pagina.IIDPAGINA,
                                mensaje = pagina.MENSAJE,
                                controlador = pagina.CONTROLADOR,
                                accion = pagina.ACCION

                            }).ToList();
            }
                return View(listaPag);
        }

        public ActionResult Filtro(PaginaCLS oPaginaCLS)
        {
            List<PaginaCLS> listaPag = new List<PaginaCLS>();
            string mensaje = oPaginaCLS.mensajeText;
            using (var bd = new BDPasajeEntities())
            {
                if (mensaje == null)
                {
                    listaPag = (from pagina in bd.Pagina
                                where pagina.BHABILITADO == 1
                                select new PaginaCLS
                                {
                                    idPagina = pagina.IIDPAGINA,
                                    mensaje = pagina.MENSAJE,
                                    controlador = pagina.CONTROLADOR,
                                    accion = pagina.ACCION

                                }).ToList();
                }
                else
                {
                    listaPag = (from pagina in bd.Pagina
                                where pagina.BHABILITADO == 1 && pagina.MENSAJE.Contains(mensaje)
                                select new PaginaCLS
                                {
                                    idPagina = pagina.IIDPAGINA,
                                    mensaje = pagina.MENSAJE,
                                    controlador = pagina.CONTROLADOR,
                                    accion = pagina.ACCION

                                }).ToList();
                }
            }
                return PartialView("_TablaPagina", listaPag);
        }

        public string Guardar(PaginaCLS oPaginaCLS, int titulo)
        {
            string resp = "0";
            int registros = 0;
            try
            {
                if (!ModelState.IsValid) {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    resp += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        resp += "<li class='list-group-item'>" + item + "</li>";
                    }
                    resp += "</ul>";
                } else { 
                    using (var bd = new BDPasajeEntities())
                    {                       
                        if (titulo.Equals( -1))
                        {
                            registros = bd.Pagina.Where(p => p.MENSAJE == oPaginaCLS.mensaje).Count();
                            if (registros >= 1)
                            {
                                resp = "-1";
                            }
                            else
                            {
                                  Pagina oPagina = new Pagina();
                            oPagina.MENSAJE = oPaginaCLS.mensaje;
                            oPagina.ACCION = oPaginaCLS.accion;
                            oPagina.CONTROLADOR = oPaginaCLS.controlador;
                            oPagina.BHABILITADO = 1;
                            bd.Pagina.Add(oPagina);
                            resp = bd.SaveChanges().ToString();
                            if (resp == "0") { resp = ""; }
                            }
                          
                        }
                        else
                        {
                            registros = bd.Pagina.Where(p => p.MENSAJE == oPaginaCLS.mensaje && p.IIDPAGINA!=oPaginaCLS.idPagina).Count();
                            if (registros >= 1)
                            {
                                resp = "-1";
                            }
                            else
                            {
                                Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == titulo).First();
                                oPagina.MENSAJE = oPaginaCLS.mensaje;
                                oPagina.CONTROLADOR = oPaginaCLS.controlador;
                                oPagina.ACCION = oPaginaCLS.accion;
                                bd.Pagina.Add(oPagina);
                                resp = bd.SaveChanges().ToString();
                            }
                           
                        }
                    }
                }

            }
            catch (Exception e) { resp = ""; }
            return resp;
        }

        public JsonResult Editar(int idPag)
        {
            PaginaCLS oPaginaCLS = new PaginaCLS();
            using (var bd = new BDPasajeEntities())
            {
                Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == idPag).First();
                oPaginaCLS.mensaje = oPagina.MENSAJE;
                oPaginaCLS.accion = oPagina.ACCION;
                oPaginaCLS.controlador = oPagina.CONTROLADOR;

            }
            return Json(oPaginaCLS, JsonRequestBehavior.AllowGet);
        }

        public int Eliminar(int idPagina)
        {
            int res = 0;
            try {
                using (var bd = new BDPasajeEntities())
                {
                    Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == idPagina).First();
                    oPagina.BHABILITADO = 0;
                    res = bd.SaveChanges();
                }
            } catch (Exception e) { res = 0; }
            return res;
        }
    }
}