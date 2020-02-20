namespace Api.Models.Pontuacao.Response
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RedemptionConfirm
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("returnCode")]
        public long ReturnCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
