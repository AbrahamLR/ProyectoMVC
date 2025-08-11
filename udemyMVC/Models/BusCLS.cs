using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace udemyMVC.Models
{
    public class BusCLS
    {
        [Display(Name ="idBus")]
        public int idBus { get; set; }
        [Display(Name = "Sucursal")]
        [Required]
        public int idSucursal { get; set; }
        [Display(Name = "Tipo Bus")]
        [Required]
        public int idTipoBuspublic{ get; set; }

        [Display(Name = "Fecha Compra")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]       
        public DateTime fechaCompra { get; set; }
        [Display(Name ="Placa")]
        public string placa { get; set; }

        [Display(Name = "Modelo")]
        [Required]
        public int idModelo { get; set; }
        [Display(Name = "Numero de filas")]
        [Required]
        public int numFilas { get; set; }
        [Display(Name = "Numero de columnas")]
        [Required]
        public int numColumnas { get; set; }
        public string descripcion { get; set; }
        public string observacion { get; set; }
        public int idMarca { get; set; }
        [Display(Name = "Modelo")]
        public string nombreModelo { get; set; }
        [Display(Name = "Bus")]
        public string nombreTipoBus { get; set; }
        [Display(Name = "Sucursal")]
        public string  nombreSucursal { get; set; }
    }
}