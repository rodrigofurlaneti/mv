using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entidade.Uteis
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
        Erro = 6,
        [Description("Em Aberto")]
        EmAberto = 7,
        [Description("Pagamento não aprovado")]
        PagamentoNaoAprovado = 8,
        [Description("Aguardando confirmação")]
        AguardandoConfirmacao = 9,
        [Description("Pedido Enviado")]
        PedidoEnviado = 10,
        [Description("Aguardando avaliação")]
        AguardandoAvaliacao = 11,
        [Description("Pedido recusado")]
        PedidoRecusado = 12,
        [Description("Pedido finalizado")]
        PedidoFinalizado = 13,
    }

    public enum TipoInfoProduto
    {
        Imagem = 1,
        Detalhe = 2,
        Termos = 3
    }

    public enum StatusLoja
    {
        [Description("Aprovada")]
        Aprovada = 1,
        [Description("Reprovada")]
        Reprovada = 2
    }

    public enum TipoClassificacao
    {
        [Description("Meu Clube")]
        MeuClube = 1,
        [Description("Meu Shopping")]
        MeuShopping = 2,
        [Description("Ambos")]
        Ambos = 3,
        [Description("Nenhum")]
        Nenhum = 4
    }

    public enum ClassificacaoLoja
    {
        [Description("Academia")]
        Academia = 1,
        [Description("Armazem")]
        Armazem = 2,
        [Description("Bar")]
        Bar = 3,
        [Description("Bomboniere")]
        Bomboniere = 4,
        [Description("Catina Escolar")]
        CantinaEscolar = 5,
        [Description("Conveniência")]
        Conveniencia = 6,
        [Description("Mercearia")]
        Mercearia = 7,
        [Description("Lanchonete")]
        Lanchonete = 8,
        [Description("Padaria")]
        Padaria = 9,
        [Description("Restaurante")]
        Restaurante = 10,
        [Description("Outros")]
        Outros = 11
    }

    public enum TipoDesconto
    {
        [Description("Valor do desconto em porcentagem"), Display(Name = @"Percentual")]
        Percentual = 1,
        [Description("Valor do desconto em reais"), Display(Name = @"Monetário")]
        Monetario = 2
    }

    public enum TipoCadastroDesconto
    {
        [Description("Global"), Display(Name = @"Global")]
        Global = 1,
        [Description("Pessoas"), Display(Name = @"Pessoas")]
        Pessoas = 2
    }

    public enum TipoArquivo
    {
        [Description("Arquivo")]
        Arquivo = 1,
        [Description("Thumbnail")]
        Thumbnail = 2,
        [Description("Planta")]
        Planta = 3,
        [Description("Foto")]
        Foto = 4,
        [Description("Arte")]
        Arte = 5,
        [Description("Material Apoio")]
        MaterialApoio = 6,
        [Description("EspacoLoja")]
        EspacoLoja = 7,
        [Description("Catalogo")]
        Catalogo = 8,
        [Description("Excel")]
        Excel = 9

    }

    public enum TipoModal
    {
        [Description("success")]
        Success,
        [Description("danger")]
        Danger,
        [Description("warning")]
        Warning,
        [Description("info")]
        Info
    }

    public enum Sexo
    {
        [Description("Nao Definido")]
        Todos = 0,
        [Description("Masculino")]
        Masculino = 1,
        [Description("Feminino")]
        Feminino = 2
    }

    public enum TipoBanner
    {
        [Description("Home")]
        Home = 1,
        [Description("Login")]
        Login = 2
    }

    public enum StatusProdutoPreco
    {
        Inexistente = 1,
        Existente = 2
    }

    public enum PerfilApp
    {
        [Description("Root")]
        Root = 99,
        [Description("App Default")]
        AppDefault = 1,
        [Description("Consumidor")]
        Consumer = 1,
        [Description("Fornecedor")]
        Fornecedor = 2
    }

    public enum DiaSemana
    {
        Domingo = 0,
        Segunda = 1,
        Terca = 2,
        Quarta = 3,
        Quinta = 4,
        Sexta = 5,
        Sabado = 6
    }

    public enum TipoAgendamento
    {
        RetiradaLoja = 1
    }

    public enum TipoPet
    {
        [Description("Cachorro")]
        Cachorro = 1,
        [Description("Gato")]
        Gato = 2,
        [Description("Peixe")]
        Peixe = 3,
        [Description("Passaro")]
        Passaro = 4,
        [Description("Coelho")]
        Coelho = 5,
        [Description("Tartaruga")]
        Tartaruga = 6,
        [Description("Hamster")]
        Hamster = 7,
        [Description("Outros")]
        Outros = 8
    }

    public enum Gravidade
    {
        [Description("Baixa")]
        Baixa = 1,
        [Description("Media")]
        Media = 2,
        [Description("Alta")]
        Alta = 3,
    }

    public enum StatusAdocao
    {
        [Description("Em Analise")]
        EmAnalise = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Negado")]
        Negado = 3,
        [Description("Cancelado")]
        Cancelado = 4,
    }

    public enum CategoriaDeProduto
    {
        [Description("Produto")]
        Produto = 1,
        [Description("Servico")]
        Servico = 2,
        [Description("Convenio")]
        Convenio = 3
    }

    public enum TipoVeiculo
    {
        [Display(Name = "Hatch")]
        [Description("Hatch")]
        Hatch = 1,
        [Display(Name = "Motocicleta")]
        [Description("Motocicleta")]
        Motocicleta = 2,
        [Display(Name = "SUV")]
        [Description("SUV")]
        SUV = 3,
        [Display(Name = "Sedan")]
        [Description("Sedan")]
        Sedan = 4,
        [Display(Name = "Picapes")]
        [Description("Picapes")]
        Picapes = 5,
        [Display(Name = "Outros")]
        [Description("Outros")]
        Outros = 6,
    }
}