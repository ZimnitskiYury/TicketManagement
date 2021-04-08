using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TicketManagement.AQA.WebPages
{
    public abstract class AbstractPage
    {
        protected IWebDriver Driver;

        protected AbstractPage(IWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement FindByCss(string css, int timeoutInSeconds)
        {
            var locator = ExpectedConditions.ElementIsVisible(By.CssSelector(css));
            new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(locator);
            return Driver.FindElement(By.CssSelector(css));
        }

        protected IWebElement FindByCssWithText(string css, string text, int timeoutInSeconds)
        {
            var locator = ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector(css), text);
            new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(locator);
            return Driver.FindElement(By.CssSelector(css));
        }

        protected IWebElement FindByClassName(string className, int timeoutInSeconds)
        {
            var locator = ExpectedConditions.ElementIsVisible(By.ClassName(className));
            new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(locator);
            return Driver.FindElement(By.ClassName(className));
        }

        protected IWebElement FindByXPath(string xPath, int timeoutInSeconds)
        {
            var locator = ExpectedConditions.ElementIsVisible(By.XPath(xPath));
            new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(locator);
            return Driver.FindElement(By.XPath(xPath));
        }
    }
}
