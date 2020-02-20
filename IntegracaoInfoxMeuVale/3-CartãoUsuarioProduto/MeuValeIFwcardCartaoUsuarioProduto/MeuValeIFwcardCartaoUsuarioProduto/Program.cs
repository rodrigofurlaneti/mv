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
    class Program
    {
        static void Main(string[] args)
        {
            CartaoUsuarioProduto cartaoUsuarioProduto = new CartaoUsuarioProduto();
            cartaoUsuarioProduto.Start();
        }
    }
}
