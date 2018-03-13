#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include <string>
#include <ctime>
#include <limits>

namespace Timetables {
	namespace Structures {
		static struct DateTime {
		public:
			static const std::time_t Infinity = std::numeric_limits<time_t>::max();

			static std::time_t ParseDate(const std::string& input);
			inline static std::time_t ParseTime(const std::string& input) { return std::stoi(input.substr(0, 2)) * 3600 + stoi(input.substr(3, 2)) * 60 + std::stoi(input.substr(6, 2)); }
			inline static std::time_t Now() { return std::time(0); }

			static inline std::size_t Hours(std::time_t dt) { return (dt / 3600) % 24; }
			static inline std::size_t Minutes(std::time_t dt) { return (dt / 60) % 60; }
			static inline std::size_t Seconds(std::time_t dt) { return dt % 60; }
			static std::size_t Day(std::time_t dt);
			static std::size_t Month(std::time_t dt);
			static std::size_t Year(std::time_t dt);
			static inline std::size_t DayInWeek(std::time_t dt) { return ((dt / 86400) + 4) % 7; }

			static inline std::string ToString(std::time_t dt) {
				return (dt >= 86400 ? ((Day(dt) < 10 ? "0" : "") + std::to_string(Day(dt)) + '.' + (Month(dt) < 10 ? "0" : "") + std::to_string(Month(dt)) + '.' + std::to_string(Year(dt)) + " ") : "") +
					std::to_string(Hours(dt)) + ':' + (Minutes(dt) < 10 ? "0" : "") + std::to_string(Minutes(dt)) + ':' + (Seconds(dt) < 10 ? "0" : "") + std::to_string(Seconds(dt));
			}

			static inline std::time_t Date(std::time_t dt) { return 86400 * (dt / 86400); } // Seconds since epoch until midnight.
			static inline std::time_t Time(std::time_t dt) { return dt % 86400; } // Seconds since midnight till time.

			static inline std::time_t AddSeconds(std::time_t dt, int seconds) { return dt + seconds; }
			static inline std::time_t AddMinutes(std::time_t dt, int minutes) { return dt + 60 * minutes; }
			static inline std::time_t AddHours(std::time_t dt, int hours) { return dt + 3600 * hours; }
			static inline std::time_t AddDays(std::time_t dt, int days) { return dt + 86400 * days; }
		};
	}
}

#endif // !UTILITIES_HPP
