using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DomainModel.Entities;
using Moq;

namespace Tests
{
    [TestFixture]
    public class CartTests
    {
        [Test]
        public void Cart_Starts_Empty()
        {
            Cart cart = new Cart();
            Assert.AreEqual(0, cart.Lines.Count);
            Assert.AreEqual(0, cart.ComputeTotalValue());
        }

        [Test]
        public void Can_Add_Items_To_Cart()
        {
            Product p1 = new Product { ProductID = 1 };
            Product p2 = new Product { ProductID = 2 };
            // додати 3 товара (2 з них одинаові)
            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 10);

            //перевірити к-сть рядків результату
            Assert.AreEqual(2, cart.Lines.Count, "Wrong number of lines in cart");

            //перевірка правильності кількості добавлених товарів
            var p1Line = cart.Lines.Where(l => l.Product.ProductID == 1).First();
            var p2Line = cart.Lines.Where(l => l.Product.ProductID == 2).First();
            Assert.AreEqual(3, p1Line.Quantity);
            Assert.AreEqual(10, p2Line.Quantity);
        }

        [Test]
        public void Can_Be_Cleared()
        {
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            Assert.AreEqual(1, cart.Lines.Count);
            cart.Clear();
            Assert.AreEqual(0, cart.Lines.Count);
        }

        [Test]
        public void Calculates_Total_Value_Correctly()
        {
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1, Price = 5 }, 10);
            cart.AddItem(new Product { ProductID = 2, Price = 2.1M }, 3);
            cart.AddItem(new Product { ProductID = 3, Price = 1000 }, 1);
            Assert.AreEqual(1056.3M, cart.ComputeTotalValue());
        }

        [Test]
        public void Can_Be_Remove_Line()
        {
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1 }, 1);
            cart.AddItem(new Product { ProductID = 2 }, 2);
            Assert.AreEqual(2, cart.Lines.Count);
            var prod = cart.Lines.Where(l => l.Product.ProductID == 1).First();
            cart.RemoveLine(prod.Product);
            Assert.AreEqual(1, cart.Lines.Count);
        }

        [Test]
        public void Cart_Shipping_Details_Start_Empty()
        {
            Cart cart = new Cart();
            ShippingDetails d = cart.ShippingDetails;
            Assert.IsNull(d.Name);
            Assert.IsNull(d.Line1);
            Assert.IsNull(d.Line2);
            Assert.IsNull(d.Line3);
            Assert.IsNull(d.City);
            Assert.IsNull(d.State);
            Assert.IsNull(d.Country);
            Assert.IsNull(d.Zip);
        }

        [Test]
        public void Cart_Not_GiftWrapped_By_Default()
        {
            Cart cart = new Cart();
            Assert.IsFalse(cart.ShippingDetails.GiftWrap);
        }
    }
}
