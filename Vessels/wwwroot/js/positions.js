"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/vesselPositionsHub").build();

connection.on("ReceiveMessage", function (position) {
    if (document.getElementById("table-body") == null) { return; }
    var tr = document.createElement("tr");
    var td = document.createElement("td");
    td.innerText = position.vessel.imo;
    tr.appendChild(td);
    td = document.createElement("td");
    td.innerText = position.vessel.name;
    tr.appendChild(td);
    td = document.createElement("td");
    td.innerText = new Date(position.date).toLocaleString();
    tr.appendChild(td);
    td = document.createElement("td");
    td.innerText = position.latitude;
    tr.appendChild(td);
    td = document.createElement("td");
    td.innerText = position.longitude;
    tr.appendChild(td);
    td = document.createElement("td");
    var id = position.id
    td.innerHTML = "<td><a href = '/Home/Edit/" + id + "'> Edit</a> | <a href='/Home/Details/" + id + "'>Details</a> | <a href='/Home/Delete/" + id + "'>Delete</a> | <a target=”_blank” href='https://www.google.com/maps/search/?api=1&query=" + position.latitude + "," + position.longitude + "'>Google Maps</a></td>";
    tr.appendChild(td);
    document.getElementById("table-body").appendChild(tr);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});