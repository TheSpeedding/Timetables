#include "Routes.hpp"
#include <array>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Routes::Routes(std::wistream&& routes) {

	routes.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring line;
	getline(routes, line); // The first line is an invalid entry.

	while (routes.good()) {

		array<wstring, 6> tokens;
		getline(routes, tokens[0], wchar_t(','));
		getline(routes, tokens[1], wchar_t(','));
		getline(routes, tokens[2], wchar_t(','));

		routes.get(); // '"' char
		getline(routes, tokens[3], wchar_t('"'));
		routes.get(); // ',' char

		getline(routes, tokens[4], wchar_t(','));
		getline(routes, tokens[5], wchar_t('\n'));

		if (tokens[0] == wstring()) continue; // Empty line.

		/*
		* tokens[0] = route id
		* tokens[1] = agency id // Currently unused.
		* tokens[2] = short name
		* tokens[3] = long name
		* tokens[4] = type // 0 tram, 1 metro, 2 train, 3 bus, 4 ship, 5 cable car
		* tokens[5] = color // Currently unused.
		*/

		Route r(string(tokens[2].begin(), tokens[2].end()), tokens[3], RouteType(stoi(tokens[4])));

		list.insert(make_pair(string(tokens[0].begin(), tokens[0].end()), move(r)));
	}
}
