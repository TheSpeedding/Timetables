#pragma once

namespace Timetables {
	namespace Algorithms {
		class departure_board;
	}
	namespace Interop {
		ref class DataFeedManaged;
		// Wrapper from native class to the managed one.
		public ref class DepartureBoardManaged {
		private:
			Timetables::Algorithms::departure_board* native_departure_board_; // Pointer to C++ heap where the native object has it place.
		public:
			DepartureBoardManaged(Timetables::Interop::DataFeedManaged^ feed, Timetables::Client::DepartureBoardRequest^ req);
			~DepartureBoardManaged() { this->!DepartureBoardManaged(); }
			!DepartureBoardManaged() { delete native_departure_board_; }

			void ObtainDepartureBoard(); // Obtains departure board.

			Timetables::Client::DepartureBoardResponse^ ShowDepartureBoard(); // Constructs managed object as a reply to the request.
		};
	}
}