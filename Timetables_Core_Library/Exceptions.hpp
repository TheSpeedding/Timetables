#ifndef EXCEPTIONS_HPP
#define EXCEPTIONS_HPP

#include <string>
#include <exception>

namespace Timetables {
	namespace Exceptions {

		// Exception which is thrown iff station with given name was not found.
		class StationNotFoundException : public std::exception {
		private:
			std::wstring stopName;
		public:
			StationNotFoundException(const std::wstring& stopName) : stopName(stopName) {}
			StationNotFoundException(const std::string& stopId) : stopName(std::wstring(stopId.cbegin(), stopId.cend())) {}

			inline const std::wstring& GetStationName() const { return stopName; }
		};

		// Exception which is thrown iff no departures from given station were found.
		class NoDeparturesFoundException : public std::exception {
		private:
			std::wstring stopName;
		public:
			NoDeparturesFoundException(const std::wstring& stopName) : stopName(stopName) {}

			inline const std::wstring& GetStationName() const { return stopName; } // Gets name of the station.
		};
		
		// Exception is thrown iff no journeys between two stations were found. Increasing number of transfers might solve this problem.
		class JourneyNotFoundException : public std::exception {
		private:
			const std::wstring& A;
			const std::wstring& B;
		public:
			JourneyNotFoundException(const std::wstring& A, const std::wstring& B) : A(A), B(B) {}

			inline const std::pair<std::wstring, std::wstring> GetStations() const { return std::make_pair(A, B); } // Gets names of stations.
		};

	}
}

#endif // !EXCEPTIONS_HPP
