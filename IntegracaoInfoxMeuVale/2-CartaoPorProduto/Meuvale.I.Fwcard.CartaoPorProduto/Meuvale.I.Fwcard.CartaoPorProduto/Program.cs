using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Meuvale.I.Fwcard.CartaoPorProduto
{
    class Program
    {
#pragma warning disable IDE0060 // Remover o parâmetro não utilizado
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remover o parâmetro não utilizado
        {
            var chromeOptions = new ChromeOptions();
            var downloadDirectory = "C:\\Temp";
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver = new ChromeDriver(chromeOptions);
            var Produto = new Cartao();
            Produto.CleanTable();
            string tp1 = "MV ADTO SALARIAL";
            Produto.Iniciar(driver, tp1);
            driver.Quit();

            var chromeOptions2 = new ChromeOptions();
            var downloadDirectory2 = "C:\\Temp";
            chromeOptions2.AddUserProfilePreference("download.default_directory", downloadDirectory2);
            chromeOptions2.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions2.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver2 = new ChromeDriver(chromeOptions2);
            var Produto2 = new Cartao();
            string tp2 = "MV REFEIÇÃO";
            Produto2.Iniciar2(driver2, tp2);
            driver2.Quit();

            var chromeOptions3 = new ChromeOptions();
            var downloadDirectory3 = "C:\\Temp";
            chromeOptions3.AddUserProfilePreference("download.default_directory", downloadDirectory3);
            chromeOptions3.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions3.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver3 = new ChromeDriver(chromeOptions3);
            var Produto3 = new Cartao();
            string tp3 = "MV FARMÁCIA";
            Produto3.Iniciar3(driver3, tp3);
            driver3.Quit();

            var chromeOptions4 = new ChromeOptions();
            var downloadDirectory4 = "C:\\Temp";
            chromeOptions4.AddUserProfilePreference("download.default_directory", downloadDirectory4);
            chromeOptions4.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions4.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver4 = new ChromeDriver(chromeOptions4);
            var Produto4 = new Cartao();
            string tp4 = "MV ODONTO";
            Produto4.Iniciar4(driver4, tp4);
            driver4.Quit();

            var chromeOptions5 = new ChromeOptions();
            var downloadDirectory5 = "C:\\Temp";
            chromeOptions5.AddUserProfilePreference("download.default_directory", downloadDirectory5);
            chromeOptions5.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions5.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver5 = new ChromeDriver(chromeOptions5);
            var Produto5 = new Cartao();
            string tp5 = "MV ALIMENTAÇÃO";
            Produto5.Iniciar5(driver5, tp5);
            driver5.Quit();

            var chromeOptions6 = new ChromeOptions();
            var downloadDirectory6 = "C:\\Temp";
            chromeOptions6.AddUserProfilePreference("download.default_directory", downloadDirectory6);
            chromeOptions6.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions6.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver6 = new ChromeDriver(chromeOptions6);
            var Produto6 = new Cartao();
            string tp6 = "MV ADTO SALARIAL - STA IZABEL - PARÁ";
            Produto6.Iniciar6(driver6, tp6);
            driver6.Quit();

            var chromeOptions7 = new ChromeOptions();
            var downloadDirectory7 = "C:\\Temp";
            chromeOptions7.AddUserProfilePreference("download.default_directory", downloadDirectory7);
            chromeOptions7.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions7.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver7 = new ChromeDriver(chromeOptions7);
            var Produto7 = new Cartao();
            string tp7 = "MV ADTO SALARIAL - ELDORADO DO CARAJÁS -PARÁ";
            Produto7.Iniciar7(driver7, tp7);
            driver7.Quit();

            var chromeOptions8 = new ChromeOptions();
            var downloadDirectory8 = "C:\\Temp";
            chromeOptions8.AddUserProfilePreference("download.default_directory", downloadDirectory8);
            chromeOptions8.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions8.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver8 = new ChromeDriver(chromeOptions8);
            var Produto8 = new Cartao();
            string tp8 = "MV BENF COMBUSTÍVEL";
            Produto8.Iniciar8(driver8, tp8);
            driver8.Quit();

            var chromeOptions9 = new ChromeOptions();
            var downloadDirectory9 = "C:\\Temp";
            chromeOptions9.AddUserProfilePreference("download.default_directory", downloadDirectory9);
            chromeOptions9.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions9.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver9 = new ChromeDriver(chromeOptions9);
            var Produto9 = new Cartao();
            string tp9 = "MV CONV. COMBUSTÍVEL";
            Produto9.Iniciar9(driver9, tp9);
            driver9.Quit();

            var chromeOptions10 = new ChromeOptions();
            var downloadDirectory10 = "C:\\Temp";
            chromeOptions10.AddUserProfilePreference("download.default_directory", downloadDirectory10);
            chromeOptions10.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions10.AddUserProfilePreference("disable-popup-blocking", "false");
            IWebDriver driver10 = new ChromeDriver(chromeOptions10);
            var Produto10 = new Cartao();
            string tp10 = "MV SUBSIDIO DE MEDICAMENTOS";
            Produto10.Iniciar10(driver10, tp10);
            driver10.Quit();
        }
    }
}
