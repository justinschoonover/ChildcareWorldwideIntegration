using System.Net.Http;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Denari.Api.Models;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
    public class DenariController : ControllerBase
    {
        [HttpGet]
        public IActionResult Lookup() => View(GetPageViewModel(new DenariLookup(), pageTitle: "Denari Account Lookup", HttpContext.User));

        [HttpPost]
        public async Task<IActionResult> LookupAsync([FromServices] IDrapiService drapiService, [FromForm] string accountNumber)
        {
            Donor? donor;
            string? json;
            try
            {
                (donor, json) = await drapiService.GetDonorByAccountAsync(accountNumber);
            }
            catch (HttpRequestException e)
            {
                return Problem("Could not connect to Denari API", e.Message);
            }

            var viewModel = new DenariLookup
            {
                AccountNumber = accountNumber,
                Donor = donor,
                RawJson = JToken.Parse(json ?? string.Empty).ToString(Formatting.Indented),
            };

            return View(GetPageViewModel(viewModel, pageTitle: "Denari Account Lookup", HttpContext.User));
        }
    }
}
