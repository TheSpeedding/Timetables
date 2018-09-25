<h1>About the Project</h1>
The aim of the project is to design and implement applications, which will offer the user a user-friendly user interface with the possibility of simple, fast and efficient searching in automatically updated timetables. There will be support for any timetables that will be stored in a well-formed GTFS format, as well as the ability to combine the dataset in different ways, creating a search engine for an area of any size, or even the whole world.


The server application will remotely provide the main application functionality using the TCP transport protocol. In some cases, client applications will also be able to work offline - the server application will be useful especially if the user does not want to have stored data on their machine, which is typical for mobile devices.


Client applications will have two main features - finding the fastest journeys between two points and displaying departures from a stop, either all lines or the specific one. There will also be a lot of additional features, such as information about lockouts, extraordinaries in traffic, or the ability to view a transport network map. The desktop application will also be able to work fully offline to take over the role of the server for one particular device. Mobile applications will require a connection to the server. Offline functionality will be supported here by selecting favorite journeys and favorite stations. These will be cached to the device for up to 24 hours in advance, and will be automatically updated whilst WiFi connection is on, so they will be available on the device without a permanent Internet connection.

<h2>Links</h2>
<a href="https://developers.google.com/transit/gtfs/reference/">GTFS Format Reference</a><br>
<a href="https://www.microsoft.com/en-us/research/wp-content/uploads/2012/01/raptor_alenex.pdf">RAPTOR Algorithm</a><br>
