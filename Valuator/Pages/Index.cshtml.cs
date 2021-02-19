using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Redirect("/");
            }

            _logger.LogDebug(text);

            string id = Guid.NewGuid().ToString();

            string similarityKey = "SIMILARITY-" + id;
            int similarity = GetSimilarity(text);
            _storage.Store(similarityKey, similarity.ToString());

            string textKey = "TEXT-" + id;
            _storage.Store(textKey, text);

            string rankKey = "RANK-" + id;
            double rank = GetRank(text);
            _logger.LogWarning(rank.ToString());
            _storage.Store(rankKey, rank.ToString());

            return Redirect($"summary?id={id}");
        }

        private double GetRank(string text)
        {
            int nonLetterCount = 0;
            foreach (var ch in text)
            {
                if (!Char.IsLetter(ch))
                {
                    nonLetterCount++;
                }
            }
            return Math.Round(((double)nonLetterCount / text.Length), 3);
        }

        private int GetSimilarity(string text)
        {
            List<string> keys = _storage.GetKeys();

            foreach (string key in keys)
            {
                if (key.Substring(0, 5) == "TEXT-" && _storage.Load(key) == text)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
