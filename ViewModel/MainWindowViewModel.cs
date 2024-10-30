﻿using Labb3_HES.Command;
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
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }

        public MainWindowViewModel()
        {
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
            var currentActivePack = ActivePack;
            UpdateActivePackBeforeDeletingPack(ActivePack);
            Packs.Remove(currentActivePack);
            DeletePackCommand.RaiseCanExecuteChanged();
        }

        private void UpdateActivePackBeforeDeletingPack(QuestionPackViewModel currentActivePack)
        {
            int currentActivePackIndex = Packs.IndexOf(ActivePack);
            if (currentActivePackIndex == 0)
            {
                ActivePack = Packs[1];
            }
            else
            {
                ActivePack = Packs[currentActivePackIndex - 1];
            }
        }

        public void AddNewPack(string name, int difficultyIndex, int timeLimitInSeconds)
        {
            var newQuestionPack = new QuestionPackViewModel(new QuestionPack(name, (Difficulty)difficultyIndex, timeLimitInSeconds));
            Packs.Add(newQuestionPack);
            ActivePack = newQuestionPack;
            DeletePackCommand.RaiseCanExecuteChanged();
        }
    }
}
