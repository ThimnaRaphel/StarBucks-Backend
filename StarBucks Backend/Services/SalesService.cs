using StarBucks_Backend.Models;
using StarBucks_Backend.Services;

namespace StarBucks_Backend.Services
{
    public interface ISalesService
    {
        IEnumerable<Sales> GetSalesRecord();
    }
}
