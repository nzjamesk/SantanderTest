using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;

namespace SantanderTest
{
    [TestFixture]
    public class HackerNewsProviderTests
    {
        [Test]
        public async Task Can_return_required_story_count()
        {
            var client = new RestClient("https://hacker-news.firebaseio.com/v0/");
            var provider = new HackerNewsProvider(client);

            var results = await provider.GetLatestStoriesAsync(3);

            Assert.That(results.Count().Equals(3));
        }

        [Test]
        public async Task Can_return_available_stories_if_not_enough()
        {
            var client = new RestClient("https://hacker-news.firebaseio.com/v0/");
            var provider = new HackerNewsProvider(client);

            var numberToReturn = 300000000;

            var results = await provider.GetLatestStoriesAsync(numberToReturn);

            Assert.That(results.Count() < numberToReturn);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(15)]
        public async Task Can_return_story_data(int storyCount)
        {
            var client = new RestClient("https://hacker-news.firebaseio.com/v0/");
            var provider = new HackerNewsProvider(client);

            var results = await provider.GetLatestStoriesAsync(storyCount);
            var stories = provider.GetStories(results);
            var actualCount = 0;

            await foreach (var story in stories)
            {
                Assert.That(!string.IsNullOrEmpty(story.PostedBy));
                Assert.That(!string.IsNullOrEmpty(story.Title));
                Assert.That(!string.IsNullOrEmpty(story.Uri));
                Assert.That(story.CommentCount > 0);
                Assert.That(story.Score > 0);
                Assert.That(story.Time > new DateTime(2000,1,1));
                actualCount++;
            }

            Assert.That(storyCount.Equals(actualCount));
        }
    }
}
