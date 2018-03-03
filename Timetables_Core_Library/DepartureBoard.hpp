#ifndef DEPARTURE_BOARD_HPP
#define DEPARTURE_BOARD_HPP

#include <vector>
#include <string>
#include <memory>
#include "Trips.hpp"
#include "DataFeed.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"

namespace Timetables {
	namespace Structures {
		// We could use StopTime class, but we will define this one instead to have it more user friendly.
		class Departure {
		private:
			DateTime departure;
			std::vector<StopTime>::const_iterator tripBegin; // Start of the trip from departure stop.
			std::vector<StopTime>::const_iterator tripEnd; // End of the trip at the end stop.
		public:
			Departure(const StopTime& stopTime, const DateTime& departure);

			inline const DateTime& ArrivalTime() const { return departure.AddSeconds(DateTime::Difference(tripBegin->Arrival(), tripBegin->Departure())); }
			inline const DateTime& DepartureTime() const { return departure; }
			inline const std::wstring& Headsign() const { return tripBegin->Trip().Headsign(); }
			inline const RouteInfo& Line() const { return tripBegin->Trip().RouteInfo(); }
			inline const Stop& Stop() const { return tripBegin->Stop(); }
			const std::vector<Departure> FollowingStops() const;

			inline bool operator< (const Departure& other) const { return tripBegin->Departure() < other.tripBegin->Departure(); }
			inline bool operator> (const Departure& other) const { return tripBegin->Departure() > other.tripBegin->Departure(); }
			inline bool operator==(const Departure& other) const { return tripBegin->Departure() == other.tripBegin->Departure(); }
		};
	}

	namespace Algorithms {	
		class DepartureBoard {
		private:
			std::vector<Timetables::Structures::Departure> foundDepartures;
			const Timetables::Structures::Station& station;
			const Timetables::Structures::DateTime& earliestDeparture;
			const std::size_t count;
		public:
			DepartureBoard(const Timetables::Structures::DataFeed& feed, const std::wstring& stationName, const Timetables::Structures::DateTime& earliestDeparture, 
				const size_t count) : earliestDeparture(earliestDeparture), count(count), station(feed.Stations().Find(stationName)) {}

			void ObtainDepartureBoard();

			inline const std::vector<Timetables::Structures::Departure>& ShowDepartureBoard() { return foundDepartures; }
		};
	}
}

#endif // !DEPARTURE_BOARD_HPP