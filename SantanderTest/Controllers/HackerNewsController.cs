using Microsoft.AspNetCore.Mvc;

namespace SantanderTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {

        private readonly ILogger<HackerNewsController> _logger;
        private readonly IHackerNewsProvider _hackerNewsProvider;

        public HackerNewsController(ILogger<HackerNewsController> logger, IHackerNewsProvider hackerNewsProvider)
        {
            _logger = logger;
            _hackerNewsProvider = hackerNewsProvider;
        }

        [HttpGet(Name = "GetBestHackerNewsStories")]
        public async Task<IEnumerable<StoryResult>> Get(int storiesToReturn)
        {
            try
            {
                var bestStories = await _hackerNewsProvider.GetLatestStoriesAsync(storiesToReturn);

                var stories = new List<StoryResult>();

                await foreach (var story in _hackerNewsProvider.GetStories(bestStories))
                {
                    if (story == null)
                        throw new NullReferenceException("Null data returned from news provider");

                    stories.Add(story);
                }

                return stories;
            }
            catch (HttpRequestException hre)
            {
                _logger.LogError(hre, "Error occurred fetching latest stories");
                throw;
            }
            catch (NullReferenceException nre)
            {
                _logger.LogError(nre, "Null data encountered whilst generating results");
                throw;
            }
        }
    }
}