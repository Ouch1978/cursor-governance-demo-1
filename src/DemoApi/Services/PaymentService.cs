using System.Net.Http.Headers;

namespace DemoApi.Services;

/// <summary>
/// ⚠️ 這個檔案是「故意寫錯」的觸發範例。
///    用途：讓 Rule 在你請 AI 修改這個檔案時自動提醒你。
///    請不要在正式專案裡這樣寫。
/// </summary>
public class PaymentService
{
    // ❌ 寫死 API Key（觸發 no-hardcoded-secrets Rule 用）
    private const string ApiKey = "sk-live-xK9mN2pQrT5vWzAb3cDeFgHiJkLmNoP";
    private const string ApiUrl = "https://api.payment-gateway.com/v1";

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
