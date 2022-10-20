using JH_Challenge.Web.Hubs;
using JH_Challenge.Web.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using Tweetinvi;
using Tweetinvi.Models.V2;
using Tweetinvi.Streaming.V2;

namespace JH_Challenge.Web.Services.TwitterService
{
    public class TweetinviService : BaseTwitterService, ITwitterService
    {
        private ITwitterClient _client = null!;
        private ISampleStreamV2 _stream = null!;

        public TweetinviService(IConfiguration configuration, IHubContext<TwitterHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;

            _client = new TwitterClient(_configuration.GetValue<string>("TwitterAPI:APIKey"),
                                        _configuration.GetValue<string>("TwitterAPI:APIKeySecret"),
                                        _configuration.GetValue<string>("TwitterAPI:BearerToken"));
        }

        public async void StartMonitoring()
        {
            ResetMonitoring();

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            if (_stream == null)
            {
                _stream = _client.StreamsV2.CreateSampleStream();

                _stream.TweetReceived += async (sender, args) =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _stream.StopStream();
                        _stream = null!;
                    }

                    TotalTweetCount += 1;

                    if (args.Tweet.Entities.Hashtags != null && args.Tweet.Entities.Hashtags.Any())
                        AddHashtags(args.Tweet.Entities.Hashtags);

                    await _hubContext.Clients.All.SendAsync("Update", TotalTweetCount, HashtagList.OrderByDescending(h => h.Value).Take(10));
                };

                await _hubContext.Clients.All.SendAsync("Start", $"Started at {DateTime.Now.ToLongTimeString()}");
                await _stream.StartAsync();
            }
        }

        public void StopMonitoring()
        {
            _cancellationTokenSource.Cancel();
        }

        private void AddHashtags(HashtagV2[] hashtagV2Array)
        {
            foreach (HashtagV2 hashtagV2 in hashtagV2Array)
            {
                //Remove Arabic Tweets (Causing RTL Display Issue in List)
                if (Regex.IsMatch(hashtagV2.Tag, @"\p{IsArabic}"))
                    return;

                if (HashtagList.ContainsKey($"{hashtagV2.Tag}"))
                    HashtagList[$"{hashtagV2.Tag}"] += 1;
                else
                    HashtagList.Add($"{hashtagV2.Tag}", 1);
            }
        }
    }
}