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
			std::time_t date;
			std::size_t time;
		public:
			DateTime(const std::string& input);

			static DateTime Now();

			inline std::size_t Hours() const { return (time % 86400) / 3600; }
			inline std::size_t Minutes() const { return ((time % 86400) % 3600) / 60; }
			inline std::size_t Seconds() const { return ((time % 86400) % 3600) % 60; }

			std::size_t Day() const;
			std::size_t Month() const;
			std::size_t Year() const;
			std::size_t DayInWeek() const;

			// inline std::string TimeToString() const { return std::to_string(Hours()) + ':' + (Minutes() < 10 ? "0" : "") + std::to_string(Minutes()) + ':' + (Seconds() < 10 ? "0" : "") + std::to_string(Seconds()); }

			
			inline bool operator< (const DateTime& other) const { return date == other.date ? time < other.time : date < other.date; }
			inline bool operator> (const DateTime& other) const { return date == other.date ? time > other.time : date > other.date; }
			inline bool operator== (const DateTime& other) const { return date == other.date && time == other.time; }


			inline DateTime AddDays(int days) const { DateTime newDate(*this); newDate.date += days * 86400; }

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
