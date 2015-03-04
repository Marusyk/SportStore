using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.HtmlHelpers;

namespace Tests
{
    [TestFixture]
    public class PageHelperTests
    {
        [Test]
        public void PageLinks_Methid_Extends_HtmlHelper()
        {
            HtmlHelper html = null;
            html.PageLinks(0, 0, null);
        }

        public void PageLinks_Produces_Anchor_Tags()
        {
            // перший параметри буде індекс поточної сторінки
            // другий - загальна кількість сторінок
            // третій - лямбда-метод для відображення номера сторінки 
            string links = ((HtmlHelper)null).PageLinks(2, 3, i => "Page" + i);
            Assert.AreEqual(@"<a href=""Page1"">1</a>
<a class=""selected"" href=""Page2"">2</a>
<a href=""Page3"">3</a>)
", links);
        }
    }
}
