using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using PortSearchMatchModule.Enums;

namespace PortSearchMatchModule
{
    class Program
    {
        static async Task Main(string[] args)
        {
             await MongoService.PostAsync();
            // await MongoService.ShowAllPorts();

            // var matchResults = await MatcherService.GetMatchCoefficient("OUM");
            //
            // foreach (var matchResult in matchResults)
            // {
            //     Console.WriteLine(matchResult);
            // }
            //
            // var theBestMatch = MatcherService.GetTheBestMatch(matchResults);
            // Console.WriteLine($"THE BEST: {theBestMatch}");

            Console.ReadKey();
        }
    }
}
