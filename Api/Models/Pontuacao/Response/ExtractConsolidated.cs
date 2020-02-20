namespace Api.Models.Pontuacao.Response
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ExtractConsolidated
    {
        [JsonProperty("creditBalance")]
        public long CreditBalance { get; set; }

        [JsonProperty("debitBalance")]
        public long DebitBalance { get; set; }

        [JsonProperty("redeemBalance")]
        public long RedeemBalance { get; set; }

        [JsonProperty("expiredBalance")]
        public long ExpiredBalance { get; set; }

        [JsonProperty("lockedBalance")]
        public long LockedBalance { get; set; }
    }
}
