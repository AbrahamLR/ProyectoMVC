using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class EmpleadoCLS
    {
        [Display(Name ="Id Empleado")]
        
        public int idempleado { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required]
        public string apaterno { get; set; }
        [Display(Name = "Apellido Materno")]
        [Required]
        public string amaterno { get; set; }
        [Display(Name = "Fecha Contrato")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaContrato { get; set; }
        [Display(Name = "Tipo Usuario")]
        [Required]
        public int idtipoUsuario { get; set; }

        [Display(Name = "Sueldo")]
        [Range(0,100000,ErrorMessage ="Fuera de rango")]
        [Required]
        public decimal sueldo { get; set; }
        [Display(Name = "Tipo Contrato")]
        [Required]
        public int idtipoContrato { get; set; }
        [Display(Name = "Sexo")]
        [Required]
        public int idsexo { get; set; }
        public int bhabilitado { get; set; }

        [Display(Name ="Tipo Contrato")]
        public string nombreTipoContrato { get; set; }
        [Display(Name = "Tipo Usuario")]
        public string nombreTipoUsuario { get; set; }

       
    }
}