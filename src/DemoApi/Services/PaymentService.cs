using System.Net.Http.Headers;

namespace DemoApi.Services;

public class PaymentService
{    

    public async Task<string> ChargeAsync(string cardToken, decimal amount)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", ApiKey);

        var response = await client.PostAsJsonAsync($"{ApiUrl}/charge", new
        {
            card_token = cardToken,
            amount
        });

        return await response.Content.ReadAsStringAsync();
    }
}
