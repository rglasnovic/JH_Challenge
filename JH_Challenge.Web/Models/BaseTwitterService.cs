using JH_Challenge.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace JH_Challenge.Web.Models
{
    public class BaseTwitterService
    {
        public CancellationTokenSource _cancellationTokenSource = null!;
        public IConfiguration _configuration { get; set; } = null!;
        public IHubContext<TwitterHub> _hubContext { get; set; } = null!;

        public Dictionary<string, int> HashtagList { get; set; } = new Dictionary<string, int>();
        public int TotalTweetCount { get; set; } = 0;

        public void ResetMonitoring()
        {
            HashtagList = new Dictionary<string, int>();
            TotalTweetCount = 0;
        }
    }
}
