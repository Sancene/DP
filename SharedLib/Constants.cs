﻿using System;

namespace SharedLib
{
    public static class Constants
    {
        public const string SimilarityKeyPrefix = "SIMILARITY-";
        public const string RankKeyPrefix = "RANK-";
        public const string TextKeyPrefix = "TEXT-";
        public const string Host = "localhost";
        public const string RankKey = "valuator.processing.rank";
        public const string SimilarityKeyCalculated = "valuator.similarity_calculated";
        public const string RankKeyCalculated = "rank_calculator.rank_calculated";
        public const string ShardIdRus = "RUS";
        public const string ShardIdEu = "EU";
        public const string ShardIdOther = "OTHER";

        public static string HostName
        {
            get
            {
                var hostName = Environment.GetEnvironmentVariable("MACHINE_IP");
                return string.IsNullOrWhiteSpace(hostName) ? "localhost" : hostName;
            }
        }
    }
}