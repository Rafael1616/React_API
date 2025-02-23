using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Datos;
using API.Migrations;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIREST.Repository
{
    public class ProductoDAO
    {
        private readonly AplicationDbContext _contexto;

        public ProductoDAO(AplicationDbContext context)
        {
            _contexto = context;
        }

        public async Task<List<Producto>> SelectAllAsync()
        {
            return await _contexto.Producto.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _contexto.Producto.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            try
            {
                var producto = await GetByIdAsync(id);
                if (producto == null)
                {
                    return false;
                }
                _contexto.Producto.Remove(producto);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Producto?> InsertProductoAsync(Producto producto)
        {
            try
            {
                await _contexto.Producto.AddAsync(producto);
                await _contexto.SaveChangesAsync();
                return producto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            try
            {
                if (producto == null || string.IsNullOrWhiteSpace(producto.Nombre))
                {
                    return false;
                }

                var productoExistente = await GetByIdAsync(producto.Id);

                if (productoExistente == null)
                {
                    return false;
                }

                productoExistente.Nombre = producto.Nombre;
                _contexto.Producto.Update(productoExistente);
                await _contexto.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
