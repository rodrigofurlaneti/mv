using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class Cliente : IEntityPcSist
    {
        public virtual bool Importado { get; set; }
        public virtual string TipoOperacao { get; set; }
        public virtual string CPFCNPJ { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual string Praca { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string NumeroEndereco { get; set; }
        public virtual string ComplementoEndereco { get; set; }
        public virtual string BairroEndereco { get; set; }
        public virtual int CidadeEndereco { get; set; }
        public virtual string EstadoEndereco { get; set; }
        public virtual int MunicipioEndereco { get; set; }
        public virtual string CEP { get; set; }
        public virtual string Email { get; set; }
        public virtual string Contribuinte { get; set; }
        public virtual string ConsumidorFinal { get; set; }
        public virtual string CodigoUsuario { get; set; }
        public virtual int CodigoAtividade { get; set; }
    }
}
