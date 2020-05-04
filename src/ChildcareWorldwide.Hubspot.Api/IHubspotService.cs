using System.Collections.Generic;
using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api
{
    public interface IHubspotService
    {
        IAsyncEnumerable<CrmPropertyGroup> ListContactPropertyGroupsAsync();
        Task<CrmPropertyGroup?> GetContactPropertyGroupAsync(string groupName);
        Task CreateContactPropertyGroupAsync(CrmPropertyGroup propertyGroup);

        IAsyncEnumerable<CrmProperty> ListContactPropertiesAsync();
        Task<CrmProperty?> GetContactPropertyAsync(string propertyName);
        Task CreateContactPropertyAsync(CrmProperty contactProperty);

        IAsyncEnumerable<CrmPropertyGroup> ListCompanyPropertyGroupsAsync();
        Task<CrmPropertyGroup?> GetCompanyPropertyGroupAsync(string groupName);
        Task CreateCompanyPropertyGroupAsync(CrmPropertyGroup propertyGroup);

        IAsyncEnumerable<CrmProperty> ListCompanyPropertiesAsync();
        Task<CrmProperty?> GetCompanyPropertyAsync(string propertyName);
        Task CreateCompanyPropertyAsync(CrmProperty contactProperty);

        Task<Company> GetCompanyAsync();
        Task CreateCompanyAsync(Company company);

        Task<Contact> GetContactAsync();
        Task CreateContactAsync(Contact contact);
    }
}
