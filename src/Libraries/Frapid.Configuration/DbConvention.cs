using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

namespace Frapid.Configuration
{
    public class DbConvention
    {
        public static string GetDomain()
        {
            if (HttpContext.Current == null)
            {
                Log.Information($"Cannot resolve a domain for current request because HttpContext was null.");
                return string.Empty;
            }
            string url = HttpContext.Current.Request.Url.Authority;

            if (url.StartsWith("www."))
            {
                url = url.Replace("www.", "");
            }

            return url;
        }


        public static bool IsStaticDomain(string domain = "")
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = GetDomain();
            }

            Log.Verbose($"Checking if the domain \"{domain}\" is a static domain.");

            var approved = new DomainSerializer("DomainsApproved.json");
            var approvedDomains = approved.Get();

            var tenant = approvedDomains.FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            if (tenant != null)
            {
                bool isStatic = domain.ToUpperInvariant().Equals(tenant.CdnDomain.ToUpperInvariant());

                Log.Verbose(isStatic
                    ? $"The domain \"{domain}\" is a static domain."
                    : $"The domain \"{domain}\" is not a static domain.");

                return isStatic;
            }

            return false;
        }

        public static bool EnforceSsl(string domain = "")
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = GetDomain();
            }

            Log.Verbose($"Getting SSL configuration for domain \"{domain}\".");

            var approved = new DomainSerializer("DomainsApproved.json");
            var approvedDomains = approved.Get();

            var tenant = approvedDomains.FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            if (tenant != null)
            {
                Log.Verbose(tenant.EnforceSsl
                    ? $"SSL is enforced on domain \"{domain}\"."
                    : $"SSL is optional on domain \"{domain}\".");

                return tenant.EnforceSsl;
            }

            Log.Verbose($"Cannot find SSL configuration because no approved domain entry found for \"{domain}\".");
            return false;
        }

        private static string ConvertToDbName(string domain)
        {
            return domain.Replace(".", "_");
        }

        public static string GetDbNameByConvention(string domain = "")
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = GetDomain();
            }

            Log.Verbose($"Getting tenant name for domain \"{domain}\"");

            var approved = new DomainSerializer("DomainsApproved.json");
            var tenant = approved.Get().FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            if (tenant != null)
            {
                Log.Verbose($"Tenant found for domain \"{domain}\". Tenant domain: \"{tenant.DomainName}\".");
                return ConvertToDbName(tenant.DomainName);
            }

            return ConvertToDbName(domain);
        }

        public static bool IsValidTenant(string tenant = "")
        {
            if (string.IsNullOrWhiteSpace(tenant))
            {
                tenant = GetDbNameByConvention(tenant);
                Log.Verbose($"The tenant for empty domain was automatically resolved to \"{tenant}\".");
            }

            var serializer = new DomainSerializer("DomainsApproved.json");


            bool result = serializer.Get().Any(domain => GetDbNameByConvention(domain.DomainName) == tenant);

            if (!result)
            {
                Log.Information($"The tenant \"{tenant}\" was not found on list of approved domains. Please check your configuration");
            }

            return result;
        }

        public static List<ApprovedDomain> GetDomains()
        {
            var serializer = new DomainSerializer("DomainsApproved.json");
            return serializer.Get();
        }

        public static List<string> GetTenants()
        {
            var serializer = new DomainSerializer("DomainsApproved.json");
            return serializer.Get().Select(member => GetDbNameByConvention(member.DomainName)).ToList();
        }

        public static string GetTenant(string url = "")
        {
            string tenant = GetDbNameByConvention(url);
            string defaultTenant = System.Configuration.ConfigurationManager.AppSettings["DefaultTenant"];

            if (!IsValidTenant(tenant))
            {
                Log.Information($"Falling back to default tenant \"{defaultTenant}\" because the requested tenant \"{tenant}\" was invalid.");
                tenant = defaultTenant;
            }

            Log.Verbose($"The tenant for domain \"{url}\" is \"{tenant}\".");

            return tenant;
        }
    }
}