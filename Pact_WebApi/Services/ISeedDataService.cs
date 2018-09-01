using System.Threading.Tasks;

namespace Pact_WebApi.Services
{
    public interface ISeedDataService
    {
        Task EnsureSeedData();
    }
}