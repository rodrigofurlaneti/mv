using System.Net;

namespace Aplicacao.ViewModels.ApiMobiseg
{
    public class ApiBaseViewModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string detail { get; set; }

        public bool Error => StatusCode != HttpStatusCode.OK && StatusCode != HttpStatusCode.Created;
    }
}