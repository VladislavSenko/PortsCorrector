using System;
using PortSearchMatchModule.Comparers;
using PortSearchMatchModule.Enums;
using System.Linq;

namespace PortSearchMatchModule
{
    public class Matcher
    {
        private string[] _words;
        private string _inputWord;
        private MethodsEnum _method;
        private SorensenDiceCoefficient _sdc;
        public Matcher(MethodsEnum method, params string[] words)
        {
            _method = method;
            _words = words;
        }

        public Matcher(MethodsEnum method, string word)
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
