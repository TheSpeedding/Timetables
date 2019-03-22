#ifndef LABELS_STRUCT_HPP
#define LABELS_STRUCT_HPP

#include "../macros_definition.hpp"
#include <memory> 
#include <utility>
#include <vector> 
#include <map> 
#include "data_feed.hpp"

// Structure for labels used in algorithm. Performance = array. Memory = red-black tree. Same interface for both cases.

namespace Timetables {
	namespace Structures {
#ifdef PERFORMANCE_OPTIMIZED
		template <typename T>
		class labels_struct {
			using const_iterator = typename std::vector<std::pair<const Timetables::Structures::stop*, T>>::const_iterator;
			using iterator = typename std::vector<std::pair<const Timetables::Structures::stop*, T>>::iterator;
		private:
			std::vector<std::pair<const Timetables::Structures::stop*, T>> list_;
		public:
			labels_struct(const Timetables::Structures::data_feed& feed) { 
				list_.reserve(feed.stops().size()); 
				for (std::size_t i = 0; i < feed.stops().size(); ++i) {
					list_.push_back(std::make_pair(&(feed.stops().at(i)), T()));
				}
			}

			inline const_iterator cbegin() const { return list_.cbegin(); }
			inline const_iterator cend() const { return list_.cend(); }

			inline iterator begin() { return list_.begin(); }
			inline iterator end() { return list_.end(); }

			inline const_iterator find(const Timetables::Structures::stop* stop) const { return list_.at(stop->id()).second == T() ? cend() : cbegin() + stop->id(); }
			inline iterator find(const Timetables::Structures::stop* stop) { return list_.at(stop->id()).second == T() ? end() : begin() + stop->id(); }

			inline T& operator[] (const Timetables::Structures::stop* stop) { return list_.at(stop->id()).second; }

			inline void clear() {
				for (auto&& item : list_) {
					item.second = T();
				}
			}
		};
#endif

#ifdef MEMORY_OPTIMIZED 
		template <typename T>
		class labels_struct : public std::map<const Timetables::Structures::stop*, T> {
		public:
			labels_struct(const Timetables::Structures::data_feed& feed) {}
		};
#endif
	}
}

#endif // !ROUTER_RAPTOR_HPP