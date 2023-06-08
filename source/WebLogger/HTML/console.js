var defaultPort = "54321";
var defaultIP = `127.0.0.1`;
var connection = null;
var lastCommands = [];
var scrolling = false;
var connected = false;

var connect;// = document.getElementById("connectBtn");
var disconnect;// = document.getElementById("disconnectBtn");

function init(ip) {
  const inputBox = document.getElementById("wsurladdress");
  const window = document.getElementById("output");

    this.connect = document.getElementById("connectBtn");
    this.disconnect = document.getElementById("disconnectBtn");

  if (window != null) {
    window.addEventListener("mouseenter", (e) => {
      console.log(e);
      scrolling = true;
    });

    window.addEventListener("mouseleave", (e) => {
      console.log(e);
      scrolling = false;
    });
  }

  var location = "";

  if (ip != null && ip.length > 0) {
    location = `${ip}:${defaultPort}`;
    console.warn(`Using window location ${location}`);
  } else {
    location = `${defaultIP}:${defaultPort}`;
    console.warn(`Using default location ${location}`);
  }

  if (inputBox != null) inputBox.value = location;

  startWebsocket();

  if (connect != null)
    connect.addEventListener("click", () => {
      console.log("Connect");
      if (!connected) startWebsocket();
    });

  if (disconnect != null)
    disconnect.addEventListener("click", () => {
      if (connected) socketclose();
    });
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
  //connectBtn = document.getElementById("connectBtn");
  //disconnectBtn = document.getElementById("disconnectBtn");
  commandWindow = document.getElementById("command-input");
  commandSend = document.getElementById("sendCommand");
  commandClear = document.getElementById("clear-btn");
  output = document.getElementById("output");

    //if (connect != null) 
    this.connect.classList.add("disabled");
    //if (disconnect != null) 
    this.disconnect.classList.remove("disabled");
  if (commandWindow != null) commandWindow.classList.remove("disabled");
  if (commandSend != null) commandSend.classList.remove("disabled");
  if (commandClear != null) commandClear.classList.remove("disabled");
  if (output != null) output.classList.remove("disabled");
}

function onClose(evt) {
  console.log(evt);

  //connectBtn = document.getElementById("connectBtn");
  //disconnectBtn = document.getElementById("disconnectBtn");
  commandWindow = document.getElementById("command-input");
  commandSend = document.getElementById("sendCommand");
  commandClear = document.getElementById("clear-btn");
  output = document.getElementById("output");

    //if (connect != null)
    this.connect.classList.remove("disabled");
    //if (disconnect != null) 
this.disconnect.classList.add("disabled");
  if (commandWindow != null) commandWindow.classList.add("disabled");
  if (commandSend != null) commandSend.classList.add("disabled");
  if (commandClear != null) commandClear.classList.add("disabled");
  if (output != null) output.classList.add("disabled");
}

function onMessage(evt) {
  console.log(evt);
  if (evt != null) {
    let msg = "";
    msg = evt.data;

    var other = document.getElementById("consoleWindow");
    var window = document.getElementById("output");

    if (window != null && !scrolling) window.scrollTop = window.scrollHeight;

    if (other != null) {
      other.innerHTML += `${msg}\n`;
    }
  }
}

function onError(evt) {
  console.error(evt);
}

function doSend(message) {
  console.log(message);
  websocket.send(message);
}

function socketclose(evt) {
  websocket.close();
}

var lastCommandIndex = 0;

function sendString() {
  var inputText = document.getElementById("command-input");

  if (inputText != null) {
    lastCommandIndex = 0;
    doSend(inputText.value);

    if (inputText.value.length > 0) {
      var index = lastCommands.indexOf(inputText.value);
      if (index >= 0) lastCommands.splice(index);

      lastCommands.unshift(inputText.value);
    }
    inputText.value = "";
  }
}

function historyInc() {
  if (lastCommands.length - 1 == lastCommandIndex) lastCommandIndex = 0;
  else lastCommandIndex++;
}

function historyDec() {
  if (lastCommandIndex == 0) lastCommandIndex = lastCommands.length - 1;
  else lastCommandIndex--;
}

function commandKeyUp(event) {
  console.log(event);
  if (event.keyCode === 13) {
    event.preventDefault();
    document.getElementById("sendCommand").click();
  } else if (event.keyCode === 38) {
    historyInc();
    console.log(
      `HISTORY INDEX ${lastCommandIndex} COMMAND ${lastCommand}`,
      lastCommands
    );

    if (lastCommands.length > 0) {
      var lastCommand = lastCommands[lastCommandIndex];
      var inputText = document.getElementById("command-input");
      inputText.value = lastCommand;
    }
  } else if (event.keyCode === 40) {
    historyDec();
    console.log(
      `HISTORY INDEX ${lastCommandIndex} COMMAND ${lastCommand}`,
      lastCommands
    );

    if (lastCommands.length > 0) {
      var lastCommand = lastCommands[lastCommandIndex];
      var inputText = document.getElementById("command-input");
      inputText.value = lastCommand;
    }
  }
}

function urlKeyUp(event) {
  console.log(event);
  if (event.keyCode === 13) {
    event.preventDefault();
    document.getElementById("connectBtn").click();
  }
}

function clearConsole() {
  var output = document.getElementById("consoleWindow");
  output.innerHTML = "";
}

window.addEventListener("load", (event) => {
  setTimeout(() => {
    console.log(window.location.hostname);
    init(window.location.hostname);
  }, 200);
});
