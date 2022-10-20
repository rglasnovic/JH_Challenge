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
            Clients.All.SendAsync("StartMonitoring");
            _twitterService.StartMonitoring();
        }

        public void StopMonitoring()
        {
            _logger.LogInformation("StopMonitoring");
            Clients.All.SendAsync("StopMonitoring");
            _twitterService.StopMonitoring();
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