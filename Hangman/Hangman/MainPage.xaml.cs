namespace Hangman
{
    using System.Diagnostics;

    public partial class MainPage : ContentPage
    {
        #region UI Properties

        public string Highlight
        {
            get => _highlight;
            set
            {
                _highlight = value;
                OnPropertyChanged();
            }
        }

        public List<char> Letters
        {
            get => _letters;
            set
            {
                _letters = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus
        {
            get => _gameStatus;
            set
            {
                _gameStatus = value;
                OnPropertyChanged();
            }
        }

        public string CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Fields

        List<string> _words = new List<string>()
        {
            "python",
            "javascript",
            "maui",
            "csharp",
            "mongodb",
            "sql",
            "xaml",
            "word",
            "excel",
            "powerpoint",
            "abacaxi",
            "windows",
            "ubatuba"
        };

        string _answer = "";
        int _errors = 0;
        int _maxErrors = 6;
        List<char> _hunch = new List<char>();

        private string _highlight;
        private List<char> _letters = new List<char>();
        private string _message;      
        private string _gameStatus;
        private string _currentImage = "img0.jpg";

        #endregion

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
            PickWord();
            CalculateWord(_answer, _hunch);
        }

        #region Game

        private void PickWord()
        {
            _answer =_words[new Random().Next(0, _words.Count)];
            Debug.WriteLine(_answer);
        }

        private void CalculateWord(string answer, List<char> hunch)
        {
            var temp = answer
                .Select(x => (hunch.IndexOf(x) >= 0 ? x : '_'))
                .ToArray();

            Highlight = string.Join(' ', temp);
        }

        private void HandleHunch(char letter)
        {
            if (_hunch.IndexOf(letter) == -1)
            {
                _hunch.Add(letter);
            }
            if (_answer.IndexOf(letter) >= 0)
            {
                CalculateWord(_answer, _hunch);
                CheckIfWon();
            }
            else if (_answer.IndexOf(letter) == -1)
            {
                _errors++;
                UpdateStatus();
                CheckIfLost();
                CurrentImage = $"img{_errors}.jpg";
            }
        }

        private void CheckIfLost()
        {
            if (_errors == _maxErrors)
            {
                Message = "You lost!";
                DisableLetters();
            }
        }

        private void DisableLetters()
        {
            foreach (var children in LettersContainer.Children)
            {
                var btn = children as Button;

                if (btn != null)
                    btn.IsEnabled = false;
            }
        }
        private void EnableLetters()
        {
            foreach (var children in LettersContainer.Children)
            {
                var btn = children as Button;

                if (btn != null)
                    btn.IsEnabled = true;
            }
        }

        private void CheckIfWon()
        {
            if (Highlight.Replace(" ", "") == _answer)
            {
                Message = "You won!";
                DisableLetters();
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Errors: {_errors} of {_maxErrors}";
        }

        #endregion

        private void Reset_Clicked(object sender, EventArgs e)
        {
            _errors = 0;
            _hunch = new List<char>();
            CurrentImage = "img0.jpg";
            PickWord();
            CalculateWord(_answer, _hunch);
            Message = "";
            UpdateStatus();
            EnableLetters();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;

            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleHunch(letter[0]);
            }
        }
    }
}
