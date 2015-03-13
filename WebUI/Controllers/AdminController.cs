using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private IProductsRepository productRepository;

        public AdminController(IProductsRepository prodRepos)
        {
            productRepository = prodRepos;
        }

        public ViewResult Index()
        {
            return View(productRepository.Products.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Edit(int productID)
        {
            Product product = (from p in productRepository.Products
                               where p.ProductID == productID
                               select p).First();
            return View(product);
        }
    }
}
