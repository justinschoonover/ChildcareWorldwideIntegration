using System;
using System.Linq;
using System.Security.Claims;
using ChildcareWorldwide.Integration.Manager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChildcareWorldwide.Integration.Manager.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected static ViewModelBase GetPageViewModel(string pageTitle, ClaimsPrincipal user)
        {
            return new ViewModelBase
            {
                MetaData = new MetaDataViewModel
                {
                    PageTitle = pageTitle,
                },
                FullName = user.Identity.Name,
                AvatarUri = new Uri(user.Claims.SingleOrDefault(c => c.Type == "urn:google:picture").Value ?? string.Empty),
            };
        }


        protected static ViewModelBase<T> GetPageViewModel<T>(T viewModel, string pageTitle, ClaimsPrincipal user)
            where T : IViewModel
        {
            return new ViewModelBase<T>
            {
                MetaData = new MetaDataViewModel
                {
                    PageTitle = pageTitle,
                },
                FullName = user.Identity.Name,
                AvatarUri = new Uri(user.Claims.SingleOrDefault(c => c.Type == "urn:google:picture").Value ?? string.Empty),
                Data = viewModel,
            };
        }
    }
}
