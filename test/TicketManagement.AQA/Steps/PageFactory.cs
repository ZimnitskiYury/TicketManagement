using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;
using TicketManagement.AQA.Utils;
using TicketManagement.AQA.WebPages;

namespace TicketManagement.AQA.Steps
{
    [Binding]
    public class PageFactory
    {
        private static IWebDriver _driver;

        private PageFactory() { }

        private static readonly Lazy<PageFactory> Lazy = new Lazy<PageFactory>(() => new PageFactory());

        public static PageFactory Instance => Lazy.Value;

        public static T Get<T>() where T : AbstractPage
        {
            object[] args = {_driver};
            return (T) Activator.CreateInstance(typeof(T), args);
        }

        [BeforeFeature]
        public static void OpenBrowser()
        {
            _driver = DriverFactory.GetDriver();
        }

        [AfterFeature]
        public static void CloseBrowser()
        {
            _driver.Close();
            _driver.Dispose();
        }
    }
}
