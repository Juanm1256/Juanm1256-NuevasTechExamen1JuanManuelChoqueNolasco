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
    public class ProveedorRepo : IProveedorLogic
    {
        private readonly string? cadenaconexion;
        private readonly string? tablanombre;
        private readonly IConfiguration configuration;

        public ProveedorRepo(IConfiguration conf)
        {
            configuration = conf;
            cadenaconexion = configuration.GetSection("cadenaconexion").Value;
            tablanombre = "Proveedor";
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

        public async Task<Proveedor> get(string id)
        {
            var tablaCliente = new TableClient(cadenaconexion, tablanombre);
            var filtro = $"PartitionKey eq 'Educacion' and RowKey eq '{id}'";
            await foreach (Proveedor proveedor in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                return proveedor;
            }
            return null;
        }

        public async Task<List<Proveedor>> getall()
        {
            List<Proveedor> lista = new List<Proveedor>();
            var tablaCliente = new TableClient(cadenaconexion, tablanombre);
            var filtro = $"PartitionKey eq 'Educacion'";
            await foreach (Proveedor institucion in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                lista.Add(institucion);
            }
            return lista;
        }

        public async Task<bool> Insertar(Proveedor proveedor)
        {
            try
            {
                var tablacliente = new TableClient(cadenaconexion, tablanombre);
                await tablacliente.UpsertEntityAsync(proveedor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaconexion, tablanombre);
                await tablaCliente.UpdateEntityAsync(proveedor, proveedor.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
