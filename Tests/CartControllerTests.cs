using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using WebUI.Controllers;
using DomainModel.Abstract;
using DomainModel.Entities;
using System.Web.Mvc;

namespace Tests
{
    [TestFixture]
    public class CartControllerTests
    {
        [Test]
        public void Can_Add_Product_To_Cart()
        {
            // підготовка: встановити імітований репозиторій з 2-ма товарами
            var mockProductsRepos = new Mock<IProductsRepository>();
            var products = new List<Product> {
                new Product { ProductID = 14, Name = "Much Ado About Nothing" },
                new Product { ProductID = 27, Name = "The Comedy of Errors" }
            };
            mockProductsRepos.Setup(x => x.Products).Returns(products.AsQueryable());
            var cart = new Cart();
            var controller = new CartController(mockProductsRepos.Object);

            //дія: спроба додати товар в кошик
            RedirectToRouteResult result = controller.AddToCart(cart, 27, "someReturnUrl");

            //підтвердження
            Assert.AreEqual(1, cart.Lines.Count);
            Assert.AreEqual("The Comedy of Errors", cart.Lines[0].Product.Name);
            Assert.AreEqual(1, cart.Lines[0].Quantity);
            // перевірити що користувач направлений на екран корзини
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("someReturnUrl", result.RouteValues["returnUrl"]);
        }

        [Test]
        public void Can_Remove_Product_From_Cart()
        {
            // підготовка: встановити імітований репозиторій з 2-ма товарами
            var mockProductsRepos = new Mock<IProductsRepository>();
            var products = new List<Product> {
                new Product { ProductID = 14, Name = "Much Ado About Nothing" },
                new Product { ProductID = 27, Name = "The Comedy of Errors" }
            };
            mockProductsRepos.Setup(x => x.Products).Returns(products.AsQueryable());
            var cart = new Cart();
            var controller = new CartController(mockProductsRepos.Object);

            //дія: спроба додати товар в кошик
            RedirectToRouteResult result = controller.AddToCart(cart, 27, "someReturnUrl");

            //підтвердження
            Assert.AreEqual(1, cart.Lines.Count);
            Assert.AreEqual("The Comedy of Errors", cart.Lines[0].Product.Name);
            Assert.AreEqual(1, cart.Lines[0].Quantity);
            // перевірити що користувач направлений на екран корзини
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("someReturnUrl", result.RouteValues["returnUrl"]);

            //дія: спроба видалити товар з кошика
            result = controller.RemoveFromCart(cart, 27, "someReturnUrl");

            //підтвердження
            Assert.AreEqual(0, cart.Lines.Count);
        }
    }
}
