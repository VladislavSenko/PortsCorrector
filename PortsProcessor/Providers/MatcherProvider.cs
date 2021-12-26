using System.Collections.Generic;
using System.Linq;
using PortsProcessor.Data.Entities;
using PortsProcessor.Enums;
using PortsProcessor.Helpers;
using PortsProcessor.Models;
using PortsProcessor.Settings;

namespace PortsProcessor.Providers
{
    public class MatcherProvider
    {
        public MatcherProvider()
        {
        }

        public List<MatchResultModel> GetMatchCoefficient(InputPort inputPort, List<PortCode> ports)
        {
            var codeMatcher = new MatcherHelper(MethodsEnum.SorensenDice, inputPort.PortCode);
            var nameMatcher = new MatcherHelper(MethodsEnum.SorensenDice, inputPort.PortName);

            var matchResults = new List<MatchResultModel>();

            foreach (var port in ports)
            {
                var matchResult = new MatchResultModel();

                matchResult.PortCodeId = port.Id;
                matchResult.PortCode = port.Code;
                matchResult.PortCodeMatchCoefficient = codeMatcher.GetCoefficient(port.Code);

                matchResult.PortNameMatchResults = port.PortNames.Select(p => new PortNameMatchResult
                {
                    PortName = p.Name,
                    PortNameMatchCoefficient =  nameMatcher.GetCoefficient(p.Name)
                }).OrderByDescending(p => p.PortNameMatchCoefficient)
                    .ToList();
               
                matchResults.Add(matchResult);
            }

            return matchResults;
        }

        public List<TheBestMatchModel> GetTheBestMatch(List<MatchResultModel> matchResults)
        {
            var howMuchMatchesReturn = AppSettings.ReturnMatches;

            var maxMatch = matchResults.Select(m => new
            {
                PortCodeId = m.PortCodeId,
                Score = m.PortCodeMatchCoefficient + m.PortNameMatchResults.Max(p => p.PortNameMatchCoefficient)
            }).OrderByDescending(s => s.Score).Take(howMuchMatchesReturn).ToList();

            var matchResultOrderByCodeCoeff = matchResults.OrderByDescending(m => m.PortCodeMatchCoefficient)
                .Select(r => new TheBestMatchModel()
                {
                    PortCodeId = r.PortCodeId,
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
                    PortCodeId = r.PortCodeId,
                    Value = r.PortNameMatchResults.OrderByDescending(s => s.PortNameMatchCoefficient).First().PortName,
                    isPortCode = false,
                    MatchScore = r.PortNameMatchResults.Max(p => p.PortNameMatchCoefficient)
                })
                .Take(howMuchMatchesReturn)
                .ToList();

            var theBestMatchModels = new List<TheBestMatchModel>();
            theBestMatchModels.AddRange(matchResultOrderByCodeCoeff);
            theBestMatchModels.AddRange(matchResultOrderByNameCoeff);


            return theBestMatchModels.OrderByDescending(m => m.isPortCode)
                .ToList();
        }
    }
}
