using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;
using udemyMVC.Filtros;

namespace udemyMVC.Controllers
{
    //[Acceder]
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            listaPersonas();
            listarRol();
            List<UsuarioCLS> listaUsrs = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                List<UsuarioCLS> listaUsuarioCLiente = (from usrs in bd.Usuario
                                                        join cliente in bd.Cliente
                                                        on usrs.IID equals cliente.IIDCLIENTE
                                                        join rol in bd.Rol
                                                        on usrs.IIDROL equals rol.IIDROL
                                                        where usrs.bhabilitado == 1 && usrs.TIPOUSUARIO == "C"
                                                        select new UsuarioCLS
                                                        {
                                                            idUsuario = usrs.IIDUSUARIO,
                                                            nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                            NomUsuario = usrs.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            nomtipoEmpleado = "Cliente"
                                                        }).ToList();
                List<UsuarioCLS> listaUsuarioEmpleado = (from usrs in bd.Usuario
                                                         join empleado in bd.Empleado
                                                         on usrs.IID equals empleado.IIDEMPLEADO
                                                         join rol in bd.Rol
                                                         on usrs.IIDROL equals rol.IIDROL
                                                         where usrs.bhabilitado == 1 && usrs.TIPOUSUARIO == "E"
                                                         select new UsuarioCLS
                                                         {
                                                             idUsuario = usrs.IIDUSUARIO,
                                                             nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                             NomUsuario = usrs.NOMBREUSUARIO,
                                                             nombreRol = rol.NOMBRE,
                                                             nomtipoEmpleado = "Empleado"
                                                         }).ToList();
                listaUsrs.AddRange(listaUsuarioCLiente);
                listaUsrs.AddRange(listaUsuarioEmpleado);
                listaUsrs = listaUsrs.OrderBy(p => p.idUsuario).ToList();
            }
            return View(listaUsrs);
        }

        public void listaPersonas()
        {
            List<SelectListItem> listaPersona = new List<SelectListItem>();
            using (var bd = new BDPasajeEntities())
            {
                List<SelectListItem> listaCliente = (from item in bd.Cliente
                                                     where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                     select new SelectListItem
                                                     {
                                                         Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (C) ",
                                                         Value = item.IIDCLIENTE.ToString()
                                                     }).ToList();

                List<SelectListItem> listaEmpleado = (from item in bd.Empleado
                                                      where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                      select new SelectListItem
                                                      {
                                                          Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (E) ",
                                                          Value = item.IIDEMPLEADO.ToString()
                                                      }).ToList();

                listaPersona.AddRange(listaCliente);
                listaPersona.AddRange(listaEmpleado);
                listaPersona = listaPersona.OrderBy(p => p.Text).ToList();
                listaPersona.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaPersona = listaPersona;
            }
        }

        public void listarRol()
        {
            List<SelectListItem> listaRol;
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from item in bd.Rol
                            where item.BHABILITADO == 1
                            select new SelectListItem
                            {
                                Text = item.NOMBRE,
                                Value = item.IIDROL.ToString()
                            }).ToList();


            }
            listaRol.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            ViewBag.listaRol = listaRol;
        }

        public string Guardar(UsuarioCLS oUsuarioCLS, int titulo)
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
                        int registro = 0;
                        using (var transa = new TransactionScope())
                        {
                            if (titulo == -1)
                            {
                                registro = bd.Usuario.Where(p => p.NOMBREUSUARIO == oUsuarioCLS.NomUsuario).Count();
                                if (registro >= 1)
                                {
                                    res = "-1";
                                }
                                else
                                {
                                    Usuario oUsuario = new Usuario();
                                    oUsuario.NOMBREUSUARIO = oUsuarioCLS.NomUsuario;
                                    SHA256Managed sha = new SHA256Managed();
                                    byte[] bytePass = Encoding.Default.GetBytes(oUsuarioCLS.contrasena);
                                    byte[] bytePassEncri = sha.ComputeHash(bytePass);
                                    string passEncry = BitConverter.ToString(bytePassEncri).Replace("-", "");
                                    oUsuario.CONTRA = passEncry;
                                    oUsuario.TIPOUSUARIO = oUsuarioCLS.nombrePersonaHD.Substring(oUsuarioCLS.nombrePersonaHD.Length - 2, 1);
                                    oUsuario.IID = oUsuarioCLS.iid;
                                    oUsuario.IIDROL = oUsuarioCLS.idRol;
                                    oUsuario.bhabilitado = 1;
                                    bd.Usuario.Add(oUsuario);
                                    if (oUsuario.TIPOUSUARIO.Equals("C"))
                                    {
                                        Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(oUsuarioCLS.iid)).First();
                                        oCliente.bTieneUsuario = 1;
                                    }
                                    else
                                    {
                                        Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(oUsuarioCLS.iid)).First();
                                        oEmpleado.bTieneUsuario = 1;
                                    }
                                    res = bd.SaveChanges().ToString();
                                    if (res == "0") { res = ""; }
                                    transa.Complete();
                                }
                                
                            }
                            else
                            {
                                registro = bd.Usuario.Where(p => p.NOMBREUSUARIO == oUsuarioCLS.NomUsuario && p.IIDUSUARIO!=titulo).Count();
                                if (registro >= 1)
                                {
                                    res = "-1";
                                }
                                else
                                {
                                    Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();
                                    oUsuario.IIDROL = oUsuarioCLS.idRol;
                                    oUsuario.NOMBREUSUARIO = oUsuarioCLS.NomUsuario;
                                    res = bd.SaveChanges().ToString();
                                    transa.Complete(); //no se ejecutan los cambios sin está linea Sesion 62
                                }
                               
                            }
                        }
                    }
                }
            }
            catch (Exception e) { res = ""; }
            return res;
        }

        public JsonResult recuperaInfo(int idUsuario)
        {
            UsuarioCLS oUsuarioCLS = new UsuarioCLS();
            int idUsu = oUsuarioCLS.idUsuario;
            using (var bd=new BDPasajeEntities())
            {
                Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == idUsuario).First();
                oUsuarioCLS.NomUsuario = oUsuario.NOMBREUSUARIO;                
                oUsuarioCLS.idRol =(int)oUsuario.IIDROL;
               

            }
            return Json(oUsuarioCLS, JsonRequestBehavior.AllowGet);
        }

        public int Eliminar(int idUsuario)
        {
            int res = 0;
            try
            {
                using (BDPasajeEntities bd = new BDPasajeEntities())
                {
                    Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == idUsuario).First();
                    oUsuario.bhabilitado = 0;
                    res = bd.SaveChanges();
                }
            }
            catch (Exception e) { res = 0; }
            return res;
        }


        public ActionResult Filtrar(UsuarioCLS oUsuarioCLS)
        {
            listaPersonas();
            listarRol();
            string nombre = oUsuarioCLS.nombrePersona;
            List<UsuarioCLS> listaUsrs = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (oUsuarioCLS.nombrePersona == null)
                {

                    List<UsuarioCLS> listaUsuarioCLiente = (from usrs in bd.Usuario
                                                            join cliente in bd.Cliente
                                                            on usrs.IID equals cliente.IIDCLIENTE
                                                            join rol in bd.Rol
                                                            on usrs.IIDROL equals rol.IIDROL
                                                            where usrs.bhabilitado == 1 && usrs.TIPOUSUARIO == "C"
                                                            select new UsuarioCLS
                                                            {
                                                                idUsuario = usrs.IIDUSUARIO,
                                                                nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                                NomUsuario = usrs.NOMBREUSUARIO,
                                                                nombreRol = rol.NOMBRE,
                                                                nomtipoEmpleado = "Cliente"
                                                            }).ToList();
                    List<UsuarioCLS> listaUsuarioEmpleado = (from usrs in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usrs.IID equals empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usrs.IIDROL equals rol.IIDROL
                                                             where usrs.bhabilitado == 1 && usrs.TIPOUSUARIO == "E"
                                                             select new UsuarioCLS
                                                             {
                                                                 idUsuario = usrs.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 NomUsuario = usrs.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nomtipoEmpleado = "Empleado"
                                                             }).ToList();
                    listaUsrs.AddRange(listaUsuarioCLiente);
                    listaUsrs.AddRange(listaUsuarioEmpleado);
                    listaUsrs = listaUsrs.OrderBy(p => p.idUsuario).ToList();
                }
                else
                {
                    List<UsuarioCLS> listaUsuarioCLiente = (from usrs in bd.Usuario
                                                            join cliente in bd.Cliente
                                                            on usrs.IID equals cliente.IIDCLIENTE
                                                            join rol in bd.Rol
                                                            on usrs.IIDROL equals rol.IIDROL
                                                            where usrs.bhabilitado == 1 && (cliente.NOMBRE.Contains(nombre)
                                                            || cliente.APMATERNO.Contains(nombre)
                                                            || cliente.APMATERNO.Contains(nombre)) && usrs.TIPOUSUARIO == "C"
                                                            select new UsuarioCLS
                                                            {
                                                                idUsuario = usrs.IIDUSUARIO,
                                                                nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                                NomUsuario = usrs.NOMBREUSUARIO,
                                                                nombreRol = rol.NOMBRE,
                                                                nomtipoEmpleado = "Cliente"
                                                            }).ToList();
                    List<UsuarioCLS> listaUsuarioEmpleado = (from usrs in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usrs.IID equals empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usrs.IIDROL equals rol.IIDROL
                                                             where usrs.bhabilitado == 1 && (empleado.NOMBRE.Contains(nombre)
                                                            || empleado.APMATERNO.Contains(nombre)
                                                            || empleado.APMATERNO.Contains(nombre)) && usrs.TIPOUSUARIO == "E"
                                                             select new UsuarioCLS
                                                             {
                                                                 idUsuario = usrs.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 NomUsuario = usrs.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nomtipoEmpleado = "Empleado"
                                                             }).ToList();
                    listaUsrs.AddRange(listaUsuarioCLiente);
                    listaUsrs.AddRange(listaUsuarioEmpleado);
                    listaUsrs = listaUsrs.OrderBy(p => p.idUsuario).ToList();
                }
            }
            return PartialView("_tableUsuario", listaUsrs);
        }
    }
}