using Labb3_HES.Command;
using Labb3_HES.Model;
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
        public event EventHandler AnswerRecievedMessage;
        public event EventHandler NoAnswerRecievedMessage;
        public event EventHandler IsNewQuestionMessage;

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            PlayQuizCommand = new DelegateCommand(PlayQuiz, CanPlayQuiz);
            GiveAnswerCommand = new DelegateCommand(GetAnswer);
        }

        private void PlayQuiz(object obj)
        {
            IsPlayerMode = true;
            SendIsPlayerModeMessage();
            CurrentQuestionIndex = 0;
            CorrectAnswersGiven = 0;

            QuestionPackWithRandomizedOrder = mainWindowViewModel.ActivePack.GetQuestionPackWithRandomizedOrderOfQuestions();
            NumberOfQuestionsInPack = QuestionPackWithRandomizedOrder.Questions.Count;

            StartNewQuestion();
        }
        private bool CanPlayQuiz(object? arg) => mainWindowViewModel.ActivePack.Questions.Count > 0 && !IsPlayerMode;
        private void SendIsPlayerModeMessage() => IsPlayerModeMessage.Invoke(this, EventArgs.Empty);
        private void StartNewQuestion()
        {
            isWaitingForAnswer = true;
            GivenAnswer = string.Empty;
            IsNewQuestionMessage.Invoke(this, EventArgs.Empty);
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

                if (string.IsNullOrEmpty(GivenAnswer)) NoAnswerRecievedMessage.Invoke(this, EventArgs.Empty);

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
                GivenAnswer = (string)givenAnswer;

                if (GivenAnswer == ActiveQuestion.CorrectAnswer)
                {
                    CorrectAnswersGiven++;
                }

                AnswerRecievedMessage.Invoke(this, EventArgs.Empty);
                PrepareForNextQuestionOrResult();
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
