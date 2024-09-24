using intermail.Clients;
using intermail.Requests;

public class IntermailHttpClient : IIntermailHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IntermailHttpClient> _logger;

    public IntermailHttpClient(HttpClient httpClient, ILogger<IntermailHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> AddLoyaltyPointsToCustomer(AddLoyaltyPointsRequest request, int transactionId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"customer/{request.customerId}/loyaltypoints", request);
            _logger.LogInformation("Request: {request}. Response: {response}. TransactionId: {transactionId}", request, response, transactionId);


            response.EnsureSuccessStatusCode();

            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error adding loyalty points to customer {CustomerId}", request.customerId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding loyalty points to customer {CustomerId}", request.customerId);
            throw; 
        }
    }
}