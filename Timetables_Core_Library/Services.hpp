#ifndef SERVICES_HPP
#define SERVICES_HPP

#include "Utilities.hpp"
#include "Exceptions.hpp"
#include <map>
#include <string>
#include <vector>
#include <array>

namespace Timetables {
	namespace Structures {
		class Service {
		private:
			bool IsAddedInDate(const Date& date) const;
			bool IsRemovedInDate(const Date& date) const;
			Date validSince;
			Date validUntil;
			std::array<bool, 7> operatingDays; // True at i-th position = the service is operating on this weekday.
			std::map<Date, bool> exceptions; // True = service added in this date. False = service removed in this date.
		public:
			Service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const Date& start, const Date& end);

			inline bool IsOperatingOnDay(int day) const { return operatingDays.at(day - 1); }
			inline void AddExtraordinaryEvent(const Date& date, bool type) { exceptions.insert(std::make_pair(date, type)); }
			bool IsOperatingInDate(const Date& date) const;
		};

		class Services {
		private:
			std::vector<Service> list;
		public:
			Services(std::istream&& calendar, std::istream&& calendar_dates);

			inline const Service& GetService(std::size_t id) const {
				if (id > list.size()) throw Timetables::Exceptions::ServiceNotFoundException(id);
				else return list[id - 1];
			}
		};
	}
}

#endif // !SERVICES_HPP
