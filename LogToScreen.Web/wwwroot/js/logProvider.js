"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/logHub").build();
console.log(connection);

connection.on("ReceiveMessage", function (message) {
    console.log("here 2");
    console.log(message);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var div = document.createElement("div");
    div.textContent = msg;
    document.getElementById("messagesList").innerText = msg;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
