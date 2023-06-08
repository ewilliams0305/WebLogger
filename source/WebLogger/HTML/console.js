var defaultPort = "54321";
var defaultIP = `127.0.0.1`;
var connection = null;
var scrolling = false;
var connected = false;
var lastCommands = [];

/****************
 * ELEMENTS
 ****************/
var connect;          //connect button
var disconnect;       //disconnect button
var locationInput;    //url input
var output;           //output display
var console;          //console window
var command;          //command input
var send;             //send button
var clear;            //clear button

/****************
 * INITIALIZE
 ****************/

function init(ip) {

    this.locationInput = document.getElementById("wsurladdress");
    this.output = document.getElementById("output-input");
    this.display = document.getElementById("consoleWindow");

    this.command = document.getElementById("command-input");
    this.connect = document.getElementById("connect-btn");
    this.disconnect = document.getElementById("disconnect-btn");
    this.send = document.getElementById("send-btn");
    this.clear = document.getElementById("clear-btn");

    //Regsiter handlers
    if (this.connect != null) this.connect.addEventListener("click", connectPressed);
    if (this.disconnect != null) this.disconnect.addEventListener("click", disconnectPressed);
    if (this.send != null) this.send.addEventListener("click", sendPressed);
    if (this.clear != null) this.clear.addEventListener("click", clearPressed);

    if (this.output != null) {
        this.output.addEventListener("mouseenter", (e) => {
            console.log(e);
            scrolling = true;
        });

        this.output.addEventListener("mouseleave", (e) => {
            console.log(e);
            scrolling = false;
        });
    }

    //Set URL
    var url = "";

    if (ip != null && ip.length > 0) {
        url = `${ip}:${defaultPort}`;
        console.warn(`Using window location ${url}`);
    } else {
        url = `${defaultIP}:${defaultPort}`;
        console.warn(`Using default location ${url}`);
    }

    if (this.locationInput != null)
        this.locationInput.value = url;

    startWebsocket();
}

function startWebsocket(evt) {
    var ip = document.getElementById("wsurladdress").value;

    if (ip != null) {
        let wsUri = "ws://";

        if (ip.includes(":")) {
            wsUri += ip;
        } else {
            wsUri += ip + ":54321/";
        }

        websocket = new WebSocket(wsUri);
        websocket.onopen = function (evt) {
            connected = true;
            onOpen(evt);
        };

        websocket.onclose = function (evt) {
            connected = false;
            onClose(evt);
        };

        websocket.onmessage = function (evt) {
            onMessage(evt);
        };

        websocket.onerror = function (evt) {
            onError(evt);
        };
    }
}

function onOpen(evt) {
    console.log(evt);
    this.connect.classList.add("disabled");
    this.disconnect.classList.remove("disabled");
    this.output.classList.remove("disabled");
    this.send.classList.remove("disabled");
    this.clear.classList.remove("disabled");
    this.display.classList.remove("disabled");
    this.command.classList.remove("disabled");
}

function onClose(evt) {
    console.log(evt);
    this.connect.classList.remove("disabled");
    this.disconnect.classList.add("disabled");
    this.output.classList.add("disabled");
    this.send.classList.add("disabled");
    this.clear.classList.add("disabled");
    this.display.classList.add("disabled");
    this.command.classList.add("disabled");
}

function onMessage(evt) {
    console.log(evt);

    if (evt != null) {
        let msg = "";
        msg = evt.data;

        if (!scrolling)
            this.output.scrollTop = this.output.scrollHeight;

        this.display.innerHTML += `${msg}\n`;
    }
}

function onError(evt) {
    console.error(evt);
}

function socketclose(evt) {
    websocket.close();
}

function sendString() {

    lastCommandIndex = 0;
    websocket.send(this.command.value);

    if (this.command.value.length > 0) {

        var index = lastCommands.indexOf(this.command.value);
        lastCommands.unshift(this.command.value);
    }
    this.command.value = "";
}

function historyInc() {
    if (lastCommands.length - 1 == lastCommandIndex)
        lastCommandIndex = 0;
    else
        lastCommandIndex++;

    console.log(`HISTORY INDEX ${lastCommandIndex} COMMAND ${lastCommand}`, lastCommands);

    if (lastCommands.length > 0) {
        var lastCommand = lastCommands[lastCommandIndex];
        var inputText = document.getElementById("command-input");
        inputText.value = lastCommand;
    }
}

function historyDec() {
    if (lastCommandIndex == 0)
        lastCommandIndex = lastCommands.length - 1;
    else
        lastCommandIndex--;

    console.log(`HISTORY INDEX ${lastCommandIndex} COMMAND ${lastCommand}`, lastCommands);

    if (lastCommands.length > 0) {
        var lastCommand = lastCommands[lastCommandIndex];
        var inputText = document.getElementById("command-input");
        inputText.value = lastCommand;
    }
}

function commandKeyUp(event) {
    console.log(event);
    if (event.keyCode === 13) {
        event.preventDefault();
        this.sendString();

    } else if (event.keyCode === 38) {
        historyInc();

    } else if (event.keyCode === 40) {
        historyDec();
    }
}

/***********************
 * USER EVENTS
 ***********************/

function connectPressed(event) {
    console.log(event);
    if (!connected) startWebsocket();
}

function disconnectPressed(event) {
    console.log(event);
    if (connected) socketclose();
}

function sendPressed(event) {
    console.log(event);
    sendString();
}

function clearPressed(event) {
    console.log(event);
    var output = document.getElementById("consoleWindow");
    output.innerHTML = "";
}

function urlKeyUp(event) {
    console.log(event);
    if (event.keyCode === 13) {
        event.preventDefault();
        this.connect.click();
    }
}

/***********
 * ENTRY
 ***********/

window.addEventListener("load", (event) => {
    setTimeout(() => {
        console.log(window.location.hostname);
        init(window.location.hostname);
    }, 200);
});
