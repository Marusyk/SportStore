using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Concrete;

namespace WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsRepository productRepository;
        public int PageSize = 4;

        public ProductsController(IProductsRepository productsRepository)
        {
            //string connString = @"Data Source=MARUSYK\SQLEXPRESS;Initial Catalog=SportStore;Integrated Security=True;Pooling=False";
            //productRepository = new SqlProductsRepository(connString);
            productRepository = productsRepository;
        }

        public ViewResult List(int page)
        {
            return View(productRepository.Products
                                         .Skip((page - 1) * PageSize)
                                         .Take(PageSize)
                                         .ToList()
                       );
        }

    }
}
