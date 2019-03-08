#include "../Timetables_Core_Library/Structures/data_feed.hpp"
#include "../Timetables_Core_Library/Structures/date_time.hpp"
#include "../Timetables_Core_Library/Algorithms/router_raptor.hpp"

#include <string>
#include <chrono>
#include <tuple>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;

static tuple<double, size_t, size_t, size_t> run_one_iteration(const data_feed& feed, size_t A, size_t B, const date_time& dep) {
	router_raptor r(feed, A, B, dep, 50, 10);

	auto start = chrono::high_resolution_clock::now();

	r.obtain_journeys();

	auto end = std::chrono::high_resolution_clock::now();

	auto duration = std::chrono::duration<double, std::milli>(end - start).count();
	auto et_calls = r.total_et_calls();
	auto marked_stops = r.total_marked_stops();
	auto routes_traversed = r.total_traversed_routes();

	return make_tuple(duration, et_calls, marked_stops, routes_traversed);
}

static void run_benchmark(const data_feed& feed, size_t A, size_t B) {
	constexpr size_t iterations = 100;

	cout << "Running " + to_string(iterations) + " iterations with EDT at 2:00." << endl;	

	double duration = 0;
	size_t et_calls = 0;
	size_t marked_stops = 0;
	size_t routes_traversed = 0;

	for (size_t i = 0; i < 100; ++i) {
		auto res = run_one_iteration(feed, A, B, date_time(date_time::now().date(), HOUR * 2));
		cout << "Time: " + to_string(get<0>(res)) + " ms. ET calls: " + to_string(get<1>(res)) + ". Marked stops: " + to_string(get<2>(res)) + ". Routes traversed: " + to_string(get<3>(res)) + "." << endl;
	}

	cout << endl << "Averages:" << endl << "Time one journey: " << to_string(duration / (iterations * 50)) << " ms. Time: " + to_string(duration / iterations) + " ms. ET calls: " + to_string(et_calls / iterations) + ". Marked stops: " + to_string(marked_stops / iterations) + ". Routes traversed: " + to_string(routes_traversed / iterations) + ".";
}

int main() {
	data_feed feed;

	cout << "Data loaded successfully." << endl;
	
	run_benchmark(feed, feed.stations().find_index(L"Pod Jezerkou"), feed.stations().find_index(L"Malostranské námìstí"));

	return 0;
}