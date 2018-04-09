#pragma once

#include "../Timetables_Core_Library/Algorithms/departure_board.hpp"
#include "../Timetables_Core_Library/Structures/date_time.hpp"
#include "DataFeedManaged.hpp""

namespace Timetables {
	namespace Interop {
		public ref class DepartureBoardManaged {
		private:
			Timetables::Algorithms::departure_board* native_departure_board_;
		public:
			DepartureBoardManaged(Timetables::Interop::DataFeedManaged^ feed, Timetables::Client::DepartureBoardRequest^ req) {
				native_departure_board_ = new Timetables::Algorithms::departure_board(feed->Get(), req->StopID, Timetables::Structures::date_time(req->DepartureTime), req->Count, req->TrueIfStation);
			}
			~DepartureBoardManaged() { this->!DepartureBoardManaged(); }
			!DepartureBoardManaged() { delete native_departure_board_; }

			inline void ObtainDepartureBoard() {
				native_departure_board_->obtain_departure_board();
			}

			Timetables::Client::DepartureBoardReply^ ShowDepartureBoard();
		};
	}
}