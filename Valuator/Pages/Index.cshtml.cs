using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SharedLib;

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

        public IActionResult OnPost(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Redirect("/");
            }

            _logger.LogDebug(text);

            string id = Guid.NewGuid().ToString();

            _storage.Store(Constants.SimilarityKeyPrefix + id, GetSimilarity(text).ToString());

            _storage.Store(Constants.TextKeyPrefix + id, text);

            _messageBroker.Publish(Constants.RankKeyPrefix, id);

            return Redirect($"summary?id={id}");
        }

        private int GetSimilarity(string text)
        {
            var keys = _storage.GetKeys();

            foreach (string key in keys)
            {
                if (key.Substring(0, 5) == Constants.TextKeyPrefix && _storage.Load(key) == text)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
