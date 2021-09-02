using System;
using System.Collections.Generic;

namespace CodeBreaker_MonoGame
{
    public enum DigitState { Good, Bad, Diffrent}
    public class GameLogic
    {
        public int[] goodCode { get; set; }
        public int[] currentCode { get; set; }
        public List<SingleDigit> guessCodeHistory { get; set; }
        public int rowCount { get; set; }
        public GameLogic(int codeLength)
        {
            RandomCode(codeLength);
            guessCodeHistory = new List<SingleDigit>();
        }
        public void RandomCode(int codeLength)
        {
            goodCode = new int[codeLength];
            List<int> possibleDigits = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                possibleDigits.Add(i);
            }
            Random random = new Random();
            int indexDigit;
            for (int i = 0; i < goodCode.Length; i++)
            {
                indexDigit = random.Next(0, possibleDigits.Count);
                goodCode[i] = possibleDigits[indexDigit];
                possibleDigits.RemoveAt(indexDigit);
            }
            currentCode = new int[codeLength];
            rowCount = 0;
        }
        public string CurrentCodeString()
        {
            string currentCodeString = "";
            for (int i = 0; i < goodCode.Length; i++)
            {
                currentCodeString += "[" + goodCode[i].ToString() + "] ";
            }
            return currentCodeString;
        }
        public bool TryCode()
        {
            bool correctCode = true;
            Random random = new Random();
            for (int i = 0; i < currentCode.Length; i++)
            {
                SingleDigit singleDigit = new SingleDigit();
                singleDigit.row = rowCount;
                singleDigit.column = i;
                singleDigit.value = currentCode[i];
                if (currentCode[i] == goodCode[i])
                {
                    singleDigit.digitState = DigitState.Good;
                }
                else if (Array.Exists(goodCode, element => element == currentCode[i]))
                {
                    singleDigit.digitState = DigitState.Diffrent;
                    correctCode = false;
                }
                else
                {
                    singleDigit.digitState = DigitState.Bad;
                    correctCode = false;
                }
                guessCodeHistory.Add(singleDigit);
            }
            rowCount++;
            return correctCode;
        }
    }
}
