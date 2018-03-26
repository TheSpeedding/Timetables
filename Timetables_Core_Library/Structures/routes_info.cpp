#include "../Structures/routes_info.hpp"
#include <array>
#include <codecvt>

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::routes_info::routes_info(std::wistream&& routes_info) {

	routes_info.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(routes_info, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<wstring, 5> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: RouteInfoID, ShortName, LongName, MeanOfTransport, Color

		for (size_t i = 0; i < 5; i++)
			std::getline(routes_info, tokens[i], wchar_t(';'));
		
		route_info r(string(tokens[1].begin(), tokens[1].end()), tokens[2], mean_of_transport(stoi(tokens[3])), stoul(tokens[4], nullptr, 16));
		
		list.push_back(move(r));

	}

}
