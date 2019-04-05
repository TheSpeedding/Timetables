function getJourneyBlock(journey, index, stops) {
	let mainBlock = document.createElement("div");
	mainBlock.className = "journey";

	let leavesInBlock = document.createElement("div");
	leavesInBlock.className = "leaves-in";

	if (journey["Outdated"]) {
		let outdatedSpan = document.createElement("span");
		outdatedSpan.className = "outdated";
		outdatedSpan.innerText = "Outdated!"; // TODO: Localization.
		leavesInBlock.appendChild(outadatedSpan);
	}

	let leavingSpan = document.createElement("span");
	leavingSpan.className = "leaving-time";
	leavingSpan.innerText = leavingTimeToString(journey["DepartureDateTime"]); 

	leavesInBlock.appendChild(leavingSpan); 
	mainBlock.appendChild(leavesInBlock);

	let tools = document.createElement("ul");
	tools.className = "tools";

	let detailLink = document.createElement("li");

	let detailLinkLink = document.createElement("a");
	detailLinkLink.className = "detail-link";
	detailLinkLink.href = "#";
	detailLinkLink.id = index;
	detailLinkLink.innerText = "Show detail";
	detailLinkLink.addEventListener('click', function (event) {
		// TODO.
	});

	detailLink.appendChild(detailLinkLink);
	tools.appendChild(detailLink);
	mainBlock.appendChild(tools);

	let boxBlock = document.createElement("div");
	boxBlock.className = "box";

	let infoBlock = document.createElement("div");
	infoBlock.className = "info";

	let durationBlock = document.createElement("div");
	durationBlock.className = "duration";
	durationBlock.innerText = timeSpanToString((new Date(journey["ArrivalDateTime"]) - new Date(journey["DepartureDateTime"])) / 1000);

	infoBlock.appendChild(durationBlock);

	let transfersBlock = document.createElement("div");
	transfersBlock.className = "transfers";	
	transfersBlock.innerText = totalTransfersToString(parseInt(journey["TransfersCount"]));

	infoBlock.appendChild(transfersBlock);
	boxBlock.appendChild(infoBlock);

	let mainJourneyBlock = document.createElement("div");
	mainJourneyBlock.className = "main";

	let underBlock1 = document.createElement("div");
	underBlock1.className = "departure";

	let timeBlock1 = document.createElement("div");
	timeBlock1.className = "time";
	timeBlock1.innerText = iso8601ToSimpleString(journey["DepartureDateTime"]);
	let stationBlock1 = document.createElement("div");
	stationBlock1.className = "station"; 
	stationBlock1.innerText = stops[parseInt(journey["JourneySegments"][0]["SourceStopID"])]["Name"]; 

	underBlock1.appendChild(timeBlock1);
	underBlock1.appendChild(stationBlock1);

	mainJourneyBlock.appendChild(underBlock1);

	let segmentsBlock = document.createElement("ol");
	segmentsBlock.className = "segments";

	for (let i in journey["JourneySegments"]) {
		let segment = journey["JourneySegments"][i];

		let li = document.createElement("li");

		if ("Headsign" in segment) { // Trip.
			li.className = segment["MeanOfTransport"].toString();
			li.style.backgroundColor = segment["LineColor"]["Hex"];
			li.style.color = segment["LineTextColor"]["Hex"];
			li.innerText = segment["LineLabel"];

			let details = document.createElement("span");
			details.className = "details";

			details.innerText = " · ";

			details.innerText += stops[parseInt(segment["SourceStopID"])]["Name"];
			details.innerText += " (" + iso8601ToSimpleString(segment["DepartureDateTime"]) + ")";

			details.innerText += " - ";

			details.innerText += stops[parseInt(segment["TargetStopID"])]["Name"];
			details.innerText += " (" + iso8601ToSimpleString(segment["ArrivalDateTime"]) + ")";
			
			li.appendChild(details);
		}
		else { // Transfer.
			li.className = "Footpath";

			let icon = document.createElement("span");
			icon.className = "icon";

			let img = document.createElement("img");
			img.width = 20;
			img.height = 20;
			img.src = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAB3RJTUUH4gcIDykjDbFNcgAAAiFJREFUWMPFlzFoVEEQhv/RM4l3yIkkNkEtRJRTo4WEkEoQRJRoZWG0tAjaiIWdjYKkEOxSi40RVAghEFDEwtIrUmgjYiEIJgp6xhzh4n02e7Cuz+Pu3tvn3+wyOzv/v7uzO+9JKQBUgBdAA6gC48oLQBlY5k/8Aka7ibMphYZjkoYS4p3LS8BAhJhdH8FP/sZonnkwBtSAOvADOK3/AWAXsLWXuYUeyAYkTUk6K6lf0pKkG2ZWz2OlO4D3Cec+lwd5BfjEv3E4JvkJl2jt8CgW+eWAaBFYTRBQB0pZk/cDHzyS20CpzS7MxEi61ns/5WyXEojXvf7eLAXsBtZc4O3O9jRBwD5XiFq4BwxmlfkAX4Gis60H5MvANuBUYK8BT4ByGgHnXbA3QJ87khBLQMH5bwFmEq7rSK/VcMy1nyU1JH1L8HltZhuSZGYNM7si6YCkSUlrzmdP2nJcNTPMrClpOhgbAsw3mFnNzB6aWUlS0czmez2CAnAwsG0G3gVbfCivqnfLtROBgJVwF2IJWAUWXH82EHE3NvlRj+yiu3pfgseoHFPAfY/ssbNdDXahGot8p/vkAmgCE97YfCDiQgwBDzyC761X0Y0NB6X6Y+xyPJLgcy3wmc2KfNBlftMFvtPG96UnoAkcyULAcy/o2w4K14bn/yot+aS3mpVOrhhwPSzTaQTc9H44z3Qx75knYDiNgD7gJLC/y3lF4DhQ6cT/N9dexPM3mnpEAAAAAElFTkSuQmCC";

			icon.appendChild(img);
			li.appendChild(icon);

			let details = document.createElement("span");
			details.className = "details";
			details.innerText = "Transfer" + " · " + segment["Duration"]; // TODO: Localization.

			li.appendChild(details);
		}

		segmentsBlock.appendChild(li);
	}

	mainJourneyBlock.appendChild(segmentsBlock);

	let underBlock2 = document.createElement("div");
	underBlock2.className = "arrival";

	let timeBlock2 = document.createElement("div");
	timeBlock2.className = "time";
	timeBlock2.innerText = iso8601ToSimpleString(journey["ArrivalDateTime"]);
	let stationBlock2 = document.createElement("div");
	stationBlock2.className = "station";
	stationBlock2.innerText = stops[parseInt(journey["JourneySegments"][journey["JourneySegments"].length - 1]["TargetStopID"])]["Name"];

	underBlock2.appendChild(timeBlock2);
	underBlock2.appendChild(stationBlock2);

	mainJourneyBlock.appendChild(underBlock2);

	boxBlock.appendChild(mainJourneyBlock);
	mainBlock.appendChild(boxBlock);

	return mainBlock;
}

function showRouterResponse(response, stops) {
	let journeys = response["Journeys"];
	for (let i in journeys) {
		document.body.appendChild(getJourneyBlock(journeys[i], i, stops));
	}
}


function routerRequest(sourceId, targetId) {
	sendRequestAsync("Router/" + sourceId + "/" + targetId)
		.then(x => {
			getSimplifiedStops()
				.then(y => showRouterResponse(x, y));
		})
		.catch(function () {
			alert('Server is offline. Please, try again later.'); // TODO: Localization.
		});
}

routerRequest(1170, 415);