using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api
{
    public interface IHubspotService
    {
        IAsyncEnumerable<CrmPropertyGroup> ListContactPropertyGroupsAsync(CancellationToken cancellationToken = default);
        Task<CrmPropertyGroup?> GetContactPropertyGroupAsync(string groupName, CancellationToken cancellationToken = default);
        Task CreateContactPropertyGroupAsync(CrmPropertyGroup propertyGroup, CancellationToken cancellationToken = default);

        IAsyncEnumerable<CrmProperty> ListContactPropertiesAsync(CancellationToken cancellationToken = default);
        Task<CrmProperty?> GetContactPropertyAsync(string propertyName, CancellationToken cancellationToken = default);
        Task CreateContactPropertyAsync(CrmProperty contactProperty, CancellationToken cancellationToken = default);

        IAsyncEnumerable<CrmPropertyGroup> ListCompanyPropertyGroupsAsync(CancellationToken cancellationToken = default);
        Task<CrmPropertyGroup?> GetCompanyPropertyGroupAsync(string groupName, CancellationToken cancellationToken = default);
        Task CreateCompanyPropertyGroupAsync(CrmPropertyGroup propertyGroup, CancellationToken cancellationToken = default);

        IAsyncEnumerable<CrmProperty> ListCompanyPropertiesAsync(CancellationToken cancellationToken = default);
        Task<CrmProperty?> GetCompanyPropertyAsync(string propertyName, CancellationToken cancellationToken = default);
        Task CreateCompanyPropertyAsync(CrmProperty contactProperty, CancellationToken cancellationToken = default);

        Task<Company?> GetCompanyByDenariAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
        Task<Company> CreateOrUpdateCompanyAsync(Company company, CancellationToken cancellationToken = default);

        Task<Contact?> GetContactByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Contact> CreateOrUpdateContactAsync(Contact contact, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> GetOptedOutEmailsAsync(CancellationToken cancellationToken = default);
    }
}
