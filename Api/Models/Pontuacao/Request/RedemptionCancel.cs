namespace Api.Models.Pontuacao.Request
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RedemptionCancel
    {
        [JsonProperty("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("orderId")]
        public long OrderId { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; }
    }
}
