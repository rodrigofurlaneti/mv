using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IVendedorAplicacao : IBaseAplicacao<Vendedor>
    {
    }

    public class VendedorAplicacao : BaseAplicacao<Vendedor, IVendedorServico>, IVendedorAplicacao
    {
        public new void Salvar(Vendedor entity)
        {
            var vendedorRetorno = BuscarPorId(entity.Id) ?? entity;
            
            vendedorRetorno.Nome = entity.Nome;
            vendedorRetorno.Sobrenome = entity.Sobrenome;
            vendedorRetorno.Sexo = entity.Sexo;
            vendedorRetorno.DataNascimento = entity.DataNascimento;
            //if (!vendedorRetorno.Documentos.Any())
            //    vendedorRetorno.Documentos.Add(new Documento { Numero = entity.NumeroCpf, Tipo = 1 });

            //var documento = entity.Documentos.FirstOrDefault(x => x.Tipo == 1);

            //foreach (var doc in vendedorRetorno.Documentos.Where(x => x.Tipo == 1))
            //{
            //    if (documento != null)
            //        doc.Numero = documento.Numero;
            //}


            Servico.Salvar(vendedorRetorno);

        }
    }
}