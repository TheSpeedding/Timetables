#pragma once

namespace Timetables {
	namespace Structures {
		class data_feed;
	}
	namespace Interop {
		// Wrapper from native class to the managed one.
		public ref class DataFeedManaged {
		private:
			Timetables::Structures::data_feed* native_data_feed_; // Pointer to C++ heap where the native object has its place.
		internal:
			inline const Timetables::Structures::data_feed& Get() { return *native_data_feed_; } // As this object has no methods, we have to supply reference to the native object for other native objects.
		public:
			DataFeedManaged(System::String^ path);
			DataFeedManaged();

			~DataFeedManaged() { this->!DataFeedManaged(); }
			!DataFeedManaged() { delete native_data_feed_; };
		};
	}
}