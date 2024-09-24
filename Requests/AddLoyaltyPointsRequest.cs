namespace intermail.Requests
{
    public record AddLoyaltyPointsRequest(int customerId, int amount, string source = "AutomationFixedLoyaltyPoints");
}
