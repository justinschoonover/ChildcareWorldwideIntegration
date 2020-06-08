using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChildcareWorldwide.Google.Api.PubSub
{
    public static class Topics
    {
        public const string HubspotBeginImport = "hubspot-begin-import";
        public const string HubspotImportFromDonor = "hubspot-import-from-donor";

        public static IReadOnlyList<string> AllTopics =>
            new List<string>(typeof(Topics)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(null) as string)
                .Where(x => x != null) !);
    }
}
