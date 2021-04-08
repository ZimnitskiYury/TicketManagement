using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TicketManagement.AQA.WebPages;

namespace TicketManagement.AQA.Steps
{
    [Binding]
    public class AuthorizationSteps
    {
        private static HomePage HomePage => PageFactory.Get<HomePage>();

        [Given(@"User is on localhost")]
        public void GivenUserIsOnLocalhost()
        {
            HomePage.Open();
        }

        [When(@"User press link Login in header")]
        public async Task WhenUserPressLinkLoginInHeader()
        {
            await Task.Delay(3000);
            HomePage.LoginButton.Click();
        }

        [When(@"Enters ""(.*)"" to Login input form")]
        public void WhenEntersToLoginInputForm(string p0)
        {
            HomePage.LoginInput.SendKeys(p0);
        }

        [When(@"Enters ""(.*)"" to Password input form")]
        public void WhenEntersToPasswordInputForm(string p0)
        {
            HomePage.PasswordInput.SendKeys(p0);
        }

        [When(@"User clicks button LoginSubmit")]
        public void WhenUserClicksButtonSubmit()
        {
            HomePage.SubmitLoginFormButton.Click();
        }

        [Then(@"Header text change to Login ""(.*)""")]
        public void ThenHeaderTextChangeToLogin(string p1)
        {
            HomePage.LoggedUser(p1);
        }

        [Then(@"Login form has error ""(.*)""")]
        public void ThenLoginFormHasError(string p1)
        {
            HomePage.LoginFormError(p1);
        }
    }
}

