using System.ComponentModel;

namespace Aplicacao.apipayzen
{
    public enum PaymentSource
    {
        EC,
        MOTO,
        CC,
        OTHER
    }

    public enum PayzenMethods
    {
        createPayment
    }

    public enum TransactionStatus
    {
        //Em andamento.
        //Status temporário. O status definitivo será retornado assim que a sincronização for realizada.
        [Description("Em andamento")]
        INITIAL,

        //A transação não foi criada e portanto não será exibida no Back Office.
        [Description("Não criada")]
        NOT_CREATED,

        //Captura em andamento.
        //A transação foi aceita e será capturada automaticamente no banco na data prevista.
        [Description("Autorizada")]
        AUTHORISED,

        //Para ser aprovado.
        //A transação, criada em validação manual, foi autorizada. O vendedor deve validar manualmente a captura
        //no banco. A transação pode ser aprovada enquanto a data de captura não for vencida. Se esta data estiver
        //vencida, então o pagamento tem o status Expirado (status definitivo).
        [Description("Para ser aprovado")]
        AUTHORISED_TO_VALIDATE,

        //Autorização em andamento.
        //A data de captura solicitada é superior à data de fim de validade da solicitação de autorização.
        //Uma autorização de 1 BRL foi efetuada e aceita pelo banco emissor. A solicitado de autorização e a captura
        //no banco serão acionadas automaticamente.
        [Description("Autorização em andamento")]
        WAITING_AUTHORISATION,

        //Para ser aprovado e autorizado.
        //A data de captura solicitada é superior à data de fim de validade da solicitação de autorização.
        //Uma autorização de 1 BRL foi efetuada e aceita pelo banco emissor. A solicitação de autorização será
        //automaticamente efetuada a D-1 antes da data de captura no banco. O pagamento poderá ser aceito ou
        //recusado. Captura automática no banco.
        [Description("Para ser aprovado e autorizado")]
        WAITING_AUTHORISATION_TO_VALIDATE,

        //Recusada.
        //A transação foi recusada
        [Description("Recusada")]
        REFUSED
    }
}
