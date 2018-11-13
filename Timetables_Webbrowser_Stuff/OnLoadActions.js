window.onload = function () {
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

	
	alert('Everything is OK.');
}