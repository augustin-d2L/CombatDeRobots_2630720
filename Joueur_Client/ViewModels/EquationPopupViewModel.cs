using Joueur_Client.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Joueur_Client.ViewModels
{
    internal class EquationPopupViewModel : BaseViewModel
    {
        private readonly System.Action<int> _onAnswered;

        public string Equation { get; }

        private string _answerString;

        public string AnswerString
        {
            get => _answerString;
            set
            {
                if (SetProperty(ref _answerString, value))
                    (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand ConfirmCommand { get; }

        public EquationPopupViewModel(string equation, System.Action<int> onAnswered)
        {
            Equation = equation;
            _onAnswered = onAnswered;
            ConfirmCommand = new RelayCommand(Confirm, () => int.TryParse(AnswerString, out _));
        }

        private void Confirm()
        {
            if (int.TryParse(AnswerString, out int answer))
                _onAnswered(answer);
        }
    }
}
