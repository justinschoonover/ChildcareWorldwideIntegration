using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api;
using ChildcareWorldwide.Google.Api.PubSub;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
	[Authorize]
	public class HubspotController : ControllerBase
	{
		[HttpGet]
		public IActionResult ImportFromDenari() => View(GetPageViewModel(new HubspotImportFromDenari(), pageTitle: "Hubspot Import From Denari", HttpContext.User));

		[HttpPost]
		public async Task<IActionResult> ImportFromDenariAsync([FromServices] IGoogleCloudPubSubService pubSubService)
		{
			var viewModel = new HubspotImportFromDenari
			{
				ImportMessageId = await pubSubService.PublishMessageAsync(Topics.HubspotBeginImport, "all"),
			};

			return View(GetPageViewModel(viewModel, pageTitle: "Hubspot Import From Denari", HttpContext.User));
		}
	}
}
