using System;
using System.Xml;
using Timetables.Utilities;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Global variables.
	/// </summary>
	public static class GlobalData
	{
		public static readonly double CoefficientUndergroundTransfersWithinSameLine; // Total duration will be multiplied by this constant.
		public static readonly double CoefficientUndergroundTransfersWithinDifferentLines; // Total duration will be multiplied by this constant.
		public static readonly double CoefficientUndergroundToSurfaceTransfer; // Total duration will be multiplied by this constant.
		public static readonly int MaximalDurationOfTransfer; // In seconds.
		public static readonly double AverageWalkingSpeed; // Meters per second.

		public static readonly CPColor DefaultBusColor;
		public static readonly CPColor DefaultTramColor;
		public static readonly CPColor DefaultCableCarColor;
		public static readonly CPColor DefaultRailColor;
		public static readonly CPColor DefaultSubwayColor;
		public static readonly CPColor DefaultShipColor;

		static GlobalData()
		{
			try
			{
				XmlDocument settings = new XmlDocument();
				settings.Load(".settings");

				CoefficientUndergroundTransfersWithinSameLine = double.Parse(settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinSameLine")?[0].InnerText);
				CoefficientUndergroundTransfersWithinDifferentLines = double.Parse(settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinDifferentLines")?[0].InnerText);
				CoefficientUndergroundToSurfaceTransfer = double.Parse(settings.GetElementsByTagName("CoefficientUndergroundToSurfaceTransfer")?[0].InnerText);
				MaximalDurationOfTransfer = int.Parse(settings.GetElementsByTagName("MaximalDurationOfTransfer")?[0].InnerText);
				AverageWalkingSpeed = double.Parse(settings.GetElementsByTagName("AverageWalkingSpeed")?[0].InnerText);
			}

			catch
			{
				CoefficientUndergroundTransfersWithinSameLine = 0.5;
				CoefficientUndergroundTransfersWithinDifferentLines = 1.5;
				CoefficientUndergroundToSurfaceTransfer = 2;
				MaximalDurationOfTransfer = 600; 
				AverageWalkingSpeed = 0.9;

				DefaultBusColor = CPColor.FromHtml("#4A90E2");
				DefaultTramColor = CPColor.FromHtml("#8B572A");
				DefaultCableCarColor = CPColor.FromHtml("#FF8C00");
				DefaultRailColor = CPColor.FromHtml("#006600");
				DefaultSubwayColor = CPColor.FromHtml("#FFFF00");
				DefaultShipColor = CPColor.FromHtml("#0033CC");
			}
		}
	}
}