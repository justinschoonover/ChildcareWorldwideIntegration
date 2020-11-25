﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldwide.Tools.CloneHubspotProperties
{
	[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable")]
	public class Program
	{
		public static async Task Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("Usage: CloneHubspotProperties.exe [Production API key] [Sandbox API key]");
				return;
			}

			await CloneHubspotProperties(args[0], args[1]);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
		public static async Task CloneHubspotProperties(string productionApiKey, string sandboxApiKey)
		{
			using var productionService = new HubspotService(
				new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "HubspotApiKey", productionApiKey },
				})
				.Build(), new MemoryCache(new MemoryCacheOptions()));
			using var sandboxService = new HubspotService(
				new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "HubspotApiKey", sandboxApiKey },
				})
				.Build(), new MemoryCache(new MemoryCacheOptions()));

			Console.WriteLine("Cloning Custom Contact Property Groups...");
			await foreach (var propertyGroup in productionService.ListContactPropertyGroupsAsync())
			{
				if (await sandboxService.GetContactPropertyGroupAsync(propertyGroup.Name) == null)
				{
					Console.WriteLine(propertyGroup.Name);
					await sandboxService.CreateContactPropertyGroupAsync(propertyGroup);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Cloning Custom Contact Properties...");
			await foreach (var property in productionService.ListContactPropertiesAsync())
			{
				if (property.Name.StartsWith("hs_", StringComparison.InvariantCultureIgnoreCase))
					continue;

				if (await sandboxService.GetContactPropertyAsync(property.Name) == null)
				{
					Console.WriteLine(property.Name);
					await sandboxService.CreateContactPropertyAsync(property);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Cloning Custom Company Property Groups...");
			await foreach (var propertyGroup in productionService.ListCompanyPropertyGroupsAsync())
			{
				if (await sandboxService.GetCompanyPropertyGroupAsync(propertyGroup.Name) == null)
				{
					Console.WriteLine(propertyGroup.Name);
					await sandboxService.CreateCompanyPropertyGroupAsync(propertyGroup);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Cloning Custom Company Properties...");
			await foreach (var property in productionService.ListCompanyPropertiesAsync())
			{
				if (property.Name.StartsWith("hs_", StringComparison.InvariantCultureIgnoreCase))
					continue;

				if (await sandboxService.GetCompanyPropertyAsync(property.Name) == null)
				{
					Console.WriteLine(property.Name);
					await sandboxService.CreateCompanyPropertyAsync(property);
				}
			}
		}
	}
}
