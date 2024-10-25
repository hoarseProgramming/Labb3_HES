using Labb3_HES.Model;
using System.Collections.ObjectModel;

namespace Labb3_HES.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel>? Packs { get; set; }

        public ConfigurationViewModel ConfigurationViewModel { get; }

        public PlayerViewModel PlayerViewModel { get; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged("ActivePack");
            }
        }

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayerViewModel = new PlayerViewModel(this);

            Packs = new ObservableCollection<QuestionPackViewModel>();

            Packs.Add(new QuestionPackViewModel(new QuestionPack("Default Lovely Question Pack")));

            ActivePack = Packs[0];
            ActivePack.Questions.Add(new Question("What is Love?", "Baby don't hurt me", "No more", "No more", "No more"));
            ActivePack.Questions.Add(new Question("What is Love?", "Baby don't hurt me", "No more", "No more", "No more"));

        }
    }
}
