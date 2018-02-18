#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <string>
#include "GtfsFeed.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"

namespace Timetables {
	namespace Algorithms {
		void FindRoutes(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& datetime, const std::size_t count, const std::size_t transfers = 10);
	}
}

#endif // !ROUTER_HPP
