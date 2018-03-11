#ifndef REPORTS_HPP
#define REPORTS_HPP

#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/DataFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"

namespace Timetables {
	namespace SampleApp {
		void GetDepartureBoardReport(const Timetables::Structures::DataFeed& feed, const std::wstring& stationName, std::time_t dateTime, const size_t count);
		void GetJourneysReport(const Timetables::Structures::DataFeed& feed, const std::wstring& A, const std::wstring& B, std::time_t dateTime, const size_t count, const size_t maxTransfers);
	}
}

#endif // !REPORTS_HPP
