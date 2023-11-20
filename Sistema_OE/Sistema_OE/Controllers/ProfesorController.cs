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
    public class ProfesorController : Controller
    {
        #region Variables Globales
        private const int RegistrosPorPagina = 8; // Define la constante para la cantidad de registros por página

        #endregion

        #region BD
        private readonly SistemaOeContext _dbContext;

        public ProfesorController(SistemaOeContext context)
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

                var profesores = await _dbContext.Profesors
                    .FromSqlRaw("EXEC paMostrarProfesor @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual),
                        new SqlParameter("@CantReg", RegistrosPorPagina), // Cantidad de registros por página
                        new SqlParameter("@palabraBusc", ""),
                        totalPag)
                    .ToListAsync();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var profesPaged = new StaticPagedList<Profesor>(profesores, PagActual + 1, 10, totalPages * 10);

                return View(profesPaged);
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

                var profesores = _dbContext.Profesors
                    .FromSqlRaw("EXEC paMostrarProfesor @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual), // Ajustar el índice de página
                        new SqlParameter("@CantReg", RegistrosPorPagina),
                        new SqlParameter("@palabraBusc", searchTerm),
                        totalPag)
                    .ToList();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var profesPaged = new StaticPagedList<Profesor>(profesores, PagActual + 1, 10, totalPages * 10);


                return Json(new { Profesores = profesPaged, PagActual = PagActual + 1, TotalPaginas = totalPages });
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
        public JsonResult CrearProfesor([FromBody] Profesor profesor)
        {
            try
            {
                SqlParameter msjParam = new SqlParameter("@msj", System.Data.SqlDbType.VarChar, 40);
                msjParam.Direction = System.Data.ParameterDirection.Output;

                SqlParameter seAgregoParam = new SqlParameter("@seAgrego", System.Data.SqlDbType.TinyInt);
                seAgregoParam.Direction = System.Data.ParameterDirection.Output;

                _dbContext.Database.ExecuteSqlRaw("EXEC paCrearProfesor @cedula, @nombre, @apellidos, @email, @domicilio, @msj OUTPUT, @seAgrego OUTPUT",
                    new SqlParameter("@cedula", profesor.Cedula),
                    new SqlParameter("@nombre", profesor.Nombre),
                    new SqlParameter("@apellidos", profesor.Apellidos),
                    new SqlParameter("@email", profesor.Email),
                    new SqlParameter("@domicilio", profesor.Domicilio),
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

        #region Obtener PROFESOR
        public JsonResult GetProfesor(int id, bool verId)
        {
            try
            {
                var profesores = _dbContext.Profesors
                    .FromSqlRaw("EXEC paBuscarUnPro @p0, @p1", id, verId)
                    .ToList(); // Ejecuta la consulta y obtiene los resultados

                if (profesores.Count > 0)
                {
                    return Json(profesores.First()); // Devuelve el primer administrador encontrado
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
        public JsonResult ActualizarProfesor(int id, [FromBody] Profesor profesor)
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

                _dbContext.Database.ExecuteSqlRaw("EXEC paActualizarProfesor @cedula, @nombre, @apellidos, @email, @domicilio, @msj output, @seModifico output",
                    new SqlParameter("@cedula", id),
                    new SqlParameter("@nombre", profesor.Nombre),
                    new SqlParameter("@apellidos", profesor.Apellidos),
                    new SqlParameter("@email", profesor.Email),
                    new SqlParameter("@domicilio", profesor.Domicilio),
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
        public JsonResult EliminarProfesor(int id)
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

                _dbContext.Database.ExecuteSqlRaw("EXEC paEliminarProfesor @cedula, @msj output, @seElimino output",
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

        #region getSeccion
        public JsonResult GetSecciones()
        {
            try
            {
                var secciones = _dbContext.Set<Seccion>()
             .FromSqlRaw("SELECT * FROM  fnListaSeccion()")
             .ToList();

                return Json(secciones);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { error = ex.Message });
            }
        }
        #endregion 

        #region asignar seccion
        [HttpPost]
        public async Task<IActionResult> AsignarSeccion([FromBody] Seccionasignadum seccionAsignada)
        {
            try
            {
                var profesorCedulaParam = new SqlParameter("@profesorCedula", seccionAsignada.ProfesorCedula);
                var numSeccionParam = new SqlParameter("@numSeccion", seccionAsignada.NumSeccion);

                await _dbContext.Database.ExecuteSqlRawAsync("EXEC sp_AsignarSeccion @profesorCedula, @numSeccion", profesorCedulaParam, numSeccionParam);

                return Json(new { success = true, message = "Sección asignada con éxito." });
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar el error. Por ejemplo, podrías registrar el error en tus logs.
                Console.WriteLine(ex.Message);

                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion 

        #region getMateria
        public JsonResult GetMaterias()
        {
            try
            {
                var materias = _dbContext.Set<Materium>()
             .FromSqlRaw("SELECT * FROM  fnListaMateria()")
             .ToList();

                return Json(materias);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { error = ex.Message });
            }
        }
        #endregion 

        #region asignar materia
        [HttpPost]
        public async Task<IActionResult> AsignarMateria([FromBody] Impartematerium materiaAsignada)
        {
            try
            {
                var profesorCedulaParam = new SqlParameter("@profesorCedula", materiaAsignada.ProfesorCedula);
                var numMateriaParam = new SqlParameter("@numMateria", materiaAsignada.NumMateria);

                await _dbContext.Database.ExecuteSqlRawAsync("EXEC sp_AsignarMateria @profesorCedula, @numMateria", profesorCedulaParam, numMateriaParam);

                return Json(new { success = true, message = "Materia asignada con éxito." });
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar el error. Por ejemplo, podrías registrar el error en tus logs.
                Console.WriteLine(ex.Message);

                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion 


    }
}
