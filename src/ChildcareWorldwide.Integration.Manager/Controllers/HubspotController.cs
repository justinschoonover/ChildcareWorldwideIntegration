using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
    public class HubspotController : ControllerBase
    {
        [HttpGet]
        public IActionResult CloneProperties() => View(GetPageViewModel(new HubspotCloneProperties(), pageTitle: "Hubspot Clone Custom Properties", HttpContext.User));

        [HttpPost]
        public async Task<IActionResult> ClonePropertiesAsync([FromServices] IHubspotService hubspotService, [FromForm] string apiKey)
        {
            await Task.Delay(2000);

            var viewModel = new HubspotCloneProperties
            {
                SandboxApiKey = apiKey,
            };

            return View(GetPageViewModel(viewModel, pageTitle: "Hubspot Clone Custom Properties", HttpContext.User));
        }
    }
}
