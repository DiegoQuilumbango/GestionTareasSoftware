using Api.Cliente;
using Client;
using Escuela.Modelos;
using EscuelaWeb.Data;
using EscuelaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscuelaWeb.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;


        public EstudiantesController(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }
        // GET: Estudiantes
        //public IActionResult Index()
        //{
        //    var estudiantes = Crud<Estudiante>.GetAll(); // Obtener los estudiantes

        //    var listaCreditos = new Dictionary<int, int>(); // Diccionario para almacenar los créditos por estudiante

        //    foreach (var estudiante in estudiantes)
        //    {
        //        var creditos = GetCreditosTotalesPorEstudiante(estudiante.Id); // Obtener los créditos para cada estudiante
        //        listaCreditos[estudiante.Id] = creditos; // Guardar en el diccionario usando el ID como clave
        //    }

        //    ViewBag.Creditos = listaCreditos; // Asignar el diccionario a ViewBag
        //    return View(estudiantes); // Pasar los estudiantes a la vista
        //}

        public IActionResult Index()
        {
            var estudiantes = Crud<Estudiante>.GetAll(); // Obtener todos los estudiantes

            var estudiantesConCreditos = estudiantes.Select(estudiante => new EstudianteConCreditosViewModel
            {
                Estudiante = estudiante,
                Creditos = GetCreditosTotalesPorEstudiante(estudiante.Id) // Obtener los créditos para cada estudiante
            }).ToList();

            return View(estudiantesConCreditos); // Pasar los estudiantes con créditos a la vista
        }

        // GET: Estudiantes/Details/5
        public ActionResult Details(int id)
        {
            var estudiantes = Crud<Estudiante>.GetById(id);
            ViewBag.Creditos = GetCreditosTotalesPorEstudiante(id);
            return View(estudiantes);
        }

        private List<SelectListItem> GetPaises()
        {
            var paises = Crud<Pais>.GetAll();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private List<SelectListItem> GetColegio()
        {
            var paises = Crud<Colegio>.GetAll();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        //private int GetCreditosTotalesPorEstudiante(int estudianteId)
        //{
        //    var url = $"https://localhost:7138/api/Estudiantes/creditos/{estudianteId}";  // API Endpoint
        //    var response = _client.GetAsync(url).Result; // Realizar la petición

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = response.Content.ReadAsStringAsync().Result;

        //        // Utilizamos JObject para leer el valor "creditosTotales"
        //        var jsonResponse = JObject.Parse(json);
        //        var creditosTotales = jsonResponse["creditosTotales"].Value<int>();  // Accedemos al valor de "creditosTotales"

        //        return creditosTotales;
        //    }
        //    else
        //    {
        //        throw new Exception($"Error: {response.StatusCode}");
        //    }
        //}

        private int GetCreditosTotalesPorEstudiante(int estudianteId)
        {
            var url = $"https://localhost:7138/api/Estudiantes/creditos/{estudianteId}";  // API Endpoint
            var response = _client.GetAsync(url).Result; // Realizamos la petición a la API

            if (response.IsSuccessStatusCode)
            {
                // Leemos el valor entero directamente de la respuesta
                var json = response.Content.ReadAsStringAsync().Result;
                return int.Parse(json); // Convertimos la respuesta JSON en un entero
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
        // GET: Estudiantes/Create
        public ActionResult Create()
        {
            ViewBag.Paises = GetPaises();
            ViewBag.Colegio = GetColegio();
            return View();
        }

        // POST: PaisVController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estudiante data)
        {
            try
            {
                Crud<Estudiante>.Create(data);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaisVController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Estudiante>.GetById(id);
            ViewBag.Paises = GetPaises();
            ViewBag.Colegio = GetColegio();
            return View(data);
        }

        // POST: PaisVController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Estudiante data)
        {
            if (id != data.Id)
            {
                return NotFound();
            }

            try
            {
                Crud<Estudiante>.Update(id, data); // <-- aquí se llama al método que hará el PUT
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Paises = GetPaises();
                ViewBag.Colegio = GetColegio();
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: PaisVController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Estudiante>.GetById(id);
            return View();
        }

        // POST: PaisVController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Estudiante data)
        {
            try
            {
                Crud<Estudiante>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
