using System;
using System.Collections.Generic;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Core.Extensions;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;

namespace Dominio
{
    public interface ICartaoServico : IBaseServico<Cartao>
    {
        void ValidaESalva(Cartao cartao);
        IList<Cartao> GetByPessoa(int pessoaId);
        Cartao BuscarPorNumero(string numero);
        void Valida(Cartao cartao); 
        IList<Cartao> DescriptografarCartoes(IList<Cartao> cartoes);
        Cartao DescriptografarCartao(Cartao cartao);
        void EncryptAllCreditCard();
        Cartao Criptografar(Cartao cartao);
        IList<Cartao> GetCartoesParcialBloqPorPessoa(int pessoaId);
    }

    public class CartaoServico : BaseServico<Cartao, ICartaoRepositorio>, ICartaoServico
    {
        public void Valida(Cartao cartao)
        {
            try
            {
                if (cartao.Numero.Length < 16)
                    throw new Exception("Informe o número do cartão corretamente.");

                if (string.IsNullOrWhiteSpace(cartao.Validade))
                    throw new Exception("Informe uma data de validade.");

                DateTime validade;
                var dataCorreta = cartao.Validade.Length <= 5 ? cartao.Validade.Insert(3, "20") : cartao.Validade;
                DateTime.TryParse(dataCorreta, out validade);
                if (validade == DateTime.MinValue || (validade.Year < DateTime.Now.Year || (validade.Year == DateTime.Now.Year && validade.Month < DateTime.Now.Month)))
                    throw new Exception("Informe um cartão dentro do prazo de validade.");
                cartao.Validade = validade.ToString("MM/yy");

                if (cartao.Cvv.Length != 3 && cartao.Cvv.Length != 4)
                    throw new Exception("Informe o código de verificação corretamente.");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public void ValidaESalva(Cartao cartao)
        {
            Valida(cartao);
            Salvar(Criptografar(cartao));
        }

        public IList<Cartao> GetByPessoa(int pessoaId)
        {
            return DesCriptografar(BuscarPor(x => x.Pessoa.Id.Equals(pessoaId)));
        }

        public IList<Cartao> GetCartoesParcialBloqPorPessoa(int pessoaId)
        {
            var cartoes = DesCriptografar(BuscarPor(x => x.Pessoa.Id.Equals(pessoaId)));
            foreach (var card in cartoes)
            {
                card.Numero = card.Numero.Substring(0, 4) + "XXXXXXXX" + card.Numero.Substring(12, 4);
                card.Cvv = "XXX";
            }
            return cartoes;
        }

        public Cartao Criptografar(Cartao cartao)
        {
            cartao.Cvv = Crypt.EnCrypt(cartao.Cvv, ConfigurationManager.AppSettings["CryptKey"]);
            cartao.Numero = Crypt.EnCrypt(cartao.Numero, ConfigurationManager.AppSettings["CryptKey"]);
            cartao.IsEncrypted = true;
            return cartao;
        }

        public Cartao DesCriptografar(Cartao cartao)
        {
            if (!cartao.IsEncrypted || cartao.Decrypted) return cartao;

            cartao.Cvv = Crypt.DeCrypt(cartao.Cvv, ConfigurationManager.AppSettings["CryptKey"]);
            cartao.Numero = Crypt.DeCrypt(cartao.Numero, ConfigurationManager.AppSettings["CryptKey"]);
            cartao.Decrypted = true;
            return cartao;
        }

        public IList<Cartao> DesCriptografar(IList<Cartao> cartoes)
        {
            if (cartoes == null || !cartoes.Any()) return cartoes;

            cartoes.ToList().ForEach(x => DesCriptografar(x));

            return cartoes;
        }

        public new IList<Cartao> Buscar()
        {
            return DesCriptografar(Repositorio.List());
        }

        public new Cartao BuscarPorId(int id)
        {
            return DesCriptografar(Repositorio.GetById(id));
        }

        public Cartao BuscarPorNumero(string numero)
        {
            var numberEncrypted = Crypt.EnCrypt(numero, ConfigurationManager.AppSettings["CriptKey"]);
            return PrimeiroPor(c => (c.Numero == numberEncrypted && c.IsEncrypted) || (c.Numero == numero && !c.IsEncrypted));
        }

        public new Cartao PrimeiroPor(Expression<Func<Cartao, bool>> query)
        {
            return DesCriptografar(Repositorio.FirstBy(query));
        }

        public new IList<Cartao> BuscarPor(Expression<Func<Cartao, bool>> query)
        {
            return DesCriptografar(Repositorio.ListBy(query));
        }

        public new IList<Cartao> BuscarPorIntervalo(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy)
        {
            return DesCriptografar(Repositorio.ListByInterval(firstResult, maxResult, entityOrderBy, columnOrderBy));
        }

        public IList<Cartao> DescriptografarCartoes(IList<Cartao> cartoes)
        {
            return DesCriptografar(cartoes);
        }

        public Cartao DescriptografarCartao(Cartao cartao)
        {
            return DesCriptografar(cartao);
        }

        public void EncryptAllCreditCard()
        {
            using (var transaction = Repositorio.SetupNewTransaction())
            {
                BuscarPor(c => !c.IsEncrypted).ToList()
                    .ForEach(c =>
                    {
                        c.Cvv = Crypt.EnCrypt(c.Cvv, ConfigurationManager.AppSettings["CryptKey"]);
                        c.Numero = Crypt.EnCrypt(c.Numero, ConfigurationManager.AppSettings["CryptKey"]);
                        c.IsEncrypted = true;
                    });

                transaction.Commit();
                transaction.Dispose();
            }
        }
    }
}