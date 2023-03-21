using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FrontEnd_E2E_Test
{
    public class FrontEnd_E2E_Test : IDisposable 
    {
        private readonly IWebDriver _webDriver;

        public FrontEnd_E2E_Test()
        {
            _webDriver = new ChromeDriver();
        }

        [Fact]
        public void CheckHomePageTitle()
        {
            _webDriver.Navigate().GoToUrl("https://localhost:44327/");
            Assert.Equal("Home Page - Microservices.Web.Frontend", _webDriver.Title);
        }

        [Fact]
        public void CheckHomePageText()
        {
            _webDriver.Navigate().GoToUrl("https://localhost:44327/");
            Assert.Contains("Welcome", _webDriver.PageSource);
        }

        [Fact]
        public void CheckProductListData()
        {
            _webDriver.Navigate().GoToUrl("https://localhost:44327/Product");

            IWebElement elementTable = _webDriver.FindElement(By.XPath("//div[@class='container']//table[1]"));
            List<IWebElement> listTR = new List<IWebElement>(elementTable.FindElements(By.TagName("tr")));
            Assert.Single(listTR);
        }

        [Fact]
        public void CheckAddProductToBasket()
        {
            _webDriver.Navigate().GoToUrl("https://localhost:44327/Product");

            _webDriver.FindElement(By.Id("addtobasket")).Click();

            Assert.Equal("https://localhost:5001/basket", _webDriver.Url.ToLower());
        }

        //[Fact]
        //public void CheckEmptyDiscountCodeInBasket()
        //{
        //    _webDriver.Navigate().GoToUrl("https://localhost:44327/Product");

        //    _webDriver.FindElement(By.Id("addtobasket")).Click();

        //    Assert.Equal("https://localhost:5001/basket", _webDriver.Url.ToLower());
        //}

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}