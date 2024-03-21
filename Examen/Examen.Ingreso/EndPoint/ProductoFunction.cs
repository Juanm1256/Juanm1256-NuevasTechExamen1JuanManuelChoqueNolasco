using Examen.Ingreso.Contratos.Repositorios;
using Examen.Ingreso.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Examen.Ingreso.EndPoint
{
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProductoRepo repos;

        public ProductoFunction(ILogger<ProductoFunction> logger, IProductoRepo repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProducto")]
        [OpenApiOperation("Insertarspec", "InsertarProducto", Description = " Sirve para listar todas los productos")]
        [OpenApiRequestBody("application/json", typeof(Producto), Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "El Producto creado con su ID asignado.")]
        public async Task<HttpResponseData> Insertar([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var regis = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar una producto con todos los datos");
                regis.RowKey = Guid.NewGuid().ToString();
                regis.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Insertar(regis);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ListarProducto")]
        [OpenApiOperation("Listarspec", "ListarProducto", Description = " Sirve para listar todas los productos")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Producto>), Description = "Mostrar una lista de productos")]
        public async Task<HttpResponseData> ListarProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            try
            {
                var lista = repos.getall();
                var respuest = req.CreateResponse(HttpStatusCode.OK);
                await respuest.WriteAsJsonAsync(lista.Result);
                return respuest;

            }
            catch (Exception)
            {

                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        /*tarea para ahora*/
        [Function("EliminarProducto")]
        [OpenApiOperation("Eliminarprec", "EliminarProducto", Description = "Este endpoint nos sirve para eliminar")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Confirmación de eliminación exitosa")]
        public async Task<HttpResponseData> EliminarProducto([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, string partitionkey, string rowkey)
        {
            try
            {
                var lista = repos.Eliminar(partitionkey, rowkey);
                var respuest = req.CreateResponse(HttpStatusCode.OK);
                await respuest.WriteAsJsonAsync(lista);
                return respuest;

            }
            catch (Exception)
            {

                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ObtenerProducto")]
        [OpenApiOperation("Listarspec", "ObtenerProducto", Description = " Sirve para obtener todas los productos")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Producto>), Description = "Datos del Producto correspondiente al ID proporcionado.")]
        public async Task<HttpResponseData> ObtenerProducto([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string id)
        {
            try
            {
                var lista = repos.get(id);
                var respuest = req.CreateResponse(HttpStatusCode.OK);
                await respuest.WriteAsJsonAsync(lista.Result);
                return respuest;

            }
            catch (Exception)
            {

                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ModificarProducto")]
        [OpenApiOperation("Modificarspec", "ModificarProducto", Description = " Sirve para listar todas las actualizaciones")]
        [OpenApiRequestBody("application/json", typeof(Producto), Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Confirmación de actualización exitosa.")]
        public async Task<HttpResponseData> ModificarProducto([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
        {
            try
            {
                var registro = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar una producto con todos sus datos");
                bool sw = await repos.Update(registro);
                if (sw)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }

            }
            catch (Exception)
            {

                var respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}
