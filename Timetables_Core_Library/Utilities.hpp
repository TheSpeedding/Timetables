#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include <vector>
#include <string>
#include <exception>
#include <ctime>
#include <chrono>

namespace Timetables {
	namespace Structures {
		class DateTime {
		private:
			struct tm dateTime;
		public:
			DateTime(const std::string& input);
			// DateTime(int day, int month, int year, int h, int m, int s);

			inline std::size_t DayInWeek() const { return dateTime.tm_wday; }
			
			inline bool operator< (const DateTime& other) const { return dateTime.tm_hour < other.dateTime.tm_hour; }
			inline bool operator> (const DateTime& other) const { return dateTime.tm_hour > other.dateTime.tm_hour; }
			inline bool operator== (const DateTime& other) const { return dateTime.tm_hour == other.dateTime.tm_hour; }

		};

		class GpsCoords {
		private:
			const double latitude, longitude;
		public:
			GpsCoords(double latitude, double longitude) : latitude(latitude), longitude(longitude) {}
			
			inline double GetLatitude() const { return latitude; }
			inline double GetLongitude() const { return longitude; }

			inline bool operator==(const GpsCoords& other) const { return latitude == other.latitude && longitude == other.longitude; }
		};

	}
}

#endif // !UTILITIES_HPP
