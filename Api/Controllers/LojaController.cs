using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Core.Exceptions;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [AllowAnonymous]
    //[EnableCors(origins: "http:/localhost:8100", headers: "*", methods: "*")]
    [RoutePrefix("api/loja")]
    public class LojaController : BaseController<Loja, ILojaServico>
    {
        [HttpGet]
        [Route("lojasProximas")]
        public IEnumerable<LojaModelView> GetLojas(float latitude, float longitude, int inicio, int quantidade)
        {
            var lojas = new List<LojaModelView>();

            var lojasBase = Servico.BuscarLojas();

            foreach (var loja in lojasBase)
            {
                //loja.HorarioFuncionamento = Servico.BuscarHorarioFuncionamento(loja.Id);
                lojas.Add(new LojaModelView(loja, latitude, longitude));
            }

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("lojasProximasPorClassificacao")]
        public IEnumerable<LojaModelView> GetLojas(float latitude, float longitude, int inicio, int quantidade, string classificacao)
        {
            var lojas = new List<LojaModelView>();

            var lojasBase = Servico.BuscarLojas(classificacao);

            foreach (var loja in lojasBase)
            {
                lojas.Add(new LojaModelView(loja, latitude, longitude));
            }

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("{id}/produtos")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int id, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.Loja.Id == id).Skip(inicio).Take(quantidade))
            {
                produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
            }
            //
            return produtos;
        }

        [HttpGet]
        [Route("{id}/produtoCodBarras")]
        public ProdutoPrecoModelView GetProdutoPorCodigoBarras(int id, string codBarras)
        {
            var produto = Servico.BuscaProdutoPorCodBarras(id, codBarras);
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            if (produto == null)
                return null;

            var preco = produtoPrecoBusiness.GetByLojaProduto(id, produto.Id);
            return new ProdutoPrecoModelView(produto, preco);
        }

        [HttpGet]
        [Route("lojasUsuario")]
        public IEnumerable<LojaModelView> GetLojasUsuario(int pessoa, float latitude, float longitude)
        {
            var usr = Servico.RecuperaUsuarioPorPessoa(pessoa);
            var lojas = usr.ListaUsuarioLoja.Select(item => new LojaModelView(item.Loja, latitude, longitude)).ToList();
            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia);
        }

        [HttpGet]
        [Route("buscaLojaPorCNPJ/{cnpj}")]
        public LojaModelView GetLojaByCNPJ(string cnpj)
        {
            var loja = Servico.BuscarPor(x => x.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Equals(cnpj)).FirstOrDefault();
            return loja == null ? null : new LojaModelView(loja);
        }

        [HttpGet]
        [Route("tiposClassificacao")]
        public IEnumerable<ChaveValorModelView> GetTiposClassificacao()
        {
            return Enum.GetValues(typeof(TipoClassificacao)).Cast<TipoClassificacao>().Select(x => new ChaveValorModelView { Id = (int)x, Descricao = x.ToDescription() });
        }

        [HttpGet]
        [Route("classificacoes/{tipo}")]
        public IEnumerable<ChaveValorModelView> GetClassificacoes(int tipo)
        {
            return Enum.GetValues(typeof(ClassificacaoLoja)).Cast<ClassificacaoLoja>().Select(x => new ChaveValorModelView { Id = (int)x, Descricao = x.ToDescription() });
        }

        [HttpPost]
        [Route("salvarLoja/{id}")]
        public virtual void SalvarLoja(int id, [FromBody] Loja loja)
        {
            try
            {
                Servico.ValidaESalva(id, loja);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("salvarLojaIndicacao")]
        public virtual void SalvarLojaIndicacao([FromBody] Loja loja)
        {
            try
            {
                Servico.SalvarLojaIndicacao(loja);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("salvarProdutoPrecoLoja/{id}")]
        public virtual void SalvarProdutoPrecoLoja(int id, [FromBody] ProdutoPreco produtoPreco)
        {
            try
            {
                Servico.SalvarProdutoPrecoLoja(id, produtoPreco);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("LojasPorNome")]
        public IEnumerable<LojaModelView> GetLojasPorNome(string nome,int inicio, int quantidade)
        {
            var lojas = new List<LojaModelView>();

            if (quantidade < 0)
                quantidade = 0;

            var lojasBase = Servico.BuscarLojasPorNome(nome);

            foreach (var loja in lojasBase)
            {
                lojas.Add(new LojaModelView(loja, 0, 0));
            }

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("LojasPorNomeDocumentoOuCidade")]
        public IEnumerable<LojaModelView> GetLojasPorNomeDocumentoOuCidade(int estado, int cidade, string bairro, string dadosPesquisa, int lote, int inicio, int quantidade)
        {
            var lojas = new List<LojaModelView>();

            if (quantidade < 0)
                quantidade = 0;

            var lojasBase = Servico.BuscaLojasPor(estado, cidade, bairro, dadosPesquisa);

            foreach (var loja in lojasBase)
            {
                for (int i = 0; i < lote; i++)
                {
                    lojas.Add(new LojaModelView(loja, 0, 0));
                }
            }

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("TipoEstabelecimentoPorNome")]
        public IEnumerable<LojaModelView> GetLojasPorTipoPorNome(string nome)
        {
            var lojas = new List<LojaModelView>();

            var lojasBase = Servico.BuscarLojasPorTipoPorNome(nome);
            
            return lojas.OrderBy(x => x.Descricao);
        }

        [HttpGet]
        [Route("obterTerminal/{serial}")]
        public TerminalModelView ObterPorTerminal (string serial)
        {
            var terminalServico = ServiceLocator.Current.GetInstance<ITerminalCobrancaLojaServico>();
            TerminalCobrancaLoja terminalLoja;
            try
            {
                terminalLoja = terminalServico.PrimeiroPor(tc => tc.Terminal.NumeroSerial.Trim().ToLower() == serial.Trim().ToLower());
                if (terminalLoja == null)
                    throw new BusinessRuleException("Não foi possível encontrar o terminal solicitado!");
            }
            catch (Exception)
            {
                throw new BusinessRuleException("Não foi possível encontrar o terminal solicitado!");
            }

            return TerminalModelView.FromModel(terminalLoja);
        }
    }
}