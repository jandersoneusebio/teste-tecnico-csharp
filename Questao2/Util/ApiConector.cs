using Newtonsoft.Json;
using Questao2.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Util
{
    class ApiConector
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public static async Task<T> GetRequest<T>(string url)
        {
            HttpClient client = new HttpClient();

            var responseHttp = await client.GetAsync(url);

            responseHttp.EnsureSuccessStatusCode();

            var responseBody = await responseHttp.Content.ReadAsStringAsync();

            var str = JsonConvert.DeserializeObject<T>(responseBody);

            return str;
        }

    }
}
