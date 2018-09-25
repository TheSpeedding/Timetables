#ifndef ROUTER_BASE_HPP
#define ROUTER_BASE_HPP

#include <set>
#include "../Structures/journey.hpp"

namespace Timetables {
	namespace Algorithms {
		class router_base {
		public: 
			virtual void obtain_journeys() = 0;
			virtual const std::set<Timetables::Structures::journey>& show_journeys() const = 0;
		};
	}
}

#endif