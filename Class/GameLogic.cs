using System;
using System.Collections.Generic;

namespace CodeBreaker_MonoGame.Class
{
    public enum DigitState { Good, Bad, Different }
    public class GameLogic
    {
        public int[] GoodCode { get; set; }
        public int[] CurrentCode { get; set; }
        public List<List<SingleDigit>> GuessCodeHistory;
        private readonly int _maxHistoryLength;
        public int NumberOfAttempts { get; set; }
        public bool CodeGuessed { get; set; }
        public GameLogic(int codeLength, int maxNumberOfHints, int maxDigit)
        {
            InitRandomCode(codeLength, maxDigit);
            GuessCodeHistory = new List<List<SingleDigit>>();
            _maxHistoryLength = maxNumberOfHints;
            CodeGuessed = false;
        }
        public void InitRandomCode(int codeLength, int maxDigit)
        {
            GoodCode = new int[codeLength];
            List<int> possibleDigits = new List<int>();
            for (int i = 0; i <= maxDigit; i++)
            {
                possibleDigits.Add(i);
            }
            Random random = new Random();
            int indexDigit;
            for (int i = 0; i < GoodCode.Length; i++)
            {
                indexDigit = random.Next(0, possibleDigits.Count);
                GoodCode[i] = possibleDigits[indexDigit];
                possibleDigits.RemoveAt(indexDigit);
            }
            CurrentCode = new int[codeLength];
            NumberOfAttempts = 0;
        }
        public string CurrentCodeString()
        {
            string currentCodeString = "";
            for (int i = 0; i < GoodCode.Length; i++)
            {
                currentCodeString += "[" + GoodCode[i].ToString("X") + "] ";
            }
            return currentCodeString;
        }
        public string GoodCodeString()
        {
            string goodCodeString = "";
            for (int i = 0; i < GoodCode.Length; i++)
            {
                goodCodeString += GoodCode[i].ToString("X");
            }
            return goodCodeString;
        }
        public bool TryCode()
        {
            bool correctCode = true;
            Random random = new Random();
            List<SingleDigit> rowDigits = new List<SingleDigit>();
            for (int i = 0; i < CurrentCode.Length; i++)
            {
                SingleDigit singleDigit = new SingleDigit();
                singleDigit.Value = CurrentCode[i];
                if (CurrentCode[i] == GoodCode[i])
                {
                    singleDigit.DigitState = DigitState.Good;
                }
                else if (Array.Exists(GoodCode, element => element == CurrentCode[i]))
                {
                    singleDigit.DigitState = DigitState.Different;
                    correctCode = false;
                }
                else
                {
                    singleDigit.DigitState = DigitState.Bad;
                    correctCode = false;
                }
                rowDigits.Add(singleDigit);
            }
            if (GuessCodeHistory.Count >= _maxHistoryLength)
            {
                GuessCodeHistory.RemoveAt(GuessCodeHistory.Count - 1);
            }
            GuessCodeHistory.Insert(0,rowDigits);
            NumberOfAttempts++;
            CodeGuessed = correctCode;
            return correctCode;
        }
    }
}
