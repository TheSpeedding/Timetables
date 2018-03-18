#ifndef EXCEPTIONS_HPP
#define EXCEPTIONS_HPP

#include <string>
#include <exception>

namespace Timetables {
	namespace Exceptions {

		// Exception which is thrown iff station with given name was not found.
		class station_not_found : public std::exception {
		private:
			std::wstring station_name_;
		public:
			station_not_found(const std::wstring& station_name) : station_name_(station_name) {}
			station_not_found(const std::string& stopId) : station_name_(std::wstring(stopId.cbegin(), stopId.cend())) {}

			inline const std::wstring& station_name() const { return station_name_; }
		};

		// Exception which is thrown iff no departures from given station were found.
		class no_departures_found : public std::exception {
		private:
			std::wstring station_name_;
		public:
			no_departures_found(const std::wstring& station_name) : station_name_(station_name) {}

			inline const std::wstring& station_name() const { return station_name_; } // Gets name of the station.
		};
		
		// Exception is thrown iff no journeys between two stations were found. Increasing number of transfers might solve this problem.
		class journey_not_found : public std::exception {
		private:
			const std::wstring& first_;
			const std::wstring& second_;
		public:
			journey_not_found(const std::wstring& A, const std::wstring& B) : first_(A), second_(B) {}

			inline const std::pair<std::wstring, std::wstring> stations() const { return std::make_pair(first_, second_); } // Gets names of stations.
		};

	}
}

#endif // !EXCEPTIONS_HPP
