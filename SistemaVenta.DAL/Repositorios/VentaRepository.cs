using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _context;

        public VentaRepository(DbventaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = _context.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _context.Productos.Update(producto_encontrado);
                    }
                    await _context.SaveChangesAsync();

                    NumeroDocumento correlativo = _context.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;
                    _context.NumeroDocumentos.Update(correlativo);
                    await _context.SaveChangesAsync();

                    int cantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", cantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantidadDigitos, cantidadDigitos);
                    modelo.NumeroDocumento = numeroVenta;
                    await _context.Venta.AddAsync(modelo);
                    await _context.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                return ventaGenerada;
            }
        }
    }
}
