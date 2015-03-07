using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUI.Controllers;
using NUnit.Framework;
using DomainModel.Abstract;
using DomainModel.Entities;
using Moq;
using System.Web.Mvc;

namespace Tests
{
    [TestFixture]
    class NavControllerTests
    {
        [Test]
        public void Takes_IProductsRepository_As_Construstor_Param()
        {
            new NavController((IProductsRepository)null);
        }

        [Test]
        public void Produces_Home_NavLink_Object_For_Each_Distinct_Category()
        {
            // підготовка: репозиторій з декількома категоріями
            IQueryable<Product> products = new[] {
                new Product { Name = "A", Category = "Animal" },
                new Product { Name = "B", Category = "Vegetable" },
                new Product { Name = "C", Category = "Mineral" },
                new Product { Name = "D", Category = "Vegetable" },
                new Product { Name = "E", Category = "Animal" }
            }.AsQueryable();
            
            var mockProductsRepos = new Moq.Mock<IProductsRepository>();
            mockProductsRepos.Setup(x => x.Products).Returns(products);
            var controller = new NavController(mockProductsRepos.Object);

            //Дія: викликати дію Menu
            ViewResult result = controller.Menu(null);

            //Твердження: провірити візуалізацію по одній NavLink на категорію в алфавітному порядку
            var links = ((IEnumerable<NavLink>)result.ViewData.Model).ToList();
            Assert.IsEmpty(result.ViewName);
            Assert.AreEqual(4, links.Count);
            Assert.AreEqual("Home", links[0].Text);
            Assert.AreEqual("Animal", links[1].Text);
            Assert.AreEqual("Mineral", links[2].Text);
            Assert.AreEqual("Vegetable", links[3].Text);

            foreach(var link in links)
            {
                Assert.AreEqual("Products", link.RouteValues["controller"]);
                Assert.AreEqual("List", link.RouteValues["action"]);
                Assert.AreEqual(1, link.RouteValues["page"]);
                if (links.IndexOf(link) == 0)
                    Assert.IsNull(link.RouteValues["category"]);
                else
                    Assert.AreEqual(link.Text, link.RouteValues["category"]);
            }
        }

        [Test]
        public void Highlights_Current_Category()
        {
            IQueryable<Product> products = new[] {
                new Product { Name = "A", Category = "Animal" },
                new Product { Name = "B", Category = "Vegetable" }
            }.AsQueryable();

            var mockProductsRepos = new Mock<IProductsRepository>();
            mockProductsRepos.Setup(x => x.Products).Returns(products);
            var controller = new NavController(mockProductsRepos.Object);

            var result = controller.Menu("Vegetable");

            var highlightedLinks = ((IEnumerable<NavLink>)result.ViewData.Model)
                .Where(x => x.IsSelected).ToList();
            Assert.AreEqual(1, highlightedLinks.Count);
            Assert.AreEqual("Vegetable", highlightedLinks[0].Text);
        }
    }
}
