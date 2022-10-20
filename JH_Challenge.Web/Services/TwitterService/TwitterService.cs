using JH_Challenge.Web.Hubs;
using JH_Challenge.Web.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace JH_Challenge.Web.Services.TwitterService
{
    public class TwitterService : BaseTwitterService, ITwitterService
    {
        private HttpClient _httpClient = new HttpClient();
        private Stream _stream;

        public TwitterService(IConfiguration configuration, IHubContext<TwitterHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;

            _httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("TwitterAPI:BaseUrl"));
            _httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _configuration.GetValue<string>("TwitterAPI:BearerToken"));
        }

        public async void StartMonitoring()
        {
            ResetMonitoring();

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _stream = _httpClient.GetStreamAsync("2/tweets/sample/stream?tweet.fields=entities").Result;
            using (var reader = new StreamReader(_stream))
            {
                string response;
                while (!reader.EndOfStream)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _httpClient.DefaultRequestHeaders.ConnectionClose = true;
                    }

                    response = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(response)) continue;

                    Tweet tweet = JsonConvert.DeserializeObject<Tweet>(response);
                    if (tweet != null)
                    {
                        TotalTweetCount += 1;

                        if (tweet.Data.Entities.Hashtags != null && tweet.Data.Entities.Hashtags.Any())
                            AddHashtags(tweet.Data.Entities.Hashtags);

                        await _hubContext.Clients.All.SendAsync("Update", TotalTweetCount, HashtagList.OrderByDescending(h => h.Value).Take(10));
                    }
                }
                //}
            }
        }

        public void StopMonitoring()
        {
            _cancellationTokenSource.Cancel();
        }

        private void AddHashtags(List<Hashtag> hashtagList)
        {
            foreach (Hashtag hashtag in hashtagList)
            {
                //Remove Arabic Tweets (Causing RTL Display Issue in List)
                if (Regex.IsMatch(hashtag.Tag!, @"\p{IsArabic}"))
                    return;

                if (HashtagList.ContainsKey($"{hashtag.Tag}"))
                    HashtagList[$"{hashtag.Tag}"] += 1;
                else
                    HashtagList.Add($"{hashtag.Tag}", 1);
            }
        }
    }
}