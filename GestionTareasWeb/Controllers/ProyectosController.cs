using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionTareasWeb.Data;
using Modelo.Software;
using Client;

namespace GestionTareasWeb.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProyectosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: Proyectos
        public async Task<IActionResult> Index()
        {
            var proyectos = Crud<Proyecto>.GetAll();
            return View(proyectos);
        }

        // GET: Proyectos/Details/5
        public IActionResult Details(int id)
        {
            var proyecto = Crud<Proyecto>.GetById(id);
            if (proyecto == null)
            {
                return NotFound();
            }
            ViewBag.Tareas = GetTareas();

            return View(proyecto);
        }

        private List<Tarea> GetTareas()
        {
            // Obtener todas las tareas sincrónicamente
            var tareas = Crud<Tarea>.GetAll();
            return tareas;
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
        public ActionResult Create(Proyecto data)
        {
            try
            {
                Crud<Proyecto>.Create(data);
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
            var data = Crud<Proyecto>.GetById(id);
            return View(data);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Proyecto data)
        {
            if (id != data.Id)
            {
                return NotFound();
            }

            try
            {
                Crud<Proyecto>.Update(id, data); // aquí se llama al método que hará el PUT
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
            var data = Crud<Proyecto>.GetById(id);
            return View();
        }

        // POST: PaisVController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Proyecto data)
        {
            try
            {
                Crud<Proyecto>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
