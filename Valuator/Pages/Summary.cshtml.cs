using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Globalization;
using SharedLib;

namespace Valuator.Pages
{
    public class SummaryModel : PageModel
    {
        private readonly ILogger<SummaryModel> _logger;
        private readonly IStorage _storage;

        public SummaryModel(ILogger<SummaryModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public double Rank { get; set; }
        public double Similarity { get; set; }

        private async Task<string> GetRankAsync(string id, string shard)
        {
            const int tryCount = 1000;
            for (var i = 0; i < tryCount; i++)
            {
                var rank = _storage.Load(shard, Constants.RankKeyPrefix + id);
                if (rank != null)
                    return rank;

                await Task.Delay(10);
            }

            return null;
        }

        public async Task OnGetAsync(string id)
        {
            _logger.LogDebug(id);

            var shard = _storage.LoadShard(id);
            _logger.LogInformation(shard + " : " + id);

            string rank;

            if ((rank = await GetRankAsync(id, shard)) != null)
                Rank = double.Parse(rank, CultureInfo.InvariantCulture);
            else
                _logger.LogWarning("Could not get rank value  on shard " + shard + " for id: " + id);

            Similarity = double.Parse(_storage.Load(shard, Constants.SimilarityKeyPrefix + id), CultureInfo.InvariantCulture);
        }
    }
}
