using CarvedRock.Api.ApiModels;
using CarvedRock.Api.Interfaces;
using Serilog;

namespace CarvedRock.Api.Domain
{
    public class QuickOrderLogic : IQuickOrderLogic
    {
        public QuickOrderLogic()
        {
        }

        public Guid PlaceQuickOrder(QuickOrder order, int customerId)
        {
            Log.Information("Placing order and sending update for inventory...");

            return Guid.NewGuid();
        }
    }
}
