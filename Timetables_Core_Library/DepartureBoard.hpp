#ifndef DEPARTURE_BOARD_HPP
#define DEPARTURE_BOARD_HPP

#include <vector>
#include <string>
#include <memory>
#include "Trips.hpp"
#include "GtfsFeed.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"

namespace Timetables {
	namespace Algorithms {

		// We could use StopTime class, but we will define this one instead to have it more user friendly (and usable for some mobile app).
		class Departure {
		private:
			std::vector<std::unique_ptr<Timetables::Structures::StopTime>>::const_iterator tripBegin; // Start of the trip from departure stop.
			std::vector<std::unique_ptr<Timetables::Structures::StopTime>>::const_iterator tripEnd; // End of the trip in the end stop.
		public:
			Departure(const Timetables::Structures::StopTime& stopTime);

			inline const Timetables::Structures::Time& GetArrival() const { return tripBegin->get()->GetArrival(); }
			inline const Timetables::Structures::Time& GetDeparture() const { return tripBegin->get()->GetDeparture(); }
			inline const std::wstring& GetHeadsign() const { return tripBegin->get()->GetTrip().GetHeadsign(); }
			inline const std::string& GetLine() const { return tripBegin->get()->GetTrip().GetRoute().GetShortName(); }
			inline const Timetables::Structures::Stop& GetStop() const { return tripBegin->get()->GetStop(); }
			const std::vector<Departure> GetFollowingStops() const;

			inline bool operator< (const Departure& other) const { return (**tripBegin).GetDeparture() < (**other.tripBegin).GetDeparture(); }
			inline bool operator> (const Departure& other) const { return (**tripBegin).GetDeparture() > (**other.tripBegin).GetDeparture(); }
			inline bool operator==(const Departure& other) const { return (**tripBegin).GetDeparture() == (**other.tripBegin).GetDeparture(); }
		};

		std::vector<Departure> GetDepartureBoard(const Timetables::Structures::GtfsFeed& feed, const std::wstring& stationName, const Timetables::Structures::Datetime& datetime, const size_t count);
	}
}

#endif // !DEPARTURE_BOARD_HPP
