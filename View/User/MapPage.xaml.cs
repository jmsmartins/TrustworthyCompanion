using System;
using System.Threading.Tasks;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Model;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrustworthyCompanion.View.User {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MapPage : Page {

		private Geolocator _geolocator;
		private Geoposition _geoPosition;
		private Geopoint _startPosition;
		private Geopoint _endPosition;

		private BasicInformationModel _basicInformation;

		public MapPage() {
			this.InitializeComponent();
			this.NavigationCacheMode = NavigationCacheMode.Required;
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e) {
			MyMap.MapServiceToken = "abcdef-abcdefghijklmno";

			_basicInformation = await DatabaseService.GetBasicInformation();

			_geolocator = new Geolocator();
			_geolocator.DesiredAccuracyInMeters = 50;
			try {
				_geoPosition = await _geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));

				//MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(_basicInformation.Address, new Geopoint(new BasicGeoposition()), 5);
				//var point = result.Locations;

				MapIcon mapIcon = new MapIcon();
				mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/my-position.png"));
				mapIcon.Title = "Current Location";
				mapIcon.Location = new Geopoint(new BasicGeoposition() {
					Latitude = _geoPosition.Coordinate.Point.Position.Latitude,
					Longitude = _geoPosition.Coordinate.Point.Position.Longitude
				});
				mapIcon.NormalizedAnchorPoint = new Point(0.5, 0.5);
				MyMap.MapElements.Add(mapIcon);
				await MyMap.TrySetViewAsync(mapIcon.Location, 18D, 0, 0, MapAnimationKind.Bow);

				// reverse geocoding
				BasicGeoposition myLocation = new BasicGeoposition {
					Longitude = _geoPosition.Coordinate.Longitude,
					Latitude = _geoPosition.Coordinate.Latitude
				};
				_startPosition = new Geopoint(myLocation);

				progressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
				mySlider.Value = MyMap.ZoomLevel;
			}
			catch(UnauthorizedAccessException) {
				//MessageBox("Location service is turned off!");
			}
			base.OnNavigatedTo(e);

			await Page_Loaded(null, null);
		}

		private DependencyObject CreatePushPin() {
			// Creating a Polygon Marker
			Polygon polygon = new Polygon();
			polygon.Points.Add(new Point(0, 0));
			polygon.Points.Add(new Point(0, 50));
			polygon.Points.Add(new Point(25, 0));
			polygon.Fill = new SolidColorBrush(Colors.Red);

			return polygon;
		}

		private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
			if(MyMap != null) {
				MyMap.ZoomLevel = e.NewValue;
			}
		}

		private async Task Page_Loaded(object sender, RoutedEventArgs e) {
			BasicGeoposition endPosition = new BasicGeoposition {
				Latitude = 41.178465,
				Longitude = -8.606394
			};
			_endPosition = new Geopoint(endPosition);

			MapIcon mapIcon = new MapIcon();
			mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/my-position.png"));
			mapIcon.Title = "Home";
			mapIcon.Location = _endPosition;
			mapIcon.NormalizedAnchorPoint = new Point(0.5, 0.5);
			MyMap.MapElements.Add(mapIcon);

			await ShowRouteOnMap();

			await MyMap.TrySetViewAsync(mapIcon.Location, 18D, 0, 0, MapAnimationKind.Default);
		}


		private async Task ShowRouteOnMap() {
			// Get a route as shown previously.
			// Get the route between the points.
			MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(_startPosition, _endPosition, MapRouteOptimization.Time, MapRouteRestrictions.None);

			if(routeResult.Status == MapRouteFinderStatus.Success) {
				// Use the route to initialize a MapRouteView.
				MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
				viewOfRoute.RouteColor = Colors.Yellow;
				viewOfRoute.OutlineColor = Colors.Black;

				// Add the new MapRouteView to the Routes collection of the MapControl.
				MyMap.Routes.Add(viewOfRoute);

				// Fit the MapControl to the route.
				await MyMap.TrySetViewBoundsAsync(routeResult.Route.BoundingBox, null, MapAnimationKind.None);
			}
		}
	}
}
