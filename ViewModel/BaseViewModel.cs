using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        public BaseViewModel ()
        {
            
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        bool isBusy;
        
        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;
    }
}
