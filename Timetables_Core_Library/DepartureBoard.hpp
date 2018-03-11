#ifndef DEPARTURE_BOARD_HPP
#define DEPARTURE_BOARD_HPP

#include "DataFeed.hpp"
#include "Trips.hpp"
#include "StopTime.hpp"

namespace Timetables {
	namespace Structures {
		// We could use StopTime class, but we will define this one instead to have it more user friendly.
		class Departure {
		private:
			std::time_t departure;
			std::vector<StopTime>::const_iterator tripBegin; // Start of the trip from departure stop.
			std::vector<StopTime>::const_iterator tripEnd; // End of the trip at the end stop.
		public:
			Departure(const StopTime& stopTime, std::time_t departure);

			inline std::time_t DepartureTime() const { return departure; }
			inline const std::wstring& Headsign() const { return tripBegin->Trip().Route().Headsign(); }
			inline const RouteInfo& Line() const { return tripBegin->Trip().Route().Info(); }
			inline const Stop& Stop() const { return tripBegin->Stop(); }
			const std::vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> FollowingStops() const;

			inline bool operator< (const Departure& other) const { return departure < other.departure; }
			inline bool operator> (const Departure& other) const { return departure > other.departure; }
			inline bool operator==(const Departure& other) const { return departure == other.departure; }
		};
	}

	namespace Algorithms {	
		class DepartureBoard {
		private:
			std::vector<Timetables::Structures::Departure> foundDepartures;
			const Timetables::Structures::Station& station;
			const std::time_t earliestDeparture;
			const std::size_t count;
		public:
			DepartureBoard(const Timetables::Structures::DataFeed& feed, const std::wstring& stationName, std::time_t earliestDeparture,
				const size_t count) : earliestDeparture(earliestDeparture), count(count), station(feed.Stations().Find(stationName)) {}

			void ObtainDepartureBoard();

			inline const std::vector<Timetables::Structures::Departure>& ShowDepartureBoard() { return foundDepartures; }
		};
	}
}

#endif // !DEPARTURE_BOARD_HPP