using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemyMVC.Models
{
    public class ReservaCLS
    {
        public int idviaje { get; set; }
        public string nombreArchivo { get; set; }
        public byte[] foto { get; set; }
        public string lugarOrigen { get; set; }
        public string lugarDestino { get; set; }
        public string nombreBus { get; set; }
        public int asientos { get; set; }

         
    }
}