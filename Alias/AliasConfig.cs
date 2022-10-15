using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Auxiliary.Configuration;

namespace Alias
{
    public class AliasSettings : ISettings
    {
        [JsonPropertyName("commands")]
        public Dictionary<string, string> Commands { get; set; } = new();
    }
}
