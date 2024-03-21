using Examen.Ingreso.Modelo;
using Examen.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Ingreso.Contratos.Repositorios
{
    public interface IProductoRepo
    {
        public Task<bool> Insertar(Producto producto);
        public Task<bool> Update(Producto producto);
        public Task<bool> Eliminar(string partitionkey, string rowkey);
        public Task<List<Producto>> getall();
        public Task<Producto> get(string id);
    }
}
