using JH_Challenge.Web.Services;
using Microsoft.AspNetCore.SignalR;

namespace JH_Challenge.Web.Hubs
{
    public class TwitterHub : Hub
    {
        ILogger<TwitterHub> _logger;
        ITwitterService _twitterService = null!;

        public TwitterHub(ILogger<TwitterHub> logger, ITwitterService twitterService)
        {
            _logger = logger;
            _twitterService = twitterService;
        }

        public void StartMonitoring()
        {
            _logger.LogInformation("StartMonitoring");
            _twitterService.StartMonitoring();
            Clients.All.SendAsync("StartMonitoring");
        }

        public void StopMonitoring()
        {
            _logger.LogInformation("StopMonitoring");
            _twitterService.StopMonitoring();
            Clients.All.SendAsync("StopMonitoring");
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}