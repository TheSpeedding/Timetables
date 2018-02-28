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
			StopNotFoundException(const std::string& stopId) : stopName(std::wstring(stopId.cbegin(), stopId.cend())) {}

			inline const std::wstring& GetStopName() const { return stopName; }
		};

		class NoDeparturesFoundException : public std::exception {
		private:
			std::wstring stopName;
		public:
			NoDeparturesFoundException(const std::wstring& stopName) : stopName(stopName) {}

			inline const std::wstring& GetStopName() const { return stopName; }
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

	}
}

#endif // !EXCEPTIONS_HPP
