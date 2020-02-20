using System.Web.Services.Protocols;

namespace Aplicacao.apipayzen
{
    public class PayzenHeader : SoapHeader
    {
        public string ShopId { get; set; }
        public string RequestId { get; set; }
        public string Timestamp { get; set; }
        public string Mode { get; set; }
        public string AuthToken { get; set; }
    }
}
