using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace YoutubeWatcherService.SeleniumVideoByPass
{
    public class EnSureVideoPlayingByPass
    {
        public bool EnsureVideoPlaying(ChromeDriver driver)
        {
            try
            {
                driver.FindElement(By.ClassName("paused-mode"));
                IWebElement playButton = driver.FindElement(By.Id("movie_player"));
                playButton.SendKeys("keys.SPACE");
                return true;
            }
            catch (NoSuchElementException ex)
            {
                try
                {
                    driver.FindElement(By.ClassName("playing-mode"));
                    return true;
                }
                catch (Exception ex2)
                {
                    return false;
                }
            }
            catch (ElementNotInteractableException ex)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
