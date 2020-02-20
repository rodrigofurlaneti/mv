using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class AuthenticatedParticipantePremiarClubeProvider : ParticipantePremiarClubeProvider
    {
        public AuthenticatedParticipantePremiarClubeProvider(PremiarClubeSettings clubeSettings, Participant loggedUser)
            : base(clubeSettings)
        {
            ClubeSettings = clubeSettings;

            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public bool isAuthenticated
        {
            get
            {
                return Api.Get<bool>("participants/me/isAuthenticated");
            }
        }

        public Participant Obter()
        {
            try { return Api.Get<Participant>("participants/me"); }
            catch (PremiarClubeException) { throw; }
            catch { return null; }
        }

        public object SaldoCompleto()
        {
            return Api.Get<object>("participants/me/balance");
        }

        public object Saldo()
        {
            return Api.Get<object>("participants/me/simpleBalance");
        }

        public object Extrato(DateTime? startDate, DateTime? endDate, int? _offset, int? _limit)
        {
            var url = "participants/me/extract";

            var qParams = new Dictionary<string, string>();
            if (startDate.HasValue) qParams.Add(nameof(startDate), startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate.HasValue) qParams.Add(nameof(endDate), startDate.Value.ToString("yyyy-MM-dd"));
            if (_offset.HasValue) qParams.Add(nameof(_offset), _offset?.ToString());
            if (_limit.HasValue) qParams.Add(nameof(_limit), _limit?.ToString());

            var query = string.Join("&", qParams.Select(kv => kv.Key + "=" + kv.Value));
            return Api.Get<object>(url + (string.IsNullOrEmpty(query) ? "" : "?" + query));
        }

        public Participant Alterar(Participant participante)
        {
            participante = Api.Put<Participant, Participant>("participants/me", participante);
            return participante;
        }

        public object AlterarSenha(string oldPassword, string newPassword)
        {
            return Api.Put<object, dynamic>("participants/me/password", new { oldPassword, newPassword });
        }
    }

    public class ParticipantePremiarClubeProvider
    {
        protected  PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public ParticipantePremiarClubeProvider(PremiarClubeSettings clubeSettings)
        {
            ClubeSettings = clubeSettings;

            Api = new Api(clubeSettings);
        }
        
        public Participant Criar(Participant participante)
        {
            var newParticipante = Api.Post<Participant, Participant>("participants", participante);
            participante.id = newParticipante.id;
            return participante;
        }
        
        public void RecuperarSenha(Participant participante)
        {
            Api.Post($"participants/{participante.username}/password-reset");
        }

        public Participant AlterarCatalogo(Participant participante)
        {
            participante= Api.Put<Participant, Participant>($"participants/{participante.id}/updateCatalog", participante);
            return participante;
        }
    }
}
