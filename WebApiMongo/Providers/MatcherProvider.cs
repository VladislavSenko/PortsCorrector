using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using WebApiMongo.Entities;
using WebApiMongo.Enums;
using WebApiMongo.Helpers;
using WebApiMongo.Models;
using WebApiMongo.Settings;

namespace WebApiMongo.Services
{
    public class MatcherProvider
    {
        public MatcherProvider()
        {
        }

        public List<MatchResultModel> GetMatchCoefficient(string inputWord, List<Port> ports)
        {
            var matcher = new MatcherHelper(MethodsEnum.SorensenDice, inputWord);

            var matchResults = new List<MatchResultModel>();

            foreach (var port in ports)
            {
                var matchResult = new MatchResultModel();

                matchResult.Id = port.Id;
                matchResult.PortCode = port.PortCode;
                matchResult.PortCodeMatchCoefficient = matcher.GetCoefficient(port.PortCode);

                matchResult.PortNameMatchResults = port.PortName.Select(p => new PortNameMatchResult
                {
                    PortName = p,
                    PortNameMatchCoefficient =  matcher.GetCoefficient(p)
                }).OrderByDescending(p => p.PortNameMatchCoefficient)
                    .ToList();
               
                matchResults.Add(matchResult);
            }

            return matchResults;
        }

        public List<TheBestMatchModel> GetTheBestMatch(List<MatchResultModel> matchResults)
        {
            var howMuchMatchesReturn = AppSettings.ReturnMatches;

            var matchResultOrderByCodeCoeff = matchResults.OrderByDescending(m => m.PortCodeMatchCoefficient)
                .Select(r => new TheBestMatchModel()
                {
                    PortId = r.Id,
                    Value =  r.PortCode,
                    isPortCode = true,
                    MatchScore =  r.PortCodeMatchCoefficient
                })
                .Take(howMuchMatchesReturn)
                .ToList();

            var matchResultOrderByNameCoeff = matchResults
                .OrderByDescending(m => m.PortNameMatchResults.Max(p => p.PortNameMatchCoefficient))
                .Select(r => new TheBestMatchModel()
                {
                    PortId = r.Id,
                    Value = r.PortNameMatchResults.OrderByDescending(s => s.PortNameMatchCoefficient).First().PortName,
                    isPortCode = false,
                    MatchScore = r.PortNameMatchResults.Max(p => p.PortNameMatchCoefficient)
                })
                .Take(howMuchMatchesReturn)
                .ToList();

            var theBestMatchModels = new List<TheBestMatchModel>();
            theBestMatchModels.AddRange(matchResultOrderByCodeCoeff);
            theBestMatchModels.AddRange(matchResultOrderByNameCoeff);

            // decimal theBestCoefficient = 0;

            // var theBestMatchModel = new TheBestMatchModel();
            // foreach (var matchResult in matchResults)
            // {
            //     if (matchResult.PortCodeMatchCoefficient > theBestCoefficient)
            //     {
            //         theBestMatchModel.PortId = matchResult.Id;
            //         theBestMatchModel.Value = matchResult.PortCode;
            //         theBestMatchModel.isPortCode = true;
            //
            //         theBestCoefficient = matchResult.PortCodeMatchCoefficient;
            //     }
            //     foreach (var portNameMatchResult in matchResult.PortNameMatchResults)
            //     {
            //         if (portNameMatchResult.PortNameMatchCoefficient > theBestCoefficient)
            //         {
            //             theBestMatchModel.PortId = matchResult.Id;
            //             theBestMatchModel.Value = portNameMatchResult.PortName;
            //             theBestMatchModel.isPortCode = false;
            //
            //             theBestCoefficient = portNameMatchResult.PortNameMatchCoefficient;
            //         }
            //     }
            // }

            return theBestMatchModels.OrderByDescending(m => m.isPortCode)
                .ToList();
        }
    }
}
