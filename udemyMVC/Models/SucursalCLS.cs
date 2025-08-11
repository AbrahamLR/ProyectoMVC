using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace udemyMVC.Models
{
    public class SucursalCLS
    {
        [Display(Name = "Id Sucursal")]
        public int idSucursal { get; set; }

        [Display(Name = "Nombre Sucursal")]
        [Required]
        public string  nombre { get; set; }

        [Display(Name = "Direccion de la Sucursal")]
        [Required]
        public string direccion { get; set; }

        [Display(Name = "Telefono Sucursal")]
        [Required]
        public string telefono { get; set; }

        [Display(Name = "Email Sucursal")]
        [EmailAddress(ErrorMessage ="No es un email valido")]
        public string email { get; set; }

        [Display(Name ="Fecha Apertura")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true )]
        [Required]
        public DateTime fechaApertura { get; set; }
        public int bhabilitado { get; set; }
    }
}