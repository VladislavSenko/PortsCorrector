using System;
using System.Linq;
using PortsProcessor.Comparers;
using PortsProcessor.Enums;

namespace PortsProcessor.Helpers
{
    public class MatcherHelper
    {
        private string[] _words;
        private string _inputWord;
        private MethodsEnum _method;
        private SorensenDiceCoefficient _sdc;

        public MatcherHelper(MethodsEnum method, string word)
        {
            _method = method;
            _inputWord = word;
        }


        public decimal GetCoefficient(string word)
        {
            switch (_method)
            {
                case MethodsEnum.SorensenDice:
                    var sd = new SorensenDiceCoefficient();
                    return sd.CompareTwoStrings(_inputWord, word);
                default:
                    throw new ArgumentException();
            }
        }

        public string GetBestMatch()
        {
            switch (_method)
            {
                case MethodsEnum.SorensenDice:
                    var sd = new SorensenDiceCoefficient();
                    return sd.FindBestMatch(_words[0], _words.Skip(1).ToArray());
                default:
                    throw new ArgumentException();
            }
        }
    }
}
