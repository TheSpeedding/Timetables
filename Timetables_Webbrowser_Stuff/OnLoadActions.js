// window.onload = function () // This script is appended to the page, thus it does not have to wait for load.
{
	// Change this to false if application should not support printing option. By default, enabled in desktop application, disabled in mobile application.
	var printingOptionsEnabled = !window.external.IsMobileVersion();

	// Some methods may differ for journey and departure board, the rest is the same.
	var isJourney = document.getElementsByTagName("body")[0].id === "journey-type";

	// Write basic info in case of printing.
	let basicInfoPrintElement = document.getElementById("basic-info");
	if (basicInfoPrintElement !== null) {
		if (isJourney)
			basicInfoPrintElement.innerText = window.external.ShowJourneyText();
		else
			basicInfoPrintElement.innerText = window.external.ShowDepartureText();
	}

	// Customize a link to print a list.
	let printListElement = document.getElementById("print-list-link");
	if (printListElement !== null) {
		printListElement.addEventListener('click', function (event) {
			if (isJourney)
				window.external.PrintJourneyList();
			else
				window.external.PrintDepartureBoardList();
		});
		printListElement.innerHTML = window.external.PrintListStringConstant();
	}

	// Customize a link to edit parameters.
	let editParametersLinkElement = document.getElementById("edit-parameters-link");
	if (editParametersLinkElement !== null) {
		editParametersLinkElement.addEventListener('click', function (event) {
			if (isJourney)
				window.external.EditJourneysParameters();
			else
				window.external.EditDeparturesParameters();
		});
		editParametersLinkElement.innerHTML = window.external.EditParametersStringConstant();
	}

	// Set all outdated localization string constants.
	let outdatedClassCollection = document.getElementsByClassName("outdated");
	for (let i = 0; i < outdatedClassCollection.length; ++i) {
		outdatedClassCollection[i].innerHTML = window.external.OutdatedStringConstant();
	}

	// Customize a link to print detailed information.
	let printLinkClassCollection = document.getElementsByClassName("print-link");
	for (let i = 0; i < printLinkClassCollection.length; ++i) {
		let printLink = printLinkClassCollection[i];
		printLink.addEventListener('click', function (event) {
			if (isJourney)
				window.external.PrintJourneyDetail();
			else
				window.external.PrintDepartureDetail();
		});
		printLink.innerHTML = window.external.PrintStringConstant();
	}

	// Customize a link to show a map.
	let mapLinkClassCollection = document.getElementsByClassName("map-link");
	for (let i = 0; i < mapLinkClassCollection.length; ++i) {
		let mapLink = mapLinkClassCollection[i];
		mapLink.addEventListener('click', function (event) { window.external.ShowMap(); });
		mapLink.innerHTML = window.external.MapStringConstant();
	}

	// Convert all ISO8901 formatted datetimes to a readable ones and remove classes from those elements, because they contain ISO8601 no longer.
	let iso8601ClassCollection = document.getElementsByClassName("iso8601");
	while (iso8601ClassCollection.length > 0) {
		let iso8601 = iso8601ClassCollection[0];
		iso8601.innerHTML = window.external.Iso8601ToSimpleString(iso8601.innerHTML);
		iso8601.classList.remove("iso8601");
	}

	// Convert all station IDs to station names and remove classes from those elements, because they contain IDs no longer.
	let stationIdClassCollection = document.getElementsByClassName("station-id");
	while (stationIdClassCollection.length > 0) {
		let stationId = stationIdClassCollection[0];
		stationId.innerHTML = window.external.ReplaceIdWithName(stationId.innerHTML);
		stationId.classList.remove("station-id");
	}

	// Converts all absolute leaving times into relative ones.
	let leavingTimesClassCollection = document.getElementsByClassName("leaving-time");
	for (let i = 0; i < leavingTimesClassCollection.length; ++i) {
		leavingTimesClassCollection[i].innerHTML = window.external.LeavingTimeToString(leavingTimesClassCollection[i].innerHTML);
	}

	// Print that there are no departures if so. Case of map.
	let noDeparturesElement = document.getElementById("no-departures");
	if (noDeparturesElement !== null) {
		noDeparturesElement.innerHTML = window.external.NoDepartures();
	}

	// Print arrival to the station. Case of map.
	let arrivalToTheStationElement = document.getElementById("arrival-to-station");
	if (arrivalToTheStationElement !== null) {
		arrivalToTheStationElement.innerHTML = window.external.ShowArrivalConstant() + window.external.ShowArrivalTime(arrivalToTheStationElement.innerHTML);
	}

	// Set the link to the details.
	let detailClassCollection = document.getElementsByClassName("detail-link");
	for (let i = 0; i < detailClassCollection.length; ++i) {
		let detail = detailClassCollection[i];
		detail.addEventListener('click', function (event) {
			if (isJourney)
				window.external.ShowJourneyDetail(detail.id);
			else
				window.external.ShowDepartureDetail(detail.id);
		});
		detail.innerHTML = window.external.DetailStringConstant();
	}

	// Write transfer constants to the given positions.
	let transferClassCollection = document.getElementsByClassName("transfer-constant");
	for (let i = 0; i < transferClassCollection.length; ++i) {
		transferClassCollection[i].innerHTML = window.external.TransferStringConstant();
	}

	// Compute total duration of the journey.
	let totalDurationClassCollection = document.getElementsByClassName("total-duration");
	for (let i = 0; i < totalDurationClassCollection.length; ++i) {
		let el = totalDurationClassCollection[i];
		let departure = el.getElementsByClassName("departure-from-source")[0].innerHTML;
		let arrival = el.getElementsByClassName("arrival-to-target")[0].innerHTML;
		el.innerHTML = window.external.TotalDurationToString(departure, arrival);
	}

	// Write number of transfers for given journey.
	let totalTransfersClassCollection = document.getElementsByClassName("total-transfers");
	for (let i = 0; i < totalTransfersClassCollection.length; ++i) {
		let transfer = totalTransfersClassCollection[i];
		transfer.innerHTML = window.external.TotalTransfersToString(transfer.innerHTML);
	}
}