using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;
using udemyMVC.Filtros;
namespace udemyMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        [Acceder]
        // GET: Empleado
        public ActionResult Index(EmpleadoCLS oEmpleadoCLS)
        {
            int idTipoUsu = oEmpleadoCLS.idtipoUsuario;
            ListarComboUsuario();
            List<EmpleadoCLS> listaEmpleado = null;
            using (var bd = new BDPasajeEntities()) {
                if (idTipoUsu == 0)
                {
                    listaEmpleado = (from empleado in bd.Empleado
                                     join tipousr in bd.TipoUsuario
                                     on empleado.IIDTIPOUSUARIO equals tipousr.IIDTIPOUSUARIO
                                     join tipoContrato in bd.TipoContrato
                                     on empleado.IIDTIPOCONTRATO equals tipoContrato.IIDTIPOCONTRATO
                                     where empleado.BHABILITADO == 1
                                     select new EmpleadoCLS
                                     {
                                         idempleado = empleado.IIDEMPLEADO,
                                         nombre = empleado.NOMBRE,
                                         apaterno = empleado.APPATERNO,
                                        
                                         nombreTipoUsuario = tipousr.NOMBRE,
                                         nombreTipoContrato = tipoContrato.NOMBRE

                                     }).ToList();
                }
                else
                {
                    listaEmpleado = (from empleado in bd.Empleado
                                     join tipousr in bd.TipoUsuario
                                     on empleado.IIDTIPOUSUARIO equals tipousr.IIDTIPOUSUARIO
                                     join tipoContrato in bd.TipoContrato
                                     on empleado.IIDTIPOCONTRATO equals tipoContrato.IIDTIPOCONTRATO
                                     where empleado.BHABILITADO==1 && empleado.IIDTIPOUSUARIO==idTipoUsu
                                     select new EmpleadoCLS
                                     {
                                         idempleado = empleado.IIDEMPLEADO,
                                         nombre = empleado.NOMBRE,
                                         apaterno = empleado.APPATERNO,
                                        
                                         nombreTipoUsuario = tipousr.NOMBRE,
                                         nombreTipoContrato = tipoContrato.NOMBRE

                                     }).ToList();
                }
               
            }
                return View(listaEmpleado);
        }

        public ActionResult Agregar()
        {
            listarCombos();
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(EmpleadoCLS oEmpleadosCLS)
        {
            if (!ModelState.IsValid)
            {
                listarCombos();
                return View(oEmpleadosCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = new Empleado();
                oEmpleado.NOMBRE = oEmpleadosCLS.nombre;
                oEmpleado.APPATERNO = oEmpleadosCLS.apaterno;
                oEmpleado.APMATERNO = oEmpleadosCLS.amaterno;
                oEmpleado.FECHACONTRATO = oEmpleadosCLS.fechaContrato;
                oEmpleado.SUELDO = oEmpleadosCLS.sueldo;
                oEmpleado.IIDTIPOUSUARIO = oEmpleadosCLS.idtipoUsuario;
                oEmpleado.IIDTIPOCONTRATO = oEmpleadosCLS.idtipoContrato;
                oEmpleado.IIDTIPOUSUARIO = oEmpleadosCLS.idtipoUsuario;
                oEmpleado.IIDSEXO = oEmpleadosCLS.idsexo;
                oEmpleado.BHABILITADO = 1;
                bd.Empleado.Add(oEmpleado);
                bd.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public void ListarComboSexo()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from sexo in bd.Sexo
                         where sexo.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = sexo.NOMBRE,
                             Value = sexo.IIDSEXO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value ="" });
                ViewBag.listaSexo = lista;
            }
        }

        public void ListarComboContrato()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from contrato in bd.TipoContrato
                         where contrato.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = contrato.NOMBRE,
                             Value = contrato.IIDTIPOCONTRATO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaContrato = lista;
            }
        }

        public void ListarComboUsuario()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from usuario in bd.TipoUsuario
                         where usuario.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = usuario.NOMBRE,
                             Value = usuario.IIDTIPOUSUARIO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione...--", Value = "" });
                ViewBag.listaUsuario = lista;
            }
        }

        public void listarCombos()
        {
            ListarComboSexo();
            ListarComboContrato();
            ListarComboUsuario();
        }

        public ActionResult Editar(int id)
        {
            listarCombos();
            EmpleadoCLS oEmpleadosCLS = new EmpleadoCLS();
            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(id)).First();
                oEmpleadosCLS.nombre = oEmpleado.NOMBRE;
                oEmpleadosCLS.apaterno = oEmpleado.APPATERNO;
                oEmpleadosCLS.amaterno = oEmpleado.APMATERNO;
                oEmpleadosCLS.fechaContrato = (DateTime)oEmpleado.FECHACONTRATO;
                oEmpleadosCLS.sueldo = (decimal)oEmpleado.SUELDO;
                oEmpleadosCLS.idtipoUsuario = (int)oEmpleado.IIDTIPOUSUARIO;
                oEmpleadosCLS.idtipoContrato = (int)oEmpleado.IIDTIPOCONTRATO;
                oEmpleadosCLS.idsexo = (int)oEmpleado.IIDSEXO;
            }
                return View(oEmpleadosCLS);
        }
    }
}