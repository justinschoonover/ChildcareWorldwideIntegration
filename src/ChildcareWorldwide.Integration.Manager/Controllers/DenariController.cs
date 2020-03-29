using System.Net.Http;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Denari.Api.Models;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
    public class DenariController : ControllerBase
    {
        [HttpGet]
        public IActionResult Lookup() => View(GetPageViewModel(new DenariLookup(), pageTitle: "Denari Account Lookup", HttpContext.User));

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> LookupAsync([FromServices] IDrapiService drapiService, [FromForm] string accountNumber)
        {
            Donor? donor;
            try
            {
                donor = await drapiService.GetDonorByAccountAsync(accountNumber);
            }
            catch (HttpRequestException e)
            {
                return Problem("Could not connect to Denari API", e.Message);
            }

            if (donor == null)
                return NotFound();

            var vm = new DenariLookup
            {
                Donor = donor,
            };

            return View(GetPageViewModel(vm, pageTitle: "Denari Account Lookup", HttpContext.User));
        }
    }
}
