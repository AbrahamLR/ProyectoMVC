using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class ClienteCLS
    {
        [Display(Name = "Id Cliente")]
        public int idcliente { get; set; }

        [Display(Name = "Nombre Cliente")]
        [Required]
        public string nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required]
        public string apePaterno{ get; set; }

        [Display(Name = "Apellido Materno")]
        [Required]
        public string apeMaterno { get; set; }

        [Display(Name ="Email")]
        [Required]
        [EmailAddress(ErrorMessage ="Formato de email no valido")]
        public string email { get; set; }
        [DataType(DataType.MultilineText)]
        public string direccion { get; set; }
        public int idsexo { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage ="Formato no valido")]
        public string telefono { get; set; }
        public int bhabilitado { get; set; }

    }
}