using System.Diagnostics;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> m_logger;

        public HomeController(ILogger<HomeController> logger)
        {
            m_logger = logger;
        }

        public IActionResult Index()
        {
            return View(GetPageViewModel(pageTitle: "Integration Dashboard", HttpContext.User));
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return View();
        }

        [Route("Home/Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            m_logger.LogError("Unhandled exception.", exceptionHandlerPathFeature?.Error);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
