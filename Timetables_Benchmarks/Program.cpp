#include "../Timetables_Core_Library/Structures/data_feed.hpp"
#include "../Timetables_Core_Library/Structures/date_time.hpp"
#include "../Timetables_Core_Library/Algorithms/router_raptor.hpp"

#include <string>
#include <chrono>
#include <tuple>

#include <codecvt>
#include <locale>

#include <Windows.h>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;

static tuple<double, size_t, size_t, size_t, size_t> run_one_iteration(const data_feed& feed, size_t A, size_t B, const date_time& dep) {
	router_raptor r(feed, A, B, dep, 50, 10000);

	auto start = chrono::high_resolution_clock::now();

	r.obtain_journeys();

	auto end = std::chrono::high_resolution_clock::now();

	auto duration = std::chrono::duration<double, std::milli>(end - start).count();
	auto et_calls = r.total_et_calls();
	auto marked_stops = r.total_marked_stops();
	auto routes_traversed = r.total_traversed_routes();
	auto rounds = r.total_rounds();

	return make_tuple(duration, et_calls, marked_stops, routes_traversed, rounds);
}

static void run_round(const data_feed& feed, size_t A, size_t B, const date_time& de, size_t iterations) {
	double duration = 0;
	size_t et_calls = 0;
	size_t marked_stops = 0;
	size_t routes_traversed = 0;
	size_t rounds = 0;

	cout << endl << "Running " + to_string(iterations) + " iterations with EDT at " << de << "." << endl;

	for (size_t i = 0; i < iterations; ++i) {
		auto res = run_one_iteration(feed, A, B, de);

		duration += get<0>(res);
		et_calls += get<1>(res);
		marked_stops += get<2>(res);
		routes_traversed += get<3>(res);
		rounds += get<4>(res);
	}

	cout << endl << "Averages (per 50 journeys):" << endl << " Time: " + to_string(duration / iterations) + " ms. ET calls: " + to_string(et_calls / iterations) + ". Marked stops: " + to_string(marked_stops / iterations) + ". Routes traversed: " + to_string(routes_traversed / iterations) + ". Total rounds: " << to_string(rounds / iterations) << ".";

}

static void run_benchmark(const data_feed& feed, size_t A, size_t B) {
	constexpr size_t iterations = 100;

	wcout << endl << endl << L"Stations " << feed.stations().at(A).name() << L" and " << feed.stations().at(B).name() << L"." << endl;

	run_round(feed, A, B, date_time(date_time::now().date(), HOUR * 2), 100);
	run_round(feed, A, B, date_time(date_time::now().date(), HOUR * 8), 100);
	run_round(feed, A, B, date_time(date_time::now().date(), HOUR * 13), 100);
}

int main() {
	setlocale(LC_ALL, "");
	SetConsoleCP(GetACP());
	SetConsoleOutputCP(GetACP());

	locale::global(locale("Czech"));

	data_feed feed;

	cout << "Data loaded successfully." << endl;

	run_benchmark(feed, feed.stations().find_index(L"Pod Jezerkou"), feed.stations().find_index(L"Malostranské námìstí"));
	run_benchmark(feed, feed.stations().find_index(L"Horèièkova"), feed.stations().find_index(L"Letištì"));
	run_benchmark(feed, feed.stations().find_index(L"Kladno,Americká"), feed.stations().find_index(L"Benešov,Pekárny"));

	return 0;
}