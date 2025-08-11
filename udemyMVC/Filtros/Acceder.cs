using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;

namespace udemyMVC.Filtros
{
    public class Acceder:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Si session usuario es null regresa a login
            var usuario = HttpContext.Current.Session["Usuario"];
            var rol =(List<MenuCLS>) HttpContext.Current.Session["Rol"];
            string nomControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string nomAccion = filterContext.ActionDescriptor.ActionName;
            int cantidad = rol.Where(p => p.nomControlado == nomControlador).Count();
            if (usuario == null||cantidad==0)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}