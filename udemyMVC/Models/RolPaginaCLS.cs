using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class RolPaginaCLS
    {
        public int idRolPagina { get; set; }
        [Required]
        public int idRol { get; set; }
        [Required]
        public int idPagina { get; set; }
        public int bhabilitado { get; set; }
        [Display(Name ="Nombre Rol")]
        public string nomRol { get; set; }
        [Display(Name = "Nombre Mensaje")]
        public string nomMensaje { get; set; }
    }
}