namespace ApiInfox.Models
{
    public class InicializacaoModelView : ResponseModelView
    {
        public string CodCliente { get; set; }
        public string ChaveAcesso { get; set; }
        public string TipoCaptura { get; set; }
        public string NSU { get; set; }
    }
}