using Client;
using GestionTareasWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modelo.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private List<SelectListItem> GetProyectos()
        {
            var proyectos = Crud<Proyecto>.GetAll();
            return proyectos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private List<SelectListItem> GetUsuarios()
        {
            var usuarios = Crud<Usuario>.GetAll();
            return usuarios.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        // GET: Tareas/TareasPorPrioridad
        //GET: Tareas/TareasPorPrioridad
        public async Task<IActionResult> TareasPorPrioridad()
        {
            try
            {
                var prioridad1 = Crud<Tarea>.GetBy("prioridad", 1).ToList();  // Alta prioridad
                var prioridad2 = Crud<Tarea>.GetBy("prioridad", 2).ToList();  // Media prioridad
                var prioridad3 = Crud<Tarea>.GetBy("prioridad", 3).ToList();  // Baja prioridad

                if (!prioridad1.Any() && !prioridad2.Any() && !prioridad3.Any())
                {
                    ViewBag.Message = "No se encontraron tareas con las prioridades seleccionadas.";
                }

                ViewBag.TareasAlta = prioridad1;
                ViewBag.TareasMedia = prioridad2;
                ViewBag.TareasBaja = prioridad3;

                return View();
            }
            catch (Exception ex)
            {
                // Manejar excepciones y mostrar un mensaje adecuado
                ViewBag.ErrorMessage = "Ocurrió un error al intentar obtener las tareas: " + ex.Message;
                return View();
            }
        }

        // GET: Tareas/TareasPorEstado
        public async Task<IActionResult> TareasPorEstado()
        {
            try
            {
                string Estado1 = "Pendiente";
                string Estado2 = "En Progreso";
                string Estado3 = "Terminado";

                // Obtener tareas por cada estado
                var tareasPendiente = Crud<Tarea>.GetBy("Estado", Estado1).ToList();
                var tareasEnProgreso = Crud<Tarea>.GetBy("Estado", Estado2).ToList();
                var tareasTerminado = Crud<Tarea>.GetBy("Estado", Estado3).ToList();

                // Verificar si no se encontraron tareas
                if (!tareasPendiente.Any() && !tareasEnProgreso.Any() && !tareasTerminado.Any())
                {
                    ViewBag.Message = "No se encontraron tareas con los estados seleccionados.";
                }

                // Asignar las tareas a ViewBag
                ViewBag.TareasPendiente = tareasPendiente;
                ViewBag.TareasEnProgreso = tareasEnProgreso;
                ViewBag.TareasTerminado = tareasTerminado;

                return View();
            }
            catch (Exception ex)
            {
                // Manejar excepciones y mostrar un mensaje adecuado
                ViewBag.ErrorMessage = "Ocurrió un error al intentar obtener las tareas: " + ex.Message;
                return View();
            }
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
            ViewBag.Proyectos = GetProyectos();
            ViewBag.Usuarios = GetUsuarios();
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

        // GET: Tareas/BuscarProyecto
        public async Task<IActionResult> BuscarTareasPorProyecto(string nombreProyecto)
        {
            var tareas = Crud<Modelo.Software.Tarea>.GetBy("proyectoNombre", nombreProyecto);

            ViewBag.Tareas = tareas;

            return View("ResultadosTareas");
        }

    }
}
