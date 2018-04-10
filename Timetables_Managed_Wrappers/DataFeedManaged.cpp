#include <string>
#include <msclr/marshal_cppstd.h>
#include "../Timetables_Core_Library/Structures/stop_time.hpp" // This must be here, otherwise it would not compile (due to forward declaration only).
#include "../Timetables_Core_Library/Structures/data_feed.hpp"
#include "DataFeedManaged.hpp"

Timetables::Interop::DataFeedManaged::DataFeedManaged (System::String^ path) {
	msclr::interop::marshal_context context;
	native_data_feed_ = new Timetables::Structures::data_feed(context.marshal_as<std::string>(path));
}

Timetables::Interop::DataFeedManaged::DataFeedManaged() { 
	native_data_feed_ = new Timetables::Structures::data_feed();
}