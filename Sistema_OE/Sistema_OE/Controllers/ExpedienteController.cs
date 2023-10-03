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
    public class ExpedienteController : Controller
    {
        #region Variables Globales
        private const int RegistrosPorPagina = 8; // Define la constante para la cantidad de registros por página

        #endregion

        #region BD
        private readonly SistemaOeContext _dbContext;

        public ExpedienteController(SistemaOeContext context)
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

                var expedientes = await _dbContext.Expedientes
                    .FromSqlRaw("EXEC paMostrarExpediente @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual),
                        new SqlParameter("@CantReg", RegistrosPorPagina), // Cantidad de registros por página
                        new SqlParameter("@palabraBusc", ""),
                        totalPag)
                    .ToListAsync();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var expPaged = new StaticPagedList<Expediente>(expedientes, PagActual + 1, 10, totalPages * 10);

                return View(expPaged);
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

                var expedientes = _dbContext.Expedientes
                    .FromSqlRaw("EXEC paMostrarExpediente @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual), // Ajustar el índice de página
                        new SqlParameter("@CantReg", RegistrosPorPagina),
                        new SqlParameter("@palabraBusc", searchTerm),
                        totalPag)
                    .ToList();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var expPaged = new StaticPagedList<Expediente>(expedientes, PagActual + 1, 10, totalPages * 10);


                return Json(new { Expedientes = expPaged, PagActual = PagActual + 1, TotalPaginas = totalPages });
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
        public JsonResult CrearExpediente([FromBody] Expediente expe)
        {
            try
            {
                SqlParameter msjParam = new SqlParameter("@msj", System.Data.SqlDbType.VarChar, 40);
                msjParam.Direction = System.Data.ParameterDirection.Output;

                SqlParameter seAgregoParam = new SqlParameter("@seAgrego", System.Data.SqlDbType.TinyInt);
                seAgregoParam.Direction = System.Data.ParameterDirection.Output;

                _dbContext.Database.ExecuteSqlRaw("EXEC paCrearExpediente @cedulaEst, @cedulaEn, @encargado, @emailEn, @obervaciones, @msj OUTPUT, @seAgrego OUTPUT",
                    new SqlParameter("@cedulaEst", expe.Estudiante),
                    new SqlParameter("@cedulaEn", expe.CedulaEncargado),
                    new SqlParameter("@encargado", expe.EncargadoLegal),
                    new SqlParameter("@emailEn", expe.EmailEncargado),
                    new SqlParameter("@obervaciones", expe.Observaciones),
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

        #region Obtener 
        public JsonResult GetExpediente(int id, bool verId)
        {
            try
            {
                var expedientes = _dbContext.Expedientes
                    .FromSqlRaw("EXEC paBuscarUnEx @p0, @p1", id, verId)
                    .ToList(); // Ejecuta la consulta y obtiene los resultados

                if (expedientes.Count > 0)
                {
                    return Json(expedientes.First()); // Devuelve el primer administrador encontrado
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
        public JsonResult ActualizarExpediente(int id, [FromBody] Expediente expe)
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

                _dbContext.Database.ExecuteSqlRaw("EXEC paActualizarExpediente @num, @encargado, @emailEn, @obervaciones, @msj output, @seModifico output",
                    new SqlParameter("@num", id),
                    new SqlParameter("@encargado", expe.EncargadoLegal),
                    new SqlParameter("@emailEn", expe.EmailEncargado),
                    new SqlParameter("@obervaciones", expe.Observaciones),
                    outputMessage,
                    outputModified);

                string? v = outputMessage.Value.ToString();
                int seModifico = Convert.ToInt32(outputModified.Value);

                return Json(new { status = seModifico, mensaje = v });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = 0, mensaje = "Error en la actualización del profesor" });
            }

        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult EliminarExpediente(int id)
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

                _dbContext.Database.ExecuteSqlRaw("EXEC paEliminarExpediente @num, @msj output, @seElimino output",
                    new SqlParameter("@num", id),
                    outputMessage,
                    outputDeleted);

                string? v = outputMessage.Value.ToString();
                int seElimino = Convert.ToInt32(outputDeleted.Value);

                return Json(new { status = seElimino, mensaje = v });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = 0, mensaje = "Error al eliminar el expediente" });
            }
        }
        #endregion

    }
}
