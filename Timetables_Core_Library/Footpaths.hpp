#ifndef FOOTPATH_HPP
#define FOOTPATH_HPP

#include "Utilities.hpp"
#include "Stops.hpp"
#include <map>

namespace Timetables {
	namespace Structures {
		class Stop; using StopPtrObserver = const Stop*;

		class Footpaths {
		private:
			std::map<std::pair<StopPtrObserver, StopPtrObserver>, int> walkingTime;
		public:
			Footpaths(const Stops& stops);
			int GetWalkingTime(const Stop& A, const Stop& B) const {
				auto it = walkingTime.find(std::make_pair(&A, &B));
				if (it == walkingTime.cend()) return -1; // Infinity.
				else return it->second;
			}
		};
	}
}

#endif // !FOOTPATH_HPP
