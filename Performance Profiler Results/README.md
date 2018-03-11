<h1>Performance Profiler Results</h1>

<h2>Benchmarks using different datetime approaches</h2>
Using DateTime class, similar one to .NET DateTime class, there are many allocations which slow down the application very much. The slowest method using this approach is FindEarliestTrip method, to be exact the line where we are looking for starting date for the trip.


| Name of the file | Commit ID | Description | Results | CPU |
| --- | --- | --- | --- | --- |
| `Report20180311-1045.diagsession` | 1c4afe5 | Searching 10.000 journeys using DateTime class. | ~ 178 ms per journey | i5-2400 |

<hr>
