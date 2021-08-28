using System;
using System.Collections.Generic;
using System.Linq;

namespace JeuDuPendu
{
    class Program
    {
        
        private static int _maxFails = 5;
        private static bool _gameOver = false;
        private static int _attempts = 0;
        private static Hangman _hangman;
        
        private static void Main(string[] args)
        {
            _hangman = new();
            while (!_hangman.End)
            {
                HangmanConsoleVisual(_hangman.Fails);
                
                Console.Write("Quelle lettre recherchez-vous? ");
                var letter = Console.ReadLine();
                var a = string.IsNullOrWhiteSpace(letter);
                if(string.IsNullOrWhiteSpace(letter) || _hangman.UsedLetters.Contains(letter.ToLower())) continue;
                
                _hangman.CheckLetter(letter);
                _gameOver = _hangman.Fails > _maxFails;
                _hangman.End = _hangman.End || _gameOver;
                _attempts++;
            }
            
            HangmanConsoleVisual(_hangman.Fails);
            
            Console.WriteLine(_gameOver
                ? $"Partie Terminée, vous avez perdu!\nLe mot était: {_hangman.SelectedWord}"
                : "Partie Terminée, félicitations!");
        }

        private static void HangmanConsoleVisual(int totalOfFails)
        {
            Console.Clear();
            new List<string>()
            {
                $"Nombre d'essais: {_attempts}",
                $"Nombre de fails: {_hangman.Fails}",
                $"Lettres utilisées: {string.Join(", ", _hangman.UsedLetters)}\n",
                $"Le Mot: {_hangman.UserWord}",
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

    public class Hangman
    {
        public bool End;
        public List<string> Words { get; set; }
        public string SelectedWord { get; private set; }
        public List<string> UsedLetters { get; private set; }
        public string UserWord { get; private set; }
        public int Fails { get; private set; }
        
        /// <summary>
        /// Create new pendu Game and select a new word to start the game.
        /// </summary>
        /// <param name="words">List of strings to use during the Game</param>
        public Hangman(List<string> words)
        {
            UsedLetters = new List<string>();
            End = false;
            Words = words.Count > 0 ? words : new List<string>() {"Hello", "World"};
            SelectWord();
            UpdateUserWord();
        }
        private void UpdateUserWord()
        {
            //each not found letter is replaced by an underscore
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
            
            if (!result) Fails++;
            
            UpdateUserWord();
            
            if (SelectedWord == UserWord) End = true;
            
            return result;
        }
    }
}