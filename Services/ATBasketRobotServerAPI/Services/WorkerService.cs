using ATBasketRobotServerAPI.Models;

namespace ATBasketRobotServerAPI.Services;
public class WorkerService : BackgroundService
{
    private readonly ILogger<WorkerService> _logger;
    IConfigurationRoot _configuration;
    bool _disposed;
    public WorkerService(ILogger<WorkerService> logger, IConfigurationRoot configuration = null)
    {
        _logger = logger;
        _configuration = configuration;
    }

    private ApiSettings LoadApiSettings()
    {
        var apiSetting = new ApiSettings();
        _configuration.GetSection("ApiSettings").Bind(apiSetting);
        return apiSetting;
    }
    //private const int generalDelay = 1 * 10 * 1000; // 10 seconds
    private const int generalDelay = 1 * 10 * 10000; // 10 seconds

    private readonly string[] endpoints = {
        "Product/CreateProductAll",
        "Customer/CreateCustomerAll",
        "ProductCampaign/CreateProductCampaignAll",
        "Document/CreateDocumentAll",
        "BasketStatus/CreateBasketStatusAll",
        "Offer/CreateOfferAll"
    };

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (_disposed)
        {
            _logger.LogInformation("Worker is running");
            // await Task.Delay(generalDelay, stoppingToken);
            // await DoBackupAsync();
        }
        _logger.LogInformation("Worker is stopping");
    }

    private async Task DoBackupAsync()
    {
        //string baseURL = "https://localhost:7065/api";
        //APIClients client = new APIClients(baseURL);

        //foreach (var endpoint in endpoints)
        //{
        //    await ProcessEndpointAsync(client, endpoint);
        //    await Task.Delay(1000);
        //}

        var apiSetting = LoadApiSettings(); // Change to apiSetting
        using (var client = new APIClients(apiSetting.BaseURL))
        {
            foreach (var endpoint in endpoints)
            {
                await ProcessEndpointAsync(client, endpoint);
                await Task.Delay(apiSetting.DelayMilliseconds);
            }
        }
    }

    private async Task ProcessEndpointAsync(APIClients client, string endpoint)
    {
        string data = await client.GetDataAsync(endpoint);

        if (!string.IsNullOrEmpty(data))
        {
            Console.WriteLine($"Executing {endpoint} task");
            Console.WriteLine(data);
        }
        else
        {
            Console.WriteLine($"Veri alınamadı. - {endpoint}");
        }
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {

        _logger.LogWarning("Consume Scoped Service Hosted Service is starting.");
        _disposed = true;
        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogWarning("Consume Scoped Service Hosted Service is stopping.");
        _disposed = false;
        //var result = await _chafone718Service.StopReadAsync();
        await base.StopAsync(stoppingToken);
    }
}