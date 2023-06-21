using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaMVVM.Models
{
    public class Contacto
    {
        public string Nombre { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string? Telefono { get; set;}
        public string Email { get; set; } = "";
        public DateTime FechaNacimiento { get; set; } = DateTime.Now.Date;
    }
}
