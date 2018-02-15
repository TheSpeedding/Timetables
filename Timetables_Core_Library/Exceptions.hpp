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
			std::string stopName;
		public:
			StopNotFoundException(const std::wstring& stopName) : stopName(std::string(stopName.cbegin(), stopName.cend())) {}
			StopNotFoundException(const std::string& stopId) : stopName(stopId) {}

			inline virtual const char* what() const override {
				return std::string("Stop " + std::string(stopName.cbegin(), stopName.cend()) + " not found.").c_str();
			}
		};

		class ServiceNotFoundException : public std::exception {
		private:
			std::size_t serviceId;
		public:
			ServiceNotFoundException(const std::size_t id) : serviceId(id) {}

			inline virtual const char* what() const override {
				return std::string("Service with ID " + std::to_string(serviceId) + " not found.").c_str();
			}
		};

		class TripNotFoundException : public std::exception {
		private:
			std::size_t tripId;
		public:
			TripNotFoundException(const std::size_t id) : tripId(id) {}

			inline virtual const char* what() const override {
				return std::string("Service with ID " + std::to_string(tripId) + " not found.").c_str();
			}
		};

		class RouteNotFoundException : public std::exception {
		private:
			std::string routeId;
		public:
			RouteNotFoundException(const std::string& id) : routeId(id) {}

			inline virtual const char* what() const override {
				return std::string("Route with ID " + routeId + " not found.").c_str();
			}
		};

		class ShapesSequenceNotFoundException : public std::exception {
		private:
			std::size_t shapeId;
		public:
			ShapesSequenceNotFoundException(const std::size_t id) : shapeId(id) {}

			inline virtual const char* what() const override {
				return std::string("Shape with ID " + std::to_string(shapeId) + " not found.").c_str();
			}
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

			inline virtual const char* what() const override {
				std::string("No route between " + std::string(A.cbegin(), A.cend()) + " and " + std::string(B.cbegin(), B.cend()) + " found.").c_str();
			}
		};

	}
}

#endif // !EXCEPTIONS_HPP
