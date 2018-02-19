#include "CppUnitTest.h"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include <ctime>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

namespace Timetables {
	namespace tests {
		TEST_CLASS(TimeTests) {
public:
	TEST_METHOD(TimeOperatorLowerThanTest1) {
		auto time1 = Timetables::Structures::Time(8, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsTrue(time1 < time2);
	}
	TEST_METHOD(TimeOperatorLowerThanTest2) {
		auto time1 = Timetables::Structures::Time(9, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsTrue(time1 < time2);
	}
	TEST_METHOD(TimeOperatorLowerThanTest3) {
		auto time1 = Timetables::Structures::Time(9, 45, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsTrue(time1 < time2);
	}
	TEST_METHOD(TimeOperatorLowerThanTest4) {
		auto time1 = Timetables::Structures::Time(8, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsFalse(time2 < time1);
	}
	TEST_METHOD(TimeOperatorLowerThanTest5) {
		auto time1 = Timetables::Structures::Time(9, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsFalse(time2 < time1);
	}
	TEST_METHOD(TimeOperatorLowerThanTest6) {
		auto time1 = Timetables::Structures::Time(9, 45, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsFalse(time2 < time1);
	}
	TEST_METHOD(TimeOperatorGreaterThanTest1) {
		auto time1 = Timetables::Structures::Time(8, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsTrue(time2 > time1);
	}
	TEST_METHOD(TimeOperatorGreaterThanTest2) {
		auto time1 = Timetables::Structures::Time(9, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsTrue(time2 > time1);
	}
	TEST_METHOD(TimeOperatorGreaterThanTest3) {
		auto time1 = Timetables::Structures::Time(9, 45, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsTrue(time2 > time1);
	}
	TEST_METHOD(TimeOperatorGreaterThanTest4) {
		auto time1 = Timetables::Structures::Time(8, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsFalse(time1 > time2);
	}
	TEST_METHOD(TimeOperatorGreaterThanTest5) {
		auto time1 = Timetables::Structures::Time(9, 12, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsFalse(time1 > time2);
	}
	TEST_METHOD(TimeOperatorGreaterThanTest6) {
		auto time1 = Timetables::Structures::Time(9, 45, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 57);
		Assert::IsFalse(time1 > time2);
	}
	TEST_METHOD(TimeOperatorEqualityTest1) {
		auto time1 = Timetables::Structures::Time(9, 45, 15);
		auto time2 = Timetables::Structures::Time(9, 45, 15);
		Assert::IsTrue(time1 == time2);
	}
	TEST_METHOD(TimeOperatorEqualityTest2) {
		auto time1 = Timetables::Structures::Time(9, 45, 15);
		auto time2 = Timetables::Structures::Time(9, 40, 15);
		Assert::IsFalse(time1 == time2);
	}
	TEST_METHOD(TimeNowTest) {
		time_t t = std::time(0);
		struct tm now; localtime_s(&now, &t);
		auto time = Timetables::Structures::Time::Now();
		Assert::IsTrue(time.GetHours() == now.tm_hour);
		Assert::IsTrue(time.GetMinutes() == now.tm_min);
		Assert::IsTrue(time.GetSeconds() == now.tm_sec);
	}
	TEST_METHOD(TimeGetTest) {
		auto time = Timetables::Structures::Time(7, 02, 14);
		Assert::AreEqual(string("7:02:14"), time.ToString());
	}
	TEST_METHOD(TimeParseTest1) {
		auto Time = Timetables::Structures::Time("7:02:14");
		Assert::IsTrue(Time.GetHours() == 7);
		Assert::IsTrue(Time.GetMinutes() == 2);
		Assert::IsTrue(Time.GetSeconds() == 14);
	}
	TEST_METHOD(TimeParseTest2) {
		auto time = Timetables::Structures::Time("17:12:04");
		Assert::IsTrue(time.GetHours() == 17);
		Assert::IsTrue(time.GetMinutes() == 12);
		Assert::IsTrue(time.GetSeconds() == 04);
	}
		};
		TEST_CLASS(DateTests) {
public:
	TEST_METHOD(DateOperatorLowerThanTest1) {
		auto date1 = Timetables::Structures::Date(29, 6, 1945);
		auto date2 = Timetables::Structures::Date(28, 5, 1946);
		Assert::IsTrue(date1 < date2);
	}
	TEST_METHOD(DateOperatorLowerThanTest2) {
		auto date1 = Timetables::Structures::Date(29, 4, 1996);
		auto date2 = Timetables::Structures::Date(28, 5, 1996);
		Assert::IsTrue(date1 < date2);
	}
	TEST_METHOD(DateOperatorLowerThanTest3) {
		auto date1 = Timetables::Structures::Date(29, 7, 1996);
		auto date2 = Timetables::Structures::Date(30, 7, 1996);
		Assert::IsTrue(date1 < date2);
	}
	TEST_METHOD(DateOperatorLowerThanTest4) {
		auto date1 = Timetables::Structures::Date(29, 6, 1945);
		auto date2 = Timetables::Structures::Date(28, 5, 1946);
		Assert::IsFalse(date2 < date1);
	}
	TEST_METHOD(DateOperatorLowerThanTest5) {
		auto date1 = Timetables::Structures::Date(29, 4, 1996);
		auto date2 = Timetables::Structures::Date(28, 5, 1996);
		Assert::IsFalse(date2 < date1);
	}
	TEST_METHOD(DateOperatorLowerThanTest6) {
		auto date1 = Timetables::Structures::Date(29, 7, 1996);
		auto date2 = Timetables::Structures::Date(30, 7, 1996);
		Assert::IsFalse(date2 < date1);
	}
	TEST_METHOD(DateOperatorGreaterThanTest1) {
		auto date1 = Timetables::Structures::Date(29, 6, 1945);
		auto date2 = Timetables::Structures::Date(28, 5, 1946);
		Assert::IsTrue(date2 > date1);
	}
	TEST_METHOD(DateOperatorGreaterThanTest2) {
		auto date1 = Timetables::Structures::Date(29, 4, 1996);
		auto date2 = Timetables::Structures::Date(28, 5, 1996);
		Assert::IsTrue(date2 > date1);
	}
	TEST_METHOD(DateOperatorGreaterThanTest3) {
		auto date1 = Timetables::Structures::Date(29, 7, 1996);
		auto date2 = Timetables::Structures::Date(30, 7, 1996);
		Assert::IsTrue(date2 > date1);
	}
	TEST_METHOD(DateOperatorGreaterThanTest4) {
		auto date1 = Timetables::Structures::Date(29, 6, 1945);
		auto date2 = Timetables::Structures::Date(28, 5, 1946);
		Assert::IsFalse(date1 > date2);
	}
	TEST_METHOD(DateOperatorGreaterThanTest5) {
		auto date1 = Timetables::Structures::Date(29, 4, 1996);
		auto date2 = Timetables::Structures::Date(28, 5, 1996);
		Assert::IsFalse(date1 > date2);
	}
	TEST_METHOD(DateOperatorGreaterThanTest6) {
		auto date1 = Timetables::Structures::Date(29, 7, 1996);
		auto date2 = Timetables::Structures::Date(30, 7, 1996);
		Assert::IsFalse(date1 > date2);
	}
	TEST_METHOD(DateOperatorEqualityTest1) {
		auto date1 = Timetables::Structures::Date(29, 7, 1996);
		auto date2 = Timetables::Structures::Date(29, 7, 1996);
		Assert::IsTrue(date1 == date2);
	}
	TEST_METHOD(DateOperatorEqualityTest2) {
		auto date1 = Timetables::Structures::Date(29, 7, 1996);
		auto date2 = Timetables::Structures::Date(8, 1, 1997);
		Assert::IsFalse(date1 == date2);
	}
	TEST_METHOD(DateNowTest) {
		time_t t = std::time(0);
		struct tm now; localtime_s(&now, &t);
		auto date = Timetables::Structures::Date::Now();
		Assert::IsTrue(date.GetDay() == now.tm_mday);
		Assert::IsTrue(date.GetDayInWeek() == now.tm_wday);
		Assert::IsTrue(date.GetMonth() == now.tm_mon + 1);
		Assert::IsTrue(date.GetYear() == now.tm_year + 1900);
	}
	TEST_METHOD(DateGetTest) {
		auto date = Timetables::Structures::Date(29, 7, 1996);
		Assert::AreEqual(string("19960729"), date.ToString());
	}
	TEST_METHOD(DateParseTest1) {
		auto date = Timetables::Structures::Date("19960729");
		Assert::IsTrue(date.GetDay() == 29);
		Assert::IsTrue(date.GetDayInWeek() == 1);
		Assert::IsTrue(date.GetMonth() == 7);
		Assert::IsTrue(date.GetYear() == 1996);
	}
	TEST_METHOD(DateParseTest2) {
		auto date = Timetables::Structures::Date("19970108");
		Assert::IsTrue(date.GetDay() == 8);
		Assert::IsTrue(date.GetDayInWeek() == 3);
		Assert::IsTrue(date.GetMonth() == 1);
		Assert::IsTrue(date.GetYear() == 1997);
	}
		};
		TEST_CLASS(DatetimeTests) {
public:
	TEST_METHOD(DatetimeOperatorLowerThanTest1) {
		auto datetime1 = Timetables::Structures::Datetime(Date(29, 7, 1996), Timetables::Structures::Time(12, 15, 13));
		auto datetime2 = Timetables::Structures::Datetime(Date(29, 7, 1997), Timetables::Structures::Time(12, 15, 13));
		Assert::IsTrue(datetime1 < datetime2);
	}
	TEST_METHOD(DatetimeOperatorLowerThanTest2) {
		auto datetime1 = Timetables::Structures::Datetime(Date(29, 7, 1996), Timetables::Structures::Time(12, 15, 13));
		auto datetime2 = Timetables::Structures::Datetime(Date(29, 7, 1996), Timetables::Structures::Time(13, 15, 13));
		Assert::IsTrue(datetime1 < datetime2);
	}
	TEST_METHOD(DatetimeOperatorLowerThanTest3) {
		auto datetime1 = Timetables::Structures::Datetime(Date(10, 2, 2018), Timetables::Structures::Time(23, 59, 59));
		auto datetime2 = Timetables::Structures::Datetime(Date(11, 2, 2018), Timetables::Structures::Time(0, 0, 1));
		Assert::IsTrue(datetime1 < datetime2);
	}
	TEST_METHOD(DatetimeOperatorGreaterThanTest1) {
		auto datetime1 = Timetables::Structures::Datetime(Date(29, 7, 1996), Timetables::Structures::Time(12, 15, 13));
		auto datetime2 = Timetables::Structures::Datetime(Date(29, 7, 1997), Timetables::Structures::Time(12, 15, 13));
		Assert::IsTrue(datetime2 > datetime1);
	}
	TEST_METHOD(DatetimeOperatorGreaterThanTest2) {
		auto datetime1 = Timetables::Structures::Datetime(Date(29, 7, 1996), Timetables::Structures::Time(12, 15, 13));
		auto datetime2 = Timetables::Structures::Datetime(Date(29, 7, 1996), Timetables::Structures::Time(13, 15, 13));
		Assert::IsTrue(datetime2 > datetime1);
	}
	TEST_METHOD(DatetimeOperatorGreaterThanTest3) {
		auto datetime1 = Timetables::Structures::Datetime(Date(10, 2, 2018), Timetables::Structures::Time(23, 59, 59));
		auto datetime2 = Timetables::Structures::Datetime(Date(11, 2, 2018), Timetables::Structures::Time(0, 0, 1));
		Assert::IsTrue(datetime2 > datetime1);
	}
	TEST_METHOD(DatetimeOperatorEqualityTest) {
		auto datetime1 = Timetables::Structures::Datetime(Date(10, 2, 2018), Timetables::Structures::Time(23, 59, 59));
		auto datetime2 = Timetables::Structures::Datetime(Date(10, 2, 2018), Timetables::Structures::Time(23, 59, 59));
		Assert::IsTrue(datetime2 == datetime1);
	}
		};
}
}