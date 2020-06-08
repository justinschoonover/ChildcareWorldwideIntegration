using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChildcareWorldwide.Hubspot.Api.Helpers
{
    public static class HttpRequestUtility
    {
        public static async Task EnsureSuccessStatusCodeWithResponseBodyInException(this HttpResponseMessage? response)
        {
            if (response == null)
                return;

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync(), e);
            }
        }
    }
}
