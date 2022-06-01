using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder.ViewModel
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        MonkeyService monkeyService;
        IConnectivity connectivity;
        IGeolocation geolocation;
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            Title = "Monkey finder";
            this.monkeyService = monkeyService;
            this.connectivity = connectivity;
            this.geolocation = geolocation;
        }

        [ICommand]
        async Task GetMonkeysAsync ()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Connectivity problems", "There is a problem with your internet connection.", "OK");
                    return;
                }
                IsBusy = true;
                List<Monkey> monkeys = await monkeyService.GetMonkeys();

                if (Monkeys.Count != 0)
                {
                    Monkeys.Clear();
                }

                foreach (Monkey monkey in monkeys)
                {
                    Monkeys.Add(monkey);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unable to get monkeys: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [ICommand]
        async Task GoToDetailsAsync (Monkey monkey)
        {
            if (monkey == null)
            {
                return;
            }
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
            {
                {"Monkey", monkey }
            });
        }
        [ICommand]
        async Task GetClosestMonkeyAsync ()
        {
            if (IsBusy || Monkeys.Count == 0)
            {
                return;
            }

            try
            {
                Location location = await geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await geolocation.GetLocationAsync(new GeolocationRequest()
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location == null)
                {
                    return;
                }

                Monkey first = Monkeys
                    .OrderBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Miles))
                    .FirstOrDefault();

                await Shell.Current.DisplayAlert("Closest monkey", $"{first.Name} in {first.Location}", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unable to get closest monkey: {ex.Message}", "OK");
            }
        }
    }
}
