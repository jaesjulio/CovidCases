using CovidCases.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidCases.Core.Services
{
    public class CovidReportService : ICovidReportService
    {
            static readonly HttpClient client = new HttpClient();
        public async Task<List<CovidInfo>> CovidInfoRequest(string Country = null, int count = 10)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = string.IsNullOrEmpty(Country) ?  new Uri("https://covid-19-statistics.p.rapidapi.com/reports") : new Uri($"https://covid-19-statistics.p.rapidapi.com/reports?iso={Country}"),
                    Headers =
                    {
                        { "x-rapidapi-key", "43f4143ac1msh218bf92088a05a0p1265dajsn131337a1a5db" },
                        { "x-rapidapi-host", "covid-19-statistics.p.rapidapi.com" },
                    },
                };

                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                Root covidinfo = JsonConvert.DeserializeObject<Root>(body);

                List<CovidInfo> filteredcovidinfo = new List<CovidInfo>();

                if (Country == null)
                {
                     filteredcovidinfo = covidinfo.Data.Where(x => string.IsNullOrEmpty(x.Region.Province)).OrderByDescending(x => x.Confirmed).Take(count).ToList();
                }
                else
                {
                     filteredcovidinfo = covidinfo.Data.OrderByDescending(x => x.Confirmed).Take(count).ToList();
                }
              
                return filteredcovidinfo;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                return new List<CovidInfo>();
            }
        }
    }

    public interface ICovidReportService
    {
        Task<List<CovidInfo>> CovidInfoRequest(string Country = null, int count = 10);
    }
}
