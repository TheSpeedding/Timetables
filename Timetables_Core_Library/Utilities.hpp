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

		public:
			std::time_t date;
			long int time;
		public:
			DateTime(const std::string& input);
			DateTime(std::time_t totalSecs);
			DateTime(std::size_t time, std::time_t date) : date(date + 86400 * (time / 86400)), time(time % 86400) {}
			DateTime(std::size_t hours, std::size_t mins, std::size_t secs, std::size_t day, std::size_t month, std::size_t year);

			static DateTime Now();

			static long int Difference(const DateTime& first, const DateTime& second);

			inline std::size_t Hours() const { return (time % 86400) / 3600; }
			inline std::size_t Minutes() const { return ((time % 86400) % 3600) / 60; }
			inline std::size_t Seconds() const { return ((time % 86400) % 3600) % 60; }

			inline long int TotalSecondsSinceMidnight() const { return time; }
			inline std::time_t TotalSecondsSinceEpoch() const { return date + time; }
			inline std::time_t TotalSecondsSinceEpochUntilMidnight() const { return date; }

			inline std::string ToString() const {
				return (date == 0 ? "" : ((Day() < 10 ? "0" : "") + std::to_string(Day()) + '.' + (Month() < 10 ? "0" : "") + std::to_string(Month()) + '.' + std::to_string(Year()) + " ")) +
					   (std::to_string(Hours()) + ':' + (Minutes() < 10 ? "0" : "") + std::to_string(Minutes()) + ':' + (Seconds() < 10 ? "0" : "") + std::to_string(Seconds()));
			}

			friend std::ostream& operator<<(std::ostream& output, const DateTime& dateTime) { output << dateTime.ToString() ; return output; }

			std::size_t Day() const;
			std::size_t Month() const;
			std::size_t Year() const;
			std::size_t DayInWeek() const;

			inline bool operator< (const DateTime& other) const { return date == other.date ? (date == 0 ? time % 86400 < other.time % 86400 : time < other.time) : date < other.date; }
			inline bool operator> (const DateTime& other) const { return date == other.date ? (date == 0 ? time % 86400 > other.time % 86400 : time > other.time) : date > other.date; }
			inline bool operator<= (const DateTime& other) const { return date == other.date ? (date == 0 ? time % 86400 <= other.time % 86400 : time <= other.time) : date <= other.date; }
			inline bool operator>= (const DateTime& other) const { return date == other.date ? (date == 0 ? time % 86400 >= other.time % 86400 : time >= other.time) : date >= other.date; }
			inline bool operator== (const DateTime& other) const { return date == other.date && time == other.time; }

			inline DateTime AddSeconds(int seconds) const {
				DateTime newDate(*this);
				newDate.time += seconds;
				newDate.date += 86400 * (newDate.time / 86400);
				newDate.time %= 86400;
				if (newDate.time < 0) {
					newDate.date -= 86400; newDate.time += 86400;
				}
				return newDate;
			}

			inline DateTime AddMinutes(int minutes) const {
				DateTime newDate(*this);
				newDate.time += minutes * 60;
				newDate.date += 86400 * (newDate.time / 86400);
				newDate.time %= 86400;
				if (newDate.time < 0) {
					newDate.date -= 86400; newDate.time += 86400;
				}
				return newDate;
			}

			inline DateTime AddHours(int hours) const { 
				DateTime newDate(*this); 
				newDate.time += hours * 3600;
				newDate.date += 86400 * (newDate.time / 86400);
				newDate.time %= 86400;
				if (newDate.time < 0) {
					newDate.date -= 86400; newDate.time += 86400;
				}
				return newDate; 
			}

			inline DateTime AddDays(int days) const { DateTime newDate(*this); newDate.date += days * 86400; return newDate; }


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
