using System.Net;
using RestSharp;

namespace SantanderTest;

public sealed class HackerNewsProvider : IHackerNewsProvider
{
    private readonly IRestClient _client;

    private const string BestStoriesUrl = "beststories.json";

    public HackerNewsProvider(IRestClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<int>> GetLatestStoriesAsync(int storiesToReturn)
    {
            
        var request = new RestRequest(BestStoriesUrl);
        var response = await _client.ExecuteAsync<int[]>(request);
        var bestStories = response.Data ?? Array.Empty<int>();
        return bestStories.Take(storiesToReturn);
    }

    public async IAsyncEnumerable<StoryResult> GetStories(IEnumerable<int> stories)
    {
        foreach (var storyId in stories)
        {
            var request = new RestRequest($"item/{storyId}.json");
            var response = await _client.ExecuteAsync<HackerNewsStory>(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Error retrieving story with ID {storyId}", response.ErrorException, response.StatusCode);

            if (response.Data == null)
                throw new HttpRequestException($"No data returned for story with ID {storyId}");

            yield return StoryResult.FromHackerNewsStory(response.Data);
        }
    }

}