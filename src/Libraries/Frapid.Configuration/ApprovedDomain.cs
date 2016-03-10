using System.Collections.Generic;
using System.Linq;

namespace Frapid.Configuration
{
    public class ApprovedDomain
    {
        public string DomainName { get; set; }
        public string BackupDirectory { get; set; }
        public bool BackupDirectoryIsFixedPath { get; set; }
        public bool EnforceSsl { get; set; }
        public string CdnDomain { get; set; }
        public string AdminEmail { get; set; }

        public string[] Synonyms { get; set; }

        public object Do { get; internal set; }

        public List<string> GetSubtenants()
        {
            var subtenants = new List<string>();

            subtenants.Add(this.DomainName);
            subtenants.Add(this.CdnDomain);
            subtenants.AddRange(this.Synonyms);

            return subtenants;
        }
    }
}