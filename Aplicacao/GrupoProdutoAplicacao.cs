using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.ViewModels;

namespace Aplicacao
{
    public interface IGrupoProdutoAplicacao : IBaseAplicacao<GrupoProduto>
    {
        IEnumerable<GrupoProdutoViewModel> Listar();
        IEnumerable<GrupoProdutoViewModel> Listar(int idSecao);
    }

    public class GrupoProdutoAplicacao : BaseAplicacao<GrupoProduto, IGrupoProdutoServico>, IGrupoProdutoAplicacao
    {
        private readonly IGrupoProdutoServico _servico;
        public GrupoProdutoAplicacao(IGrupoProdutoServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(GrupoProduto entity)
        {
            Servico.Salvar(entity);
        }

        public IEnumerable<GrupoProdutoViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new GrupoProdutoViewModel(x)).ToList();
        }

        public IEnumerable<GrupoProdutoViewModel> Listar(int idSecao)
        {
            return _servico.Buscar().Where(x => x.Secao.Id == idSecao).Select(x => new GrupoProdutoViewModel(x)).ToList();
        }
    }
}