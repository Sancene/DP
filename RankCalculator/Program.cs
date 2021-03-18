﻿using System.Threading.Tasks;
using SharedLib;

namespace RankCalculator
{
    internal static class Program
    {
        private static async Task Main()
        {
            var calculator = new RankCalculator(new RedisStorage());
            calculator.Run();
            await Task.Delay(-1);
        }
    }
}