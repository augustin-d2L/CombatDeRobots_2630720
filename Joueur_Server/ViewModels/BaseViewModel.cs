using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notify the vue that a property .
        /// CallerMemberName fills automatically the property name.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Change the value of a field and notify the view if the value changed.
        /// Return true if value changed, else false.
        /// </summary>
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
