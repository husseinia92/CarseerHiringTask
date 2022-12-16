using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarseerWebAPP.Helpers
{
    public class APIManager
    {
        static public async Task<string> GetURI(Uri u)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                    response = await result.Content.ReadAsStringAsync();
            }
            return response;
        }
    }
}
