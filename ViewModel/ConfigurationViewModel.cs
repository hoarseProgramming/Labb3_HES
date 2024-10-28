﻿using Labb3_HES.Command;
using Labb3_HES.Dialogs;
using Labb3_HES.Model;

namespace Labb3_HES.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }

        public Question _activeQuestion;
        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand PackOptionsCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
            PackOptionsCommand = new DelegateCommand(OpenPackOptions);
        }

        private void OpenPackOptions(object obj)
        {
            //TODO: Make pakcoptiondialog MVVM way
            PackOptionsDialog packOptionsDialog = new PackOptionsDialog();
        }

        private bool CanRemoveQuestion(object? arg) => ActiveQuestion != null;

        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
        }

        private void AddQuestion(object obj) => ActivePack.Questions.Add(new Question());

    }
}