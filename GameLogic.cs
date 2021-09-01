using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBreaker_MonoGame
{
    public class GameLogic
    {
        public int[] goodCode { get; set; }
        public int[] currentCode { get; set; }
        public GameLogic(int codeLength)
        {
            RandomCode(codeLength);
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
    }
}
