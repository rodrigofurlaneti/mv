using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeuValeIFwcardCartaoUsuarioProduto
{
    public class CartaoUsuarioProduto
    {
        public void Start()
        {
            SelectTxt();
            SendTxtFtp();
            SaveBd();
        }
        public void SelectTxt()
        {
            MySqlConnection comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
            MySqlCommand cmd = new MySqlCommand("SELECT `meuvale_infox`.`cartaoproduto`.`Cartao`, `meuvale_infox`.`cartaoproduto`.`Nome`, `meuvale_infox`.`usuariosgeral`.`Cpf`, `meuvale_infox`.`cartaoproduto`.`Status`, `meuvale_infox`.`cartaoproduto`.`Produto`, `meuvale_infox`.`usuariosgeral`.`Saldo` FROM `meuvale_infox`.`cartaoproduto` JOIN `meuvale_infox`.`usuariosgeral` ON `meuvale_infox`.`cartaoproduto`.`cartao` = `meuvale_infox`.`usuariosgeral`.`Cartao`;", comm);
            cmd.CommandTimeout = 999999;                
            comm.Open();
            MySqlDataReader rd;
            rd = cmd.ExecuteReader();
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Temp\\cartoes.txt"))
                {
                    file.Write("Cartao;Cliente;Cpf;Status;Produto;Saldo\n");
                    int cont = 0;
                    while (rd.Read())
                    {
                        Console.WriteLine(cont.ToString()+"||"+rd.GetString("Produto") + "||" + rd.GetString("Status") + "||" + rd.GetString("Saldo"));
                        file.Write(rd.GetString("Cartao") + ';');
                        file.Write(StringSemAcentos(rd.GetString("Nome")) + ';');
                        file.Write(rd.GetString("Cpf") + ';');
                        file.Write(StringSemAcentos(rd.GetString("Status")) + ';');
                        file.Write(StringSemAcentos(rd.GetString("Produto")) + ';');
                        file.Write(rd.GetString("Saldo") + '\n');

                        cont++;
                    }
                    file.Close();
                }

            }
            finally
            {
                rd.Close();
                comm.Close();
            }
        }
        
        public void SaveBd()
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
            List<CartaoUP> linhas = new List<CartaoUP>();
            CleanTable();
            foreach (var line in lines)
            {
                if (line.Contains("6088"))
                {
                    string[] obj = line.Split(';');
                    if (obj.Length == 6)
                    {
                        string cartao = obj[0];
                        string nome = obj[1];
                        string cpf = obj[2];
                        string status = obj[3];
                        string produto = obj[4];
                        string saldo = obj[5];
                        linhas.Add(new CartaoUP
                        {
                            Cartao = cartao,
                            Nome = nome,
                            Cpf = cpf,
                            Status = status,
                            Produto = produto,
                            Saldo = saldo
                        });
                    }
                }
            }
            int cont = 0;
            foreach (var i in linhas)
            {
                MySqlConnection com = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
                com.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO meuvale_infox.cartaousuarioproduto (Cartao, Nome, Cpf, Status, Saldo, Produto, Data)" +
                "VALUES('" + i.Cartao + "','" + StringSemAcentos(i.Nome) + "','" + i.Cpf + "','" + StringSemAcentos(i.Status) + "','" + i.Saldo + "','" + StringSemAcentos(i.Produto) + "','" + DateTime.Now + "')", com);
                command.ExecuteNonQuery();
                Console.WriteLine(cont);
                com.Close();
                cont++;
            }
        }
    public void SendTxtFtp()
        {
            FileInfo arqInfo = new FileInfo("C:\\Temp\\cartoes.txt");
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri("ftp://ftp.meuvale.com.br/usuarios/cartoes/cartoes.txt"));
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential("4world@meuvale", "4World123");
            req.UseBinary = true;
            req.ContentLength = arqInfo.Length;
            using (FileStream fs = arqInfo.OpenRead())
            {
                byte[] buffer = new byte[2048];
                int bytesSent = 0;
                int bytes = 0;
                using (Stream stream = req.GetRequestStream())
                {
                    while (bytesSent < arqInfo.Length)
                    {
                        bytes = fs.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, bytes);
                        bytesSent += bytes;
                    }
                }
            }
        }
        public string StringSemAcentos(string str)
        {
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "Ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
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
        private void CleanTable()
        {
            MySqlConnection comm = new MySqlConnection("Persist Security Info=False; server=54.233.179.247;database=meuvale_infox;uid=meuvale;password=meuvale@123");
            comm.Open();
            MySqlCommand comman = new MySqlCommand("TRUNCATE `meuvale_infox`.`cartaousuarioproduto`;", comm);
            comman.ExecuteNonQuery();
            comm.Close();
        }
    }
}
