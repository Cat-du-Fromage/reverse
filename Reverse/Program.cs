using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Reverse;
namespace Reverse
{
    class Program
    {
        /// <summary>
        /// 
        ///                   TextLeng           00.0005385 ms with LengthToReverse
        ///                   TextLeng  in       00.0000042 ms with LengthToReverse
        ///                   Length             00.0000035 ms with Length
        ///                   .ToCharArray()     00.0000004 ms
        ///                   strlen:            00.0000097 ms intern 
        ///                   strLen:            00.0004976 ms extern(function call)
        ///                   Enumerable.Reverse:00.0076088 ms
        ///                   Array.Reverse     :00.0000129 ms
        ///                   ReverseXOR        :00.0001081 ms
        ///                   ReverseXOR in     :00.0000002 ms
        /// </summary>
        /// <param name="args"></param>
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

            char[] charTextArray = new char[texteToReverse.Length];

            for (int i = 0; i < charTextArray.Length; i++)
            {
                charTextArray[i] = texteToReverse[i];                                                   //                   00.0000946 ms with LengthToReverse
            }                                                                                           //Length             00.0000035 ms with Length
            var charText = texteToReverse.ToCharArray();                                                //.ToCharArray()     00.0000004 ms


            var testsw = Stopwatch.StartNew();
            //var test = Enumerable.Reverse(charText).ToArray();
            //char[] test = ReverseAllXOR(charText);
            var test = ReverseUtils.TextLength(texteToReverse);
            //Array.Reverse(charText, 0, charText.Length);
            testsw.Stop();
            Console.WriteLine($"TextLength : {testsw.Elapsed} ms");

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
            var sw6 = Stopwatch.StartNew();
            char[] charTextArray = TextToReverse;
            int endArray = charTextArray.Length - 1;
            for (int j = 0; j < endArray; j++, endArray--)
            {
                charTextArray[j] ^= charTextArray[endArray];
                charTextArray[endArray] ^= charTextArray[j];
                charTextArray[j] ^= charTextArray[endArray];
            }
            sw6.Stop();
            Console.WriteLine($"ReverseAllXOR intern: {sw6.Elapsed} ms");
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

        /*
        struct WordNode
        {
            public WordNode(char[] node)
            {
                wordNode = node;
                prev = null;
                next = null;
            }

            public WordNode prev;
            public WordNode next;
            public char[] wordNode;
        }

        static LinkedListNode<WordNode> FindWords(char[] text)
        {
            WordNode wNode;
            int count = 0;
            byte startWord = 0;
            for (byte i = 0; i < (byte)text.Length; i++)
            {
                if((text[i] ^ 32) == 0 || (text[i] ^ 44) == 0 || (text[i] ^ 46) == 0) // missing . and ,
                {
                    char[] wordSplit;
                    wordSplit = new char[i];

                    //Add Words first
                    for (byte j = 0; j < (i - 1); j++)
                    {
                        wordSplit[j] = text[startWord + j];
                    }

                    if ((text[i+1] ^ 32) == 0 || (text[i+1] ^ 44) == 0 || (text[i + 1] ^ 46) == 0)
                    {
                        wNode.wordNode = wordSplit;
                        words.AddLast(wNode);

                        //Add " " + "," or "."
                        WordNode wNodeS = new WordNode();
                        char[] space = new char[2] { text[i], text[i+1]};
                        wNodeS.wordNode = space;
                        words.AddLast(wNodeS);
                        i++; //go after . or ,
                    }
                    else
                    {
                        wordSplit = new char[i - startWord];
                        wNode.wordNode = wordSplit;
                        words.AddLast(wNode);
                    }
                    startWord = (byte)(i+1);
                }
                LinkedListNode<WordNode> words = new LinkedListNode<WordNode>(wNode);
            }
            foreach (WordNode n in words)
            {
                foreach (char c in n.wordNode)
                {
                    Console.WriteLine($"{c}");
                }
            }
            return words;
        }
        */
    }
}
