var isDesktop = typeof window.external.IsMobileVersion !== "undefined" && !window.external.IsMobileVersion();
var isMobile = !isDesktop;

function callCSharp(name, args) {
	if (isMobile) {
		return jsBridge.invoke(name, args);
	}
	else {
		if (args === null) {
			return window.external[name]();
		}
		else {
			return window.external[name](args);
		}
	}
}

var printingOptionsEnabled = isDesktop;

var isJourney = document.getElementsByTagName("body")[0].id === "journey-type";

let basicInfoPrintElement = document.getElementById("basic-info");
if (basicInfoPrintElement !== null) {
	if (isJourney)
		basicInfoPrintElement.innerText = callCSharp("ShowJourneyText", null);
	else
		basicInfoPrintElement.innerText = callCSharp("ShowDepartureText", null);
}

let printListElement = document.getElementById("print-list-link");
if (printListElement !== null && printingOptionsEnabled) {
	printListElement.addEventListener('click', function (event) {
		if (isJourney)
			callCSharp("PrintJourneyList", null);
		else 
			callCSharp("PrintDepartureBoardList", null);
	});
	printListElement.innerHTML = callCSharp("PrintListStringConstant", null);
}

let editParametersLinkElement = document.getElementById("edit-parameters-link");
if (editParametersLinkElement !== null && isDesktop) {
	editParametersLinkElement.addEventListener('click', function (event) {
		if (isJourney)
			callCSharp("EditJourneysParameters", null);
		else 
			callCSharp("EditDeparturesParameters", null);
	});
	editParametersLinkElement.innerHTML = callCSharp("EditParametersStringConstant", null);
}

let outdatedClassCollection = document.getElementsByClassName("outdated");
for (let i = 0; i < outdatedClassCollection.length; ++i) {
	outdatedClassCollection[i].innerHTML = callCSharp("OutdatedStringConstant", null);
}

let printLinkClassCollection = document.getElementsByClassName("print-link");
for (let i = 0; i < printLinkClassCollection.length && printingOptionsEnabled; ++i) {
	let printLink = printLinkClassCollection[i];
	printLink.addEventListener('click', function (event) {
		if (isJourney)
			callCSharp("PrintJourneyDetail", null);
		else 
			callCSharp("PrintDepartureDetail", null);
	});
	printLink.innerHTML = callCSharp("PrintStringConstant", null);
}


let mapLinkClassCollection = document.getElementsByClassName("map-link");
for (let i = 0; i < mapLinkClassCollection.length; ++i) {
	let mapLink = mapLinkClassCollection[i];
	mapLink.addEventListener('click', function (event) {
		callCSharp("ShowMap", null);
	});
	mapLink.innerHTML = callCSharp("MapStringConstant", null);
}


let iso8601ClassCollection = document.getElementsByClassName("iso8601");
while (iso8601ClassCollection.length > 0) {
	let iso8601 = iso8601ClassCollection[0];
	iso8601.innerHTML = callCSharp("Iso8601ToSimpleString", iso8601.innerHTML);
	iso8601.classList.remove("iso8601");
}


let stationIdClassCollection = document.getElementsByClassName("station-id");
while (stationIdClassCollection.length > 0) {
	let stationId = stationIdClassCollection[0];
	stationId.innerHTML = callCSharp("ReplaceIdWithName", stationId.innerHTML);
	stationId.classList.remove("station-id");
}

let leavingTimesClassCollection = document.getElementsByClassName("leaving-time");
for (let i = 0; i < leavingTimesClassCollection.length; ++i) {
	leavingTimesClassCollection[i].innerHTML = callCSharp("LeavingTimeToString", leavingTimesClassCollection[i].innerHTML);
}


let noDeparturesElement = document.getElementById("no-departures");
if (noDeparturesElement !== null) {
	noDeparturesElement.innerHTML = callCSharp("NoDepartures", null);
}


let arrivalToTheStationElement = document.getElementById("arrival-to-station");
if (arrivalToTheStationElement !== null) {
	arrivalToTheStationElement.innerHTML = callCSharp("ShowArrivalConstant", null) + callCSharp("ShowArrivalTime", arrivalToTheStationElement.innerHTML);
}


let detailClassCollection = document.getElementsByClassName("detail-link");
for (let i = 0; i < detailClassCollection.length; ++i) {
	let detail = detailClassCollection[i];
	detail.addEventListener('click', function (event) {
		if (isJourney)
			callCSharp("ShowJourneyDetail", detail.id);
		else 
			callCSharp("ShowDepartureDetail", detail.id);
	});
	detail.innerHTML = callCSharp("DetailStringConstant", null);
}

let transferClassCollection = document.getElementsByClassName("transfer-constant");
for (let i = 0; i < transferClassCollection.length; ++i) {
	transferClassCollection[i].innerHTML = callCSharp("TransferStringConstant", null);
}


let totalDurationClassCollection = document.getElementsByClassName("total-duration");
for (let i = 0; i < totalDurationClassCollection.length; ++i) {
	let el = totalDurationClassCollection[i];
	let departure = el.getElementsByClassName("departure-from-source")[0].innerHTML;
	let arrival = el.getElementsByClassName("arrival-to-target")[0].innerHTML;
	el.innerHTML = callCSharp("TotalDurationToString", departure + "," + arrival);
}


let totalTransfersClassCollection = document.getElementsByClassName("total-transfers");
for (let i = 0; i < totalTransfersClassCollection.length; ++i) {
	let transfer = totalTransfersClassCollection[i];
	transfer.innerHTML = callCSharp("TotalTransfersToString", transfer.innerHTML);
}