using LucasLanches.Migrations;
using LucasLanches.Models;
using LucasLanches.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LucasLanches.Components
{
    public class CarrinhoCompraResumo:ViewComponent
    {
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraResumo(CarrinhoCompra item)
        {
            _carrinhoCompra = item;
        }

        public IViewComponentResult Invoke()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItems();

            _carrinhoCompra.CarrinhoCompraItems = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };
            return View(carrinhoCompraVM);
        }

    }
}
