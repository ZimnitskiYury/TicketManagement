using OpenQA.Selenium;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TicketManagement.AQA.WebPages;

namespace TicketManagement.AQA.Steps
{
    [Binding]
    public class CreateEventSteps
    {

        private static HomePage HomePage => PageFactory.Get<HomePage>();

        [When(@"User clicks on Events page")]
        public void WhenUserClicksOnEventsPage()
        {
            HomePage.EventsButton.Click();
        }

        [When(@"User clicks Create button")]
        public void WhenUserPressCreateButton()
        {
            HomePage.CreateButton.Click();
        }

        [When(@"Enters ""(.*)"" to Name input form")]
        public void WhenEntersToNameInputForm(string p0)
        {
            HomePage.NameInput.SendKeys(p0);
        }

        [When(@"Enters ""(.*)"" to Description input form")]
        public void WhenEntersToDescriptionInputForm(string p0)
        {
            HomePage.DescriptionInput.SendKeys(p0);
        }

        [When(@"Enters ""(.*)"" to Layout input form")]
        public void WhenEntersToLayoutInputForm(int p0)
        {
            HomePage.LayoutInput.SendKeys(p0.ToString());
        }

        [When(@"Enters ""(.*)"" to Start Event Date input form")]
        public void WhenEntersToStartEventDateInputForm(string p0)
        {
            string[] date = p0.Split(new char[] { ' ' });
            date[0] = date[0].Replace(".", string.Empty);
            date[1] = date[1].Replace(":", string.Empty);
            HomePage.StartDateInput.SendKeys(date[0]);
            HomePage.StartDateInput.SendKeys(Keys.Tab);
            HomePage.StartDateInput.SendKeys(date[1]);
        }

        [When(@"Enters ""(.*)"" to End Event Date input form")]
        public void WhenEntersToEndEventDateInputForm(string p0)
        {
            string[] date = p0.Split(new char[] { ' ' });
            date[0] = date[0].Replace(".", string.Empty);
            date[1] = date[1].Replace(":", string.Empty);
            HomePage.EndDateInput.SendKeys(date[0]);
            HomePage.EndDateInput.SendKeys(Keys.Tab);
            HomePage.EndDateInput.SendKeys(date[1]);
        }

        [When(@"User clicks button Submit to Create Event")]
        public async Task WhenUserClicksButtonCreateSubmit()
        {
            await Task.Delay(3000);
            HomePage.CreateSubmitButton.Click();
        }

        [When(@"Enters ""(.*)"" to First Area Price input form")]
        public void WhenEntersToFirstAreaPriceInputForm(int p0)
        {
            HomePage.FirstPriceInput.Clear();
            HomePage.FirstPriceInput.SendKeys(p0.ToString());
        }

        [When(@"Enters ""(.*)"" to Second Area Price input form")]
        public void WhenEntersToSecondAreaPriceInputForm(int p0)
        {
            HomePage.SecondPriceInput.Clear();
            HomePage.SecondPriceInput.SendKeys(p0.ToString());
        }

        [When(@"User clicks button Submit to Change Prices")]
        public async Task WhenUserClicksButtonChangePrices()
        {
            await Task.Delay(1000);
            HomePage.PricesSubmitButton.Click();
        }

        [Then(@"A new event will appear on the page with the name ""(.*)""")]
        public void ThenANewEventWillAppearOnThePageWithTheName(string p0)
        {
            HomePage.EventTitle(p0.ToUpper());
        }
    }
}
