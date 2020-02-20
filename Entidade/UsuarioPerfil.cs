using Entidade.Base;

namespace Entidade
{
    public class UsuarioPerfil : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}