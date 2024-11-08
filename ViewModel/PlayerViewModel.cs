using Labb3_HES.Command;
using Labb3_HES.Model;
using System.Windows.Media;
using System.Windows.Threading;

namespace Labb3_HES.ViewModel
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _isPlayerMode;
        public bool IsPlayerMode
        {
            get => _isPlayerMode;
            set
            {
                _isPlayerMode = value;
                RaisePropertyChanged();
            }
        }

        private QuestionPackViewModel _questionPackWithRandomizedOrder;
        public QuestionPackViewModel QuestionPackWithRandomizedOrder
        {
            get => _questionPackWithRandomizedOrder;
            set
            {
                _questionPackWithRandomizedOrder = value;
                RaisePropertyChanged();
            }
        }

        private Question _activeQuestion;
        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }

        private string[] _activeAnswers;
        public string[] ActiveAnswers
        {
            get => _activeAnswers;
            set
            {
                _activeAnswers = value;
                RaisePropertyChanged();
            }
        }

        private int _currentQuestionIndex;
        public int CurrentQuestionIndex

        {
            get => _currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                RaisePropertyChanged();
            }
        }

        private int _currentQuestionNumber;
        public int CurrentQuestionNumber

        {
            get => _currentQuestionNumber;
            set
            {
                _currentQuestionNumber = value;
                RaisePropertyChanged();
            }
        }

        private int _numberOfQuestionsInPack;
        public int NumberOfQuestionsInPack

        {
            get => _numberOfQuestionsInPack;
            set
            {
                _numberOfQuestionsInPack = value;
                RaisePropertyChanged();
            }
        }

        private string _givenAnswer;
        public string GivenAnswer

        {
            get => _givenAnswer;
            set
            {
                _givenAnswer = value;
                RaisePropertyChanged();
            }
        }

        public int CorrectAnswersGiven { get; set; }

        private SolidColorBrush _colorForAnswerOne;

        public SolidColorBrush ColorForAnswerOne
        {
            get => _colorForAnswerOne;
            set
            {
                _colorForAnswerOne = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _colorForAnswerTwo;
        public SolidColorBrush ColorForAnswerTwo
        {
            get => _colorForAnswerTwo;
            set
            {
                _colorForAnswerTwo = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _colorForAnswerThree;

        public SolidColorBrush ColorForAnswerThree
        {
            get => _colorForAnswerThree;
            set
            {
                _colorForAnswerThree = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _colorForAnswerFour;

        public SolidColorBrush ColorForAnswerFour
        {
            get => _colorForAnswerFour;
            set
            {
                _colorForAnswerFour = value;
                RaisePropertyChanged();
            }
        }


        private SolidColorBrush aliceBlueBrush = new SolidColorBrush(new System.Windows.Media.Color() { R = 240, G = 248, B = 255, A = 255 });
        private SolidColorBrush greenBrush = new SolidColorBrush(new System.Windows.Media.Color() { G = 255, A = 255 });
        private SolidColorBrush redBrush = new SolidColorBrush(new System.Windows.Media.Color() { R = 255, A = 255 });

        private DispatcherTimer timeLimitTimer;
        private DispatcherTimer cooldownTimer;

        private int _timeLimit;

        public int TimeLimit
        {
            get => _timeLimit;
            private set
            {
                _timeLimit = value;
                RaisePropertyChanged();
            }
        }

        private int coolDownTime;

        private bool isWaitingForAnswer;

        public DelegateCommand PlayQuizCommand { get; }
        public DelegateCommand GiveAnswerCommand { get; }

        public event EventHandler IsPlayerModeMessage;
        public event EventHandler ActivePackIsNotPlayableMessage;

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            PlayQuizCommand = new DelegateCommand(PlayQuiz, CanPlayQuiz);
            GiveAnswerCommand = new DelegateCommand(GetAnswer);

            ResetColorsForAnswers();
        }

        private void ResetColorsForAnswers()
        {
            ColorForAnswerOne = aliceBlueBrush;
            ColorForAnswerTwo = aliceBlueBrush;
            ColorForAnswerThree = aliceBlueBrush;
            ColorForAnswerFour = aliceBlueBrush;
        }

        private void PlayQuiz(object obj)
        {
            if (mainWindowViewModel.ActivePack.CheckIfPackIsPlayable())
            {
                IsPlayerMode = true;
                IsPlayerModeMessage.Invoke(this, EventArgs.Empty);
                CurrentQuestionIndex = 0;
                CorrectAnswersGiven = 0;

                QuestionPackWithRandomizedOrder = mainWindowViewModel.ActivePack.GetQuestionPackWithRandomizedOrderOfQuestions();
                NumberOfQuestionsInPack = QuestionPackWithRandomizedOrder.Questions.Count;

                StartNewQuestion();
            }
            else
            {
                ActivePackIsNotPlayableMessage.Invoke(this, EventArgs.Empty);
            }
        }
        private bool CanPlayQuiz(object? arg) => mainWindowViewModel.ActivePack.Questions.Count > 0 && !IsPlayerMode;
        private void StartNewQuestion()
        {
            ResetColorsForAnswers();
            isWaitingForAnswer = true;
            GivenAnswer = string.Empty;
            GetNextQuestion(CurrentQuestionIndex);
            RestartTimeLimitTimer();
        }
        private void GetNextQuestion(int currentQuestionIndex)
        {
            CurrentQuestionNumber = CurrentQuestionIndex + 1;
            ActiveQuestion = QuestionPackWithRandomizedOrder.Questions[CurrentQuestionIndex];
            ActiveAnswers = QuestionPackWithRandomizedOrder.GetShuffledAnswers(CurrentQuestionIndex);
        }
        private void RestartTimeLimitTimer()
        {
            TimeLimit = QuestionPackWithRandomizedOrder.TimeLimitInSeconds;
            timeLimitTimer = new DispatcherTimer();
            timeLimitTimer.Interval = TimeSpan.FromSeconds(1);
            timeLimitTimer.Tick += CountdownTimeLimit;
            timeLimitTimer.Start();
        }
        private void CountdownTimeLimit(object? sender, EventArgs e)
        {
            if (TimeLimit > 0)
            {
                TimeLimit--;
            }
            else
            {
                timeLimitTimer.Stop();
                isWaitingForAnswer = false;

                int indexOfCorrectAnswer = Array.IndexOf(ActiveAnswers, ActiveQuestion.CorrectAnswer);

                GiveColorToAnswer(indexOfCorrectAnswer + 1, greenBrush);

                PrepareForNextQuestionOrResult();
            }
        }
        private void PrepareForNextQuestionOrResult()
        {
            coolDownTime = 1;
            cooldownTimer = new DispatcherTimer();
            cooldownTimer.Interval = TimeSpan.FromSeconds(1);
            cooldownTimer.Tick += CountdownCooldown;
            cooldownTimer.Start();
        }
        private void CountdownCooldown(object? sender, EventArgs e)
        {

            if (coolDownTime > 0)
            {
                coolDownTime--;
            }
            else
            {
                cooldownTimer.Stop();
                if (!(CurrentQuestionNumber == NumberOfQuestionsInPack))
                {
                    CurrentQuestionIndex++;
                    StartNewQuestion();
                }
                else
                {
                    mainWindowViewModel.ResultViewModel.EnableResultMode(NumberOfQuestionsInPack, CorrectAnswersGiven);
                }
            }
        }

        private void GetAnswer(object givenAnswer)
        {
            bool shouldTakeAnswer = string.IsNullOrEmpty(GivenAnswer) && isWaitingForAnswer;

            if (shouldTakeAnswer)
            {
                timeLimitTimer.Stop();
                GivenAnswer = ActiveAnswers[Int32.Parse((string)givenAnswer)];

                int indexOfCorrectAnswer = Array.IndexOf(ActiveAnswers, ActiveQuestion.CorrectAnswer);

                GiveColorToAnswer(indexOfCorrectAnswer + 1, greenBrush);

                if (GivenAnswer == ActiveQuestion.CorrectAnswer)
                {
                    CorrectAnswersGiven++;
                }
                else
                {
                    int givenAnswerIndex = Array.IndexOf(ActiveAnswers, GivenAnswer);

                    GiveColorToAnswer(givenAnswerIndex + 1, redBrush);
                }

                PrepareForNextQuestionOrResult();
            }
        }
        private void GiveColorToAnswer(int answerNumber, SolidColorBrush brush)
        {
            if (answerNumber == 1)
            {
                ColorForAnswerOne = brush;
            }
            else if (answerNumber == 2)
            {
                ColorForAnswerTwo = brush;
            }
            else if (answerNumber == 3)
            {
                ColorForAnswerThree = brush;
            }
            else
            {
                ColorForAnswerFour = brush;
            }
        }

        public void OnIsOtherModeMessageRecieved(object sender, EventArgs args)
        {
            timeLimitTimer.Stop();
            cooldownTimer?.Stop();
            IsPlayerMode = false;
            PlayQuizCommand.RaiseCanExecuteChanged();
        }
    }
}
