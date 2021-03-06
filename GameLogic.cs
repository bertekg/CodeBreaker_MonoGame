using System;
using System.Collections.Generic;

namespace CodeBreaker_MonoGame
{
    public enum DigitState { Good, Bad, Diffrent}
    public class GameLogic
    {
        public int[] goodCode { get; set; }
        public int[] currentCode { get; set; }
        public List<List<SingleDigit>> guessCodeHistory;
        int maxHistoryLength;
        public int numberOfAttempts { get; set; }
        public bool guessedCode { get; set; }
        public GameLogic(int codeLength, int maxNumberOfHints)
        {
            InitRandomCode(codeLength);
            guessCodeHistory = new List<List<SingleDigit>>();
            maxHistoryLength = maxNumberOfHints;
            guessedCode = false;
        }
        public void InitRandomCode(int codeLength)
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
            numberOfAttempts = 0;
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
        public string CorrectCodeString()
        {
            string correctCodeString = "";
            for (int i = 0; i < goodCode.Length; i++)
            {
                correctCodeString += goodCode[i].ToString();
            }
            return correctCodeString;
        }
        public bool TryCode()
        {
            bool correctCode = true;
            Random random = new Random();
            List<SingleDigit> rowDigits = new List<SingleDigit>();
            for (int i = 0; i < currentCode.Length; i++)
            {
                SingleDigit singleDigit = new SingleDigit();
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
                rowDigits.Add(singleDigit);
            }
            if (guessCodeHistory.Count >= maxHistoryLength)
            {
                guessCodeHistory.RemoveAt(0);
            }
            guessCodeHistory.Add(rowDigits);
            numberOfAttempts++;
            guessedCode = correctCode;
            return correctCode;
        }
    }
}
