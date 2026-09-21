using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Joueur_Client.ViewModels
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T champ, T valeur, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(champ, valeur))
                return false;

            champ = valeur;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
