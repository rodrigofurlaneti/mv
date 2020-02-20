using Clube.Premiar.Models;
using Core.Exceptions;
using Dominio;
using Dominio.Providers;
using Entidade;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Adapter
{
    public class PremiarClubeProviderAdapter : IClubeProvider
    {
        private readonly PremiarClubeSettings clubeSettings;
        private readonly IUsuarioServico usuarioServico;
        private readonly IPessoaServico pessoaServico;
        private readonly ICartaoServico cartaoServico;

        public PremiarClubeProviderAdapter(
            PremiarClubeSettings clubeSettings,
            IUsuarioServico usuarioServico,
            IPessoaServico pessoaServico,
            ICartaoServico cartaoServico
        )
        {
            this.clubeSettings = clubeSettings;
            this.usuarioServico = usuarioServico;
            this.pessoaServico = pessoaServico;
            this.cartaoServico = cartaoServico;
        }

        public bool TemAcesso(Usuario usuario)
        {
            var dbUsuario = usuarioServico.BuscarPorId(usuario.Id);
            cartaoServico.DescriptografarCartoes(dbUsuario.Pessoa.Cartoes);

            var participante = new Participant().FromUsuario(dbUsuario);
            participante.catalogId = clubeSettings.Credentials.CatalogId;
            participante.clientId = clubeSettings.Credentials.ClientId;
            participante.profileId = clubeSettings.Credentials.ProfileId;

            var premiarClube = new PremiarClubeProvider(clubeSettings);
            var authenticatedPremiarClube = premiarClube.Me(participante);

            var temAcesso = authenticatedPremiarClube.TemAcesso();
            if (!temAcesso)
            {
                premiarClube.Participantes.Criar(participante);

                var cartao = dbUsuario.Pessoa.Cartoes.FirstOrDefault();
                cartao.DadosClube = JsonConvert.SerializeObject(participante);
                cartaoServico.Criptografar(cartao);
                cartaoServico.Salvar(cartao);
                
                //CreditarPontos(participante);
                temAcesso = authenticatedPremiarClube.TemAcesso();
            }

            if (temAcesso)
            {
                throw new RedirectException(204, $"https://{clubeSettings.Credentials.CampaignName}.premmiar.com.br/#/auth?access_token={authenticatedPremiarClube.Participantes.Api.RawToken}");
            }

            return temAcesso;
        }
    }
}
