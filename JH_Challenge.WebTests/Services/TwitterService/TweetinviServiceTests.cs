using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Streaming.V2;

namespace JH_Challenge.Web.Services.TwitterService.Tests
{
    [TestClass()]
    public class TweetinviServiceTests
    {
        [TestMethod()]
        public void StartMonitoringTest()
        {
            ITwitterClient client;
            ISampleStreamV2 stream;

            client = new TwitterClient("rQybcvLSD5wxz9k9yLv9gc0on",
                                        "HRVW5V5scIeqCvF3bbqfPcpgqWzmd0vdCrXqdZyJHhYfqSQVcc",
                                        "AAAAAAAAAAAAAAAAAAAAAOP%2FiAEAAAAAivkHNwSgRqCIcC9uoan7FePZgwY%3DKfsB2n0OQ0VJbfWHTwY2Ogvc446018Ftjul7bc2XAuNz5ucm8F");

            stream = client.StreamsV2.CreateSampleStream();
            
            Assert.IsNotNull(stream);
        }
    }
}