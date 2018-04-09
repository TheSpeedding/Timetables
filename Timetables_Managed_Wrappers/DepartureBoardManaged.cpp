#include "DepartureBoardManaged.hpp"

Timetables::Client::DepartureBoardReply^ Timetables::Interop::DepartureBoardManaged::ShowDepartureBoard() {
	System::Collections::Generic::List<Timetables::Client::Departure^>^ departures = gcnew System::Collections::Generic::List<Timetables::Client::Departure^>();
	auto departures_native = native_departure_board_->show_departure_board();
	for (auto&& departure : departures_native) {

		auto intStops = gcnew System::Collections::Generic::List<System::Collections::Generic::KeyValuePair<unsigned long long, unsigned int>>();

		for (auto&& int_stop : departure.following_stops())
			intStops->Add(System::Collections::Generic::KeyValuePair<unsigned long long, unsigned int>(int_stop.first, int_stop.second->id()));

		departures->Add(gcnew Timetables::Client::Departure(departure.stop().id(), departure.outdated(), gcnew System::String(departure.headsign().data()),
			gcnew System::String(departure.line().short_name().data()), gcnew System::String(departure.line().long_name().data()), departure.line().color(), departure.line().type(), intStops));
	}
	return gcnew Timetables::Client::DepartureBoardReply(departures);
}