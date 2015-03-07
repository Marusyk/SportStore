using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DomainModel.Entities;
using WebUI.Controllers;
using DomainModel.Abstract;
using NUnit.Framework;
using System.Web.Routing;
using WebUI;
using System.Web;

namespace Tests
{
    [TestFixture]
    public class InboundRoutingTests
    {
        [Test]
        public void Slash_Goes_To_All_Products_Page_1()
        {
            TestRoute("~/", new
            {
                controller = "Products",
                action = "List",
                category = (string)null,
                page = 1
            });
        }

        private void TestRoute(string url, object expectedValues)
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var mockHttpContext = new Moq.Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);

            RouteData routeData = routes.GetRouteData(mockHttpContext.Object);

            Assert.IsNotNull(routeData);
            var expectedDict = new RouteValueDictionary(expectedValues);
            foreach(var exp in expectedDict)
            {
                if (exp.Value == null)
                    Assert.IsNull(routeData.Values[exp.Key]);
                else
                    Assert.AreEqual(exp.Value.ToString(), routeData.Values[exp.Key].ToString());
            }
        }

        [Test]
        public void Page2_Goes_To_All_Products_Page_2()
        {
            TestRoute("~/Page2", new
                {
                    controller = "Products",
                    action = "List",
                    category = (string)null,
                    page = 2
                });
        }

        [Test]
        public void Football_Goes_To_Football_Page_1()
        {
            TestRoute("~/Football", new
            {
                controller = "Products",
                action = "List",
                category = "Football",
                page = 1
            });
        }

        [Test]
        public void Football_Slash_Page43_Goes_To_Football_Page_43()
        {
            TestRoute("~/Football/Page43", new
            {
                controller = "Products",
                action = "List",
                category = "Football",
                page = 43
            });
        }

        [Test]
        public void Anything_Slash_Else_Goes_To_Else_On_AnythingController()
        {
            TestRoute("~/Anything/Else", new
            {
                controller = "Anything",
                action = "Else"
            });
        }
    }
}
