using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class ViajeCLS
    {
        [Display(Name = "Id Viaje")]
        [Required]
        public int iidViaje { get; set; }
        [Display(Name = "Lugar de origen")]
        [Required]
        public int idOrigen { get; set; }
        [Display(Name = "Lugar de destino")]
        [Required]
        public int idDestino { get; set; }
        [Display(Name = "Precio")]
        [Required]
        [Range(0,1000000,ErrorMessage ="Fuera de rango")]
        public decimal precio { get; set; }

        [Display(Name = "Fecha Viaje")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaViaje { get; set; }
        [Display(Name = "Bus")]
        [Required]
        public int idBus { get; set; }
        [Display(Name = "Numero")]
        [Required]
        public int numeroAsientos { get; set; }
        public string nombreLugarOrigen { get; set; }
        public string nombreLugarDestino { get; set; }
        public string nombreBus { get; set; }
        public string nombreFoto { get; set; }
        public string mensaje { get; set; }
        public string fechaViajeCadena { get; set; }
        public string extension { get; set; }
        public string fotoCadena { get; set; }
    }
}