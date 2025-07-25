using Client;
using GestionTareasWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modelo.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionTareasWeb.Controllers
{
    public class TareasController : Controller
    {
        private readonly IConfiguration _configuration;

        public TareasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var Tareas = Crud<Tarea>.GetAll();
            return View(Tareas); 
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tarea = Crud<Tarea>.GetById(id); // Obtener el usuario por ID
            if (tarea == null)
            {
                return NotFound();
            }
            return View(tarea);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea data)
        {
            try
            {
                Crud<Tarea>.Create(data);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Tarea>.GetById(id);
            return View(data);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tarea data)
        {
            if (id != data.Id)
            {
                return NotFound();
            }

            try
            {
                Crud<Tarea>.Update(id, data); // aquí se llama al método que hará el PUT
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        public ActionResult Delete(int id)
        {
            var data = Crud<Tarea>.GetById(id);
            return View();
        }

        // POST: PaisVController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tarea data)
        {
            try
            {
                Crud<Tarea>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Reporte(string estado = null, int? prioridad = null, DateTime? fechaVencimiento = null, string ordenarPor = "FechaCreacion", bool ascendente = true)
        {
            // Crear la consulta base
            var query = "SELECT * FROM Tarea WHERE 1=1";

            // Agregar filtros según los parámetros proporcionados
            if (!string.IsNullOrEmpty(estado))
            {
                query += " AND Estado = @Estado";
            }
            if (prioridad.HasValue)
            {
                query += " AND Prioridad = @Prioridad";
            }
            if (fechaVencimiento.HasValue)
            {
                query += " AND FechaVencimiento = @FechaVencimiento";
            }

            // Ordenar por el campo seleccionado
            query += $" ORDER BY {ordenarPor} {(ascendente ? "ASC" : "DESC")}";

            // Ejecutar la consulta
            var tareas = _connection.Query<Modelo.Software.Tarea>(query, new { Estado = estado, Prioridad = prioridad, FechaVencimiento = fechaVencimiento }).ToList();

            return View(tareas);
        }

        osea un controlador simple para una vista que va a mostrar tres tablas una conn con cada una de las prioridades

    }
}
