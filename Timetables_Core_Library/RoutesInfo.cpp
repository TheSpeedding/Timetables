#include "RoutesInfo.hpp"
#include <array>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::RoutesInfo::RoutesInfo(std::wistream&& routesInfo) {

	routesInfo.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(routesInfo, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<wstring, 5> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: RouteInfoID, ShortName, LongName, MeanOfTransport, Color

		for (size_t i = 0; i < 5; i++)
			std::getline(routesInfo, tokens[i], wchar_t(';'));
		
		RouteInfo r(string(tokens[1].begin(), tokens[1].end()), tokens[2], RouteType(stoi(tokens[3])), stoul(tokens[4], nullptr, 16));
		
		list.push_back(move(r));

	}

}
