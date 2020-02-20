using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class AuthenticatedPremiarClubeProvider : PremiarClubeProvider
    {
        public Participant LoggedUser { get; }

        public new AuthenticatedParticipantePremiarClubeProvider Participantes { get; set; }

        public AuthenticatedPremiarClubeProvider(PremiarClubeSettings clubeSettings, Participant loggedUser)
            : base(clubeSettings)
        {
            LoggedUser = loggedUser;
            Participantes = new AuthenticatedParticipantePremiarClubeProvider(clubeSettings, loggedUser);
        }
        
        public bool TemAcesso()
        {
            try
            {
                return Participantes.Obter() != null;
            }
            catch
            {
                return false;
            }
        }
    }

    public class PremiarClubeProvider
    {
        internal readonly PremiarClubeSettings clubeSettings;

        public ParticipantePremiarClubeProvider Participantes { get; set; }

        public PremiarClubeProvider(
            PremiarClubeSettings clubeSettings
        )
        {
            this.clubeSettings = clubeSettings;

            Participantes = new ParticipantePremiarClubeProvider(clubeSettings);
        }

        public AuthenticatedPremiarClubeProvider Me(Participant loggedUser)
        {
            return new AuthenticatedPremiarClubeProvider(clubeSettings, loggedUser);
        }
    }
}
