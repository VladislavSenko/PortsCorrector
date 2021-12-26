using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using PortSearchMatchModule.Enums;

namespace PortSearchMatchModule
{
    public class MatcherService
    {

        public static async Task<List<MatchResult>> GetMatchCoefficient(string inputWord)
        {
            var matcher = new Matcher(MethodsEnum.SorensenDice, inputWord);

            var ports = await MongoService.GetAllCollectionAsync();

            var matchResults = new List<MatchResult>();

            foreach (var port in ports)
            {
                var matchResult = new MatchResult();
                var portNameMatchResults = new List<PortNameMatchResult>();

                var portFields = port.Elements.ToList();

                matchResult.PortCode = portFields[1].Value.AsString;
                matchResult.PortCodeMatchCoefficient = matcher.GetCoefficient(portFields[1].Value.AsString);
                
                var portNameType = portFields[2].Value.BsonType;

                if (portNameType == BsonType.Array)
                {
                    portNameMatchResults = portFields[2].Value.AsBsonArray.Select(p => new PortNameMatchResult
                    {
                        PortName = p.AsString,
                        PortNameMatchCoefficient =  matcher.GetCoefficient(p.AsString)
                    }).ToList();
                }
                else if (portNameType == BsonType.String)
                {
                    portNameMatchResults.Add(new PortNameMatchResult
                    {
                        PortName = portFields[2].Value.AsString,
                        PortNameMatchCoefficient = matcher.GetCoefficient(portFields[2].Value.AsString)
                    });
                }

                matchResult.PortNameMatchResults = portNameMatchResults;
                matchResults.Add(matchResult);
            }

            return matchResults;
        }

        public static string GetTheBestMatch(List<MatchResult> matchResults)
        {
            string theBestMatchResult = "";
            decimal theBestCoefficient = 0;

            foreach (var matchResult in matchResults)
            {
                if (matchResult.PortCodeMatchCoefficient > theBestCoefficient)
                {
                    theBestMatchResult = matchResult.PortCode;
                    theBestCoefficient = matchResult.PortCodeMatchCoefficient;
                }
                foreach (var portNameMatchResult in matchResult.PortNameMatchResults)
                {
                    if (portNameMatchResult.PortNameMatchCoefficient > theBestCoefficient)
                    {
                        theBestMatchResult = portNameMatchResult.PortName;
                        theBestCoefficient = portNameMatchResult.PortNameMatchCoefficient;
                    }
                }
            }

            return theBestMatchResult;
        }
    }
}
