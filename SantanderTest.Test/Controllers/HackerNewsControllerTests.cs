using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using SantanderTest.Controllers;

namespace SantanderTest
{
    [TestFixture]
    public class HackerNewsControllerTests
    {
        [Test]
        public async Task Can_get_stories()
        {
            var provider = new Mock<IHackerNewsProvider>();
            var logger = new Mock<ILogger<HackerNewsController>>();

            var now = DateTime.UtcNow;

            var storyIds = new[] {123, 456, 789}.AsEnumerable();

            var stories = new[]
            {
                new StoryResult
                {
                    PostedBy = "by1", CommentCount = 11, Score = 111, Time = now, Title = "11",
                    Uri = "http://11"
                },
                new StoryResult
                {
                    PostedBy = "by2", CommentCount = 12, Score = 222, Time = now, Title = "22",
                    Uri = "http://22"
                },
                new StoryResult
                {
                    PostedBy = "by3", CommentCount = 13, Score = 333, Time = now, Title = "33",
                    Uri = "http://33",
                }
            };

            provider.Setup(x => x.GetLatestStoriesAsync(It.IsAny<int>())).Returns(Task.FromResult(storyIds));
            provider.Setup(x => x.GetStories(storyIds)).Returns(stories.ToAsyncEnumerable());

            var controller = new HackerNewsController(logger.Object, provider.Object);

            var result = await controller.Get(3);

            Assert.That(result.Count().Equals(3));
            Assert.That(result.Contains(stories[0]));
            Assert.That(result.Contains(stories[1]));
            Assert.That(result.Contains(stories[2]));

        }

        [Test]
        public void Throws_on_null_data_returned()
        {
            var provider = new Mock<IHackerNewsProvider>();
            var logger = new Mock<ILogger<HackerNewsController>>();

            var now = DateTime.UtcNow;

            var storyIds = new[] { 123, 456, 789 };

            var stories = new[]
            {
                new StoryResult
                {
                    PostedBy = "by1", CommentCount = 11, Score = 111, Time = now, Title = "11",
                    Uri = "http://11"
                },
                null,
                new StoryResult
                {
                    PostedBy = "by3", CommentCount = 13, Score = 333, Time = now, Title = "33",
                    Uri = "http://33",
                }
            };

            provider.Setup(x => x.GetLatestStoriesAsync(It.IsAny<int>())).Returns(Task.FromResult(storyIds.AsEnumerable()));
            provider.Setup(x => x.GetStories(storyIds)).Returns(stories.ToAsyncEnumerable());

            var controller = new HackerNewsController(logger.Object, provider.Object);

            Assert.ThrowsAsync<NullReferenceException>(async() => await controller.Get(3));
        }

        [Test]
        public void Throws_on_http_exception()
        {
            var provider = new Mock<IHackerNewsProvider>();
            var logger = new Mock<ILogger<HackerNewsController>>();

            provider.Setup(x => x.GetLatestStoriesAsync(It.IsAny<int>())).Throws<HttpRequestException>();

            var controller = new HackerNewsController(logger.Object, provider.Object);

            Assert.ThrowsAsync<HttpRequestException>(async () => await controller.Get(3));
        }
    }
}