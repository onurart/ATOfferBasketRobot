namespace ATBasketRobotServerAPI.Services;

public class APIClients
{
    private readonly HttpClient _httpClient;
    private readonly string _baseURL;

    public APIClients(string baseURL)
    {
        _httpClient = new HttpClient();
        _baseURL = baseURL;
    }
    public async Task<string> GetDataAsync(string endpoint)
    {
        string url = $"{_baseURL}/{endpoint}";
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return null;
        }
    }
}
