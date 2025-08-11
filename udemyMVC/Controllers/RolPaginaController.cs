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
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public ActionResult Index()
        {
            ListarComboRol();
            ListarComboPag();
            List<RolPaginaCLS> listaRolPag = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaRolPag = (from rolPag in bd.RolPagina
                               join rol in bd.Rol
                               on rolPag.IIDROL equals rol.IIDROL
                               join pagina in bd.Pagina
                               on rolPag.IIDPAGINA equals pagina.IIDPAGINA
                               select new RolPaginaCLS
                               {
                                   idRolPagina = rolPag.IIDROLPAGINA,
                                   nomRol = rol.NOMBRE,
                                   nomMensaje = pagina.MENSAJE
                               }).ToList();
            }
                return View(listaRolPag);
        }

        public void ListarComboRol()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Rol
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDROL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaRol = lista;
            }
        }

        public ActionResult Filtro(int? idRolF)
        {
            List<RolPaginaCLS> listaRolPag = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (idRolF == null)
                {
               
               
                        listaRolPag = (from rolPag in bd.RolPagina
                                       join rol in bd.Rol
                                       on rolPag.IIDROL equals rol.IIDROL
                                       join pagina in bd.Pagina
                                       on rolPag.IIDPAGINA equals pagina.IIDPAGINA
                                       where rolPag.BHABILITADO==1
                                       select new RolPaginaCLS
                                       {
                                           idRolPagina = rolPag.IIDROLPAGINA,
                                           nomRol = rol.NOMBRE,
                                           nomMensaje = pagina.MENSAJE
                                       }).ToList();
                
                }
                else
                {
                    listaRolPag = (from rolPag in bd.RolPagina
                                   join rol in bd.Rol
                                   on rolPag.IIDROL equals rol.IIDROL
                                   join pagina in bd.Pagina
                                   on rolPag.IIDPAGINA equals pagina.IIDPAGINA
                                   where rolPag.BHABILITADO == 1 && rolPag.IIDROL== idRolF
                                   select new RolPaginaCLS
                                   {
                                       idRolPagina = rolPag.IIDROLPAGINA,
                                       nomRol = rol.NOMBRE,
                                       nomMensaje = pagina.MENSAJE
                                   }).ToList();
                }
            }

            return PartialView("_TableRolPag",listaRolPag);
        }

        public void ListarComboPag()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Pagina
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.MENSAJE,
                             Value = item.IIDPAGINA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaPag = lista;
            }
        }

        public string Guardar(RolPaginaCLS oRolPagCLS, int titulo)
        {
            string res = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    res += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        res += "<li class='list-group-item'>" + item + "</li>";
                    }
                    res += "</ul>";

                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        int registros = 0;
                        if (titulo == -1)
                        {
                            registros = bd.RolPagina.Where(p => p.IIDROL == oRolPagCLS.idRol && p.IIDPAGINA == oRolPagCLS.idPagina).Count();
                            if (registros >= 1)
                            {
                                res = "-1";
                            }
                            else
                            {
                                RolPagina oRolPagina = new RolPagina();
                                oRolPagina.IIDROL = oRolPagCLS.idRol;
                                oRolPagina.IIDPAGINA = oRolPagCLS.idPagina;
                                oRolPagina.BHABILITADO = 1;
                                bd.RolPagina.Add(oRolPagina);
                                res = bd.SaveChanges().ToString();
                                if (res == "0") { res = ""; }
                            }                           
                            
                        }
                        else
                        {
                            registros = bd.RolPagina.Where(p => p.IIDROL == oRolPagCLS.idRol && p.IIDPAGINA == oRolPagCLS.idPagina && p.IIDROLPAGINA!=titulo).Count();
                            if (registros >= 1)
                            {
                                res = "-1";
                            }
                            else
                            {
                                RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == titulo).First();
                                oRolPagina.IIDROL = oRolPagCLS.idRol;
                                oRolPagina.IIDPAGINA = oRolPagCLS.idPagina;
                                res = bd.SaveChanges().ToString();
                            }
                            
                        }
                    }
                }
            }
            catch (Exception e) { res = ""; }  
            

            return res;
        }

        public JsonResult recuperaData(int idRolPag)
        {
            RolPaginaCLS oRolPaginaCLS = new RolPaginaCLS();
            using (var bd = new BDPasajeEntities())
            {
                RolPagina oRolPagin = bd.RolPagina.Where(p => p.IIDROLPAGINA == idRolPag).First();
                oRolPaginaCLS.idRol = (int)oRolPagin.IIDROL;
                oRolPaginaCLS.idPagina = (int)oRolPagin.IIDPAGINA;


            }
            return Json(oRolPaginaCLS, JsonRequestBehavior.AllowGet);
        }

        public int Eliminar(int idRolP)
        {
            int res = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == idRolP).First();
                    oRolPagina.BHABILITADO = 0;
                    res = bd.SaveChanges();
                }
            }
            catch (Exception e) { res = 0; }
            return res;
        }
    }

    
}