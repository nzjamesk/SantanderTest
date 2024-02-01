using System;
using NUnit.Framework;

namespace SantanderTest.Models
{
    [TestFixture]
    public class StoryResultTests
    {
        [Test]
        public void Can_convert_object()
        {
            var now = DateTime.UtcNow;
            var unixNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            var offset = new DateTimeOffset(unixNow);
            
            var source = new HackerNewsStory
            {
                By = "Banksy",
                Descendants = 123,
                Id = 51,
                Kids = new[] {1, 3, 5, 7, 9},
                Score = 411,
                Time = offset.ToUnixTimeSeconds(),
                Title = "Test title",
                Url = @"http://test.com/url?123&456"
            };

            var obj = StoryResult.FromHackerNewsStory(source);

            Assert.That(obj.PostedBy.Equals("Banksy"));
            Assert.That(obj.CommentCount.Equals(123));
            Assert.That(obj.Score.Equals(411));
            Assert.That(obj.Title.Equals("Test title"));
            Assert.That(obj.Uri.Equals(@"http://test.com/url?123&456"));
            Assert.That(obj.Time.Ticks.Equals(unixNow.Ticks));

        }
    }
}
