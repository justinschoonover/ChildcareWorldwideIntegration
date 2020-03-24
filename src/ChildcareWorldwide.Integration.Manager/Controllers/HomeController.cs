using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync([FromServices] IDrapiService drapiService)
        {
            await foreach (var donor in drapiService.GetDonorsAsync())
            {
                Console.WriteLine(donor.DonorKey);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
