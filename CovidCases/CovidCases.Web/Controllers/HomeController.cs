using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CovidCases.Models;
using CovidCases.Core.Services;
using CovidCases.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Newtonsoft.Json;
using CovidCases.Core.Models;

namespace CovidCases.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICovidReportService _reportService;

        public HomeController(ILogger<HomeController> logger, ICovidReportService reportService)
        {
            _logger = logger;

            _reportService = reportService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var reportlist = await _reportService.CovidInfoRequest();
            var model = new HomeViewModel() { Info = reportlist };

            model.Regions = new SelectList(reportlist, "Region.Iso", "Region.Name");

            var jsonreport = JsonConvert.SerializeObject(reportlist);
            HttpContext.Response.Cookies.Append("reportlist", jsonreport);

            ViewBag.Message = reportlist;

            return View(model);
        }

        public async Task<IActionResult> Region([ModelBinder] HomeViewModel result)
        {

            var reportlist = await _reportService.CovidInfoRequest(result.SelectedRegion);
            var model = new HomeViewModel() { Info = reportlist };

            return View(model);
        }

        public async Task<IActionResult> Csv(string Country)
        {

            var resultlist = await _reportService.CovidInfoRequest(Country);

            var builder = new StringBuilder();
            builder.AppendLine("Region,Cases,Deaths");
            foreach (var region in resultlist)
            {
                builder.AppendLine($"{region.Region.Name},{region.Confirmed},{region.Deaths}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "CovidCases.csv");
        }

        public async Task<IActionResult> Xml(string Country)
        {

            var resultlist = await _reportService.CovidInfoRequest(Country);

            var builder = new StringBuilder();
            builder.AppendLine("Region,Cases,Deaths");
            foreach (var region in resultlist)
            {
                builder.AppendLine($"{region.Region.Name},{region.Confirmed},{region.Deaths}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/xml", "CovidCases.xml");
        }

        public async Task<IActionResult> Json(string Country)
        {

            var resultlist = await _reportService.CovidInfoRequest();

            var builder = new StringBuilder();
            builder.AppendLine(JsonConvert.SerializeObject(resultlist));
            

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/json", "CovidCases.json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
