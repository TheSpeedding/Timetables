#ifndef REPORTS_HPP
#define REPORTS_HPP

#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/Structures/data_feed.hpp"
#include "../Timetables_Core_Library/Structures/date_time.hpp"
#include "../Timetables_Core_Library/Algorithms/departure_board.hpp"

namespace Timetables {
	namespace SampleApp {
		void get_departure_board_report(const Timetables::Structures::data_feed& feed, const std::wstring& station_name, const Timetables::Structures::date_time& date_time, const size_t count);
		void get_journeys_report(const Timetables::Structures::data_feed& feed, const std::wstring& A, const std::wstring& B, const Timetables::Structures::date_time& date_time, const size_t count, const size_t max_transfers);
		void get_hint(const Timetables::Structures::stations& stations, const std::wstring& name);
	}
}

#endif // !REPORTS_HPP
