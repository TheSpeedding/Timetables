using System;
using System.Drawing;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Global variables.
	/// </summary>
	public static partial class GlobalData
	{
		public static readonly double CoefficientUndergroundTransfersWithinSameLine; // Total duration will be multiplied by this constant.
		public static readonly double CoefficientUndergroundTransfersWithinDifferentLines; // Total duration will be multiplied by this constant.
		public static readonly double CoefficientUndergroundToSurfaceTransfer; // Total duration will be multiplied by this constant.
		public static readonly int MaximalDurationOfTransfer; // In seconds.
		public static readonly double AverageWalkingSpeed; // Meters per second.

		public static readonly Color DefaultBusColor;
		public static readonly Color DefaultTramColor;
		public static readonly Color DefaultCableCarColor;
		public static readonly Color DefaultRailColor;
		public static readonly Color DefaultSubwayColor;
		public static readonly Color DefaultShipColor;

		static GlobalData()
		{
			try
			{
				throw new NotImplementedException("TO-DO: Settings in file. Will be used in a server mode.");
			}
			catch
			{
				CoefficientUndergroundTransfersWithinSameLine = 0.5;
				CoefficientUndergroundTransfersWithinDifferentLines = 1.5;
				CoefficientUndergroundToSurfaceTransfer = 2;
				MaximalDurationOfTransfer = 600; 
				AverageWalkingSpeed = 0.9;

				DefaultBusColor = ColorTranslator.FromHtml("#4A90E2");
				DefaultTramColor = ColorTranslator.FromHtml("#8B572A");
				DefaultCableCarColor = ColorTranslator.FromHtml("#FF8C00");
				DefaultRailColor = ColorTranslator.FromHtml("#006600");
				DefaultSubwayColor = ColorTranslator.FromHtml("#FFFF00");
				DefaultShipColor = ColorTranslator.FromHtml("#0033CC");
			}
		}
	}
}