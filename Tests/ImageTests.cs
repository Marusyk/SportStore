using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using DomainModel.Entities;
using DomainModel.Abstract;
using WebUI.Controllers;
using System.Web.Mvc;

namespace Tests
{
    [TestFixture]
    public class ImageTests
    {
        [Test]
        public void Can_Retrieve_Image_Data()
        {
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1" },
                prod,
                new Product { ProductID = 3, Name = "P3" }}.AsQueryable());
            
            ProductsController controller = new ProductsController(mock.Object);

            ActionResult result = controller.GetImage(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [Test]
        public void Cannot_Retrieve_Image_Data_For_Invalid_Id()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"}
            }.AsQueryable());

            ProductsController controller = new ProductsController(mock.Object);
            ActionResult result = controller.GetImage(200);

            Assert.IsNull(result);
        }
    }
}
