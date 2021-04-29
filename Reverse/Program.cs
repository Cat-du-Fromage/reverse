using System;
using System.Diagnostics;

namespace Reverse
{
    class Program
    {
        static void Main(string[] args)
        {
            string textToReverse = "Il faut agir aussi vite que possible, mais aussi lentement que nécessaire.";
            string textReversed = "";
            
            Console.WriteLine("Text to reverse : " + textToReverse + "\r\n");

            textReversed = Reverse(textToReverse);

            Console.WriteLine("Text reversed : " + textReversed + "\r\n");
        }

        /// <summary>
        /// This function is designed to reverse a sentence, word by word. Words are not reversed, only the word order.
        /// </summary>
        /// <param name="texteToReverse"></param>
        /// <returns>Text reversed</returns>
        static string Reverse(string texteToReverse)
        {
            var mainsw = Stopwatch.StartNew();
            string result = "";

            //Get array of char
            var sw = Stopwatch.StartNew();
            //int LengthToReverse = TextLength(texteToReverse);
            char[] charTextArray = new char[texteToReverse.Length];

            for (int i = 0; i < charTextArray.Length; i++)
            {
                charTextArray[i] = texteToReverse[i];
            }
            sw.Stop();
            Console.WriteLine($"for loop : {sw.Elapsed} ms");
            // 00.0000946 ms with LengthToReverse
            // 00.0000035 ms with Length

            var sw2 = Stopwatch.StartNew();
            var charText = texteToReverse.ToCharArray();
            sw2.Stop();
            Console.WriteLine($".Net method : {sw2.Elapsed} ms");
            // 00.0000004 ms
       result = new string(ReverseWords(ReverseAllXOR(charTextArray)));
            //TODO - write code to reverse the texte
            //First Group -> try to maximize existing .net methods
            //Second Group -> full manually. Do not use existing methods.
            //Result expected
            //result = ".nécessaire que lentement aussi mais, possible que vite aussi agir faut Il";
            mainsw.Stop();
            Console.WriteLine($"speed overall : {mainsw.Elapsed} ms");
            // 00.0015934 ms
            return result;
        }
        /// <summary>
        /// Reverse all character in the string by applying XOR bitwise
        /// Exemple of the process with Array with a size of 11 (Array[10])
        /// 
        /// START char  = "i" (ASCII:73)     Binary(73): 0100 1001
        /// END char    = "." (ASCII:46)     Binary(46): 0010 1110
        /// BitWise XOR(^)
        /// 0^0 = 0
        /// 0^1 = 1
        /// 1^0 = 1
        /// 1^1 = 0
        /// 
        /// Process 1: Start = Start(73)  XOR End(46)    = 0110 0111 (start = 103(g))
        /// Process 2: End   = End(46)    XOR Start(103) = 0100 1001 (end = 73(i))
        /// Process 3: Start = Start(103) XOR End(73)    = 0010 1110 (start = 46(.))
        /// Before start = i, end  = .
        /// After start = ., end = i
        /// </summary>
        /// <param name="TextToReverse"></param>
        /// <returns></returns>
        static char[] ReverseAllXOR(char[] TextToReverse)
        {
            var swReverseAllXOR = Stopwatch.StartNew();
            char[] charTextArray = TextToReverse;
            int endArray = charTextArray.Length - 1;
            for (int j = 0; j < endArray; j++, endArray--)
            {
                charTextArray[j] ^= charTextArray[endArray];
                charTextArray[endArray] ^= charTextArray[j];
                charTextArray[j] ^= charTextArray[endArray];
            }
            swReverseAllXOR.Stop();
            Console.WriteLine($"ReverseAllXOR : {swReverseAllXOR.Elapsed} ms");
            return charTextArray;
        }

        static char[] ReverseWords(char[] TextToWordReverse)
        {
            char[] charWordArray = TextToWordReverse;
            char[] wordChar = new char[TextToWordReverse.Length];
            int startIndexWord = 0;
            for(int i = 0; i < charWordArray.Length; i++)
            {
                if (charWordArray[i] == '.' || charWordArray[i] == ',') { i += 1; startIndexWord += 1; }
                if (charWordArray[i] != ' ') //check first != ' ' / , / .
                {
                    wordChar[i- startIndexWord] = charWordArray[i];
                }
                else
                {
                    Array.Resize(ref wordChar, i - startIndexWord);
                    wordChar = ReverseAllXOR(wordChar);
                    for(int j = 0; j < wordChar.Length; j++)
                    {
                        charWordArray[j + startIndexWord] = wordChar[j];
                    }
                    Array.Clear(wordChar, 0, wordChar.Length);
                    Array.Resize(ref wordChar, TextToWordReverse.Length);
                    startIndexWord = i + 1;
                }
            }
            return charWordArray;
        }

        static int TextLength(string textToCount)
        {
            int lengthText = 0;
            foreach(char c in textToCount)
            {
                lengthText++;
            }
            return lengthText;
        }
    }
}
