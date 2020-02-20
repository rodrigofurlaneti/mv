using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System;
using System.Linq;
using MySql.Data.MySqlClient;
using System.IO.Compression;

namespace Meuvale.I.Fwcard.CartaoPorProduto
{
    public class Cartao
    {
        public IWebElement MenuProducao { get; set; }
        public IWebElement Relatorio { get; set; }
        public IWebElement OpCartao { get; set; }
        public IWebElement Geral { get; set; }
        public IWebElement Situacao { get; set; }
        public MySqlConnection comm { get; set; }
        public Cartao()
        {
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
        public void Iniciar(IWebDriver driver, string tp)
        {
            Autenticar(driver);
            AbaProducao(driver, tp);
            SerealizarCsv(tp);
            UploadArqFtp();
        }

        public void AbaProducao(IWebDriver driver, string tp)
        {
            this.MenuProducao = driver.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(40000);
            ReadOnlyCollection<string> originalHandles = driver.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver.SwitchTo().Window(popupWindowHandle);
            driver.Manage().Window.Maximize();
            Actions builder = new Actions(driver);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(180000);
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
            Thread.Sleep(180000);
            driver.Close();
        }
        public void SerealizarCsvEx(string tp)
        {
            Console.WriteLine("_SerealizarCsvEx()");
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
            int id = 1;
            foreach (var line in lines)
            {
                int tamanhoLinha = line.Length;
                if (line.Contains("6088"))
                {
                    string[] obj = line.Split(',');
                    if (obj.Length == 12)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[2];
                        string usuario = obj[3];
                        string empresa = obj[5];
                        string status = obj[7];
                        string origem = obj[9];
                        string data = obj[10];
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = DateTime.Now.ToString()
                    });
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }
                    else if (obj.Length == 13)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[3];
                        string usuario = obj[4];
                        string empresa = obj[6];
                        string status = obj[8];
                        string origem = obj[10];
                        string data = obj[11];
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = DateTime.Now.ToString()
                    });
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }
                    else if (obj.Length == 15)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[2];
                        string usuario = obj[3];
                        string empresa = obj[5];
                        string status = obj[7];
                        string origem = obj[9];
                        string data = obj[13];
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = DateTime.Now.ToString()
                    });
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }
                    else if (obj.Length == 16)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[3];
                        string usuario = obj[4];
                        string empresa = obj[6];
                        string status = obj[8];
                        string origem = obj[10];
                        string data = obj[14];
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = DateTime.Now.ToString()
                    });
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }
                }
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter("C:\\Temp\\cartoesproduto.txt", true))
            {
                foreach (Linha v in linhas)
                {
                    file.Write(v.Cartao + ';');
                    file.Write(v.Status + ';');
                    file.Write(v.Usuario + ';');
                    file.Write(v.Empresa + ';');
                    file.Write(v.Origem + ';');
                    file.Write(v.Produto + ';');
                    file.Write(v.Data + '\n');
                    Console.WriteLine(v.Cartao + "|" + v.Status + "|" + v.Usuario + "|" + v.Empresa + "|" + v.Origem + "|" + v.Produto + "|" + v.Data + "|");
                }
                file.Close();
            }
            int cont = 0;
            foreach (Linha i in linhas)
            {
                comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
                comm.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO meuvale_infox.cartaoproduto (Cartao, Status, Nome, Empresa, Origem, Produto, Data)" +
                "VALUES('" + i.Cartao + "','" + i.Status + "','" + i.Usuario + "','" + i.Empresa + "','" + i.Origem + "','" + i.Produto + "','" + i.Data + "')", comm);
                command.ExecuteNonQuery();
                cont++;
                string[] o = command.CommandText.ToString().Split(',');
                Console.WriteLine(cont.ToString() + "| |" + o[11].ToString() + "| |");
                comm.Close();
            }
            File.Delete(path);
        }
        public void SerealizarCsv(string tp)
        {
            Console.WriteLine("_SerealizarCsv()");
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
            int id = 1;
            foreach (var line in lines)
            {
                int tamanhoLinha = line.Length;
                if (line.Contains("6088"))
                {
                    string[] obj = line.Split(',');
                    if (obj.Length == 12)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[2];
                        string usuario = obj[3];
                        string empresa = obj[5];
                        string status = obj[7];
                        string origem = obj[9];
                        string data = DateTime.Now.ToString();
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = data
                        });
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }else if (obj.Length == 13)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[3];
                        string usuario = obj[4];
                        string empresa = obj[6];
                        string status = obj[8];
                        string origem = obj[10];
                        string data = DateTime.Now.ToString();
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = data
                        });
                        cartao = ""; codigo = "";  usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }else if (obj.Length == 15)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[2];
                        string usuario = obj[3];
                        string empresa = obj[5];
                        string status = obj[7];
                        string origem = obj[9];
                        string data = DateTime.Now.ToString();
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = data
                        });
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }else if (obj.Length == 16)
                    {
                        string cartao = obj[1].Replace(" ", "");
                        string codigo = obj[3];
                        string usuario = obj[4];
                        string empresa = obj[6];
                        string status = obj[8];
                        string origem = obj[10];
                        string data = DateTime.Now.ToString();
                        linhas.Add(new Linha()
                        {
                            Id = id,
                            Cartao = cartao,
                            Codigo = codigo,
                            Usuario = usuario,
                            Empresa = empresa,
                            Status = status,
                            Produto = tp,
                            Origem = origem,
                            Data = data
                        }) ;
                        cartao = ""; codigo = ""; usuario = ""; empresa = ""; status = ""; origem = ""; data = "";
                        id++;
                    }
                }
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter("C:\\Temp\\cartoesproduto.txt"))
            {
                foreach (Linha v in linhas)
                {
                    file.Write(v.Cartao + ';');
                    file.Write(v.Status + ';');
                    file.Write(v.Usuario + ';');
                    file.Write(v.Empresa + ';');
                    file.Write(v.Origem + ';');
                    file.Write(v.Produto + ';');
                    file.Write(v.Data + '\n');
                    Console.WriteLine(v.Cartao + "|" + v.Status + "|" + v.Usuario + "|" + v.Empresa + "|" + v.Origem + "|" + v.Produto + "|" + v.Data + "|");
                }
                file.Close();
            }
            int cont = 0;
            foreach (Linha i in linhas)
            {
                comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
                comm.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO meuvale_infox.cartaoproduto (Cartao, Status, Nome, Empresa, Origem, Produto, Data)" +
                "VALUES('" + i.Cartao + "','" + i.Status + "','" + i.Usuario + "','" + i.Empresa + "','" + i.Origem + "','" + i.Produto + "','" + i.Data + "')", comm);
                command.ExecuteNonQuery();
                cont++;
                string[] o = command.CommandText.ToString().Split(',');
                Console.WriteLine(cont.ToString()+"| |"+o[11].ToString() + "| |");
                comm.Close();
            }
            File.Delete(path);
        }
        // MV REFEIÇÃO
        public void Iniciar2(IWebDriver driver2, string tp)
        {
            Autenticar(driver2);
            AbaProducao2(driver2, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao2(IWebDriver driver2, string tp)
        {
            this.MenuProducao = driver2.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver2.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver2.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver2.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver2.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver2.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver2.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver2.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver2.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver2.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver2.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver2.SwitchTo().Window(popupWindowHandle);
            driver2.Manage().Window.Maximize();
            Actions builder = new Actions(driver2);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver2);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver2);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver2);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver2.Close();
        }
        // MV FARMACIA
        public void Iniciar3(IWebDriver driver3, string tp)
        {
            Autenticar(driver3);
            AbaProducao3(driver3, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao3(IWebDriver driver3, string tp)
        {
            this.MenuProducao = driver3.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver3.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver3.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver3.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver3.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver3.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver3.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver3.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver3.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver3.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(10000);
            ReadOnlyCollection<string> originalHandles = driver3.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver3.SwitchTo().Window(popupWindowHandle);
            driver3.Manage().Window.Maximize();
            Actions builder = new Actions(driver3);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(10000);
            for (int i = 0; i < 11; i++)
            {
                Actions tab = new Actions(driver3);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver3);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver3);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(10000);
            driver3.Close();
        }
        // MV ODONTO
        public void Iniciar4(IWebDriver driver4, string tp)
        {
            Autenticar(driver4);
            AbaProducao4(driver4, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao4(IWebDriver driver4, string tp)
        {
            this.MenuProducao = driver4.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver4.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver4.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver4.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver4.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver4.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver4.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver4.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver4.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver4.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver4.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver4.SwitchTo().Window(popupWindowHandle);
            driver4.Manage().Window.Maximize();
            Actions builder = new Actions(driver4);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver4);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver4);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver4);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver4.Close();
        }
        // MV ALIMENTAÇÃO
        public void Iniciar5(IWebDriver driver5, string tp)
        {
            Autenticar(driver5);
            AbaProducao5(driver5, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao5(IWebDriver driver5, string tp)
        {
            this.MenuProducao = driver5.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver5.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver5.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver5.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver5.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver5.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver5.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver5.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver5.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver5.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver5.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver5.SwitchTo().Window(popupWindowHandle);
            driver5.Manage().Window.Maximize();
            Actions builder = new Actions(driver5);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver5);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver5);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver5);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver5.Close();
        }
        // MV ADTO SALARIAL - STA IZABEL - PARÁ
        public void Iniciar6(IWebDriver driver6, string tp)
        {
            Autenticar(driver6);
            AbaProducao6(driver6, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao6(IWebDriver driver6, string tp)
        {
            this.MenuProducao = driver6.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver6.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver6.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver6.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver6.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver6.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver6.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver6.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver6.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver6.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver6.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver6.SwitchTo().Window(popupWindowHandle);
            driver6.Manage().Window.Maximize();
            Actions builder = new Actions(driver6);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver6);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver6);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver6);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver6.Close();
        }
        // MV ADTO SALARIAL - ELDORADO DO CARAJÁS -PARÁ
        public void Iniciar7(IWebDriver driver7, string tp)
        {
            Autenticar(driver7);
            AbaProducao7(driver7, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao7(IWebDriver driver7, string tp)
        {
            this.MenuProducao = driver7.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver7.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver7.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver7.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver7.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver7.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver7.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver7.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver7.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver7.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver7.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver7.SwitchTo().Window(popupWindowHandle);
            driver7.Manage().Window.Maximize();
            Actions builder = new Actions(driver7);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver7);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver7);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver7);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver7.Close();
        }
        // MV BENF COMBUSTÍVEL
        public void Iniciar8(IWebDriver driver8, string tp)
        {
            Autenticar(driver8);
            AbaProducao8(driver8, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao8(IWebDriver driver8, string tp)
        {
            this.MenuProducao = driver8.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver8.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver8.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver8.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver8.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver8.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver8.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver8.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver8.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver8.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver8.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver8.SwitchTo().Window(popupWindowHandle);
            driver8.Manage().Window.Maximize();
            Actions builder = new Actions(driver8);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver8);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver8);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver8);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver8.Close();
        }
        // MV CONV. COMBUSTÍVEL
        public void Iniciar9(IWebDriver driver9, string tp)
        {
            Autenticar(driver9);
            AbaProducao9(driver9, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao9(IWebDriver driver9, string tp)
        {
            this.MenuProducao = driver9.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver9.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver9.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver9.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver9.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver9.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver9.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver9.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver9.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver9.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver9.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver9.SwitchTo().Window(popupWindowHandle);
            driver9.Manage().Window.Maximize();
            Actions builder = new Actions(driver9);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver9);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver9);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver9);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver9.Close();
        }
        // MV CONV. COMBUSTÍVEL
        public void Iniciar10(IWebDriver driver10, string tp)
        {
            Autenticar(driver10);
            AbaProducao10(driver10, tp);
            SerealizarCsvEx(tp);
        }
        public void AbaProducao10(IWebDriver driver10, string tp)
        {
            this.MenuProducao = driver10.FindElement(By.XPath("//*[@id='jqmenu-17']/span/span[2]"));
            this.MenuProducao.Click();
            this.Relatorio = driver10.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/a"));
            this.Relatorio.Click();
            this.OpCartao = driver10.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/a/span[1]"));
            this.OpCartao.Click();
            Thread.Sleep(250);
            this.Situacao = driver10.FindElement(By.XPath("//*[@id='P0_MENU_17_DISPLAY']/div/ul/li[7]/ul/li[2]/ul/li[4]"));
            this.Situacao.Click();
            Thread.Sleep(250);
            driver10.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Click();
            driver10.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).Clear();
            driver10.FindElement(By.XPath("//*[@id='P1040_DATA_INICIAL']")).SendKeys("01/01/1980");
            driver10.FindElement(By.XPath("//*[@id='P1040_ID_PRODUTO']")).SendKeys(tp);
            driver10.FindElement(By.XPath("//*[@id='P1040_SOMENTE_PRIMEIRA_COMPRA']")).SendKeys("NÃO");
            driver10.FindElement(By.XPath("//*[@id='OK']/span")).Click();
            Thread.Sleep(20000);
            ReadOnlyCollection<string> originalHandles = driver10.WindowHandles;
            string popupWindowHandle = originalHandles[1];
            driver10.SwitchTo().Window(popupWindowHandle);
            driver10.Manage().Window.Maximize();
            Actions builder = new Actions(driver10);
            builder.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(20000);
            for (int i = 0; i < 12; i++)
            {
                Actions tab = new Actions(driver10);
                tab.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(250);
            }
            Actions tabE = new Actions(driver10);
            tabE.SendKeys(Keys.Enter).Perform();
            Thread.Sleep(250);
            Actions baixar = new Actions(driver10);
            baixar.MoveByOffset(1203, 80).Click().Build().Perform();
            Thread.Sleep(20000);
            driver10.Close();
        }

        //private string LerDirFtp()
        //{
        //    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri("ftp://ftp.meuvale.com.br/usuarios/cartoesproduto/"));
        //    req.Method = WebRequestMethods.Ftp.ListDirectory;
        //    req.Credentials = new NetworkCredential("4world@meuvale", "4World123");
        //    StreamReader srr = new StreamReader(req.GetResponse().GetResponseStream());
        //    string str = srr.ReadLine();
        //    return str;
        //}
        //private void DelArqFtp()
        //{
        //    FtpWebRequest reqDel = (FtpWebRequest)WebRequest.Create(new Uri("ftp://ftp.meuvale.com.br/usuarios/cartoes/cartoesproduto.csv"));
        //    reqDel.Credentials = new NetworkCredential("4world@meuvale", "4World123");
        //    reqDel.Method = WebRequestMethods.Ftp.DeleteFile;
        //    FtpWebResponse response = (FtpWebResponse)reqDel.GetResponse();
        //    UploadArqFtp();
        //}
        private void UploadArqFtp()
        {
            var fileInfo = new System.IO.DirectoryInfo(@"C:\Temp");
            var arquivos = fileInfo.GetFiles();
            var arquivosOrdenados = arquivos.OrderBy(x => x.CreationTime);
            var arq = arquivosOrdenados.Last();
            string nomeArquivo = arq.Name;
            string path = @"c:\Temp\" + nomeArquivo;
            FileInfo arquivoInfo = new FileInfo(path);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://ftp.meuvale.com.br/usuarios/cartoesproduto/cartoesproduto.txt"));
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
            File.Delete(path);
        }
        public string StringSemAcentos(string str)
        {
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "Ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "A", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }
            string[] caracteresEspeciais = { "\\.", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°" };
            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str = str.Replace(caracteresEspeciais[i], "");
            }
            str = str.Replace("^\\s+", "");
            str = str.Replace("\\s+$", "");
            str = str.Replace("\\s+", "");
            str = str.Replace(".", "");
            return str;
        }
        public void CleanTable()
        {
            comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
            comm.Open();
            MySqlCommand command = new MySqlCommand("TRUNCATE `meuvale_infox`.`cartaoproduto`;", comm);
            command.ExecuteNonQuery();
            comm.Close();
        }
    }
}
