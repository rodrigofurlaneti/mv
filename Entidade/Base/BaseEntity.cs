using System;

namespace Entidade.Base
{
    public class BaseEntity : IEntity
    {
        public BaseEntity()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual int Id { get; set; }
        public virtual DateTime DataInsercao { get; set; }
    }
}