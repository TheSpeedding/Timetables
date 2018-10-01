namespace Timetables.Client
{
	/// <summary>
	/// Means of transportation.
	/// </summary>
	[System.Flags]
	public enum MeanOfTransport
	{
		Tram = 1,
		Subway = 2,
		Rail = 4,
		Bus = 8,
		Ship = 16,
		CableCar = 32,
		Gondola = 64,
		Funicular = 128
	}
}
