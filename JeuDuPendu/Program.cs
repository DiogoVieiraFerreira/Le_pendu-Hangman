using System;
using System.Collections.Generic;
using System.Linq;

namespace JeuDuPendu
{
    class Program
    {
        private const int MaxFails = 5;
        private static bool _gameOver;
        private static int _attempts;
        private static Hangman _hangman;
        /// <summary>
        /// Function start the Game and initialize attributes
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            _gameOver = false;
            _attempts = 0;
            Game();
        }
        /// <summary>
        /// Manage all Game
        /// </summary>
        private static void Game()
        {
            bool restart;
            do
            {
                var exit = false;
                restart = false;
                _hangman = new Hangman();
                while (!_hangman.End)
                {
                    Update();
                }
                HangmanConsoleVisual(_hangman.Fails);
            
                Console.WriteLine(_gameOver
                    ? $"Partie Terminée, vous avez perdu!\nLe mot était: {_hangman.SelectedWord}"
                    : "Partie Terminée, félicitations!");
                do
                {
                    Console.Clear();
                    HangmanConsoleVisual(_hangman.Fails);
                    Console.Write("\nVoulez-vous recommencer une nouvelle partie ? (o/n)");

                    var newGame = Console.ReadLine()?.ToLower();
                    
                    if ( newGame == "o") restart = true;
                    else if (newGame == "n") exit = true;
                    
                } while (!exit && !restart);
            } while (restart);
        }
        private static void Update()
        {
            HangmanConsoleVisual(_hangman.Fails);
            Console.Write("Quelle lettre recherchez-vous? ");
            var letter = Console.ReadLine();
            
            if(string.IsNullOrWhiteSpace(letter) || _hangman.UsedLetters.Contains(letter.ToLower())) return;
                
            _hangman.CheckLetter(letter);
            _gameOver = _hangman.Fails > MaxFails;
            _hangman.End = _hangman.End || _gameOver;
            _attempts++;
        }
        /// <summary>
        /// Display on the console the hangman and show the number of
        /// attempts with failures and used letters
        /// </summary>
        /// <param name="totalOfFails">the number of the currents failures</param>
        private static void HangmanConsoleVisual(int totalOfFails)
        {
            Console.Clear();
            new List<string>()
            {
                $"Nombre d'essais: {_attempts}",
                $"Nombre de fails: {totalOfFails}",
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
        /// <summary>
        /// Define the state of the game
        /// </summary>
        public bool End;
        /// <summary>
        /// Get all words available for the hangman
        /// </summary>
        public List<string> Words { get; private set; }
        /// <summary>
        /// Get the selected word on the current game
        /// </summary>
        public string SelectedWord { get; private set; }
        /// <summary>
        /// List of letters that user has tried
        /// </summary>
        public List<string> UsedLetters { get; private set; }
        /// <summary>
        /// Word to display at the user,
        /// Show only found letters
        /// </summary>
        public string UserWord { get; private set; }
        /// <summary>
        /// Number of fails to find letters
        /// </summary>
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
        public Hangman() : this(new List<string>()) { }
        /// <summary>
        /// Replace unfound letters by underscores
        /// </summary>
        private void UpdateUserWord()
        {
            //each not found letter is replaced by an underscore
            UserWord = string.Join("",
                string.Join("", SelectedWord.Select(x => UsedLetters.Contains(x.ToString().ToLower()) ? x : '_')));
        }
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
        /// <summary>
        /// Look on the word if the letter is available and increment fails
        /// when the letter isn't in the word
        /// </summary>
        /// <param name="letter">the letter to check in the word</param>
        /// <returns>return true when word contains the letter</returns>
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