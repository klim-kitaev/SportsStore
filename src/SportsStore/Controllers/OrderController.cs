using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
    public class OrderController:Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(o => !o.Shipped));

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
