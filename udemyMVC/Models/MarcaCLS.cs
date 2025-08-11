using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class MarcaCLS
    {
        //Modelo copia de las propiedades de una tabla
        [Display(Name = "Id Marca")] /*Display muestra el nombre del campo de manera personalizada*/
        public int idmarca { get; set; }

        [Display(Name = "Nombre Marca")]
        [Required]
        [StringLength(100,ErrorMessage ="No valido")]
        public string nombre { get; set; }

        [Display(Name = "Descipcion Marca")]
        [Required]
        [StringLength(200,ErrorMessage ="No valido")]
        public string descripcion { get; set; }
        public string bhabilitado { get; set; }
        public string mensajeError { get; set; }
    }
}