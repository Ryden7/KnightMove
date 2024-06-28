using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightMove
{
    [Serializable]
    public class KnightPathResponse
    {
        [JsonProperty("ShortestPath")]
        public string? ShortestPath { get; set; }

        [JsonProperty("NumberOfMoves")]
        public int? NumberOfMoves { get; set; }

        [JsonProperty("Starting")]
        public string? Starting { get; set; }

        [JsonProperty("Ending")]
        public string? Ending { get; set; }

        [JsonProperty("OperationId")]
        public string? OperationId { get; set; }
    }
}
