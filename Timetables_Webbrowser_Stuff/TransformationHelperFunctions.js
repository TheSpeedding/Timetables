javascript:

// Parses the datetime in ISO8601 format YYYY-MM-DDThh:mm:ss and returns Date object.
function parseDateTime(input) {
	return new Date(
		parseInt(input.slice(0, 4), 10),
		parseInt(input.slice(5, 7), 10) - 1,
		parseInt(input.slice(8, 10), 10),
		parseInt(input.slice(11, 13), 10),
		parseInt(input.slice(14, 16), 10),
		parseInt(input.slice(17, 19), 10)
	);
}

// Computes the difference between two datetimes, returns total hours as an int.
function diffInHours(d1, d2) {
	var t1 = d1.getTime();
	var t2 = d2.getTime();

	return parseInt((t2 - t1) / (3600 * 1000));
}

// Computes the difference between two datetimes, returns total minutes as an int.
function diffInMinutes(d1, d2) {
	var t1 = d1.getTime();
	var t2 = d2.getTime();

	return parseInt((t2 - t1) / (60 * 1000));
}

// Computes the difference between two datetimes, returns total seconds as an int.
function diffInSeconds(d1, d2) {
	var t1 = d1.getTime();
	var t2 = d2.getTime();

	return parseInt((t2 - t1) / 1000);
}

// Returns the timespan (as a string) in which the journey leaves. 
function leavingTimeText(date) {
	var d1 = parseDateTime(date);
	var d2 = new Date(Date.now());
	var dM = Math.abs(diffInMinutes(d1, d2));
	var dH = Math.abs(diffInHours(d1, d2));
	var text = (dM == 1) ? "1 minute" : ((dM % 60) + " minutes");
	if (dM >= 60) text = ((dH == 1) ? "1 hour " : ((dH % 60) + " hours ")) + text;
	if (d1 > d2) text = "in " + text;
	else text += " ago";
	return text;
}

// Returns the timespan (as a string) which represents total duration of the journey.
function getDuration(x, y) {
	var d1 = parseDateTime(x);
	var d2 = parseDateTime(y);
	var dM = Math.abs(diffInMinutes(d1, d2));
	var dH = Math.abs(diffInHours(d1, d2));
	var text = (dM == 1) ? "1 minute" : ((dM % 60) + " minutes");
	if (dM >= 60) text = ((dH == 1) ? "1 hour " : ((dH % 60) + " hours ")) + text;
	return text;
}

// Returns total number of transfer (as a string).
function totalTransfers(x) {
	--x;
	if (x == 0) return "no transfers";
	if (x == 1) return "1 transfer";
	else return (x + " transfers");
}

// Converts ISO8601 datetime represented as a string to local time.
function getTimeFromIso8601(x) {
	parseDateTime(x).toLocaleTimeString();
}