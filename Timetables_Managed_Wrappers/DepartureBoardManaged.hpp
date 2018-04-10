#pragma once

namespace Timetables {
	namespace Algorithms {
		class departure_board;
	}
	namespace Interop {
		ref class DataFeedManaged;
		public ref class DepartureBoardManaged {
		private:
			Timetables::Algorithms::departure_board* native_departure_board_;
		public:
			DepartureBoardManaged(Timetables::Interop::DataFeedManaged^ feed, Timetables::Client::DepartureBoardRequest^ req);
			~DepartureBoardManaged() { this->!DepartureBoardManaged(); }
			!DepartureBoardManaged() { delete native_departure_board_; }

			void ObtainDepartureBoard();

			Timetables::Client::DepartureBoardReply^ ShowDepartureBoard();
		};
	}
}