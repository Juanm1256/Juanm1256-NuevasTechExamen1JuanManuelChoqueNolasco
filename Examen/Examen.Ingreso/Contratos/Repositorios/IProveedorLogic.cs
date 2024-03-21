using Examen.Ingreso.Modelo;
using Examen.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Ingreso.Contratos.Repositorios
{
    public interface IProveedorLogic
    {
        public Task<bool> Insertar(Proveedor proveedor);
        public Task<bool> Update(Proveedor proveedor);
        public Task<bool> Eliminar(string partitionkey, string rowkey);
        public Task<List<Proveedor>> getall();
        public Task<Proveedor> get(string id);
    }
}
