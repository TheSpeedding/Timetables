#ifndef UTILITIES_HPP
#define UTILITIES_HPP

#include "Exceptions.hpp"
#include <vector>
#include <string>

namespace Timetables {
	namespace Structures {
		class Time {
		private:
			// We will store time as seconds from midnight. 
			// Easier comparison (operators implementation), saves memory and afterall even time.
			int seconds;
		public:
			Time(const std::string& time);
			Time(int h, int m, int s) : seconds(s + m * 60 + h * 3600) { 
				if (s > 60 || m > 60) throw Timetables::Exceptions::InvalidDataFormatException("Invalid time format."); 
			}
			Time(int s) : seconds(s) {};
				
			static Time Now();

			inline int GetHours() const { return seconds / 3600; }
			inline int GetMinutes() const { return (seconds % 3600) / 60; }
			inline int GetSeconds() const { return (seconds % 3600) % 60; }

			inline std::string ToString() const { return std::to_string(GetHours()) + ':' + (GetMinutes() < 10 ? "0" : "") + std::to_string(GetMinutes()) + ':' + (GetSeconds() < 10 ? "0" : "") + std::to_string(GetSeconds()); }

			inline bool operator< (const Time& other) const { return seconds < other.seconds; }
			inline bool operator> (const Time& other) const { return seconds > other.seconds; }
			inline bool operator==(const Time& other) const { return seconds == other.seconds; }
			inline Time operator- (const Time& other) const { return Time(seconds - other.seconds); }
			inline Time operator+ (const Time& other) const { return Time(seconds + other.seconds); }
		};

		class Date {
		private:
			static int GetDaysInMonth(int month, int year);
			int day, month, year;
		public:
			Date(const std::string& date);
			Date(int day, int month, int year) : day(day), month(month), year(year) {
				if (month > 12 || day > 31) throw Timetables::Exceptions::InvalidDataFormatException("Invalid date format.");
			}

			static Date Now();

			inline int GetDay() const { return day; }
			inline int GetMonth() const { return month; }
			inline int GetYear() const { return year; }
			int GetDayInWeek() const;

			inline std::string ToString() const { return std::to_string(year) + (month < 10 ? "0" : "") + std::to_string(month) + (day < 10 ? "0" : "") + std::to_string(day); }

			bool operator< (const Date& other) const;
			bool operator> (const Date& other) const;
			inline bool operator== (const Date& other) const { return day == other.day && month == other.month && year == other.year; }
			Date& operator++(); // Adds one day.
		};

		class Datetime {
		private:
			Date date;
			Time time;
		public:
			Datetime(const Date& date, const Time& time) : date(date), time(time) {}
			Datetime(int day, int month, int year, int h, int m, int s) :
				date(day, month, year), time(h, m, s) {}

			static Datetime Now() { return Datetime(Date::Now(), Time::Now()); }

			inline const Time& GetTime() const { return time; }
			inline const Date& GetDate() const { return date; }

			inline bool operator< (const Datetime& other) const { return other.date == date ? time < other.time : date < other.date; }
			inline bool operator> (const Datetime& other) const { return other.date == date ? time > other.time : date > other.date; }
			inline bool operator== (const Datetime& other) const { return other.date == date && other.time == time; }
		};

		class GpsCoords {
		private:
			inline static double Deg2Rad(double deg) { return (deg * 3.141592653589793 / 180.0); }
			const double latitude, longitude;
		public:
			GpsCoords(double latitude, double longitude) : latitude(latitude), longitude(longitude) {}

			// Returns walking time in seconds between two points. Assuming average walking speed 0.5 m/s.
			static int GetWalkingTime(const GpsCoords& A, const GpsCoords& B);

			inline double GetLatitude() const { return latitude; }
			inline double GetLongitude() const { return longitude; }

			inline bool operator==(const GpsCoords& other) const { return latitude == other.latitude && longitude == other.longitude; }
		};

	}
}

#endif // !UTILITIES_HPP
