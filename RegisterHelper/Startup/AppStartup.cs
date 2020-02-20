using System.Collections.Generic;

namespace InitializerHelper.Startup
{
    public static class AppStartup
    {
        public static readonly Dictionary<string, string> PerfilPermissaoDefault = new Dictionary<string, string> { { "Root", "root" }, { "App Default", "app_default" } };

        public static void Initialize()
        {
            PermissaoStartup.Start();
            PerfilStartup.Start();
            PessoaStartup.Start();
            UsuarioStartup.Start();
            LojaStartup.Start();
            AgendamentoStartup.Start();
        }
    }
}