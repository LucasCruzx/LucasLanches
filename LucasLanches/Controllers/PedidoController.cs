using LucasLanches.Models;
using LucasLanches.Repositories;
using LucasLanches.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasLanches.Controllers
{
    public class PedidoController : Controller
    {
       private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize] 
        public IActionResult Checkout(Pedido pedido)
        {
            int totalItemPedido = 0;
            decimal precoTotalPedido = 0.0m;

            //obtem os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.CarrinhoCompraItems = items;

            //verifica se existe itens de pedido
            if(_carrinhoCompra.CarrinhoCompraItems.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinño esta vazio, que tal incluir um lanche...");
            }

            //calcule o total de itens e o total do pedido
            foreach(var item in items)
            {
                totalItemPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido= totalItemPedido;
            pedido.PedidoTotal= precoTotalPedido;

            //valida os dados do pedido
            if(ModelState.IsValid)
            {
                //cria o pedido e os detalhes 
                _pedidoRepository.CriarPedido(pedido);

                //define mensagens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido !!!";
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                //limpar carrinho do cliente
                _carrinhoCompra.LimparCarrinho();

                //exibe a view com dados do clente e do pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);




        }

    }
}
