using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuValeFwcardGeral
{
    public class Linha
    {
        public string Cartao { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Status { get; set; }
        public string Saldo { get; set; }
        public Linha(string cartao, string nome, string cpf, string status, string saldo)
        {
            Cartao = cartao;
            Nome = nome;
            Cpf = cpf;
            Status = status;
            Saldo = saldo;
        }

        public Linha()
        {
        }
    }
}
