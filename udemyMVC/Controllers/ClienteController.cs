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
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index(ClienteCLS oClienteCLS)
        {
            int idSex = oClienteCLS.idsexo;
            List<ClienteCLS> listaCliente = null;
            using (var bd = new BDPasajeEntities())
            {
                if (oClienteCLS.idsexo == 0)
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    select new ClienteCLS
                                    {
                                        idcliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        apePaterno = cliente.APPATERNO,
                                        apeMaterno = cliente.APMATERNO,
                                        direccion = cliente.DIRECCION,
                                        email = cliente.EMAIL,
                                        telefono = cliente.TELEFONOFIJO


                                    }
                                ).ToList();
                }
                else
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1 && cliente.IIDSEXO==idSex
                                    select new ClienteCLS
                                    {
                                        idcliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        apePaterno = cliente.APPATERNO,
                                        apeMaterno = cliente.APMATERNO,
                                        direccion = cliente.DIRECCION,
                                        email = cliente.EMAIL,
                                        telefono = cliente.TELEFONOFIJO


                                    }
                                ).ToList();
                }
                
            }
            llenarSexo();
            ViewBag.lista = listaSexo;
            return View(listaCliente);
        }

        List<SelectListItem> listaSexo;
        private void llenarSexo()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaSexo = (from sexo in bd.Sexo
                             where sexo.BHABILITADO == 1
                             select new SelectListItem
                             {
                                 Text = sexo.NOMBRE,
                                 Value = sexo.IIDSEXO.ToString()
                             }).ToList();
                listaSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
        }
        public ActionResult Agregar()
        {
            llenarSexo();
            ViewBag.lista = listaSexo;
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClienteCLS)
        {
            if (!ModelState.IsValid)
            {
                llenarSexo();
                ViewBag.lista = listaSexo;
                return View(oClienteCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = new Cliente();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.apePaterno;
                oCliente.APMATERNO = oClienteCLS.apeMaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.TELEFONOFIJO = oClienteCLS.telefono;
                oCliente.IIDSEXO = oClienteCLS.idsexo;
                oCliente.BHABILITADO = 1;
                bd.Cliente.Add(oCliente);
                bd.SaveChanges();


            }
                return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            ClienteCLS oClienteCLS = new ClienteCLS();
            using (var bd = new BDPasajeEntities())
            {
                llenarSexo();
                ViewBag.lista = listaSexo;
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(id)).First();
                oClienteCLS.idcliente = oCliente.IIDCLIENTE;
                oClienteCLS.nombre = oCliente.NOMBRE;
                oClienteCLS.apePaterno = oCliente.APPATERNO;
                oClienteCLS.apeMaterno = oCliente.APMATERNO;
                oClienteCLS.direccion = oCliente.DIRECCION;
                oClienteCLS.email = oCliente.EMAIL;
                oClienteCLS.idsexo = (int)oCliente.IIDSEXO;
                oClienteCLS.telefono = oCliente.TELEFONOCELULAR;

               

            }
            return View(oClienteCLS);
        }

        [HttpPost]
        public ActionResult Editar(ClienteCLS oClienteCLS)
        {
            int idCliente = oClienteCLS.idcliente;
            if (!ModelState.IsValid) { return View(oClienteCLS); }
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(idCliente)).First();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.apePaterno;
                oCliente.APMATERNO = oClienteCLS.apeMaterno;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefono;
                bd.SaveChanges();
            }
            return RedirectToAction("Index"); 
        }

        public ActionResult Eliminar(int idCliente)
        {
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(idCliente)).First();
                oCliente.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}