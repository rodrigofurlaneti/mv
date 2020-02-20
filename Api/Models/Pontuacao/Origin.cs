using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models.Pontuacao
{
    public partial class Origin
    {
        [JsonProperty("originId")]
        public long OriginId { get; set; }

        [JsonProperty("apportionment")]
        public long Apportionment { get; set; }
    }
}