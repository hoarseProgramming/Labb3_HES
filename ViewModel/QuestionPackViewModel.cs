using Labb3_HES.Model;
using System.Collections.ObjectModel;

namespace Labb3_HES.ViewModel
{
    class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack model;

        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            this.Questions = new ObservableCollection<Question>(model.Questions);
        }

        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> Questions { get; }

        private static Random rng = new Random();
        public QuestionPackViewModel GetQuestionPackWithRandomizedOrderOfQuestions()
        {
            QuestionPackViewModel randomizedQuestionPack = new(new QuestionPack());

            foreach (Question question in Questions)
            {
                randomizedQuestionPack.Questions.Add(question);
            }

            randomizedQuestionPack.TimeLimitInSeconds = TimeLimitInSeconds;
            randomizedQuestionPack.Name = Name;
            randomizedQuestionPack.Difficulty = Difficulty;

            randomizedQuestionPack.ShuffleQuestions();

            return randomizedQuestionPack;
        }
        private void ShuffleQuestions()
        {
            int n = Questions.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Question value = Questions[k];
                Questions[k] = Questions[n];
                Questions[n] = value;
            }
        }
        public string[] GetShuffledAnswers(int currentIndexOfQuestions)
        {
            string[] shuffledAnswers = new string[4];

            for (int i = 0; i < 3; i++)
            {
                shuffledAnswers[i] = Questions[currentIndexOfQuestions].IncorrectAnswers[i];
            }
            shuffledAnswers[3] = Questions[currentIndexOfQuestions].CorrectAnswer;

            int n = shuffledAnswers.Length;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = shuffledAnswers[k];
                shuffledAnswers[k] = shuffledAnswers[n];
                shuffledAnswers[n] = value;
            }

            return shuffledAnswers;
        }

        public override string ToString()
        {
            return $"<{Name}> ({Difficulty})";
        }

        public QuestionPack GetQuestionPackModel()
        {
            return model;
        }
    }
}
