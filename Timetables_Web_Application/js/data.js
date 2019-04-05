function getSimplifiedStops() {
	return sendRequestAsync("Data/SimplifiedStops")
		.catch(function () {
			alert('Server is offline. Please, try again later.'); // TODO: Localization.
		});
}