using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace GestionTasks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private DbConnection connection;

        public TareaController(IConfiguration config)
        {
            var connString = config.GetConnectionString("DefaultConnection");
            connection = new SqlConnection(connString);
            connection.Open();
        }

        // GET: api/Tarea
        [HttpGet]
        public IEnumerable<Modelo.Software.Tarea> Get()
        {
            var tareas = connection.Query<Modelo.Software.Tarea>("SELECT * FROM Tarea").ToList();            
            

            var proyectos = connection.Query<Modelo.Software.Proyecto>(
                "SELECT * FROM Proyecto").ToList();

            var usuarios = connection.Query<Modelo.Software.Usuario>(
                "SELECT * FROM Usuario").ToList();

            foreach (var tarea in tareas)
            {
                tarea.Proyecto = proyectos.FirstOrDefault(p => p.Id == tarea.ProyectoId);
                tarea.Usuario = usuarios.FirstOrDefault(a => a.Id == tarea.UsuarioId);
            }

            return tareas;
        }

        // GET api/Tarea/5
        [HttpGet("{id}")]
        public Modelo.Software.Tarea Get(int id)
        {
            var tarea = connection.QuerySingle<Modelo.Software.Tarea>("SELECT * FROM Tarea WHERE Id = @Id", new { Id = id });

            var proyecto = connection.QuerySingleOrDefault<Modelo.Software.Proyecto>(
                "SELECT * FROM Proyecto WHERE Id = @Id", new { Id = tarea.ProyectoId });

            var usuario = connection.QuerySingleOrDefault<Modelo.Software.Usuario>(
                "SELECT * FROM Usuario WHERE Id = @Id", new { Id = tarea.UsuarioId });

            tarea.Proyecto = proyecto;
            tarea.Usuario = usuario;

            return tarea;
        }

        [HttpGet("proyecto/{id}")]
        public Modelo.Software.Proyecto GetTareasByProyecto(int id)
        {
            var proyecto = connection.QuerySingle<Modelo.Software.Proyecto>(
                "SELECT * FROM Proyecto WHERE Id = @Id",
                new { Id = id });

            var tareas = connection.Query<Modelo.Software.Tarea>(
                "SELECT * FROM Tarea WHERE ProyectoId = @Id",
                new { Id = id }).ToList();

            proyecto.Tareas = tareas;

            return proyecto;
        }

        // POST api/Tarea
        [HttpPost]
        public Modelo.Software.Tarea Post([FromBody] Modelo.Software.Tarea tarea)
        {
            // Asignar el estado 'Pendiente' antes de insertar
            tarea.Estado = "Pendiente";

            connection.Execute("INSERT INTO Tarea (Nombre, Descripcion, Estado, prioridad, ProyectoId, UsuarioId, FechaCreacion, FechaModificacion) VALUES (@Nombre, @Descripcion, @Estado, @prioridad, @ProyectoId, @UsuarioId, @FechaCreacion, @FechaModificacion)", tarea);
            return tarea;
        }


        // PUT api/Tarea/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Modelo.Software.Tarea tarea)
        {
            connection.Execute("UPDATE Tarea SET Nombre = @Nombre, Descripcion = @Descripcion, Estado = @Estado, prioridad = @prioridad, ProyectoId = @ProyectoId, UsuarioId = @UsuarioId WHERE Id = @Id",
                new { tarea.Nombre, tarea.Descripcion, tarea.Estado, tarea.prioridad, tarea.ProyectoId, tarea.UsuarioId, id });
        }

        // DELETE api/Tarea/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            connection.Execute("DELETE FROM Tarea WHERE Id = @Id", new { Id = id });
        }
    }

}
