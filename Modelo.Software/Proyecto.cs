using System.ComponentModel.DataAnnotations;

namespace Modelo.Software
{
    public class Proyecto
    {
        [Key]public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public List<Tarea>? Tareas { get; set; } 

    }
}
