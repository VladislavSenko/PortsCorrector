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
                    Id = p.Id,
                    PortName = p.Name,
                    PortNameMatchCoefficient =  nameMatcher.GetCoefficient(p.Name)
                }).OrderByDescending(p => p.PortNameMatchCoefficient)
                    .ToList();
               
                matchResults.Add(matchResult);
            }

            return matchResults;
        }

        public List<TheBestMatchModel> GetTheBestMatch(List<MatchResultModel> matchResults, SearchMatchKindEnum searchMatchKind)
        {
            var howMuchMatchesReturn = AppSettings.ReturnMatches;
            var theBestMatchModels = new List<TheBestMatchModel>();

            foreach (var matchResult in matchResults)
            {
                foreach (var portNameMatchResult in matchResult.PortNameMatchResults)
                {
                    var theBestMatch = new TheBestMatchModel()
                    {
                        PortNameId = portNameMatchResult.Id,
                        PortCodeId = matchResult.PortCodeId,
                        PortNameMatchWeight = portNameMatchResult.PortNameMatchCoefficient,
                        PortCodeMatchWeight = matchResult.PortCodeMatchCoefficient,
                        SearchMatchKind = searchMatchKind
                    };
                    theBestMatchModels.Add(theBestMatch);
                }
            }

            var orderedMatchResult = new List<TheBestMatchModel>();

            switch (searchMatchKind)
            {
                case SearchMatchKindEnum.PortCodeAndName:
                    orderedMatchResult = theBestMatchModels
                        .OrderByDescending(m => m.PortCodeMatchWeight + m.PortNameMatchWeight)
                        .Take(howMuchMatchesReturn)
                        .ToList();
                    break;
                case SearchMatchKindEnum.PortCode:
                    orderedMatchResult = theBestMatchModels
                        .OrderByDescending(m => m.PortCodeMatchWeight)
                        .Take(howMuchMatchesReturn)
                        .ToList();
                    break;
                case SearchMatchKindEnum.PortName:
                    orderedMatchResult = theBestMatchModels
                        .OrderByDescending(m => m.PortNameMatchWeight)
                        .Take(howMuchMatchesReturn)
                        .ToList();
                    break;
            }
            
            return orderedMatchResult;
        }
    }
}
