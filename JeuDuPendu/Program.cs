using System;
using System.Collections.Generic;
using System.Linq;

namespace JeuDuPendu
{
    class Program
    {
        private static void Main(string[] args)
        {
            var attempts = 0;
            Pendu pendu = new();
            while (!pendu.End)
            {
                Console.Clear();
                Console.WriteLine($"Nombre d'essais: {attempts}");
                Console.WriteLine($"Lettres utilisées: {string.Join(", ", pendu.UsedLetters)}\n");
                Console.WriteLine($"Le Mot: {pendu.UserWord}");
                PenduConsoleVisual(pendu.Fails);
                Console.Write("Quelle lettre recherchez-vous? ");
                string letter = Console.ReadLine();
                pendu.CheckLetter(letter);
                pendu.End = pendu.End || pendu.Fails > 4;
                attempts++;
            }
            Console.Write("Partie Terminée, félicitations!");
        }

        private static void PenduConsoleVisual(int totalOfFails)
        {
            new List<string>()
            {
                "  .---------.",
                "  |/        |",
                $"  |         {(totalOfFails > 0 ? "O" : "")}",
                $"  |        {(totalOfFails > 2 ? "/" : " ")}{(totalOfFails > 1 ? "|" : "")}{(totalOfFails > 3 ? "\\" : "")}",
                $"  |        {(totalOfFails > 4 ? "/ " : "")}{(totalOfFails > 5 ? "\\" : "")}",
                "  |         ",
                $"  |   {(totalOfFails > 5 ? "Vous Avez perdu !" : "")}",
                "_/|\\_____________________",
            }.ForEach( Console.WriteLine);
        }
    }

    public class Pendu
    {
        public bool End;
        public int Fails { get; private set; }
        public string UserWord { get; private set; }
        public List<string> Words { get; set; }
        public List<string> FindedLetters { get; private set; }
        public List<string> UsedLetters { get; private set; }
        public string SelectedWord { get; private set; }
        
        /// <summary>
        /// Create new pendu Game and select a new word to start the game.
        /// </summary>
        /// <param name="words">List of strings to use during the Game</param>
        public Pendu(List<string> words)
        {
            UsedLetters = new List<string>();
            FindedLetters = new List<string>();
            End = false;
            Words = words.Count > 0 ? words : new List<string>() {"Hello", "World"};
            SelectWord();
            
            UserWord = string.Join("",
                string.Join("", SelectedWord.Select(x => UsedLetters.Contains(x.ToString().ToLower()) ? x : '_')));
        }
        public Pendu() : this(new List<string>()) { }
        /// <summary>
        /// Select a new Word for the game and update "SelectedWord" attribute.
        /// </summary>
        /// <returns>The new selected word</returns>
        public string SelectWord()
        {
            Random random = new();
            SelectedWord = Words[random.Next(0, Words.Count)];
            return SelectedWord;
        }
        
        public bool CheckLetter(string letter)
        {
            var lowerWord = SelectedWord.ToLower();
            var lowerLetter = letter.ToLower();
            var result = lowerWord.Contains(lowerLetter);
            
            UsedLetters.Add(lowerLetter);
            
            if (result)
                FindedLetters.Add(letter);
            else
                Fails++;
            //each not found letter is replaced by an underscore
            UserWord = string.Join("",
                string.Join("", SelectedWord.Select(x => UsedLetters.Contains(x.ToString().ToLower()) ? x : '_')));
            
            if (SelectedWord == UserWord) End = true;
            
            return result;
        }
    }
}