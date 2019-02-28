using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
		}

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; }
            
			public Localization Localization { get; }

            public MainPageMasterViewModel()
            {
				Localization = Settings.Localization;

                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem { Id = 0, Title = Localization.FindJourney, TargetType = typeof(FindJourneyPage) },
                    new MainPageMenuItem { Id = 1, Title = Localization.FindDeparturesFromTheStation, TargetType = typeof(FindStationInfoPage) },
					new MainPageMenuItem { Id = 2, Title = Localization.FindInformationAboutLine, TargetType = typeof(FindLineInfoPage) },
					new MainPageMenuItem { Id = 3, Title = Localization.ShowMap, TargetType = typeof(ShowMapPage) },
					new MainPageMenuItem { Id = 4, Title = Localization.ExtraordinaryEvents, TargetType = typeof(ExtraordinaryEventsPage) },
					new MainPageMenuItem { Id = 5, Title = Localization.Lockouts, TargetType = typeof(LockoutsPage) },
					new MainPageMenuItem { Id = 6, Title = Localization.Settings }
				});
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}