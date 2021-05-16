using CovidCases.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidCases.Core.Services
{
    public class RegionService : IRegionService
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<List<Region>> RegionsRequest(int count = 10)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://covid-19-statistics.p.rapidapi.com/regions"),
                    Headers =
                    {
                        { "x-rapidapi-key", "43f4143ac1msh218bf92088a05a0p1265dajsn131337a1a5db" },
                        { "x-rapidapi-host", "covid-19-statistics.p.rapidapi.com" },
                    },
                };

                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                List<Region> regions = JsonConvert.DeserializeObject<List<Region>>(body);

                return regions;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                return new List<Region>();
            }
        }
    }

    public interface IRegionService
    {
        Task<List<Region>> RegionsRequest(int count = 10);

    }
}
