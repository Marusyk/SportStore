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
using DomainModel.Services;

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
            var controller = new CartController(mockProductsRepos.Object, null);

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
            var controller = new CartController(mockProductsRepos.Object, null);

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

        [Test]
        public void Index_Action_Renders_Default_View_With_Cart_And_ReturnUrl()
        {
            //встановлюємо контроллер
            Cart cart = new Cart();
            CartController controller = new CartController(null, null);

            //виклик методу дії
            ViewResult result = controller.Index(cart, "myReturnUrl");

            //перевірка результів
            Assert.IsEmpty(result.ViewName);
            Assert.AreSame(cart, result.ViewData.Model);
            Assert.AreEqual("myReturnUrl", result.ViewBag.ReturnUrl);
            Assert.AreEqual("Cart", result.ViewBag.CurrentCategory);
        }

        [Test]
        public void Submitting_Order_With_No_Lines_Displays_Default_View_With_Error()
        {
            CartController controller = new CartController(null, null);
            Cart cart = new Cart();

            var result = controller.CheckOut(cart, new FormCollection());

            Assert.IsEmpty(result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Submitting_Empty_Shipping_Details_Displays_Default_View_With_Error()
        {
            var controllerContext = new Moq.Mock<ControllerContext>(); 
            CartController controller = new CartController(null, null);
            controller.ControllerContext = controllerContext.Object;
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            var result = controller.CheckOut(cart, new FormCollection { { "Name", "TEST"} });

            Assert.IsEmpty(result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Valid_Order_Goes_To_Submitter_And_Displays_Completed_View()
        {
            var controllerContext = new Moq.Mock<ControllerContext>(); 
            
            var mockSubmitter = new Mock<IOrderSubmitter>();
            CartController controller = new CartController(null, mockSubmitter.Object);
            controller.ControllerContext = controllerContext.Object;
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            var formData = new FormCollection
            {
                { "Name", "Steve" }, { "Line1", "123 My Street" },
                { "Line2", "MyArea"}, { "Line3", "" },
                { "City", "MyCity" }, { "State", "Some State" },
                { "Zip", "123ABCDEF" }, { "Country", "Far from away" },
                { "GiftWrap", bool.TrueString }
            };

           var result = controller.CheckOut(cart, formData);

           Assert.AreEqual("Completed", result.ViewName);
           mockSubmitter.Verify(x => x.SubmitOrder(cart));
           Assert.AreEqual(0, cart.Lines.Count);
        }
    }
}
