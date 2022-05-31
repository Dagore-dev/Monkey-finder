using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.ViewModel
{
    [QueryProperty(nameof(Monkey), "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        public MonkeyDetailsViewModel ()
        {

        }

        [ObservableProperty]
        Monkey monkey;
    }
}
