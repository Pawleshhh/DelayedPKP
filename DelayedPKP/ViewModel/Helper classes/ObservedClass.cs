using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged; 

namespace DelayedPKP.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ObservedClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(params string[] properties)
        {
            foreach (string property in properties)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
