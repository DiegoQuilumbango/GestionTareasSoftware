using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Software
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public string TipoUsuario { get; set; }
        public List<Tarea>? Tareas { get; set; }

    }
}
