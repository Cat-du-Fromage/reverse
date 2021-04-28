using System;

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
            string result = "";

            //TODO - write code to reverse the texte
            //First Group -> try to maximize existing .net methods
            //Second Group -> full manually. Do not use existing methods.
            //Result expected
            result = ".nécessaire que lentement aussi mais, possible que vite aussi agir faut Il";

            return result;
        }
    }
}
