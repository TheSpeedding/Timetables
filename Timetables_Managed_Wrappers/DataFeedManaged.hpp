#pragma once

namespace Timetables {
	namespace Structures {
		class data_feed;
	}
	namespace Interop {
		public ref class DataFeedManaged {
		private:
			Timetables::Structures::data_feed* native_data_feed_;
		public:
			DataFeedManaged(System::String^ path);
			DataFeedManaged();

			~DataFeedManaged() { this->!DataFeedManaged(); }
			!DataFeedManaged() { delete native_data_feed_; };

			inline const Timetables::Structures::data_feed& Get() { return *native_data_feed_; }
		};
	}
}