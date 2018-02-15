#ifndef SHAPES_HPP
#define SHAPES_HPP

#include <vector>
#include <string>
#include "Utilities.hpp"
#include "Exceptions.hpp"

namespace Timetables {
	namespace Structures {
		using ShapesSequence = std::vector<GpsCoords>;

		class Shapes {
		private:
			std::vector<ShapesSequence> list;
		public:
			Shapes(std::istream&& shapes);

			inline const ShapesSequence& GetShapesSequence(std::size_t id) const {
				if (id > list.size()) throw Timetables::Exceptions::ShapesSequenceNotFoundException(id);
				else return list[id - 1];
			}
		};

	}
}

#endif // !SHAPES_HPP
