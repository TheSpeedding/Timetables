#include "DataFeedManaged.hpp"
#include "../Timetables_Core_Library/Algorithms/router_raptor.hpp"
#include "../Timetables_Core_Library/Structures/date_time.hpp"
#include "RouterManaged.hpp"

Timetables::Client::RouterResponse^ Timetables::Interop::RouterManaged::ShowJourneys() {
	System::Collections::Generic::List<Timetables::Client::Journey^>^ journeys = gcnew System::Collections::Generic::List<Timetables::Client::Journey^>();
	auto journeys_native = native_router_->show_journeys();

	for (auto&& journey : journeys_native) {

		System::Collections::Generic::List<Timetables::Client::JourneySegment^>^ jss = gcnew System::Collections::Generic::List<Timetables::Client::JourneySegment^>();

		for (auto&& js : journey.journey_segments()) {
			
			if (js->trip() == nullptr) // Footpath segment.
				jss->Add(gcnew Timetables::Client::FootpathSegment(js->source_stop().id(), js->target_stop().id(), js->departure_from_source().timestamp(), js->arrival_at_target().timestamp()));

			else { // Trip segment.

				auto intStops = gcnew System::Collections::Generic::List<System::Collections::Generic::KeyValuePair<unsigned long long, int>>();

				for (auto&& stop : js->intermediate_stops()) {
					unsigned long long timestamp = stop.first + js->departure_from_source().timestamp(); // Recast from 32 bit to 64 bit if necessary. Timestamp is defined as time_t.
					intStops->Add(System::Collections::Generic::KeyValuePair<unsigned long long, int>(timestamp, stop.second->id()));
				}

				unsigned long long departure_timestamp = js->departure_from_source().timestamp(); // Recast from 32 bit to 64 bit if necessary. Timestamp is defined as time_t.
				unsigned long long arrival_timestamp = js->arrival_at_target().timestamp(); // Recast from 32 bit to 64 bit if necessary. Timestamp is defined as time_t.

				jss->Add(gcnew Timetables::Client::TripSegment(js->source_stop().id(), js->target_stop().id(), js->outdated(), gcnew System::String(js->trip()->route().headsign().data()), 
					gcnew System::String(js->trip()->route().info().short_name().data()), gcnew System::String(js->trip()->route().info().long_name().data()), 
					js->trip()->route().info().color(), js->trip()->route().info().text_color(), js->trip()->route().info().type(), departure_timestamp, arrival_timestamp, intStops));

			}
		}

		journeys->Add(gcnew Timetables::Client::Journey(jss));

	}

	return gcnew Timetables::Client::RouterResponse(journeys);
}

Timetables::Interop::RouterManaged::RouterManaged(Timetables::Interop::DataFeedManaged^ feed, Timetables::Client::RouterRequest^ req) {
	if (req->SearchByCount)
		native_router_ = new Timetables::Algorithms::router_raptor(feed->Get(), req->SourceStationID, req->TargetStationID, Timetables::Structures::date_time(req->EarliestDepartureDateTime), req->Count, req->MaxTransfers, req->TransfersDurationCoefficient, static_cast<Timetables::Structures::mean_of_transport>(req->MeansOfTransport));
	else
		native_router_ = new Timetables::Algorithms::router_raptor(feed->Get(), req->SourceStationID, req->TargetStationID, Timetables::Structures::date_time(req->EarliestDepartureDateTime), Timetables::Structures::date_time(req->MaximalArrivalDateTime), req->MaxTransfers, req->TransfersDurationCoefficient, static_cast<Timetables::Structures::mean_of_transport>(req->MeansOfTransport));
}

void Timetables::Interop::RouterManaged::ObtainJourneys() {
	native_router_->obtain_journeys();
}