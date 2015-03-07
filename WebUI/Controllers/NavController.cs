using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using System.Web.Routing;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductsRepository productRepository;

        public NavController(IProductsRepository prod)
        {
            productRepository = prod;
        }

        public ViewResult Menu(string higlightCategory)
        {
            List<NavLink> navLinks = new List<NavLink>();
            navLinks.Add(new CategoryLink(null) { IsSelected = (higlightCategory == null) });

            var categories = productRepository.Products.Select(x => x.Category);
            foreach (string category in categories.Distinct().OrderBy(x => x))
                navLinks.Add(new CategoryLink(category) { IsSelected = (category == higlightCategory)});
            return View(navLinks);
        }
    }

    public class NavLink
    {
        public string Text { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public bool IsSelected { get; set; }
    }

    public class CategoryLink: NavLink
    {
        public CategoryLink(string categorie)
        {
            Text = categorie ?? "Home";
            RouteValues = new RouteValueDictionary(new { controller = "Products", action = "List", category = categorie, page = 1 });
        }
    }
}
