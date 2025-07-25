using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Software
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } 
        public DateTime? FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; } = DateTime.Now;
        public string Estado { get; set; }
        public int prioridad { get; set; } // 1: Alta, 2: Media, 3: Baja
        public int ProyectoId { get; set; }
        public Proyecto? Proyecto { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } // Asignada a un usuario específico
    }
}
