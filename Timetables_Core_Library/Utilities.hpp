#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include <vector>
#include <string>
#include <exception>
#include <ctime>

namespace Timetables {
	namespace Structures {
		class DateTime {
		private:
			std::time_t dateTime;
		public:
			DateTime(const std::string& input);
			DateTime(std::time_t totalSecs) : dateTime(totalSecs) {}
			DateTime(std::size_t hours, std::size_t mins, std::size_t secs, std::size_t day, std::size_t month, std::size_t year);

			static DateTime Now();

			static long int Difference(const DateTime& first, const DateTime& second) { return first.dateTime - second.dateTime; }

			inline std::size_t Hours() const { return (dateTime / 3600) % 24; }
			inline std::size_t Minutes() const { return (dateTime / 60) % 60; }
			inline std::size_t Seconds() const { return dateTime % 60; }
			std::size_t Day() const;
			std::size_t Month() const;
			std::size_t Year() const;
			inline std::size_t DayInWeek() const { return ((dateTime / 86400) + 4) % 7; }

			DateTime Date() const { return DateTime(86400 * (dateTime / 86400)); } // Seconds since epoch until midnight.
			DateTime Time() const { return DateTime(dateTime % 86400); } // Seconds since midnight till time.
			
			inline std::string ToString() const {
				return (dateTime >= 86400 ? ((Day() < 10 ? "0" : "") + std::to_string(Day()) + '.' + (Month() < 10 ? "0" : "") + std::to_string(Month()) + '.' + std::to_string(Year()) + " ") : "") +
					std::to_string(Hours()) + ':' + (Minutes() < 10 ? "0" : "") + std::to_string(Minutes()) + ':' + (Seconds() < 10 ? "0" : "") + std::to_string(Seconds());
			}

			friend std::ostream& operator<<(std::ostream& output, const DateTime& dateTime) { output << dateTime.ToString() ; return output; }


			inline bool operator< (const DateTime& other) const { return dateTime < other.dateTime; }
			inline bool operator> (const DateTime& other) const { return dateTime > other.dateTime; }
			inline bool operator== (const DateTime& other) const { return dateTime == other.dateTime; }
			inline bool operator<= (const DateTime& other) const { return dateTime <= other.dateTime; }
			inline bool operator>= (const DateTime& other) const { return dateTime >= other.dateTime; }
			inline bool operator!= (const DateTime& other) const { return dateTime != other.dateTime; }

			inline DateTime operator+(const DateTime& other) {
				DateTime newDate(*this);
				newDate.dateTime += other.dateTime;
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
