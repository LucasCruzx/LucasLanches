using LucasLanches.Context;
using LucasLanches.Models;
using LucasLanches.Repositories.Interfaces;

namespace LucasLanches.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}

