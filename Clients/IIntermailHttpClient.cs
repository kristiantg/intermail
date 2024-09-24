using intermail.Requests;

namespace intermail.Clients
{
    public interface IIntermailHttpClient
    {
        Task<HttpResponseMessage> AddLoyaltyPointsToCustomer(AddLoyaltyPointsRequest request);
    }
}
