using DomainModel.Abstract;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DomainModel.Entities;
using WebUI.Controllers;

namespace Tests
{
    [TestFixture]
    class ProductsControllerTests
    {
        [Test]
        public void List_Presents_Correct_Page_Of_Products()
        {
            // Підготовка: 5 товарів в репозиторій
            IProductsRepository repository = MockProductsRepository(
                new Product { Name = "P1" }, new Product { Name = "P2" },
                new Product { Name = "P3" }, new Product { Name = "P4" },
                new Product { Name = "P5" });

            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 3;  //Ця властивість поки не існує, але ми формуємо вимогу на його створення
            // Дія: запросити другу сторінку (розмір сторінки = 3)
            var result = controller.List(null, 2);
            Assert.IsNotNull(result, "Didn't render view");
            var products = result.Model as IList<Product>;
            Assert.AreEqual(2, products.Count, "Got wrong number of products");
            Assert.AreEqual(2, (int)result.ViewBag.CurrentPage, "Wrong page number");
            Assert.AreEqual(2, (int)result.ViewBag.TotalPages, "Wrong page count");
            Assert.AreEqual("P4", products[0].Name);
            Assert.AreEqual("P5", products[1].Name);
        }

        static IProductsRepository MockProductsRepository(params Product[] prods)
        {
            var mockProductsRepos = new Mock<IProductsRepository>();
            mockProductsRepos.Setup(x => x.Products).Returns(prods.AsQueryable());
            return mockProductsRepos.Object;
        }

        [Test]
        public void List_Include_All_Products_When_Category_Is_Null()
        {
            //Підготовити сценарій з двома категоріями
            IProductsRepository repository = MockProductsRepository(
                new Product { Name = "Artemis", Category = "Greek" },
                new Product { Name = "Neptune", Category = "Roman" });

            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 10;

            //запросити нефільтрований список
            var result = controller.List(null, 1);
            // перевірити що результат включає 2 елементи
            Assert.IsNotNull(result, "Didn't render view");
            var products = (IList<Product>)result.ViewData.Model;
            Assert.AreEqual(2, products.Count, "Got wrong number of items");
            Assert.AreEqual("Artemis", products[0].Name);
            Assert.AreEqual("Neptune", products[1].Name);
        }

        [Test]
        public void List_Filters_By_Category_When_Requested()
        {
            //підготувати сценарій з 2-ма категоріями
            IProductsRepository repository = MockProductsRepository(
                new Product { Name = "Snowball", Category = "Cats" },
                new Product { Name = "Rex", Category = "Dogs" },
                new Product { Name = "Catface", Category = "Cats" },
                new Product { Name = "Woofer", Category = "Dogs" },
                new Product { Name = "Chomper", Category = "Dogs" }
                );

            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 10;

            // запросити тільки Dogs
            var result = controller.List("Dogs", 1);

            //провірка результатів
            Assert.IsNotNull(result, "Didn't render view");
            var products = (IList<Product>)result.ViewData.Model;
            Assert.AreEqual(3, products.Count, "Got wrong number of items");
            Assert.AreEqual("Rex", products[0].Name);
            Assert.AreEqual("Woofer", products[1].Name);
            Assert.AreEqual("Chomper", products[2].Name);
            Assert.AreEqual("Dogs", result.ViewBag.CurrentCategory);
        }
    }
}
