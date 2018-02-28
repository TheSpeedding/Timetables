#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include "Exceptions.hpp"
#include <vector>
#include <string>
#include <exception>

namespace Timetables {
	namespace Structures {
		class DateTime {
		private:
			const std::string& time;
		public:
			DateTime(const std::string& input) : time(input) {}
			// DateTime(int day, int month, int year, int h, int m, int s);

			std::size_t DayInWeek() const;
			
			inline bool operator< (const DateTime& other) const { return time < other.time; }
			inline bool operator> (const DateTime& other) const { return time > other.time;; }
			inline bool operator== (const DateTime& other) const { return time == other.time;; }

		};

		class GpsCoords {
		private:
			inline static double Deg2Rad(double deg) { return (deg * 3.141592653589793 / 180.0); }
			const double latitude, longitude;
		public:
			GpsCoords(double latitude, double longitude) : latitude(latitude), longitude(longitude) {}

			// Returns walking time in seconds between two points. Assuming average walking speed 0.5 m/s.
			static int GetWalkingTime(const GpsCoords& A, const GpsCoords& B);

			inline double GetLatitude() const { return latitude; }
			inline double GetLongitude() const { return longitude; }

			inline bool operator==(const GpsCoords& other) const { return latitude == other.latitude && longitude == other.longitude; }
		};

	}
}

#endif // !UTILITIES_HPP
