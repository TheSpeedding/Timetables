<h1>Introduction to the Problem</h1>
The goal of the project is to design and implement a set of applications, perceiving the end user software as one application only. An application that will have a user-friendly graphical interface and whose target platform will be Win32 will allow the offline user to search for a connection within Prague Integrated Traffic and get information about departures from the stops. Once you have switched to online mode, you will be able to see the current situation in operation, transport extraordinary and information on the exits. Online mode will be required for the occasional update of timetables, which will be done by the app itself at regular intervals (once every few days), or if this interval is exceeded whenever the target machine connects to the Internet. Timetables will be valid for a few days ahead as soon as they expire, the user will be aware of this fact. It's also going to be the future of creating a mobile app for Android targeting platform that will work online. It will work with the above-mentioned application in such a way that there will be almost no logic in the mobile application, everything will be provided by the desktop application, the mobile application will be used to input and display the results from the desktop application and will communicate with it over the network through a suitably designed protocol. However, it will be possible to switch to offline mode in such a way that the user can download timetables for his favorite route that will be available for a few days ahead, so he will be able to search for offline connections.} This will include a product web presentation written in HTML5 and CSS3, where all the important information will be.

<h1>Available resources</h1>
Since the application is intended for Prague Integrated Transport, we are forced to use the GTFS format timetable as the main data source. This format, designed by Google, is very variable. The goal is to optimize the solution for Prague, but the application will also give the user the ability to simply change the destination city - timetables in this format are also provided for other large (not only) European cities such as London, Paris or Rome. The routing algorithms in the timetables are many, most of them using some modified Dijkstra algorithm. This solution will use the RAPTOR algorithm developed by Microsoft that does not need a priority queue and is fast and efficient enough. The algorithm works in the rounds, where the k-th round finds the fastest connection to all stops we can get using k-1 transfers. This means that we do not look for a connection to only one destination stop, but to all stops in the chart of the transport network. But it does not matter, because the time complexity remains very good (linear to the lines of all lines, we only go through once) and the memory consumption is also not essential for today's conditions because we only reconstruct the various connections by three pointers - a pointer to a particular line trip, a pointer to the boarding station on a trip and a pointer to the exit station on the trip. This means that only one k transfers consumes (without the overhead of all containers and classes used) only 24 k bytes of memory on the x64 platform, 12 k bytes of memory on the x86 platform. From a software-engineering point of view, it will be possible to simply change the GTFS format by overwriting only one part of the application that takes care of parsing in some kind of inter-format. Nothing else will need to be modified.
