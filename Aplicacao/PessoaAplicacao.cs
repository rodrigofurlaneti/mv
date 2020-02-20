using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Ajax.Utilities;

namespace Aplicacao
{
    public interface IPessoaAplicacao : IBaseAplicacao<Pessoa>
    {
        void Registrar(Pessoa pessoa);
        List<Pessoa> Filtrar(PessoaFiltroViewModel filtros);
    }

    public class PessoaAplicacao : BaseAplicacao<Pessoa, IPessoaServico>, IPessoaAplicacao
    {
        public void Registrar(Pessoa pessoa)
        {
            Servico.ToCast<IPessoaServico>().Registrar(pessoa);
        }

        public List<Pessoa> Filtrar(PessoaFiltroViewModel filtros)
        {
            var pessoas = Servico.Buscar();
            var pessoasAux = new List<Pessoa>();

            //pessoas = pessoas.Where(x => x.Idade >= filtros.IdadeDe && x.Idade <= (filtros.IdadeAte > 0 ? filtros.IdadeAte : 2100)).ToList();

            if (filtros.Sexo != Sexo.Todos)
                pessoas = pessoas.Where(x => string.Equals(x.Sexo, filtros.Sexo.ToDescription(), StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (filtros.Cidade.Any(c => c > 0))
                filtros.Cidade.Where(c => c > 0).ToList().ForEach(c =>
                {
                    pessoasAux.AddRange(pessoas.Where(p => p.EnderecosEntrega.Any(en => en.Cidade.Id == c)).ToList());
                });
            else
                pessoasAux.AddRange(pessoas);

            filtros.Estado.Where(e => e > 0).ToList().ForEach(e =>
            {
                pessoasAux.AddRange(pessoas.Where(p => p.EnderecosEntrega.Any(en => en.Cidade.Estado.Id == e)).ToList());
            });

            return pessoasAux.DistinctBy(x => x.Id).ToList();
        }
    }
}