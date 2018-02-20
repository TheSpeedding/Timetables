#ifndef EXCEPTIONS_HPP
#define EXCEPTIONS_HPP

#include <string>
#include <exception>

namespace Timetables {
	namespace Structures {
		class Station;
	}

	namespace Exceptions {

		class StopNotFoundException : public std::exception {
		private:
			std::wstring stopName;
		public:
			StopNotFoundException(const std::wstring& stopName) : stopName(stopName) {}
			StopNotFoundException(const std::string& stopId) : stopName(std::wstring(stopName.cbegin(), stopName.cend())) {}

			inline const std::wstring& GetStopName() const { return stopName; }
		};

		class NoDeparturesFoundException : public std::exception {
		private:
			std::wstring stopName;
		public:
			NoDeparturesFoundException(const std::wstring& stopName) : stopName(stopName) {}

			inline const std::wstring& GetStopName() const { return stopName; }
		};

		class ServiceNotFoundException : public std::exception {
		private:
			std::size_t serviceId;
		public:
			ServiceNotFoundException(const std::size_t id) : serviceId(id) {}

			inline const std::size_t GetServiceId() const { return serviceId; }
		};

		class TripNotFoundException : public std::exception {
		private:
			std::size_t tripId;
		public:
			TripNotFoundException(const std::size_t id) : tripId(id) {}

			inline const std::size_t GetTripId() const { return tripId; }
		};

		class RouteNotFoundException : public std::exception {
		private:
			std::string routeId;
		public:
			RouteNotFoundException(const std::string& id) : routeId(id) {}

			inline const std::string& GetRouteId() const { return routeId; }
		};

		class ShapesSequenceNotFoundException : public std::exception {
		private:
			std::size_t shapeId;
		public:
			ShapesSequenceNotFoundException(const std::size_t id) : shapeId(id) {}

			inline const std::size_t GetShapeId() const { return shapeId; }
		};

		class InvalidDataFormatException : public std::invalid_argument {
		public:
			InvalidDataFormatException(const std::string& message) : std::invalid_argument(message) {};
		};

		class JourneyNotFoundException : public std::exception {
		private:
			const std::wstring& A;
			const std::wstring& B;
		public:
			JourneyNotFoundException(const std::wstring& A, const std::wstring& B) : A(A), B(B) {}

			inline const std::pair<std::wstring, std::wstring> GetStations() const { return std::make_pair(A, B); }
		};

		class FootpathNotFoundException : public std::exception {
		private:
			const std::wstring& A;
			const std::wstring& B;
		public:
			FootpathNotFoundException(const std::wstring& A, const std::wstring& B) : A(A), B(B) {}

			inline const std::pair<std::wstring, std::wstring> GetStops() const { return std::make_pair(A, B); }
		};

	}
}

#endif // !EXCEPTIONS_HPP
