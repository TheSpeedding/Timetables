#ifndef STATIONS_HPP
#define STATIONS_HPP

#include <vector>
#include <string>
#include <algorithm>
#include "Utilities.hpp"
#include "Stops.hpp"
#include "Exceptions.hpp"

namespace Timetables {
	namespace Structures {
		class Stop;
		class Station {
		private:
			const std::wstring name;
			std::vector<const Stop*> childStops; // If the station is itself a stop, then the vector contains only one item.
		public:
			Station(const std::wstring& name) : name(name) {}

			inline const std::wstring& Name() const { return name; }
			inline const std::vector<const Stop*> ChildStops() const { return childStops; }

			inline void AddChildStop(const Stop& stop) { childStops.push_back(&stop); }
		};

		class Stations {
		private:
			std::vector<Station> list;
		public:
			Stations(std::wistream&& stations);

			inline Station& Get(std::size_t id) { return list.at(id); }			
			inline const std::size_t Count() const { return list.size(); }
			inline Station& operator[](std::size_t id) { return list[id]; }

			inline const Station& Find(const std::wstring& name) const {
				auto it = std::find_if(list.cbegin(), list.cend(), [=](const Station& st) { return st.Name() == name; });
				if (it == list.cend()) throw Timetables::Exceptions::StopNotFoundException(name);
				return *it;
			}
		};
	}
}

#endif // !STATIONS_HPP
