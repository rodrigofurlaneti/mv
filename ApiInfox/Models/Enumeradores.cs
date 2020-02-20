using System.ComponentModel;

namespace ApiInfox.Models
{
    public enum CodResposta
    {
        [Description("Ok")]
        Ok = 0,
        [Description("Cartão Não Existe")]
        CartaoNaoExiste = 1,
        [Description("Cartão Bloqueado")]
        CartaoBloqueado = 2,
        [Description("Cartão Vencido")]
        CartaoVencido = 3,
        [Description("Senha Bloqueada")]
        SenhaBloqueada = 4,
        [Description("Cartão Cancelado")]
        CartaoCancelado = 5,
        [Description("Senha Inválida")]
        SenhaInvalida = 6,
        [Description("Credenciado Inválido")]
        CredenciadoInvalido = 7,
        [Description("Credenciado Sem Contrato")]
        CredenciadoSemContrato = 8,
        [Description("Parcelamento Inválido Para Transação")]
        ParcelamentoInvalidoParaTransacao = 9,
        [Description("Cartão Sem Saldo")]
        CartaoSemSaldo = 10,
        [Description("Saldo Do Cartão Está Inconsistente")]
        SaldoDoCartaoEstaInconsistente = 11,
        [Description("Erro De Processamento")]
        ErroDeProcessamento_FxPN = 12, //Fixo Padrão (NÃO)
        [Description("Produto Indeterminado")]
        ProdutoIndeterminado = 13,
        [Description("Terminal Desabilitado")]
        TerminalDesabilitado = 14,
        [Description("Transação Não Foi Localizada")]
        TransacaoNaoFoiLocalizada_D1 = 15, //Diferenças em Disp1 e Disp2
        [Description("O Lote Já Está Fechado")]
        OLoteJaEstaFechado = 16,
        [Description("Lote Com Dados Inconsistentes")]
        LoteComDadosInconsistentes = 17,
        [Description("Lote Inválido")]
        LoteInvalido = 18,
        [Description("A Transação Já Foi Cancelada")]
        ATransacaoJaFoiCancelada = 19,
        [Description("Valor Excedeu O Limite Disponível")]
        ValorExcedeuOLimiteDisponivel = 20,
        [Description("Transação Não Foi Localizada")]
        TransacaoNaoFoiLocalizada_D2 = 21, //Diferenças em Disp1 e Disp2
        [Description("O Produto Do Cartão Está Bloqueado")]
        OProdutoDoCartaoEstaBloqueado = 22,
        [Description("Terminal Inválido")]
        TerminalInvalido = 23,
        [Description("Transação Inválida")]
        TransacaoInvalida_FxPN = 24, //Fixo Padrão (NÃO)
        [Description("Operação Necessita De Senha Para Ser Realizada")]
        OperacaoNecessitaDeSenhaParaSerRealizada = 25,
        [Description("Usuário Inválido")]
        UsuarioInvalido = 26,
        [Description("Fechamento Pendente")]
        FechamentoPendente = 27,
        [Description("Não Existe Transação Para Fechamento")]
        NaoExisteTransacaoParaFechamento = 28,
        [Description("A Empresa Não Está Autorizada")]
        AEmpresaNaoEstaAutorizada = 29,
        [Description("A Empresa Não Possui Contrato Do Produto")]
        AEmpresaNaoPossuiContratoDoProduto = 30,
        [Description("Usuário Bloqueado")]
        UsuarioBloqueado = 31,
        [Description("Transaçâo Pendente De Confirmação")]
        TransacaoPendenteDeConfirmacao = 32,
        [Description("Credenciado Com Serviço Bloqueado")]
        CredenciadoComServicoBloqueado = 33,
        [Description("Credenciado Com Cadastro Não Aprovado")]
        CredenciadoComCadastroNaoAprovado = 34,
        [Description("Estado Do Cartão Inválido Para Esta Operação")]
        EstadoDoCartaoInvalidoParaEstaOperacao = 35,
        [Description("Cartão Inválido Para O Cpf Informado")]
        CartaoInvalidoParaOCpfInformado = 36,
        [Description("Credenciado Não Tem Antecipação Disponível")]
        CredenciadoNaoTemAntecipacaoDisponivel = 37,
        [Description("Cartão Roubado Ou Perdido")]
        CartaoRoubadoOuPerdido = 38,
        [Description("Tipo De Produto Do Cartao Nao Definido")]
        TipoDeProdutoDoCartaoNaoDefinido = 39,
        [Description("Documento Inválido")]
        DocumentoInvalido = 40,
        [Description("Senha De Transação Inválida")]
        SenhaDeTransacaoInvalida = 41,
        [Description("Pagamento Indevido")]
        PagamentoIndevido = 42,
        [Description("Pagamento Já Foi Efetuado")]
        PagamentoJaFoiEfetuado = 43,
        [Description("Operação Não Disponivel")]
        OperacaoNaoDisponivel = 44,
        [Description("Limite Dias Não Conciliados Atingidos")]
        LimiteDiasNaoConciliadosAtingidos = 45,
        [Description("Nro De Parcelas Maior Que Permitido No Contrato Da Empresa")]
        NroDeParcelasMaiorQuePermitidoNoContratoDaEmpresa = 46,
        [Description("Código De Lançamento De Conta Corrente Não Definido")]
        CodigoDeLancamentoDeContaCorrenteNaoDefinido = 47,
        [Description("Transação Não Efetuada - Venda Duplicada")]
        TransacaoNaoEfetuada_VendaDuplicada = 48,
        [Description("Transação Não Efetuada - Tempo Limite Cancelamento Excedido")]
        TransacaoNaoEfetuada_TempoLimiteCancelamentoExcedido = 49,
        [Description("Transação Não Efetuada - Tempo Limite Cancelamento Pagto Excedido")]
        TransacaoNaoEfetuada_TempoLimiteCancelamentoPagtoExcedido = 50,
        [Description("Antecipação De Recebíveis Temporariamente Indisponível")]
        AntecipacaoDeRecebiveisTemporariamenteIndisponivel = 51,
        [Description("Agente Sem Contrato")]
        AgenteSemContrato = 52,
        [Description("Agente Não Existe")]
        AgenteNaoExiste = 53,
        [Description("Fatura Não Localizada")]
        FaturaNaoLocalizada = 54,
        [Description("Cvc Do Cartão Inválido")]
        CvcDoCartaoInvalido = 55,
        [Description("Não Existem Compras Para O Credenciado")]
        NaoExistemComprasParaOCredenciado = 56,
        [Description("Chave De Criptografia Não Inicializada")]
        ChaveDeCriptografiaNaoInicializada = 57,
        [Description("Excedido O Valor De 1ª Compra")]
        ExcedidoOValorDe1ªCompra = 58,
        [Description("Valor Da Operacao Fora Do Permitido")]
        ValorDaOperacaoForaDoPermitido = 59,
        [Description("Valor Da Parcela Fora Do Permitido")]
        ValorDaParcelaForaDoPermitido = 60,
        [Description("Produto Nao Permite Saque")]
        ProdutoNaoPermiteSaque = 61,
        [Description("Produto Nao Permite Pagamento De Contas")]
        ProdutoNaoPermitePagamentoDeContas = 62,
        [Description("Usuario Bloqueado Para Saque")]
        UsuarioBloqueadoParaSaque = 63,
        [Description("Usuario Bloqueado Para Pagamento De Contas")]
        UsuarioBloqueadoParaPagamentoDeContas = 64,
        [Description("Quantidade De Operacoes Excedidas")]
        QuantidadeDeOperacoesExcedidas = 65,
        [Description("Não Existe Recarga Disponível")]
        NaoExisteRecargaDisponivel = 66,
        [Description("Não Existe Recarga Para Cancelar")]
        NaoExisteRecargaParaCancelar = 67,
        [Description("Registro Nao Localizado Pela Pesquisa Informada")]
        RegistroNaoLocalizadoPelaPesquisaInformada = 68,
        [Description("Cartão Não Autorizado Para Efetuar Compra")]
        CartaoNaoAutorizadoParaEfetuarCompra = 69,
        [Description("A Quantidade De Pontos Calculada E Superior A Quantidade De Pontos Disponivel")]
        AQuantidadeDePontosCalculadaESuperiorAQuantidadeDePontosDisponivel = 70,
        [Description("Credenciado Sem Campanha De Fidelidade Para O Produto")]
        CredenciadoSemCampanhaDeFidelidadeParaOProduto = 71,
        [Description("Contrato Empresa Não Permite Saque Ou Pagto")]
        ContratoEmpresaNaoPermiteSaqueOuPagto = 72,
        [Description("Limite Saque Ou Pagto No Contrato Excedido")]
        LimiteSaqueOuPagtoNoContratoExcedido = 73,
        [Description("Não Existe Pontos De Fidelidade Lançados Para Esse Produto")]
        NaoExistePontosDeFidelidadeLancadosParaEsseProduto = 74,
        [Description("O Serviço Não Está Disponível Para Este Credenciado, Antecipação Está Bloqueada")]
        OServicoNaoEstaDisponivelParaEsteCredenciado_AntecipacaoEstaBloqueada = 75,
        [Description("Plano De Compra Não Configurado Para Permitir Valor Variável Na Parcela")]
        PlanoDeCompraNaoConfiguradoParaPermitirValorVariavelNaParcela = 76,
        [Description("Valor Da Primeira Parcela Maior Que O Valor Da Compra")]
        ValorDaPrimeiraParcelaMaiorQueOValorDaCompra = 77,
        [Description("Valor Da Primeira Parcela Maior Que O Valor Original Da Primeira Parcela Da Compra")]
        ValorDaPrimeiraParcelaMaiorQueOValorOriginalDaPrimeiraParcelaDaCompra = 78,
        [Description("Valor Convertido Menor Que Minimo Permitido Para Resgate")]
        ValorConvertidoMenorQueMinimoPermitidoParaResgate = 79,
        [Description("Transação Não Localizada")]
        TransacaoNaoLocalizada = 80,
        [Description("Transação Inválida")]
        TransacaoInvalida_FxPS = 81, //Fixo Padrão (SIM)
        [Description("Transação Com Valor Não Permitido")]
        TransacaoComValorNaoPermitido = 83,
        [Description("Chave De Criptografia Inválida")]
        ChaveDeCriptografiaInvalida = 84,
        [Description("Transaçâo Confirmada Pelo Operador")]
        TransacaoConfirmadaPeloOperador = 85,
        [Description("Transaçâo Cancelada Pelo Operador")]
        TransacaoCanceladaPeloOperador = 86,
        [Description("Nao Existem Pendencias")]
        NaoExistemPendencias = 87,
        [Description("Erro Ao Gravar Ifx_Logtrans")]
        ErroAoGravarIfx_Logtrans = 88,
        [Description("Decriptografia De Dados Inválida")]
        DecriptografiaDeDadosInvalida = 89,
        [Description("Erro Na Transação")]
        ErroNaTransacao = 90,
        [Description("Ocorreu Problema No Cadastramento Da Proposta")]
        OcorreuProblemaNoCadastramentoDaProposta = 91,
        [Description("Transacao Possui Rps")]
        TransacaoPossuiRps = 92,
        [Description("Cartao Com Problemas")]
        CartaoComProblemas = 93,
        [Description("Valor De Entrada Maior Do Que O Valor Da Compra")]
        ValorDeEntradaMaiorDoQueOValorDaCompra = 94,
        [Description("Serviço De Autorização Indisponivel")]
        ServicoDeAutorizacaoIndisponivel = 98,
        [Description("Erro De Processamento")]
        ErroDeProcessamento_FxPS = 99, //Fixo Padrão (SIM)
        [Description("Tipo De Produto Invalidado")]
        TipoDeProdutoInvalidado = 160,
        [Description("Veiculo Não Esta Ativo")]
        VeiculoNaoEstaAtivo = 240,
        [Description("Km Informado Invalido")]
        KmInformadoInvalido = 241,
        [Description("Veiculo Não Localizado Com Placa Informada")]
        VeiculoNaoLocalizadoComPlacaInformada = 242,
        [Description("Condutor Não Localizado Com Veiculo Informado")]
        CondutorNaoLocalizadoComVeiculoInformado = 243,
        [Description("Condutor Não Esta Ativo")]
        CondutorNaoEstaAtivo = 244,
        [Description("Produto Frota Não Permitido")]
        ProdutoFrotaNaoPermitido = 245,
        [Description("Condutor Nao Autorizado Com Veiculo")]
        CondutorNaoAutorizadoComVeiculo = 246,
        [Description("Veiculo Nao Autorizado Com Condutor")]
        VeiculoNaoAutorizadoComCondutor = 247,
        [Description("Valor Da Compra Maior Que O Limite Transação")]
        ValorDaCompraMaiorQueOLimiteTransacao = 248,
        [Description("Valor Da Compra Maior Que O Limite Mensal")]
        ValorDaCompraMaiorQueOLimiteMensal = 249,
        [Description("Qtd Maior Que O Tanque Do Veiculo")]
        QtdMaiorQueOTanqueDoVeiculo = 250,
        [Description("Operação Não Permitida No Dia")]
        OperacaoNaoPermitidaNoDia = 251,
        [Description("Terminal Inválido Para O Tanque")]
        TerminalInvalidoParaOTanque = 252,
        [Description("Usuario Nao Localizado Para Matricula")]
        UsuarioNaoLocalizadoParaMatricula = 253
    }

    public enum TipoCartaoEnum
    {
        [Description("Cartão")]
        Cartao = 0,
        [Description("Adiantamento Salarial")]
        AdiantamentoSalarial = 1,
        [Description("Adiantamento Salarial - Sta Izabel (PARÁ)")]
        AdiantamentoSalarialStaIzabel = 2,
        [Description("Alimentação")]
        Alimentacao = 3,
        [Description("Combustível")]
        Combustivel = 4,
        [Description("Farmácia")]
        Farmacia = 5,
        [Description("Natal")]
        Natal = 6,
        [Description("Presente")]
        Presente = 7,
        [Description("Refeição")]
        Refeicao = 8
    }
}