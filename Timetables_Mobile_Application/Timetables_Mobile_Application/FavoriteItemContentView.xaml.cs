using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FavoriteItemContentView : ContentView, IEquatable<FavoriteItemContentView>
	{
		private AutoSuggestBox firstSuggestBox, secondSuggestBox; // Suggestion boxes (to insert stop names).
		private ScrollView sv; // Scroll view of the page (to auto scroll to top).
		private StackLayout sl; // Favorites stack layout (to delete item).
		public FavoriteItemContentView(StackLayout sl, ScrollView sv, AutoSuggestBox source)
		{
			InitializeComponent();
			this.sl = sl;
			this.sv = sv;
			firstSuggestBox = source;
		}
		public FavoriteItemContentView(StackLayout sl, ScrollView sv, AutoSuggestBox source, AutoSuggestBox target) : this (sl, sv, source) => secondSuggestBox = target;
		public FavoriteItemContentView(StackLayout sl, ScrollView sv, JourneyCached item, AutoSuggestBox source, AutoSuggestBox target) : this(sl, sv, source, target)
		{
			firstLabel.Text = item.SourceStation.Name;
			secondLabel.Text = item.TargetStation.Name;

			removeButton.Clicked += (s, e) => item.Remove();
		}
		public FavoriteItemContentView(StackLayout sl, ScrollView sv, StationInfoCached item, AutoSuggestBox source) : this(sl, sv, source)
		{
			firstLabel.Text = item.Station.Name;
			secondLabel.IsVisible = false;

			removeButton.Clicked += (s, e) => item.Remove();
		}
		public FavoriteItemContentView(StackLayout sl, ScrollView sv, LineInfoCached item, AutoSuggestBox source) : this(sl, sv, source)
		{
			firstLabel.Text = item.Route.Label;
			secondLabel.IsVisible = false;

			removeButton.Clicked += (s, e) => item.Remove();
		}

		private async void FindButton_Clicked(object sender, EventArgs e)
		{
			if (firstSuggestBox != null) firstSuggestBox.Text = firstLabel.Text;
			if (secondSuggestBox != null) secondSuggestBox.Text = secondLabel.Text;
			await sv.ScrollToAsync(0, 0, true);
		}

		private void RemoveButton_Clicked(object sender, EventArgs e) => sl.Children.Remove(this);

		public bool Equals(FavoriteItemContentView other) => secondLabel.Text == other.secondLabel.Text && firstLabel.Text == other.firstLabel.Text;
	}
}