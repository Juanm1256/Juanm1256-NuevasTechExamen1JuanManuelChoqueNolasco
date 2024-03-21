using Azure.Data.Tables;
using Examen.Ingreso.Contratos.Repositorios;
using Examen.Ingreso.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Ingreso.Implentacion.Repositorio
{
    public class ProductoRepo : IProductoRepo
    {
        private readonly string? cadenaconexion;
        private readonly string? tablanombre;
        private readonly IConfiguration configuration;

        public ProductoRepo(IConfiguration conf)
        {
            configuration = conf;
            cadenaconexion = configuration.GetSection("cadenaconexion").Value;
            tablanombre = "Producto";
        }
        public async Task<bool> Eliminar(string partitionkey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaconexion, tablanombre);
                await tablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Producto> get(string id)
        {
            var tablaCliente = new TableClient(cadenaconexion, tablanombre);
            var filtro = $"PartitionKey eq 'Educacion' and RowKey eq '{id}'";
            await foreach (Producto producto in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                return producto;
            }
            return null;
        }

        public async Task<List<Producto>> getall()
        {
            List<Producto> lista = new List<Producto>();
            var tablaCliente = new TableClient(cadenaconexion, tablanombre);
            var filtro = $"PartitionKey eq 'Educacion'";
            await foreach (Producto producto in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                lista.Add(producto);
            }
            return lista;
        }

        public async Task<bool> Insertar(Producto producto)
        {
            try
            {
                var tablacliente = new TableClient(cadenaconexion, tablanombre);
                await tablacliente.UpsertEntityAsync(producto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaconexion, tablanombre);
                await tablaCliente.UpdateEntityAsync(producto, producto.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
