using Entidade.Uteis;

namespace Entidade
{
    public class Resposta<T>
    {
        public Resposta()
        {
            TipoMensagem = TipoModal.Info;
        }

        public TipoModal TipoMensagem { get; set; }

        public string Mensagem { get; set; }

        public T ObjetoRetorno { get; set; }
    }
}