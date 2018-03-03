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
		void GetDepartureBoardReport(const Timetables::Structures::DataFeed& feed, const std::wstring& stationName, const Timetables::Structures::DateTime& dateTime, const size_t count);
	}
}

#endif // !REPORTS_HPP
