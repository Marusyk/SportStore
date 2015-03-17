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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            if(ModelState.IsValid)
            {
                if(image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                productRepository.SaveProduct(product);
                TempData["message"] = product.Name + " has been saved.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

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
