using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

// In this file, basic geolocator is implemented. 
// The code for both Xamarin and Windows remains the same, but a method to get current location differs a bit.

namespace Timetables.Utilities
{
	/// <summary>
	/// Structure representing a GPS position.
	/// </summary>
	public struct Position
	{
		/// <summary>
		/// Latitude.
		/// </summary>
		public double Latitude { get; }
		/// <summary>
		/// Longitude.
		/// </summary>
		public double Longitude { get; }
		public Position(double lat, double lon)
		{
			Latitude = lat;
			Longitude = lon;
		}
	}
	/// <summary>
	/// Delegate to obtain current GPS position.
	/// </summary>
	/// <returns>Current position.</returns>
	public delegate Position GetPositionHandler();
	/// <summary>
	/// Crossplatform geolocator.
	/// </summary>
	public class CPGeolocator
	{
		private readonly GetPositionHandler GetPosition;

		/// <summary>
		/// Initializes a object.
		/// </summary>
		/// <param name="fn">A function to obtain current location.</param>
		public CPGeolocator(GetPositionHandler fn) => GetPosition = fn;

		/// <summary>
		/// Returns current position of the user.
		/// </summary>
		/// <returns>Current position.</returns>
		public Task<Position> GetCurrentPosition() => Task.Run(() => GetPosition());
	}
}
