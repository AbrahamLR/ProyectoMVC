using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class PaginaCLS
    {
        [Display(Name ="Id")]
        public int idPagina { get; set; }
        [Required]
        [Display(Name = "Titulo del link")]
        public string mensaje { get; set; }
        [Required]
        [Display(Name = "Nombre accion")]
        public string accion { get; set; }
        [Required]
        [Display(Name = "Nombre controlador")]
        public string controlador { get; set; }
        [Required]
        public int bhabilitado { get; set; }
        public string mensajeText { get; set; }

    }
}