using System.Text.Json.Serialization;

namespace SantanderTest
{
    public class StoryResult
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("postedBy")]
        public string PostedBy { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("commentCount")]
        public int CommentCount { get; set; }

        public static StoryResult FromHackerNewsStory(HackerNewsStory data)
        {
            return new StoryResult
            {
                PostedBy = data.By,
                Uri = data.Url,
                Score = data.Score,
                Time = DateTimeOffset.FromUnixTimeSeconds(data.Time).DateTime,
                CommentCount = data.Descendants,
                Title = data.Title,
            };
        }

    }
}