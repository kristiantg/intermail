using intermail.Clients;
using intermail.Requests;
using System.Net;

namespace intermail.Endpoints
{
    public static class CustomersEndpoint
    {
        public static void MapCustomersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/customers");

            group.MapPost("", AddLoyaltyPoints).WithName(nameof(AddLoyaltyPoints));
        }

        public static async Task<IResult> AddLoyaltyPoints(int customerId, int amount, IIntermailHttpClient httpClient, ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger("AddLoyaltyPointsEndpoint");
            logger.LogInformation("Received request to add loyalty points customerId: {@customerId} with amount: {@amount}", customerId, amount);

            var request = new AddLoyaltyPointsRequest(customerId, amount);

            try
            {
                var response = await httpClient.AddLoyaltyPointsToCustomer(request, Thread.CurrentThread.ManagedThreadId);
                var responseContent = await response.Content.ReadAsStringAsync(); 

                return Results.Ok(responseContent); 
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "HTTP error when adding loyalty points for customer {CustomerId}", request.customerId);
                return Results.StatusCode((int) ex.StatusCode); 
            }
        }
    }
}
