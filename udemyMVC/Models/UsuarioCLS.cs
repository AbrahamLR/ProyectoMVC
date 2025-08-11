using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class UsuarioCLS
    {
        public int idUsuario { get; set; }
        [Required]
        public string NomUsuario { get; set; }
        public string contrasena { get; set; }
        public int tipoUsuario { get; set; }
        [Required]
        public int iid { get; set; }
        [Required]
        public int idRol { get; set; }
        public string nombrePersona { get; set; }
        public string nombreRol { get; set; }
        public string nomtipoEmpleado { get; set; }
        public string nombrePersonaHD { get; set; }


    }
}