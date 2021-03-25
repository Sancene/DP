using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    [Serializable]
    public struct SimilarityMessage
    {
        public string Id { get; set; }
        public double Similarity { get; set; }
    }
}
