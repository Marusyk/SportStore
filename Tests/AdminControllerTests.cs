using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using DomainModel.Abstract;
using DomainModel.Entities;
using System.Web.Mvc;
using WebUI.Controllers;

namespace Tests
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IProductsRepository> mockRepos;

        //Цей метод буде викликатися перед прогоном кожного тексту
        [SetUp]
        public void SetUp()
        {
            //створити новий макет репозиторію на 50 товарів
            List<Product> allProducts = new List<Product>();
            for (int i = 1; i < 50; i++)
                allProducts.Add(new Product { ProductID = i, Name = "Product " + i });
            mockRepos = new Mock<IProductsRepository>();
            mockRepos.Setup(x => x.Products).Returns(allProducts.AsQueryable());
        }

        [Test]
        public void Index_Action_Lists_All_Products()
        {
            AdminController controller = new AdminController(mockRepos.Object);

            //action
            ViewResult results = controller.Index();

            Assert.IsEmpty(results.ViewName);
            var prodsRendered = (List<Product>)results.ViewData.Model;
            for (int i = 0; i < 49; i++)
                Assert.AreEqual("Product " + (i + 1), prodsRendered[i].Name);

        }
    }
}
