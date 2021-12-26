using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiMongo.Models
{
    public class MatchResultModel
    {
        public string Id { get; set; }
        public string PortCode { get; set; }
        public decimal PortCodeMatchCoefficient { get; set; }
        public List<PortNameMatchResult> PortNameMatchResults { get; set; }

        public override string ToString()
        {
            var res = string.Concat($"portCode: {PortCode} | portCodeCoeff: {PortCodeMatchCoefficient}.",
                         Environment.NewLine,
                         $" portNames: {string.Join(", ", PortNameMatchResults.Select(p => p.PortName))} | portNamesCoeffs: {string.Join(", ", PortNameMatchResults.Select(p => p.PortNameMatchCoefficient))}");
            return res;
        }   
    }

    public class PortNameMatchResult
    {
        public int PortNameIndex { get; set; }
        public string PortName { get; set; }
        public decimal PortNameMatchCoefficient { get; set; }
    }
}
