#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include <string> // Date time parsing. 
#include <ctime> // A need for time_t.
#include <boost/date_time/posix_time/posix_time.hpp> // Using ptime class.
#include <limits> // Infinity.

namespace Timetables {
	namespace Structures {
		// Class that collects information about date time.
		class DateTime {
		private:
			std::time_t dateTime;
		public:
			DateTime(const std::string& input) {
				// Accepts date format in YYYYMMDD and time format in HH:MM:SS.
				// For maximal performance, no format validity check. 
				// The library operates with data that should be correct, because they were produced by preprocessor (containing format checking).

				if (input[2] == ':' && input[5] == ':')
					// Time parsing.
					dateTime = stoi(input.substr(0, 2)) * 3600 + stoi(input.substr(3, 2)) * 60 + stoi(input.substr(6, 2));

				else
					// Date parsing.
					dateTime = boost::posix_time::to_time_t(boost::posix_time::ptime(
						boost::gregorian::date(stoi(input.substr(0, 4)), stoi(input.substr(4, 2)), stoi(input.substr(6, 2)))));
			}
			DateTime(std::time_t totalSecs) : dateTime(totalSecs) {}
			DateTime(std::size_t hours, std::size_t mins, std::size_t secs, std::size_t day, std::size_t month, std::size_t year) : dateTime(
				boost::posix_time::to_time_t(boost::posix_time::ptime(boost::gregorian::date(year, month, day), boost::posix_time::time_duration(hours, mins, secs)))
			) {}

			static DateTime Infinity() { return DateTime(std::numeric_limits<time_t>::max()); }

			static DateTime Now() { return DateTime(boost::posix_time::to_time_t(boost::posix_time::second_clock::local_time())); }

			inline std::size_t Hours() const { return (dateTime / 3600) % 24; }
			inline std::size_t Minutes() const { return (dateTime / 60) % 60; }
			inline std::size_t Seconds() const { return dateTime % 60; }
			inline std::size_t Day() const { return boost::posix_time::from_time_t(dateTime).date().day(); }
			inline std::size_t Month() const { return boost::posix_time::from_time_t(dateTime).date().month(); }
			inline std::size_t Year() const { return boost::posix_time::from_time_t(dateTime).date().year(); }
			inline std::size_t DayInWeek() const { return ((dateTime / 86400) + 4) % 7; }

			DateTime Date() const { return DateTime(86400 * (dateTime / 86400)); } // Seconds since epoch until midnight.
			DateTime Time() const { return DateTime(dateTime % 86400); } // Seconds since midnight till time.
			
			friend std::ostream& operator<<(std::ostream& output, const DateTime& dateTime) { output << boost::posix_time::from_time_t(dateTime.dateTime); return output; }
			
			inline bool operator< (const DateTime& other) const { return dateTime < other.dateTime; }
			inline bool operator> (const DateTime& other) const { return dateTime > other.dateTime; }
			inline bool operator== (const DateTime& other) const { return dateTime == other.dateTime; }
			inline bool operator<= (const DateTime& other) const { return dateTime <= other.dateTime; }
			inline bool operator>= (const DateTime& other) const { return dateTime >= other.dateTime; }
			inline bool operator!= (const DateTime& other) const { return dateTime != other.dateTime; }

			inline static long int Difference(const DateTime& first, const DateTime& second) { return first.dateTime - second.dateTime; }

			inline DateTime operator+(const DateTime& other) {
				DateTime newDate(*this);
				newDate.dateTime += other.dateTime;
				return newDate;
			}

			inline DateTime operator-(const DateTime& other) {
				DateTime newDate(*this);
				newDate.dateTime -= other.dateTime;
				return newDate;
			}

			inline DateTime AddSeconds(int seconds) const {
				DateTime newDate(*this);
				newDate.dateTime += seconds;
				return newDate;
			}

			inline DateTime AddMinutes(int minutes) const {
				DateTime newDate(*this);
				newDate.dateTime += 60 * minutes;
				return newDate;
			}

			inline DateTime AddHours(int hours) const {
				DateTime newDate(*this);
				newDate.dateTime += 3600 * hours;
				return newDate;
			}

			inline DateTime AddDays(int days) const {
				DateTime newDate(*this);
				newDate.dateTime += 86400 * days;
				return newDate;
			}

		};

	}
}

#endif // !UTILITIES_HPP