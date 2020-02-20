using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models.Pontuacao
{
    public partial class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("values")]
        public Value[] Values { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("value")]
        public string ValueValue { get; set; }
    }
}