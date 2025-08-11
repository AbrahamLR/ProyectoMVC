using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class TipoUsuarioCLS
    {
        [Display(Name ="Id tipo usuario")]
        public int idTipoUsuario { get; set; }
        [Display(Name = "Nombre tipo usuario")]
        [Required]
        [StringLength(150,ErrorMessage ="Longitud")]
        public string nombre { get; set; }
        [Display(Name = "Descripcion tipo usuario")]
        [Required]
        [StringLength(250, ErrorMessage = "Longitud")]
        public string descripcion { get; set; }
             
        public int bhabilitado { get; set; }
    }
}