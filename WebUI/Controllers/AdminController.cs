using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private IProductsRepository productRepository;

        [Authorize]
        public AdminController(IProductsRepository prodRepos)
        {
            productRepository = prodRepos;
        }

        [Authorize]
        public ViewResult Index()
        {
            return View(productRepository.Products.ToList());
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Edit(int productID)
        {
            Product product = (from p in productRepository.Products
                               where p.ProductID == productID
                               select p).First();
            return View(product);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                productRepository.SaveProduct(product);
                TempData["message"] = product.Name + " has been saved.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        [Authorize]
        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [Authorize]
        public RedirectToRouteResult Delete(int productID)
        {
            Product product = (from p in productRepository.Products
                               where p.ProductID == productID
                               select p).First();
            productRepository.DeleteProduct(product);
            TempData["message"] = product.Name + " has been deleted.";
            return RedirectToAction("Index");
        }
    }
}
