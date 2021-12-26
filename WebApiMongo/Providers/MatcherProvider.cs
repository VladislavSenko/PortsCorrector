using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using WebApiMongo.Entities;
using WebApiMongo.Enums;
using WebApiMongo.Helpers;
using WebApiMongo.Models;
using WebApiMongo.Settings;
using WebApiMongo.ViewModel;

namespace WebApiMongo.Services
{
    public class MatcherProvider
    {
        public MatcherProvider()
        {
        }

        public List<MatchResultModel> GetMatchCoefficient(InputPortViewModel inputPort, List<Port> ports)
        {
            var codeMatcher = new MatcherHelper(MethodsEnum.SorensenDice, inputPort.PortCode);
            var nameMatcher = new MatcherHelper(MethodsEnum.SorensenDice, inputPort.PortName);

            var matchResults = new List<MatchResultModel>();

            foreach (var port in ports)
            {
                var matchResult = new MatchResultModel();

                matchResult.Id = port.Id;
                matchResult.PortCode = port.PortCode;
                matchResult.PortCodeMatchCoefficient = codeMatcher.GetCoefficient(port.PortCode);

                matchResult.PortNameMatchResults = port.PortName.Select((p,i) => new PortNameMatchResult
                {
                    PortNameIndex = i,
                    PortName = p,
                    PortNameMatchCoefficient =  nameMatcher.GetCoefficient(p)
                }).OrderByDescending(p => p.PortNameMatchCoefficient)
                    .ToList();
               
                matchResults.Add(matchResult);
            }

            return matchResults;
        }

        public List<TheBestMatchModel> GetTheBestMatch(List<MatchResultModel> matchResults, SearchKindEnum searchKind)
        {
            var howMuchMatchesReturn = AppSettings.ReturnMatches;
            var theBestMatchModels = new List<TheBestMatchModel>();

            foreach (var matchResult in matchResults)
            {
                foreach (var portNameMatchResult in matchResult.PortNameMatchResults)
                {
                    var theBestMatch = new TheBestMatchModel()
                    {
                        PortName = portNameMatchResult.PortName,
                        PortCode = matchResult.PortCode,
                        PortCodeId = matchResult.Id,
                        PortNameMatchWeight = portNameMatchResult.PortNameMatchCoefficient,
                        PortCodeMatchWeight = matchResult.PortCodeMatchCoefficient,
                        SearchKind = searchKind
                    };
                    theBestMatchModels.Add(theBestMatch);
                }
            }
            
            var orderedMatchResult = new List<TheBestMatchModel>();

            switch (searchKind)
            {
                case SearchKindEnum.PortCodeAndName:
                    orderedMatchResult = theBestMatchModels
                        .OrderByDescending(m => m.PortCodeMatchWeight + m.PortNameMatchWeight)
                        .Take(howMuchMatchesReturn)
                        .ToList();
                    break;
                case SearchKindEnum.PortCode:
                    orderedMatchResult = theBestMatchModels
                        .OrderByDescending(m => m.PortCodeMatchWeight)
                        .Take(howMuchMatchesReturn)
                        .ToList();
                    break;
                case SearchKindEnum.PortName:
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
