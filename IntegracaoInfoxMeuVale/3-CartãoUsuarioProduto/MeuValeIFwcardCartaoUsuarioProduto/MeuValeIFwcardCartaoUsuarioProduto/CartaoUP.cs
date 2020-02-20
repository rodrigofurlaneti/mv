using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuValeIFwcardCartaoUsuarioProduto
{
    public class CartaoUP
    {
        public string Cartao { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Status { get; set; }
        public string Produto { get; set; }
        public string Saldo { get; set; }

        public CartaoUP(string cartao, string nome, string cpf, string status, string produto, string saldo)
        {
            Cartao = cartao;
            Nome = nome;
            Cpf = cpf;
            Status = status;
            Produto = produto;
            Saldo = saldo;
        }

        public CartaoUP()
        {
        }
    }
}
