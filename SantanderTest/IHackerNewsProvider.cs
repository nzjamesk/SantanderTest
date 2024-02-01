namespace SantanderTest
{
    public interface IHackerNewsProvider
    {
        Task<IEnumerable<int>> GetLatestStoriesAsync(int storiesToReturn);
        IAsyncEnumerable<StoryResult?> GetStories(IEnumerable<int> stories);
    }
}