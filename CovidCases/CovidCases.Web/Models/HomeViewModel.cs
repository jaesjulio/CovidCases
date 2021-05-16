using CovidCases.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidCases.Web.Models
{
    public class HomeViewModel
    {
        public List<CovidInfo> Info { get; set; } = new List<CovidInfo>();

        public string SelectedRegion { get; set; }
        public SelectList Regions { get; set; }
    }
}
