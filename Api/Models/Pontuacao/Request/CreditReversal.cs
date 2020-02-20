namespace Api.Models.Pontuacao.Request
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CreditReversal
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("externalCode")]
        public long ExternalCode { get; set; }

        [JsonProperty("bankLotId")]
        public long BankLotId { get; set; }

        [JsonProperty("bankLotItemId")]
        public long BankLotItemId { get; set; }

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; }
    }
}