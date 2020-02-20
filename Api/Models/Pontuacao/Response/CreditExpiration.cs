namespace Api.Models.Pontuacao.Response
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CreditExpiration
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }
    }
}