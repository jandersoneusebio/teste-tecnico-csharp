using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Responses
{
    public class MatchInfoResponse
    {
        [JsonProperty("data")]
        public List<TeamInfoResponse> Data { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public MatchInfoResponse()
        {
            Data = new List<TeamInfoResponse>();
        }

    }

    public class TeamInfoResponse
    {
        [JsonProperty("team1goals")]
        public int Team1goals { get; set; }

        [JsonProperty("team2goals")]
        public int Team2goals { get; set; }
    }
}
