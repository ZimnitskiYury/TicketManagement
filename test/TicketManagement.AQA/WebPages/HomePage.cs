using System;
using OpenQA.Selenium;

namespace TicketManagement.AQA.WebPages
{
    public class HomePage : AbstractPage
    {
        private const int DefaultWaitingInterval = 2;

        public HomePage(IWebDriver driver) : base(driver){}
        
        public IWebElement LoginButton => FindByClassName("fa-user", DefaultWaitingInterval);
        public IWebElement EventsButton => FindByCss("li[id='events']", DefaultWaitingInterval);
        public IWebElement CreateButton => FindByCss("a[id='create-event']", DefaultWaitingInterval);

        public IWebElement NameInput => FindByCss("input[id='Name']", DefaultWaitingInterval);
        public IWebElement DescriptionInput => FindByCss("input[id='Description']", DefaultWaitingInterval);
        public IWebElement LayoutInput => FindByCss("input[id='LayoutId']", DefaultWaitingInterval);
        public IWebElement StartDateInput => FindByCss("input[name='StartDateTime']", DefaultWaitingInterval);
        public IWebElement EndDateInput => FindByCss("input[name='EndDateTime']", DefaultWaitingInterval);
        public IWebElement CreateSubmitButton => FindByCss("button[id='create-event-submit']", DefaultWaitingInterval);
        public IWebElement PricesSubmitButton => FindByCss("button[id='change-prices']", DefaultWaitingInterval);

        public IWebElement FirstPriceInput => FindByCss("input[name='[0].Price']", DefaultWaitingInterval);
        public IWebElement SecondPriceInput => FindByCss("input[name='[1].Price']", DefaultWaitingInterval);
        public IWebElement EventTitle(string textPresent) => FindByCssWithText("h2.title", textPresent, DefaultWaitingInterval);

        public IWebElement LoginInput => FindByCss("input[id='Login']", DefaultWaitingInterval);
        public IWebElement PasswordInput => FindByCss("input[id='Password']", DefaultWaitingInterval);
        public IWebElement SubmitLoginFormButton => FindByCss("button[id='login-submit']", DefaultWaitingInterval);
        public IWebElement LoginFormError(string textPresent) => FindByCssWithText("div.validation-summary-errors.text-danger", textPresent, DefaultWaitingInterval);
        public IWebElement LoggedUser(string textPresent) => FindByCssWithText("a[id='logged-user']", textPresent, DefaultWaitingInterval);


        public void Open()
        {
            
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            Driver.Url = "https://localhost:44370/";
        }
    }
}
