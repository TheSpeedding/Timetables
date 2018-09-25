#pragma once

namespace Timetables {
	namespace Algorithms {
		class router;
	}
	namespace Interop {
		ref class DataFeedManaged;
		// Wrapper from native class to the managed one.
		public ref class RouterManaged {
		private:
			Timetables::Algorithms::router_raptor* native_router_; // Pointer to C++ heap where the native object has it place.
		public:
			RouterManaged(Timetables::Interop::DataFeedManaged^ feed, Timetables::Client::RouterRequest^ req);
			~RouterManaged() { this->!RouterManaged(); }
			!RouterManaged() { delete native_router_; }

			void ObtainJourneys(); // Obtains journeys.

			Timetables::Client::RouterResponse^ ShowJourneys(); // Constructs managed object as a reply to the request.
		};
	}
}