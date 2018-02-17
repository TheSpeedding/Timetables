#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <string>
#include "GtfsFeed.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"

namespace Timetables {
	namespace Algorithms {
		void FindRoutes(const Timetables::Structures::GtfsFeed& feed, const std::wstring& A, const std::wstring& B, const Timetables::Structures::Datetime& datetime, const size_t count);
	}
}

#endif // !ROUTER_HPP
