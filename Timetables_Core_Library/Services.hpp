#ifndef SERVICES_HPP
#define SERVICES_HPP

#include "Utilities.hpp"
#include <map>
#include <string>
#include <vector>
#include <array>

namespace Timetables {
	namespace Structures {
		class Service {
		private:
			bool IsAddedInDate(const DateTime& dateTime) const;
			bool IsRemovedInDate(const DateTime& dateTime) const;
			DateTime validSince;
			DateTime validUntil;
			std::array<bool, 7> operatingDays; // True at i-th position = the service is operating on this weekday.
			std::map<DateTime, bool> exceptions; // True = service added in this date. False = service removed in this date.
		public:
			Service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const DateTime& start, const DateTime& end);

			inline bool IsOperatingOnDay(std::size_t day) const { return operatingDays.at(day); }
			inline void AddExtraordinaryEvent(const DateTime& dateTime, bool type) { exceptions.insert(std::make_pair(dateTime, type)); }
			bool IsOperatingInDate(const DateTime& dateTime) const;
		};

		class Services {
		private:
			std::vector<Service> list;
		public:
			Services(std::istream&& calendar, std::istream&& calendarDates);

			inline Service& Get(std::size_t id) { return list.at(id); }
			inline const std::size_t Count() const { return list.size(); }
			inline Service& operator[](std::size_t id) { return list[id]; }
		};
	}
}

#endif // !SERVICES_HPP
