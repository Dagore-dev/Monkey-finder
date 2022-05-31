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
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        
        public MonkeysViewModel (MonkeyService monkeyService)
        {
            Title = "Monkey finder";
            this.monkeyService = monkeyService;
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
    }
}
