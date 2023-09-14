using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Sistema_OE.Models;

namespace Sistema_OE.Controllers
{
    public class AdministradorController : Controller
    {
        #region Variables Globales
        private const int RegistrosPorPagina = 8; // Define la constante para la cantidad de registros por página

        #endregion

        #region BD
        private readonly SistemaOeContext _dbContext;

        public AdministradorController(SistemaOeContext context)
        {
            _dbContext = context;
        }
        #endregion

        #region  Mostrar, Index
        // GET: Administrativoes
        public async Task<IActionResult> Index(int PagActual = 0)
        {
            try
            {
                SqlParameter totalPag = new SqlParameter("@TotalPag", System.Data.SqlDbType.Int);
                totalPag.Direction = System.Data.ParameterDirection.Output;

                var administrativos = await _dbContext.Administrativos
                    .FromSqlRaw("EXEC paMostrarAdministrador @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual),
                        new SqlParameter("@CantReg", RegistrosPorPagina), // Cantidad de registros por página
                        new SqlParameter("@palabraBusc", ""),
                        totalPag)
                    .ToListAsync();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var administrativosPaged = new StaticPagedList<Administrativo>(administrativos, PagActual + 1, 10, totalPages * 10);

                return View(administrativosPaged);
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
                //var pageSize = 1; // Cambia esto según tus necesidades
                //var pageNumber = page ?? 1;

                SqlParameter totalPag = new SqlParameter("@TotalPag", System.Data.SqlDbType.Int);
                totalPag.Direction = System.Data.ParameterDirection.Output;

                var administrativos = _dbContext.Administrativos
                    .FromSqlRaw("EXEC paMostrarAdministrador @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@NPag", PagActual), // Ajustar el índice de página
                        new SqlParameter("@CantReg", RegistrosPorPagina),
                        new SqlParameter("@palabraBusc", searchTerm),
                        totalPag)
                    .ToList();

                int totalPages = Convert.ToInt32(totalPag.Value);

                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = totalPages; // Total de veces que puede tocar el botón
                // Crear un objeto IPagedList<Administrativo> para la vista
                var administrativosPaged = new StaticPagedList<Administrativo>(administrativos, PagActual + 1, 10, totalPages * 10);


                return Json(new { Administrativos = administrativosPaged, PagActual = PagActual + 1, TotalPaginas = totalPages });
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
        public JsonResult CrearAdministrador([FromBody] Administrativo administrativo)
        {
            try
            {
                SqlParameter msjParam = new SqlParameter("@msj", System.Data.SqlDbType.VarChar, 40);
                msjParam.Direction = System.Data.ParameterDirection.Output;

                SqlParameter seAgregoParam = new SqlParameter("@seAgrego", System.Data.SqlDbType.TinyInt);
                seAgregoParam.Direction = System.Data.ParameterDirection.Output;

                _dbContext.Database.ExecuteSqlRaw("EXEC paCrearAdministador @nombre, @apellidos, @email, @domicilio, @password, @msj OUTPUT, @seAgrego OUTPUT",
                    new SqlParameter("@nombre", administrativo.Nombre),
                    new SqlParameter("@apellidos", administrativo.Apellidos),
                    new SqlParameter("@email", administrativo.Email),
                    new SqlParameter("@domicilio", administrativo.Domicilio),
                    new SqlParameter("@password", administrativo.Password),
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

        #region Obtener ADM
        public JsonResult GetAdministrador(int id, bool verId)
        {
            try
            {
                var administrador = _dbContext.Administrativos
                    .FromSqlRaw("EXEC paBuscarUnAmd @p0, @p1", id, verId)
                    .ToList(); // Ejecuta la consulta y obtiene los resultados

                if (administrador.Count > 0)
                {
                    return Json(administrador.First()); // Devuelve el primer administrador encontrado
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
        public JsonResult ActualizarAdministrador(int id, [FromBody] Administrativo administrativo)
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

                _dbContext.Database.ExecuteSqlRaw("EXEC paActualizarAdministador @idAdministrativo, @nombre, @apellidos, @email, @domicilio, @password, @msj output, @seModifico output",
                    new SqlParameter("@idAdministrativo", id),
                    new SqlParameter("@nombre", administrativo.Nombre),
                    new SqlParameter("@apellidos", administrativo.Apellidos),
                    new SqlParameter("@email", administrativo.Email),
                    new SqlParameter("@domicilio", administrativo.Domicilio),
                    new SqlParameter("@password", administrativo.Password),
                    outputMessage,
                    outputModified);

                string? v = outputMessage.Value.ToString();
                int seModifico = Convert.ToInt32(outputModified.Value);

                return Json(new { status = seModifico, mensaje = v });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = 0, mensaje = "Error en la actualización del administrativo" });
            }

        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult EliminarAdministrativo(int id)
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

                _dbContext.Database.ExecuteSqlRaw("EXEC paEliminarNomina @idAdministra, @msj output, @seElimino output",
                    new SqlParameter("@idAdministra", id),
                    outputMessage,
                    outputDeleted);

                string? v = outputMessage.Value.ToString();
                int seElimino = Convert.ToInt32(outputDeleted.Value);

                return Json(new { status = seElimino, mensaje = v });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = 0, mensaje = "Error al eliminar el administrativo" });
            }
        }
        #endregion  
    }
}
