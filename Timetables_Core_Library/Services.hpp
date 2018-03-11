#ifndef SERVICES_HPP
#define SERVICES_HPP

#include "Utilities.hpp" // This may be removed later.

#include <map> // Data structure for extraordinary events.
#include <vector> // Data structure for services.
#include <array> // Data structure for operating days.

namespace Timetables {
	namespace Structures {
		// Class collecting information about service.
		class Service {
		private:
			bool IsAddedInDate(const DateTime& dateTime) const; // Determines whether the service is added in given date.
			bool IsRemovedInDate(const DateTime& dateTime) const; // Determines whether the service is removed in given date.
			DateTime validSince; // Date that the service is valid since.
			DateTime validUntil; // Date that the service is valid until.
			std::array<bool, 7> operatingDays; // True at i-th position = the service is operating on this weekday.
			std::map<DateTime, bool> exceptions; // True = service added in this date. False = service removed in this date.
		public:
			Service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const DateTime& start, const DateTime& end);

			inline bool IsOperatingOnDay(std::size_t day) const { return operatingDays.at(day); } // Determines whether the service is operating on given day.
			inline void AddExtraordinaryEvent(const DateTime& dateTime, bool type) { exceptions.insert(std::make_pair(dateTime, type)); } // Adds an exception.
			bool IsOperatingInDate(const DateTime& dateTime) const; // Determines whether the service is operating in given date.
		};

		// Class collecting information about collection of the services.
		class Services {
		private:
			std::vector<Service> list; // List of all the services, index of the item is also identificator for the service.
		public:
			Services(std::istream&& calendar, std::istream&& calendarDates);

			inline Service& Get(std::size_t id) { return list.at(id); } // Gets the service with given id.
			inline const Service& Get(std::size_t id) const { return list.at(id); } // Gets the service with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline Service& operator[](std::size_t id) { return list[id]; } // Gets the service with given id.
		};
	}
}

#endif // !SERVICES_HPP
