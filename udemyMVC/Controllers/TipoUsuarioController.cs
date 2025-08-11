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
    public class TipoUsuarioController : Controller
    {
        private TipoUsuarioCLS oTipoVal;
        // GET: TipoUsuario
        private bool buscarTipoUsuario(TipoUsuarioCLS oTipoUsu)
        {
            bool buscarId = true;
            bool buscarNom = true;
            bool buscarDesc = true;

            if (oTipoVal.idTipoUsuario > 0)
                buscarId = oTipoUsu.idTipoUsuario.ToString().Contains(oTipoVal.idTipoUsuario.ToString());
            if (oTipoVal.nombre != null)
               buscarNom= oTipoUsu.nombre.ToString().Contains(oTipoVal.nombre);
            if (oTipoVal.descripcion != null)
                buscarDesc = oTipoUsu.descripcion.ToString().Contains(oTipoVal.descripcion);

            return (buscarId && buscarNom && buscarDesc);
        }
        public ActionResult Index(TipoUsuarioCLS oTipoUusuarioCLS)
        {
            oTipoVal = oTipoUusuarioCLS;
            List<TipoUsuarioCLS> listaFiltro;
            List<TipoUsuarioCLS> listaTipoUsu = null;
            using (var bd = new BDPasajeEntities())
            {
                listaTipoUsu = (from tipoUsuario in bd.TipoUsuario
                                where tipoUsuario.BHABILITADO == 1
                                select new TipoUsuarioCLS
                                {
                                    idTipoUsuario = tipoUsuario.IIDTIPOUSUARIO,
                                    nombre = tipoUsuario.NOMBRE,
                                    descripcion = tipoUsuario.DESCRIPCION
                                }).ToList();
                if (oTipoUusuarioCLS.idTipoUsuario == 0 && oTipoUusuarioCLS.nombre == null && oTipoUusuarioCLS.descripcion == null)
                {
                    listaFiltro = listaTipoUsu;
                }
                else
                {
                    Predicate<TipoUsuarioCLS> pred = new Predicate<TipoUsuarioCLS>(buscarTipoUsuario);
                    listaFiltro = listaTipoUsu.FindAll(pred);
                     
                }
            }
                return View(listaFiltro);
        }

    }
}