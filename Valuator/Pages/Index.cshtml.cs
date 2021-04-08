using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SharedLib;
using System.Text.Json;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage, IMessageBroker messageBroker)
        {
            _logger = logger;
            _storage = storage;
            _messageBroker = messageBroker;
        }

        public IActionResult OnPost(string text, string country)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Redirect("/");
            }

            _logger.LogDebug(text);

            string id = Guid.NewGuid().ToString();
            _logger.LogInformation(country + " " + id);

            var similarity = GetSimilarity(text);
            _storage.StoreShard(id, country);
            _storage.Store(country, Constants.SimilarityKeyPrefix + id, similarity.ToString());

            _messageBroker.Publish(Constants.SimilarityKeyCalculated,
                JsonSerializer.Serialize(new SimilarityMessage { Id = id, Similarity = similarity }));

            _storage.Store(country, Constants.TextKeyPrefix + id, text);

            _messageBroker.Publish(Constants.RankKey, id);

            return Redirect($"summary?id={id}");
        }


        private int GetSimilarity(string text)
        {
            return _storage.DoesSimilarTextExist(text) ? 1 : 0;
        }
    }
}
