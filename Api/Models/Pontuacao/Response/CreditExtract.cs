namespace Api.Models.Pontuacao.Response
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CreditExtract
    {
        [JsonProperty("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("insertDate")]
        public DateTimeOffset InsertDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("orderId")]
        public long? OrderId { get; set; }

        [JsonProperty("externalCode")]
        public long? ExternalCode { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("expirationDate")]
        public object ExpirationDate { get; set; }

        [JsonProperty("origins")]
        public Origin[] Origins { get; set; }
    }
}
