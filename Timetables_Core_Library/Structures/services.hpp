#ifndef SERVICES_HPP
#define SERVICES_HPP

#include "../Structures/date_time.hpp" // Dealing with date time.
#include <map> // Data structure for extraordinary events.
#include <vector> // Data structure for services.
#include <array> // Data structure for operating days.

namespace Timetables {
	namespace Structures {

		// Days in week.
		enum days { monday = 1, tuesday = 2, wednesday = 4, thursday = 8, friday = 16, saturday = 32, sunday = 64 };

		inline days operator|(days a, days b) { return static_cast<days>(static_cast<int>(a) | static_cast<int>(b)); }
		
		// Class collecting information about service.
		class service {
		private:
			bool is_added_in_date(const date_time& date_time) const; // Determines whether the service is added in given date.
			bool is_removed_in_date(const date_time& date_time) const; // Determines whether the service is removed in given date.

			date_time valid_since_; // Date that the service is valid since.
			date_time valid_until_; // Date that the service is valid until.
			days operating_days_; // True at i-th bit = the service is operating on this weekday.
			std::map<date_time, bool> exceptions_; // True = service added in this date. False = service removed in this date.
		public:
			service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const date_time& start, const date_time& end);

			inline bool is_operating_on_day(std::size_t day) const { return (operating_days_ >> day) & 0x01 == 1; } // Determines whether the service is operating on given day.
			inline void add_extraordinary_event(const date_time& date_time, bool type) { exceptions_.insert(std::make_pair(date_time, type)); } // Adds an exception.
			bool is_operating_in_date(const date_time& date_time) const; // Determines whether the service is operating in given date.
		};

		// Class collecting information about collection of the services.
		class services {
		private:
			std::vector<service> list; // List of all the services, index of the item is also identificator for the service.
		public:
			services(std::istream&& calendar, std::istream&& calendar_dates);

			inline service& at(std::size_t id) { return list.at(id); } // Gets the service with given id.
			inline const service& at(std::size_t id) const { return list.at(id); } // Gets the service with given id.
			inline const std::size_t size() const { return list.size(); } // Gets count of items in the collection.
			inline service& operator[](std::size_t id) { return list[id]; } // Gets the service with given id.
		};
	}
}

#endif // !SERVICES_HPP
