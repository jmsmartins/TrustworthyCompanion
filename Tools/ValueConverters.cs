using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TrustworthyCompanion.Tools {

	/// <summary>
	/// Converts a boolean value to a fontweight type
	/// </summary>
	internal class SelectionModeValueConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			if(value is bool) {
				if((bool)value == true)
					return ListViewSelectionMode.Multiple;
				else
					return ListViewSelectionMode.Single;
			}
			return SelectionMode.Single;
		}

		// No need to implement converting back on a one-way binding 
		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			return false;
		}
	}

	/// <summary>
	/// Converts a boolean value to a visibility type
	/// </summary>
	internal class VisibilityValueConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			if(value is bool) {
				if((bool)value == true)
					return Visibility.Visible;
				else
					return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		// No need to implement ConvertBack on a One-Way binding
		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			return false;
		}
	}
}