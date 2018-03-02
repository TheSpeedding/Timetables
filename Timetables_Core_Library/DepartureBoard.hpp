/*#ifndef DEPARTURE_BOARD_HPP
#define DEPARTURE_BOARD_HPP

#include <vector>
#include <string>
#include <memory>
#include "Trips.hpp"
#include "GtfsFeed.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"

namespace Timetables {
	namespace Structures {
		// We could use StopTime class, but we will define this one instead to have it more user friendly (and usable for some bigger app).
		class Departure {
		private:
			std::vector<std::unique_ptr<Timetables::Structures::StopTime>>::const_iterator tripBegin; // Start of the trip from departure stop.
			std::vector<std::unique_ptr<Timetables::Structures::StopTime>>::const_iterator tripEnd; // End of the trip in the end stop.
		public:
			Departure(const Timetables::Structures::StopTime& stopTime);

			inline const Timetables::Structures::Time& GetArrival() const { return tripBegin->get()->GetArrival(); }
			inline const Timetables::Structures::Time& GetDeparture() const { return tripBegin->get()->GetDeparture(); }
			inline const std::wstring& GetHeadsign() const { return tripBegin->get()->GetTrip().GetHeadsign(); }
			inline const std::string& GetLine() const { return tripBegin->get()->GetTrip().GetRouteInfo().GetShortName(); }
			inline const Timetables::Structures::Stop& GetStop() const { return tripBegin->get()->GetStop(); }
			const std::vector<Departure> GetFollowingStops() const;

			inline bool operator< (const Departure& other) const { return (**tripBegin).GetDeparture() < (**other.tripBegin).GetDeparture(); }
			inline bool operator> (const Departure& other) const { return (**tripBegin).GetDeparture() > (**other.tripBegin).GetDeparture(); }
			inline bool operator==(const Departure& other) const { return (**tripBegin).GetDeparture() == (**other.tripBegin).GetDeparture(); }
		};
	}

	namespace Algorithms {	
		using StationPtrObserver = const Timetables::Structures::Station*;

		class DepartureBoard {
		private:
			std::vector<Timetables::Structures::Departure> foundDepartures;
			StationPtrObserver station;
			const Timetables::Structures::DateTime& earliestDeparture;
			const std::size_t count;
		public:
			DepartureBoard(const Timetables::Structures::GtfsFeed& feed, const std::wstring& stationName, const Timetables::Structures::DateTime& earliestDeparture, const size_t count);

			void ObtainDepartureBoard();

			inline const std::vector<Timetables::Structures::Departure>& GetDepartureBoard() { return foundDepartures; }
		};
	}
}

#endif // !DEPARTURE_BOARD_HPP
*/