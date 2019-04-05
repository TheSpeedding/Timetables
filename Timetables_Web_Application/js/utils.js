const webApiUrl = "http://localhost:61146"; // Change this if Web API provider is not localhost.

function sendRequestAsync(request) {
	return fetch(webApiUrl + "/" + request, {
		method: 'GET',
		headers: {
			"Accept": "text/json"
		}
	}).then(x => {
		if (!x.ok) {
			throw Error();
		}
		return x.json();
	});
}

function leavingTimeToString(time) { // TODO: Localization.
	let diff = new Date(time) - new Date();
	return "In " + timeSpanToString(diff / 1000);
}

function timeSpanToString(seconds) { // TODO: Localization.
	let days = Math.floor(seconds / 86400);
	let hours = Math.floor(seconds / 3600);
	let minutes = Math.floor(seconds / 60);

	return (days > 0 ? days + (days === 1 ? " " + "day" + " " : " " + "days" + " ") : "") +
		(hours > 0 ? hours + (hours === 1 ? " " + "hour" + " " : " " + "hours" + " ") : "") +
		minutes + (minutes === 1 ? " " + "minute" + " " : " " + "minutes" + " ");
}

function iso8601ToSimpleString(iso8601) {
	let date = new Date(iso8601);
	return date.getHours() + ":" + (date.getMinutes() < 10 ? "0" : "") + date.getMinutes();
}

function totalTransfersToString(totalTripSegments) { // TODO: Localization.
	if (totalTripSegments <= 0) return "No transfer";
	else if (totalTripSegments === 1) return "One transfer";
	else return totalTripSegments + " " + "transfers";
}