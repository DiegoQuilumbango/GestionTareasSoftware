using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GestionTasks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private DbConnection connection;

        public ProyectoController(IConfiguration config)
        {
            var connString = config.GetConnectionString("DefaultConnection");
            connection = new SqlConnection(connString);
            connection.Open();
        }

        // GET: api/Proyecto
        [HttpGet]
        public IEnumerable<Modelo.Software.Proyecto> Get()
        {
            var proyectos = connection.Query<Modelo.Software.Proyecto>("SELECT * FROM Proyecto").ToList();

            var tareas = connection.Query<Modelo.Software.Tarea>("SELECT * FROM Tarea").ToList();

            foreach (var proyecto in proyectos)
            {
                proyecto.Tareas = tareas.Where(t => t.ProyectoId == proyecto.Id).ToList();
            }

            return proyectos;
        }
        // GET api/Proyecto/5
        [HttpGet("{id}")]
        public Modelo.Software.Proyecto Get(int id)
        {
            var proyecto = connection.QuerySingle<Modelo.Software.Proyecto>("SELECT * FROM Proyecto WHERE Id = @Id", new { Id = id });
            var tareas = connection.Query<Modelo.Software.Tarea>("SELECT * FROM Tarea").ToList();
            proyecto.Tareas = tareas.Where(t => t.ProyectoId == proyecto.Id).ToList();
            return proyecto;
        }        

        // POST api/Proyecto
        [HttpPost]
        public Modelo.Software.Proyecto Post([FromBody] Modelo.Software.Proyecto proyecto)
        {
            connection.Execute("INSERT INTO Proyecto (Nombre, Descripcion, FechaCreacion, FechaModificacion) VALUES (@Nombre, @Descripcion, @FechaCreacion, @FechaModificacion)", proyecto);
            return proyecto;
        }

        // PUT api/Proyecto/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Modelo.Software.Proyecto proyecto)
        {
            connection.Execute("UPDATE Proyecto SET Nombre = @Nombre, Descripcion = @Descripcion, FechaCreacion = @FechaCreacion, FechaModificacion = @FechaModificacion WHERE Id = @Id",
                new { proyecto.Nombre, proyecto.Descripcion, proyecto.FechaCreacion, proyecto.FechaModificacion, id });
        }

        // DELETE api/Proyecto/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            connection.Execute("DELETE FROM Proyecto WHERE Id = @Id", new { Id = id });
        }
    }

}
