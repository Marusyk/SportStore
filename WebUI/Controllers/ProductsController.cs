using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Concrete;
using DomainModel.Entities;

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

        public ViewResult List(string category, int page)
        {
            var productsInCategory = (category == null)
                ? productRepository.Products
                : productRepository.Products.Where(x => x.Category == category);
            int numProducts = productRepository.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)numProducts / PageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CurrentCategory = category;
            return View(productsInCategory
                                         .Skip((page - 1) * PageSize)
                                         .Take(PageSize)
                                         .ToList()
                       );
        }

        public FileContentResult GetImage(int productID)
        {
            Product product = (from p in productRepository.Products
                               where p.ProductID == productID
                               select p).First();
            return File(product.ImageData, product.ImageMimeType);
        }
    }
}
