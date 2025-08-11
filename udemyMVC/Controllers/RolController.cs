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
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {
            List<RolCLS> listaRol = new List<RolCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from rol in bd.Rol
                            where rol.BHABILITADO == 1
                            select new RolCLS
                            {
                                idRol = rol.IIDROL,
                                nombre = rol.NOMBRE,
                                descripcion = rol.DESCRIPCION
                            }).ToList();
            }
                return View(listaRol);
        }

        public ActionResult Filtro(string nombreRol)
        {
            List<RolCLS> listaRol = new List<RolCLS>();
           
                using (var bd = new BDPasajeEntities())
                {
                    if (nombreRol == null)
                    {

                        listaRol = (from rol in bd.Rol
                                    where rol.BHABILITADO == 1
                                    select new RolCLS
                                    {
                                        idRol = rol.IIDROL,
                                        nombre = rol.NOMBRE,
                                        descripcion = rol.DESCRIPCION
                                    }).ToList();
                    }
                    else
                    {

                        listaRol = (from rol in bd.Rol
                                    where rol.BHABILITADO == 1 && rol.NOMBRE.Contains(nombreRol)
                                    select new RolCLS
                                    {
                                        idRol = rol.IIDROL,
                                        nombre = rol.NOMBRE,
                                        descripcion = rol.DESCRIPCION
                                    }).ToList();

                    }
                }
          
            
            return PartialView("_TablaRol",listaRol);
        }

        public string Guardar(RolCLS oRolCLS, int titulo)
        {
            string resp = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    resp += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        resp += "<li class='list-group-item'>" + item + "</li>";
                    }
                    resp += "</ul>";
                }
                else
                {
                    int registros = 0;
                    using (var bd = new BDPasajeEntities())
                    {
                        if (titulo.Equals(-1))
                        {
                            registros = bd.Rol.Where(p => p.NOMBRE == oRolCLS.nombre).Count();
                            
                            if (registros >= 1)
                            {
                                resp = "-1";
                            }
                            else
                            {
                                Rol oRol = new Rol();
                                oRol.NOMBRE = oRolCLS.nombre;
                                oRol.DESCRIPCION = oRolCLS.descripcion;
                                oRol.BHABILITADO = 1;
                                bd.Rol.Add(oRol);
                                resp = bd.SaveChanges().ToString();
                                if (resp == "0") { resp = ""; }
                            }
                        }
                        else
                        {
                            registros = bd.Rol.Where(p => p.NOMBRE == oRolCLS.nombre && p.IIDROL!=titulo).Count();

                            if (registros >= 1)
                            {
                                resp = "-1";
                            }
                            else
                            {
                                Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                                oRol.NOMBRE = oRolCLS.nombre;
                                oRol.DESCRIPCION = oRolCLS.descripcion;
                                resp = bd.SaveChanges().ToString();
                            }

                        }
                    }
                }
            }
            catch (Exception e) { resp = ""; }
            return resp;
        }

        public JsonResult recuperarDatos(int titulo)
        {
            //se ocupa jsonresult para serializar un objeto y poder pasarlo a la vista ya que los objetos no se pueden pasar directamente
            RolCLS oRolCLS = new RolCLS();
            using (var bd = new BDPasajeEntities())
            {
                Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                oRolCLS.nombre = oRol.NOMBRE;
                oRolCLS.descripcion = oRol.DESCRIPCION;
               
            }
            return Json(oRolCLS,JsonRequestBehavior.AllowGet);

        }

        public string Eliminar(RolCLS oRolCls)
        {
           
            string resp = "";
            try
            {
                int idrol = oRolCls.idRol;
                using (var bd = new BDPasajeEntities())
                {
                    Rol oRol = bd.Rol.Where(p => p.IIDROL == idrol).First();
                    oRol.BHABILITADO = 0;
                    resp = bd.SaveChanges().ToString();
                }
            }
            catch (Exception ex) { resp = ""; }
            return resp;
        }
    }
}