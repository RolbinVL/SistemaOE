using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_OE.Models;
using X.PagedList;

namespace Sistema_OE.Controllers
{
    public class EquipointerdiciplinariosController : Controller
    {
        #region Variables Globales
        private const int RegistrosPorPagina = 8; // Define la constante para la cantidad de registros por página

        #endregion

        #region BD
        private readonly SistemaOeContext _dbContext;

        public EquipointerdiciplinariosController(SistemaOeContext context)
        {
            _dbContext = context;
        }
        #endregion

        #region  Mostrar, Index
        // GET
        public async Task<IActionResult> Index(int PagActual = 0)
        {
            try
            {
                SqlParameter totalPag = new SqlParameter("@TotalPag", System.Data.SqlDbType.Int);
                totalPag.Direction = System.Data.ParameterDirection.Output;

                var equipo = await _dbContext.Equipointerdiciplinarios
                    .FromSqlRaw("EXEC paMostrarEquipo @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual),
                        new SqlParameter("@CantReg", RegistrosPorPagina), // Cantidad de registros por página
                        new SqlParameter("@palabraBusc", ""),
                        totalPag)
                    .ToListAsync();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var equipoPaged = new StaticPagedList<Equipointerdiciplinario>(equipo, PagActual + 1, 10, totalPages * 10);

                return View(equipoPaged);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Problem("An error occurred while retrieving data.");
            }


        }
        #endregion

        #region Buscar
        public JsonResult Buscar(string searchTerm, int PagActual = 0)
        {
            try
            {

                SqlParameter totalPag = new SqlParameter("@TotalPag", System.Data.SqlDbType.Int);
                totalPag.Direction = System.Data.ParameterDirection.Output;

                var equipo = _dbContext.Equipointerdiciplinarios
                    .FromSqlRaw("EXEC paMostrarEquipo @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual), // Ajustar el índice de página
                        new SqlParameter("@CantReg", RegistrosPorPagina),
                        new SqlParameter("@palabraBusc", searchTerm),
                        totalPag)
                    .ToList();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var equipoPaged = new StaticPagedList<Equipointerdiciplinario>(equipo, PagActual + 1, 10, totalPages * 10);


                return Json(new { Equipointerdiciplinario = equipoPaged, PagActual = PagActual + 1, TotalPaginas = totalPages });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { Error = "An error occurred while retrieving data." });
            }
        }
        #endregion

        #region Crear
        [HttpPost]
        public JsonResult CrearEquipo([FromBody] Equipointerdiciplinario equipo)
        {
            try
            {
                SqlParameter msjParam = new SqlParameter("@msj", System.Data.SqlDbType.VarChar, 40);
                msjParam.Direction = System.Data.ParameterDirection.Output;

                SqlParameter seAgregoParam = new SqlParameter("@seAgrego", System.Data.SqlDbType.TinyInt);
                seAgregoParam.Direction = System.Data.ParameterDirection.Output;

                _dbContext.Database.ExecuteSqlRaw("EXEC paCrearEquipo @cedula, @nombre, @especialidad, @email, @domicilio, @msj OUTPUT, @seAgrego OUTPUT",
                    new SqlParameter("@cedula", equipo.Cedula),
                    new SqlParameter("@nombre", equipo.Nombre),
                    new SqlParameter("@especialidad", equipo.Especialidad),
                    new SqlParameter("@email", equipo.Email),
                    new SqlParameter("@domicilio", equipo.Domicilio),
                    msjParam,
                    seAgregoParam
                );

                string? v = msjParam.Value.ToString();
                byte seAgrego = Convert.ToByte(seAgregoParam.Value);

                return Json(new { status = seAgrego, mensaje = v });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { status = 0, mensaje = "Error en el servidor" });
            }
        }
        #endregion

        #region ObtenerEQUIPO
        public JsonResult GetEquipo(int id, bool verId)
        {
            try
            {
                var equipo = _dbContext.Equipointerdiciplinarios
                    .FromSqlRaw("EXEC paBuscarUnEq @p0, @p1", id, verId)
                    .ToList(); // Ejecuta la consulta y obtiene los resultados

                if (equipo.Count > 0)
                {
                    return Json(equipo.First()); // Devuelve el primer administrador encontrado
                }
                else
                {
                    return Json(null); // Devuelve null si no se encontró ningún administrador
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { error = ex.Message });
            }
        }
        #endregion

        #region Editar
        [HttpPost]
        public JsonResult ActualizarEquipo(int id, [FromBody] Equipointerdiciplinario equipo)
        {
            try
            {
                SqlParameter outputMessage = new SqlParameter("@msj", System.Data.SqlDbType.VarChar, 40)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                SqlParameter outputModified = new SqlParameter("@seModifico", System.Data.SqlDbType.TinyInt)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                _dbContext.Database.ExecuteSqlRaw("EXEC paActualizarEquipo @cedula, @nombre, @especialidad, @email, @domicilio, @msj output, @seModifico output",
                    new SqlParameter("@cedula", equipo.Cedula),
                    new SqlParameter("@nombre", equipo.Nombre),
                    new SqlParameter("@especialidad", equipo.Especialidad),
                    new SqlParameter("@email", equipo.Email),
                    new SqlParameter("@domicilio", equipo.Domicilio),
                    outputMessage,
                    outputModified);

                string? v = outputMessage.Value.ToString();
                int seModifico = Convert.ToInt32(outputModified.Value);

                return Json(new { status = seModifico, mensaje = v });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = 0, mensaje = "Error en la actualización Equipo" });
            }

        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult EliminarEquipo(int id)
        {
            try
            {
                SqlParameter outputMessage = new SqlParameter("@msj", System.Data.SqlDbType.VarChar, 40)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                SqlParameter outputDeleted = new SqlParameter("@seElimino", System.Data.SqlDbType.TinyInt)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                _dbContext.Database.ExecuteSqlRaw("EXEC paEliminarEquipo @cedula, @msj output, @seElimino output",
                    new SqlParameter("@cedula", id),
                    outputMessage,
                    outputDeleted);

                string? v = outputMessage.Value.ToString();
                int seElimino = Convert.ToInt32(outputDeleted.Value);

                return Json(new { status = seElimino, mensaje = v });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = 0, mensaje = "Error al eliminar el profesor" });
            }
        }
        #endregion  
    }
}
