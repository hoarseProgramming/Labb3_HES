using Labb3_HES.Command;
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
        public DelegateCommand AddNewPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }

        public MainWindowViewModel()
        {
            AddNewPackCommand = new DelegateCommand(AddNewPack);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);

            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayerViewModel = new PlayerViewModel(this);

            Packs = JsonHandler.LoadJsonFile();
            ActivePack = Packs.FirstOrDefault();
        }

        private void SelectActivePack(object obj) => ActivePack = obj as QuestionPackViewModel;

        private bool CanDeletePack(object? arg) => Packs.Count > 1;

        private void DeletePack(object obj)
        {
            Packs.Remove(ActivePack);
            DeletePackCommand.RaiseCanExecuteChanged();
        }

        private void AddNewPack(Object obj)
        {
            Packs.Add(new QuestionPackViewModel(new QuestionPack()));
            DeletePackCommand.RaiseCanExecuteChanged();
        }
    }
}
