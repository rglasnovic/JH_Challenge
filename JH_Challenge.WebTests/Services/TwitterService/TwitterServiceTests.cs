using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JH_Challenge.Web.Services.TwitterService.Tests
{
    [TestClass()]
    public class TwitterServiceTests
    {
        [TestMethod()]
        public void StartMonitoringTest()
        {
            HttpClient httpClient = new HttpClient();
            Stream stream;

            httpClient.BaseAddress = new Uri("https://api.twitter.com/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAAOP%2FiAEAAAAAivkHNwSgRqCIcC9uoan7FePZgwY%3DKfsB2n0OQ0VJbfWHTwY2Ogvc446018Ftjul7bc2XAuNz5ucm8F");
            stream = httpClient.GetStreamAsync("2/tweets/sample/stream?tweet.fields=entities").Result;
            Assert.IsNotNull(stream);
        }
    }
}