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
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly IProveedorLogic repos;

        public ProveedorFunction(ILogger<ProveedorFunction> logger, IProveedorLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProveedor")]
        [OpenApiOperation("Insertarspec", "InsertarProveedor", Description = " Sirve para listar todas las proveedores")]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "El Proveedor creado con su ID asignado.")]
        public async Task<HttpResponseData> Insertar([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var regis = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar una proveedor con todos los datos");
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

        [Function("ListarProveedor")]
        [OpenApiOperation("Listarprec", "ListarInstitucion", Description = "Este endpoint nos sirve para algo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Proveedor>), Description = "Mostrar un lista de instituciones")]
        public async Task<HttpResponseData> ListarProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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

        [Function("EliminarProveedor")]
        [OpenApiOperation("Eliminarprec", "EliminarProveedor", Description = "Este endpoint nos sirve para eliminar")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Confirmación de eliminación exitosa")]
        public async Task<HttpResponseData> EliminarProveedor([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, string partitionkey, string rowkey)
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

        [Function("ObtenerProveedor")]
        [OpenApiOperation("Obtenerprec", "ObtenerProveedor", Description = "Este endpoint nos sirve para obtener")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Datos del Proveedor correspondiente al ID proporcionado.")]
        public async Task<HttpResponseData> ObtenerProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string id)
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

        [Function("ModificarProveedor")]
        [OpenApiOperation("Modificarspec", "ModificarProveedor", Description = " Sirve para listar todas las actualizaciones")]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Confirmación de actualización exitosa.")]
        public async Task<HttpResponseData> ModificarProveedor([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
        {
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar una proveedor con todos sus datos");
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
