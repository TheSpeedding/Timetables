#ifndef UTILITIES_HPP
#define UTILITIES_HPP

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
