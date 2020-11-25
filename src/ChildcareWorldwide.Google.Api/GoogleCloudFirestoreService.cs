﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Helpers;
using ChildcareWorldwide.Google.Api.Models;
using Google.Cloud.Firestore;

namespace ChildcareWorldwide.Google.Api
{
	public sealed class GoogleCloudFirestoreService : IGoogleCloudFirestoreService
	{
		private const string ErrorsCollection = "pubsub-errors";
		private const string AssociationsCollection = "associations";

		private readonly FirestoreDb m_db;

		public GoogleCloudFirestoreService()
		{
			m_db = FirestoreDb.Create(GoogleComputeEngineHelper.GetCurrentProjectId());
		}

		public async Task AddMessagingErrorAsync(MessagingError messagingError, CancellationToken cancellationToken)
		{
			var collection = m_db.Collection(ErrorsCollection);
			await collection.AddAsync(messagingError, cancellationToken: cancellationToken);
		}

		public async Task AddDenariCompanyAssociationAsync(string denariAccount, string companyId, CancellationToken cancellationToken)
		{
			var association = m_db.Collection(AssociationsCollection).Document(denariAccount);
			await association.SetAsync(
				new Dictionary<string, object>()
				{
					{ "CompanyId", companyId },
				},
				SetOptions.MergeAll,
				cancellationToken);
		}

		public async Task AddDenariContactAssociationsAsync(string denariAccount, IEnumerable<string> contactIds, CancellationToken cancellationToken)
		{
			var association = m_db.Collection(AssociationsCollection).Document(denariAccount);
			await association.SetAsync(
				new Dictionary<string, object>()
				{
					{ "ContactIds", contactIds },
				},
				SetOptions.MergeAll,
				cancellationToken);
		}

		public async Task<CrmAssociation?> GetHubspotAssociationsForDonorAsync(string denariAccount, CancellationToken cancellationToken)
		{
			var document = m_db.Collection(AssociationsCollection).Document(denariAccount);
			var snapshot = await document.GetSnapshotAsync(cancellationToken);
			return snapshot.Exists
				? snapshot.ConvertTo<CrmAssociation>()
				: null;
		}
	}
}
