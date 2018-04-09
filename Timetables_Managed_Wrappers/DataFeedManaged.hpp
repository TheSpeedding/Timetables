#pragma once

#include <string>
#include <msclr\marshal_cppstd.h>
#include "../Timetables_Core_Library/Structures/data_feed.hpp"

namespace Timetables {
	namespace Interop {
		public ref class DataFeedManaged {
		private:
			Timetables::Structures::data_feed* native_data_feed_;
		public:
			DataFeedManaged(System::String^ path) {
				msclr::interop::marshal_context context;
				native_data_feed_ = new Timetables::Structures::data_feed(context.marshal_as<std::string>(path));
			}
			DataFeedManaged() { native_data_feed_ = new Timetables::Structures::data_feed(); }

			~DataFeedManaged() { this->!DataFeedManaged(); }
			!DataFeedManaged() { delete native_data_feed_; };

			inline const Timetables::Structures::data_feed& Get() { return *native_data_feed_; }
		};
	}
}