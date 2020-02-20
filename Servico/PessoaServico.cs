using System;
using System.Configuration;
using System.Linq;
using Core.Extensions;
using Core.Validators;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace Dominio
{
    public interface IPessoaServico : IBaseServico<Pessoa>
    {
        void Registrar(Pessoa pessoa);
        void ValidaPessoa(Pessoa pessoa);
        Pessoa ObterPorCartao(string numero);
    }

    public class PessoaServico : BaseServico<Pessoa, IPessoaRepositorio>, IPessoaServico
    {
        public void Registrar(Pessoa pessoa)
        {
            var contatoServico = ServiceLocator.Current.GetInstance<IContatoServico>();
            var documentoServico = ServiceLocator.Current.GetInstance<IDocumentoServico>();

            foreach (var contato in pessoa.Contatos)
                contatoServico.Salvar(contato.Contato);

            foreach (var documento in pessoa.Documentos)
                documentoServico.Salvar(documento);

            var pessoaServico = ServiceLocator.Current.GetInstance<IPessoaServico>();
            pessoaServico.Salvar(pessoa);
        }

        public void ValidaPessoa(Pessoa pessoa)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();

            var contatoEmail = pessoa.Contatos.FirstOrDefault(x => x.Contato.Tipo.Equals(TipoContato.Email));
            if (contatoEmail == null || !Validators.IsValidEmail(contatoEmail.Contato.Email))
                throw new Exception("Email inválido");
        }

        public Pessoa ObterPorCartao(string numero)
        {
            numero = numero.Replace(" ", "");
            var numberEncrypted = Crypt.EnCrypt(numero, ConfigurationManager.AppSettings["CryptKey"]);
            numero = string.Format("{0} {1} {2} {3}", 
                numero.Substring(0, 4), 
                numero.Substring(Math.Min(4, numero.Length), 4), 
                numero.Substring(Math.Min(8, numero.Length), 4), 
                numero.Substring(Math.Min(12, numero.Length), 4));
            var numberFormattedEncrypted = Crypt.EnCrypt(numero, ConfigurationManager.AppSettings["CryptKey"]);
            return Repositorio.FirstBy(p => p.Cartoes.Any(c => 
                (c.Numero == numberEncrypted && c.IsEncrypted) || 
                (c.Numero == numberFormattedEncrypted && c.IsEncrypted) ||
                (c.Numero == numero && !c.IsEncrypted)));
        }
    }
}