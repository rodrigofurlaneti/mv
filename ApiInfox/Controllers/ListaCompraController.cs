using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Aplicacao;
using Core.Exceptions;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace ApiInfox.Controllers
{
    [RoutePrefix("api/listacompra")]
    public class ListaCompraController : ApiController
    {
        private readonly IListaCompraServico _listaCompraServico;
        private readonly ICupomAplicacao _cupomAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public ListaCompraController(IListaCompraServico listaCompraServico, ICupomAplicacao cupomAplicacao, IUsuarioAplicacao usuarioAplicacao)
        {
            _listaCompraServico = listaCompraServico;
            _cupomAplicacao = cupomAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
        }

        [Authorize]
        [HttpGet]
        [Route("usuario/{id}")]
        public ListaCompra GetListaAtiva(int id)
        {
            try
            {
                var listaAtiva = _listaCompraServico.BuscaListaAtiva(id);
                return FitListaCompra(listaAtiva);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("usuario/{id}/{lojaId}")]
        public ListaCompra GetListaAtivaPorLoja(int id, int lojaId)
        {
            try
            {
                var listaAtiva = _listaCompraServico.BuscaListaAtivaPorLoja(id, lojaId);
                return FitListaCompra(listaAtiva);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("mudarLojaAtiva/{listaAtual}/{novaLoja}")]
        public ListaCompra MudarLojaAtiva(int listaAtual, int novaLoja)
        {
            var listaCompra = _listaCompraServico.MudarLojaAtiva(listaAtual, novaLoja);
            return FitListaCompra(listaCompra);
        }

        [HttpPost]
        [Route("recuperarESubstituirItens/{listaAtual}/{listaAntiga}")]
        public ListaCompra RecuperarESubstituirItens(int listaAtual, int listaAntiga)
        {
            var listaCompra = _listaCompraServico.RecuperarItens(listaAtual, listaAntiga);
            return FitListaCompra(listaCompra);
        }

        [HttpPost]
        [Route("{id}/atualizar")]
        public ListaCompra AtualizaLista(int id)
        {
            var entity = _listaCompraServico.AtualizaLista(id);
            foreach (var itemCompra in entity.Itens)
                itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();
            //
            return FitListaCompra(entity);
        }

        [HttpPost]
        [Route("{id}/adicionaritem")]
        public ListaCompra AdicionarItem(int id, ItemCompra item)
        {
            try
            {
                item.ListaCompra = new ListaCompra { Id = id };

                var entity = _listaCompraServico.AdicionaItem(id, item);
                foreach (var itemCompra in entity.Itens)
                    itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();

                return FitListaCompra(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("{id}/alteraritem")]
        public ListaCompra AlterarItem(int id, ItemCompra item)
        {
            try
            {
                item.ListaCompra = new ListaCompra { Id = id };
                var entity = _listaCompraServico.AlteraItem(id, item);
                foreach (var itemCompra in entity.Itens)
                    itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();
                //
                return FitListaCompra(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("{id}/removeritem")]
        public ListaCompra RemoverItem(int id, ItemCompra item)
        {
            try
            {
                var entity = _listaCompraServico.RemoveItem(id, item);
                foreach (var itemCompra in entity.Itens)
                    itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();
                //
                return FitListaCompra(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("{id}/removerTodosItens")]
        public ListaCompra RemoverTodosItens(int id)
        {
            try
            {
                var listaCompra = _listaCompraServico.RemoverTodosItens(id);

                return FitListaCompra(listaCompra);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("validaCupom/{cupom}/{listaCompraAtivaId}")]
        public Resposta<ListaCompra> ValidarCupom(string cupom, int listaCompraAtivaId)
        {
            var resposta = new Resposta<ListaCompra>
            {
                ObjetoRetorno = null
            };

            try
            {
                var listaCompra = _listaCompraServico.BuscarPorId(listaCompraAtivaId);
                listaCompra.ValorCupom = 0;

                var respostaCupom = string.IsNullOrEmpty(cupom) || cupom == "null"
                    ? new Resposta<Cupom> { Mensagem = "Cupom não encontrado!", TipoMensagem = TipoModal.Warning }
                    : _cupomAplicacao.ValidarCupom(cupom);
                listaCompra.Cupom = string.IsNullOrEmpty(cupom) || cupom == "null" ? "" : cupom;
                listaCompra.ValorCupom = _cupomAplicacao.CalcularValorCupom(listaCompra.Total, respostaCupom?.ObjetoRetorno);

                _listaCompraServico.Salvar(listaCompra);
                listaCompra = _listaCompraServico.AtribuiDescontoAListaCompra(listaCompra);

                resposta.TipoMensagem = respostaCupom?.TipoMensagem ?? TipoModal.Success;
                resposta.Mensagem = respostaCupom?.Mensagem ?? string.Empty;
                resposta.ObjetoRetorno = FitListaCompra(listaCompra);
                return resposta;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                resposta.TipoMensagem = TipoModal.Danger;
                resposta.Mensagem = ex.Message;
                return resposta;
            }
        }

        [HttpPost]
        [Route("")]
        public ListaCompra Post([FromBody] ListaCompra entity)
        {
            //if (entity.Usuario != null)
            //    entity.Usuario = _usuarioAplicacao.BuscarPorId(entity.Usuario.Id);

            _listaCompraServico.Salvar(entity);
            //
            entity = _listaCompraServico.AtribuiDescontoAListaCompra(entity);
            return FitListaCompra(entity);
        }

        [HttpPut]
        [Route("{id}")]
        public ListaCompra Put(int id, [FromBody] ListaCompra entity)
        {
            try
            {
                // Set id in object
                entity.Id = id;

                foreach (var item in entity.Itens)
                    item.ListaCompra = new ListaCompra { Id = id };
                //
                _listaCompraServico.Salvar(entity);
                //
                foreach (var itemCompra in entity.Itens)
                    itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();
                //
                entity = _listaCompraServico.AtribuiDescontoAListaCompra(entity);
                return FitListaCompra(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public virtual ListaCompra Get(int id)
        {
            var entity = _listaCompraServico.BuscarPorId(id);
            if (entity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            foreach (var itemCompra in entity.Itens)
                itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();
            //
            entity = _listaCompraServico.AtribuiDescontoAListaCompra(entity);
            return FitListaCompra(entity);
        }

        private ListaCompra FitListaCompra(ListaCompra lista)
        {
            if (lista != null)
            {
                if (lista.Loja?.Endereco != null)
                    lista.Loja.Endereco.Pessoa = null;
                lista.Loja = new Loja
                {
                    Id = lista.Loja?.Id ?? 0,
                    Descricao = lista.Loja?.Descricao,
                    RazaoSocial = lista.Loja.RazaoSocial,
                    Status = lista.Loja.Status,
                    Telefone = lista.Loja.Telefone,
                    Cnpj = lista.Loja.Cnpj,
                    InscricaoEstadual = lista.Loja.InscricaoEstadual,
                    Endereco = lista.Loja.Endereco
                };
                lista.Usuario = new Usuario { Id = lista.Usuario?.Id ?? 0 };

                foreach (var itemCompra in lista.Itens)
                {
                    itemCompra.Produto = new Produto
                    {
                        Id = itemCompra.Produto.Id,
                        Codigo = itemCompra.Produto.Codigo,
                        CodigoAuxiliarPcSist = itemCompra.Produto.CodigoAuxiliarPcSist,
                        CodigoBarras = itemCompra.Produto.CodigoBarras,
                        CodigoPcSist = itemCompra.Produto.CodigoPcSist,
                        Descricao = itemCompra.Produto.Descricao,
                        Nome = itemCompra.Produto.Nome,
                        SubGrupoProduto = itemCompra.Produto.SubGrupoProduto
                    };
                    itemCompra.Produto.Informacoes = itemCompra.Produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();

                    if (itemCompra.Preco != null)
                    {
                        itemCompra.Preco.Fornecedor = null;
                        itemCompra.Preco.Loja = new Loja { Id = lista?.Loja?.Id ?? 0 };
                        itemCompra.Preco.Produto = new Produto { Id = itemCompra.Produto.Id, Codigo = itemCompra.Produto.Codigo, CodigoAuxiliarPcSist = itemCompra.Produto.CodigoAuxiliarPcSist, CodigoBarras = itemCompra.Produto.CodigoBarras, CodigoPcSist = itemCompra.Produto.CodigoPcSist, Descricao = itemCompra.Produto.Descricao };
                    }

                    if (itemCompra.ListaCompra != null)
                    {
                        itemCompra.ListaCompra = new ListaCompra
                        {
                            Loja = new Loja { Id = lista?.Loja?.Id ?? 0 },
                            Usuario = new Usuario { Id = lista.Usuario?.Id ?? 0 }
                        };
                    }
                }
            }
            return lista;
        }
    }
}