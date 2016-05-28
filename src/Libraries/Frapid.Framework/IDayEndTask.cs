using System.Threading.Tasks;

namespace Frapid.Framework
{
    public interface IDayEndTask
    {
        string[] Tenants { get; set; }
        string Description { get; set; }
        Task RegisterAsync();
    }
}