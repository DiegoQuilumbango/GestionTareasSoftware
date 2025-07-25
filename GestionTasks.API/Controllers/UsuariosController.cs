using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modelo.Software;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GestionTasks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private DbConnection connection;

        public UsuarioController(IConfiguration config)
        {
            var connString = config.GetConnectionString("DefaultConnection");
            connection = new SqlConnection(connString);
            connection.Open();
        }

        // GET: api/Usuario
        [HttpGet]
        public IEnumerable<Modelo.Software.Usuario> Get()
        {
            var usuarios = connection.Query<Modelo.Software.Usuario>("SELECT * FROM Usuario").ToList();
            var tareas = connection.Query<Modelo.Software.Tarea>("SELECT * FROM Tarea").ToList();

            foreach (var usuario in usuarios)
            {
                usuario.Tareas = tareas.Where(t => t.ProyectoId == usuario.Id).ToList();
            }
            return usuarios;
        }

        // GET api/Usuario/5
        [HttpGet("{id}")]
        public Modelo.Software.Usuario Get(int id)
        {
            var usuario = connection.QuerySingle<Modelo.Software.Usuario>("SELECT * FROM Usuario WHERE Id = @Id", new { Id = id });
            var tareas = connection.Query<Modelo.Software.Tarea>("SELECT * FROM Tarea").ToList();
            usuario.Tareas = tareas.Where(t => t.ProyectoId == usuario.Id).ToList();
            return usuario;
        }

        // POST api/Usuario
        [HttpPost]
        public Modelo.Software.Usuario Post([FromBody] Modelo.Software.Usuario usuario)
        {
            connection.Execute("INSERT INTO Usuario (Nombre, CorreoElectronico, TipoUsuario) VALUES (@Nombre, @CorreoElectronico, @TipoUsuario)", usuario);
            return usuario;
        }

        // PUT api/Usuario/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Modelo.Software.Usuario usuario)
        {
            connection.Execute("UPDATE Usuario SET Nombre = @Nombre, CorreoElectronico = @CorreoElectronico, TipoUsuario = @TipoUsuario WHERE Id = @Id",
                new { usuario.Nombre, usuario.CorreoElectronico, usuario.TipoUsuario, id });
        }

        // DELETE api/Usuario/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            connection.Execute("DELETE FROM Usuario WHERE Id = @Id", new { Id = id });
        }
    }

}
