using System.ComponentModel;

namespace EntidadePcSist.Uteis
{
    public enum TipoEndereco
    {
        [Description("Residencial")]
        Residencial = 1,
        [Description("Comercial")]
        Comercial = 2
    }

    public enum TipoDocumento
    {
        Rg = 1,
        Cpf = 2,
        Cnpj = 3,
        Ie = 4,
        Cfm = 5,
        TituloEleitoral = 6,
        Ctps = 7,
        Pis = 8
    }

    public enum TipoContato
    {
        Email = 1,
        Residencial = 2,
        Celular = 3,
        Recado = 4,
        Comercial = 5,
        Fax = 6,
        OutroEmail = 7
    }

    public enum StatusPedido
    {
        [Description("Aguardando pagamento")]
        AguardandoPagamento = 1,
        [Description("Pagamento aprovado")]
        PagamentoAprovado = 2,
        [Description("Em separação")]
        EmSeparacao = 3,
        [Description("Aguardando retirada")]
        AguardandoRetirada = 4,
        [Description("Pedido retirado")]
        PedidoRetirado = 5,
        [Description("Erro")]
        Erro = 6
    }

    public enum TipoInfoProduto
    {
        Imagem = 1,
        Outros = 2
    }
}