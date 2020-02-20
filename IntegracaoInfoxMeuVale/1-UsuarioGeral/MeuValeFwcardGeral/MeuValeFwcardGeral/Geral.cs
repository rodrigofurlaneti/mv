using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeuValeFwcardGeral
{
    public class Geral
    {
        public IWebElement MenuProducao { get; set; }
        public IWebElement Relatorio { get; set; }
        public IWebElement OpCartao { get; set; }
        public IWebElement OpGeral { get; set; }
        public MySqlConnection comm { get; set; }
        public Geral()
        {
        }
        public Geral(IWebElement menuProducao, IWebElement relatorio, IWebElement opCartao, IWebElement opGeral)
        {
            MenuProducao = menuProducao;
            Relatorio = relatorio;
            OpCartao = opCartao;
            OpGeral = opGeral;
        }
        public void Iniciar(IWebDriver driver)
        {
            Autenticar(driver);
            AbaProducao(driver);
            Serealizar();
            UploadArqFtp();
        }
        public void Autenticar(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://online.fwcard.com.br/fwcard/f?p=130:100");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("P100_USERNAME")).SendKeys("RODRIGO");
            driver.FindElement(By.Id("P100_PASSWORD")).SendKeys("99588Digo@");
            var entrar = driver.FindElement(By.Id("LOGIN"));
            entrar.Click();
        }
        public void AbaProducao(IWebDriver driver)
        {
            this.MenuProducao = driver.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.OpGeral = driver.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[2]/a"));
            this.OpGeral.Click();
            Thread.Sleep(250);
            driver.FindElement(By.XPath("//*[@id='P10907_DTALTERINI']")).SendKeys("01/01/1980");
            Thread.Sleep(250);
            driver.FindElement(By.XPath("//*[@id='P10907_EXIBE_SALDO']")).SendKeys("SIM");
            Thread.Sleep(250);
            driver.FindElement(By.XPath("//*[@id='P10907_DESBLOQUEIO']")).SendKeys("NÃO");
            driver.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(30000);
            ReadOnlyCollection<string> originalHandles = driver.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver.SwitchTo().Window(popupWindowHandle);
            driver.Manage().Window.Maximize();
            Actions builder = new Actions(driver);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(240000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(240000);
        }
        public void Serealizar()
        {
            var fileInfo = new System.IO.DirectoryInfo(@"C:\Temp");
            var arquivos = fileInfo.GetFiles();
            var arquivosOrdenados = arquivos.OrderBy(x => x.CreationTime);
            var arq = arquivosOrdenados.Last();
            string nomeArquivo = arq.Name;
            string path = @"c:\Temp\" + nomeArquivo;
            string fille = File.ReadAllText(path);
            string filee = StringSemAcentos(fille);
            string[] lines = filee.Split('\n');
            List<Linha> linhas = new List<Linha>();
            CleanTable();
            foreach (var line in lines)
            {
                if (line.Contains("6088"))
                {
                    string[] obj = line.Split(',');
                    if (obj.Length == 11)
                    {
                        string cartao = obj[2].Replace(" ","");
                        string nome = obj[3];
                        string cpf = obj[5].Replace(".", "");
                        string status = obj[7];
                        string saldoD = obj[8].Replace("\"", ""); ;
                        string saldoA = obj[9].Replace("\"", ""); ;
                        linhas.Add(new Linha
                        {
                            Cartao = cartao,
                            Nome = nome,
                            Cpf = cpf,
                            Status = status,
                            Saldo = saldoD + "," + saldoA
                        });
                    }
                    else if (obj.Length == 12)
                    {
                        string cartao = obj[2].Replace(" ", "");
                        string nome = obj[4];
                        string cpf = obj[6];
                        string status = obj[8];
                        string saldoD = obj[9].Replace("\"", "");
                        string saldoA = obj[10].Replace("\"", "");
                        linhas.Add(new Linha
                        {
                            Cartao = cartao,
                            Nome = nome,
                            Cpf = cpf,
                            Status = status,
                            Saldo = saldoD + "," + saldoA
                        });
                    }
                    else if (obj.Length == 15)
                    {
                        string cartao = obj[2].Replace(" ", "");
                        string nome = obj[4];
                        string cpf = obj[9];
                        string status = obj[11];
                        string saldoD = obj[12].Replace("\"", "");
                        string saldoA = obj[13].Replace("\"", "");
                        linhas.Add(new Linha
                        {
                            Cartao = cartao,
                            Nome = nome,
                            Cpf = cpf,
                            Status = status,
                            Saldo = saldoD + "," + saldoA

                        });
                    }
                    else if (obj.Length == 16)
                    {
                        string cartao = obj[2].Replace(" ", "");
                        string nome = obj[5];
                        string cpf = obj[10];
                        string status = obj[12];
                        string saldoD = obj[13].Replace("\"", "");
                        string saldoA = obj[14].Replace("\"", "");
                        linhas.Add(new Linha
                        {
                            Cartao = cartao,
                            Nome = nome,
                            Cpf = cpf,
                            Status = status,
                            Saldo = saldoD + "," + saldoA
                        });
                    }
                }
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("C:\\Temp\\usuariosGeral.txt"))
            {
                file.Write("Cartao;Cliente;Cpf;Status\n");
                foreach (Linha v in linhas)
                {
                    file.Write(v.Cartao + ',');
                    file.Write(v.Nome + ',');
                    file.Write(v.Cpf + ',');
                    file.Write(v.Status + ',');
                    file.Write(v.Saldo + '\n');
                    Console.WriteLine(v.Cartao + "-" + v.Nome + "-" + v.Cpf + "-" + v.Status + "-" + v.Saldo);
                }
                file.Close();
            }
            System.IO.File.Delete(path);

            foreach (var i in linhas)
            {
                comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
                comm.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO meuvale_infox.usuariosgeral (Cartao, Nome, Cpf, Status, Saldo, Data)" +
                "VALUES('" + i.Cartao + "','" + i.Nome + "','" + i.Cpf + "','" + i.Status + "','" + i.Saldo + "','" + DateTime.Now + "')", comm);
                command.ExecuteNonQuery();
                Console.WriteLine(command);
                comm.Close();
            }

        }
        public string StringSemAcentos(string str)
        {
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã","Ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "A", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }
            string[] caracteresEspeciais = { "\\.", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", "'", ".", "\\" };
            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str = str.Replace(caracteresEspeciais[i], "");
            }
            str = str.Replace("^\\s+", "");
            str = str.Replace("\\s+$", "");
            str = str.Replace("\\s+", " ");
            str = str.TrimStart('"').TrimEnd('"');
            return str;
        }
        private void UploadArqFtp()
        {
            FileInfo arquivoInfo = new FileInfo("C:\\Temp\\usuariosGeral.txt");
            string path = "C:\\Temp\\usuariosGeral.txt";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://ftp.meuvale.com.br/usuarios/usuariosgeral/usuariosGeral.txt"));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("4world@meuvale", "4World123");
            request.UseBinary = true;
            request.ContentLength = arquivoInfo.Length;
            using (FileStream fs = arquivoInfo.OpenRead())
            {
                byte[] buffer = new byte[2048];
                int bytesSent = 0;
                int bytes = 0;
                using (Stream stream = request.GetRequestStream())
                {
                    while (bytesSent < arquivoInfo.Length)
                    {
                        bytes = fs.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, bytes);
                        bytesSent += bytes;
                    }
                }
            }
            System.IO.File.Delete(path);
        }
        private void CleanTable()
        {
            comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
            comm.Open();
            MySqlCommand comman = new MySqlCommand("TRUNCATE `meuvale_infox`.`usuariosgeral`;", comm);
            comman.ExecuteNonQuery();
            comm.Close();
        }
    }
}