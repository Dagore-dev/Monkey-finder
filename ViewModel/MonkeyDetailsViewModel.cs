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
        IMap map;
        public MonkeyDetailsViewModel(IMap map)
        {
            this.map = map;
        }

        [ObservableProperty]
        Monkey monkey;

        [ICommand]
        async Task OpenMapAsync ()
        {
            try
            {
                await map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
                {
                    Name = Monkey.Name,
                    NavigationMode = NavigationMode.None
                });
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("Error", $"Unable to open map: {ex.Message}", "OK");
            }
        }
    }
}
