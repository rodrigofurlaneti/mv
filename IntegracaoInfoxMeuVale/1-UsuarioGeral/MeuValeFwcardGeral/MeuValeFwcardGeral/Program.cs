using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MeuValeFwcardGeral
{
    class Program
    {
        static void Main(string[] args)
        {
            var chromeOptions = new ChromeOptions();
            var downloadDirectory = "C:\\Temp";
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver = new ChromeDriver(chromeOptions);
            var Geral = new Geral();
            Geral.Iniciar(driver);
            driver.Close();
        }
    }
}
