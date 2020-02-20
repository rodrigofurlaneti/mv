using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntidadePcSist.Base;
using NHibernate;
using NHibernate.Event;

namespace RepositorioIntegracaoPCSist.Base
{
    public class AuditEventListener : IPostUpdateEventListener
    {
        private const string NoValueString = "*No Value*";

        private static string GetStringValueFromStateArray(IList<object> stateArray, int position)
        {
            var value = stateArray[position];

            return value == null || value.ToString() == string.Empty
                    ? NoValueString
                    : value.ToString();
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            if (@event.Entity is Audit || !(@event.Entity is IAudit))
            {
                return;
            }

            if (@event.OldState == null)
            {
                return;
            }

            var dirtyFieldIndexes = @event.Persister.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session);

            var session = @event.Session.GetSession(EntityMode.Poco);

            foreach (var audit in from dirtyFieldIndex in dirtyFieldIndexes
                                  let oldValue = GetStringValueFromStateArray(@event.OldState, dirtyFieldIndex)
                                  let newValue = GetStringValueFromStateArray(@event.State, dirtyFieldIndex)
                                  where oldValue != newValue
                                  select new Audit
                                      {
                                          Entidade = @event.Entity.GetType().Name,
                                          Atributo = @event.Persister.PropertyNames[dirtyFieldIndex],
                                          ValorAntigo = oldValue,
                                          ValorNovo = newValue,
                                          Usuario = HttpContext.Current.User.Identity.Name,
                                          CodigoEntidade = (int)@event.Id,
                                          Data = DateTime.Now
                                      })
            {
                session.Save(audit);
            }

            session.Flush();
        }
    }
}
