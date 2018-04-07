using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Global variables.
	/// </summary>
	static class GlobalData
	{
		public const double CoefficientUndergroundTransfersWithinSameLine = 0.5;
		public const double CoefficientUndergroundTransfersWithinDifferentLines = 1.5;
		public const double CoefficientUndergroundToSurfaceTransfer = 2;
		public const int MaximalDurationOfTransferInSeconds = 600;
		public const double AverageWalkingSpeed = 0.8;
	}
}
