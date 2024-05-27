using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace YoutubeWatcherService.SeleniumVideoByPass
{
    public class AcceptCookieByPass
    {
        public void ByPass(ChromeDriver driver)
        {
            try
            {
                driver.FindElement(By.CssSelector("[aria-label^='Accept the use of cookies']")).Click();
            }
            catch (Exception)
            {

            }
        }
    }
}
