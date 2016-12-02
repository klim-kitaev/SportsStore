using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index() => View(repository.Products);

        public ViewResult Edit(int productId) => 
            View(repository.Products.FirstOrDefault(p => p.ProductID == productId));
    }
}
