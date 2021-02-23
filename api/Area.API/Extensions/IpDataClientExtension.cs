using System.Net;
using System.Threading.Tasks;
using IpData;

namespace Area.API.Extensions
{
    public static class IpDataClientExtension
    {
        public static async Task<string?> GetCountry(this IpDataClient ipDataClient, IPAddress ipAddress)
        {
            try {
                var ipv4 = ipAddress.MapToIPv4().ToString();
                var ipInfo = await ipDataClient.Lookup(ipv4);
                return ipInfo.CountryName;
            } catch {
                return null;
            }
        }
    }
}