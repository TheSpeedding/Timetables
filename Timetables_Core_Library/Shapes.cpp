#include "Shapes.hpp"
#include "Exceptions.hpp"
#include <array>
#include <string>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Shapes::Shapes(std::istream&& shapes) {
	string line;
	getline(shapes, line); // The first line is an invalid entry.

	while (shapes.good()) {
		array<string, 4> tokens;
		for (int i = 0; i < 3; i++)
			getline(shapes, tokens[i], ',');
		getline(shapes, tokens[3], '\n');
		if (tokens[0] == "") continue; // Empty line.

		/*
		* tokens[0] = shape id
		* tokens[1] = latitude
		* tokens[2] = longitude
		* tokens[3] = point sequence // Stored as a position in the vector.
		*/

		size_t id(stoi(tokens[0]));

		if (id - 1 == list.size()) {
			list.push_back(ShapesSequence()); // Creates new shape entry if neccessary. 
			(list.end() - 1)->reserve(128); // 128 considered to be the best trade-off.
		}

		(list.end() - 1)->push_back(GpsCoords(stod(tokens[1]), stod(tokens[2])));
	}
}
