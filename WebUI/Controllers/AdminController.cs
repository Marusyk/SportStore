using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;

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
    }
}
