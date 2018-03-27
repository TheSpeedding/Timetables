#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include <string> // Date time parsing. 
#include <ctime> // A need for time_t.
#include <boost/date_time/posix_time/posix_time.hpp> // Using ptime class.
#include <limits> // Infinity.

namespace Timetables {
	namespace Structures {
		// Class that collects information about date time.
		class date_time {
		private:
			std::time_t date_time_;
		public:
			date_time(const std::string& input) {
				// Accepts date format in YYYYMMDD and time format in HH:MM:SS.
				// For maximal performance, no format validity check. 
				// The library operates with data that should be correct, because they were produced by preprocessor (containing format checking).

				if (input[2] == ':' && input[5] == ':')
					// Time parsing.
					date_time_ = stoi(input.substr(0, 2)) * 3600 + stoi(input.substr(3, 2)) * 60 + stoi(input.substr(6, 2));

				else
					// Date parsing.
					date_time_ = boost::posix_time::to_time_t(boost::posix_time::ptime(
						boost::gregorian::date(stoi(input.substr(0, 4)), stoi(input.substr(4, 2)), stoi(input.substr(6, 2)))));
			}
			date_time(std::time_t totalSecs) : date_time_(totalSecs) {}
			date_time(std::size_t hours, std::size_t mins, std::size_t secs, std::size_t day, std::size_t month, std::size_t year) : date_time_(
				boost::posix_time::to_time_t(boost::posix_time::ptime(boost::gregorian::date(year, month, day), boost::posix_time::time_duration(hours, mins, secs)))
			) {}

			date_time() : date_time_(std::numeric_limits<time_t>::max()) {}
			
			static date_time now() { return date_time(boost::posix_time::to_time_t(boost::posix_time::second_clock::local_time())); }

			inline std::size_t hours() const { return (date_time_ / 3600) % 24; }
			inline std::size_t minutes() const { return (date_time_ / 60) % 60; }
			inline std::size_t seconds() const { return date_time_ % 60; }
			inline std::size_t day() const { return boost::posix_time::from_time_t(date_time_).date().day(); }
			inline std::size_t month() const { return boost::posix_time::from_time_t(date_time_).date().month(); }
			inline std::size_t year() const { return boost::posix_time::from_time_t(date_time_).date().year(); }
			inline std::size_t day_in_week() const { return ((date_time_ / 86400) + 3) % 7; }

			inline std::time_t timestamp() const { return date_time_; }

			date_time date() const { return date_time(86400 * (date_time_ / 86400)); } // Seconds since epoch until midnight.
			date_time time() const { return date_time(date_time_ % 86400); } // Seconds since midnight till time.
			
			friend std::ostream& operator<<(std::ostream& output, const date_time& date_time) { output << boost::posix_time::from_time_t(date_time.date_time_); return output; }
			
			inline bool operator< (const date_time& other) const { return date_time_ < other.date_time_; }
			inline bool operator> (const date_time& other) const { return date_time_ > other.date_time_; }
			inline bool operator== (const date_time& other) const { return date_time_ == other.date_time_; }
			inline bool operator<= (const date_time& other) const { return date_time_ <= other.date_time_; }
			inline bool operator>= (const date_time& other) const { return date_time_ >= other.date_time_; }
			inline bool operator!= (const date_time& other) const { return date_time_ != other.date_time_; }

			inline static long int difference(const date_time& first, const date_time& second) { return first.date_time_ - second.date_time_; }

			inline date_time operator+(const date_time& other) {
				date_time new_date_time(*this);
				new_date_time.date_time_ += other.date_time_;
				return new_date_time;
			}

			inline date_time operator-(const date_time& other) {
				date_time new_date_time(*this);
				new_date_time.date_time_ -= other.date_time_;
				return new_date_time;
			}

			inline date_time add_seconds(int seconds) const {
				date_time new_date_time(*this);
				new_date_time.date_time_ += seconds;
				return new_date_time;
			}

			inline date_time add_minutes(int minutes) const {
				date_time new_date_time(*this);
				new_date_time.date_time_ += 60 * minutes;
				return new_date_time;
			}

			inline date_time add_hours(int hours) const {
				date_time new_date_time(*this);
				new_date_time.date_time_ += 3600 * hours;
				return new_date_time;
			}

			inline date_time add_days(int days) const {
				date_time new_date_time(*this);
				new_date_time.date_time_ += 86400 * days;
				return new_date_time;
			}

		};

	}
}

#endif // !UTILITIES_HPP