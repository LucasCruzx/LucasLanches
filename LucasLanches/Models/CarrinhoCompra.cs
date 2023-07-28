using LucasLanches.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace LucasLanches.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }

        public string CarrinhoDeCompraId { get; set; }

        public List<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            //Define uma sessão
            ISession session =
                services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //Obtem um serviço do tipo do nosso contexto
            var context = services.GetService<AppDbContext>();

            //Obtem ou gera um Id do carrinho
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //Atribui o Id do carrinho na sessão
            session.SetString("CarrinhoId", carrinhoId);

            //Retorna o carrinho com contexto e o Id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoDeCompraId = carrinhoId
            };

        }
        public void AdicionarAoCarrinho(Lanche lanche)

        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoDeCompraId);

            if(carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = CarrinhoDeCompraId,
                    Lanche = lanche,
                    Quantidade = 1
                };

                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
            }
            else
            {
                carrinhoCompraItem.Quantidade++;
            };
            _context.SaveChanges();
        }
        public void RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoDeCompraId);

            if (carrinhoCompraItem != null)
            {
                if (carrinhoCompraItem.Quantidade > 1)
                {
                    carrinhoCompraItem.Quantidade --;

                }
                else
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }
                _context.SaveChanges();
              
            }
        }
        public List<CarrinhoCompraItem> GetCarrinhoCompraItems()
        {
            return CarrinhoCompraItems ?? (CarrinhoCompraItems =
                                          _context.CarrinhoCompraItens
                                          .Where(c => c.CarrinhoCompraId == CarrinhoDeCompraId)
                                          .Include(s => s.Lanche)
                                          .ToList());
                                   
        }
        public void LimparCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                                .Where(carrinho =>
                                carrinho.CarrinhoCompraId == CarrinhoDeCompraId);
            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            _context.SaveChanges(); 
        }
        public decimal GetCarrinhoCompraTotal()
        {
            var total = _context.CarrinhoCompraItens
                        .Where(c => c.CarrinhoCompraId == CarrinhoDeCompraId)
                        .Select(c => c.Lanche.Preco * c.Quantidade).Sum();
            return total;
    
        }
    }


}
