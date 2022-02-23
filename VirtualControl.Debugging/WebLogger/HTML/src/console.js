
var defaultIP = `${location.host}:54321`;
	var connection = null;  

	function init() 
	{ 
		var inputBox = document.getElementById("wsurladdress");
		var window = document.getElementById("scroll-window");

		if(window != null){
			window.addEventListener('mouseenter', e => {
				console.log(event);
				scrolling = true;
			});

			window.addEventListener('mouseleave', e => {
				console.log(event);
				scrolling = false;
			});
		}

		if (inputBox != null)
			inputBox.value = defaultIP;
	} 
	
	var scrolling = false;
	
	function getBoundString(msg, startChar, stopChar)
	{
		var response = "";
			
		if (msg != null && msg.length > 0)
		{
			var start = msg.indexOf(startChar);
				
			if (start >= 0)
			{
				start += startChar.length;
					
				var end = msg.indexOf(stopChar, start);
				
				if (start < end)
				{
					response = msg.substring(start, end);
				}
			}
		}
			
		return response;
	}
	
	function getBoundString_EndLastIndex(msg, startChar, stopChar)
	{
		var response = "";
			
		if (msg != null && msg.length > 0)
		{
			var start = msg.indexOf(startChar);
				
			if (start >= 0)
			{
				start += startChar.length;
					
				var end = msg.lastIndexOf(stopChar);
				
				if (start < end)
				{
					response = msg.substring(start, end);
				}
			}
		}
			
		return response;
	}	

	
	function startWebsocket() 
	{ 
		var ip = document.getElementById("wsurladdress").value;
		
		if (ip != null)
		{
			let wsUri = "ws://";
 
        	if (ip.includes(':')) {
            	wsUri += ip;
			}
			else {
				wsUri += ip + ":8081/";
			}
			
			websocket = new WebSocket(wsUri); 
			
			websocket.onopen = function(evt) 
			{ 
				onOpen(evt) 
			}; 
			
			websocket.onclose = function(evt) 
			{ 
				onClose(evt) 
			}; 
			
			websocket.onmessage = function(evt) 
			{ 
				onMessage(evt) 
			}; 
			
			websocket.onerror = function(evt) 
			{ 
				onError(evt) 
			};
		}		
	}  

	function onOpen(evt) 
	{ 
		console.log(evt);
		connection = document.getElementById("connection");
        connectBtn = document.getElementById("wsconnect");
        disconnectBtn = document.getElementById("wsdisconnect");
        commandWindow = document.getElementById("commandWindow");
        output = document.getElementById("output");
		
		if (connection != null)
			connection.innerHTML = "VC-4 Server: Connected";

        if(connectBtn != null)
            connectBtn.classList.add('hidden');

        if(disconnectBtn != null)
            disconnectBtn.classList.remove('hidden');

        if(commandWindow != null)
                commandWindow.classList.remove('disabled');
        if(output != null)
            output.classList.remove('disabled');
	}  

	function onClose(evt) 
	{ 
		console.log(evt);
		connection = document.getElementById("connection");
        connectBtn = document.getElementById("wsconnect");
        disconnectBtn = document.getElementById("wsdisconnect");
        commandWindow = document.getElementById("commandWindow");
        output = document.getElementById("output");
		
		if (connection != null)
			connection.innerHTML = "VC-4 Server: Disconnected";

        if(connectBtn != null)
            connectBtn.classList.remove('hidden');

        if(disconnectBtn != null)
            disconnectBtn.classList.add('hidden');

        if(commandWindow != null)
            commandWindow.classList.add('disabled');
        if(output != null)
            output.classList.add('disabled');
	}  




	function onMessage(evt) 
	{ 
		console.log(evt);
		if (evt != null)
		{
			var msg = evt.data;				
								
            // set receiving text
            var other = document.getElementById("consoleWindow");
			var window = document.getElementById("scroll-window");

			if(window != null && !scrolling)
				window.scrollTop = window.scrollHeight;
            
            if (other != null)
                other.innerHTML += msg + `\n`;
		}
	}

	function onError(evt) 
	{ 
		console.error(evt);
	}  

	function doSend(message) 
	{ 
		console.log(message);
		websocket.send(message); 
	}  
	
	function socketclose()
	{
		websocket.close();
	}
	
	function doPush(channel)
	{
		doSend("PUSH[" + channel + "]");
	}
	
	function doRelease(channel)
	{
		doSend("RELEASE[" + channel + "]");
	}

	function sendLevel(sig)
	{
		var inputRange = document.getElementById("sliderInput" + sig);
	
		if (inputRange != null)
			doSend("LEVEL[" + sig + "," + inputRange.value + "]");
	}

	function sendString()
	{
		var inputText = document.getElementById("command-input");
		
		if (inputText != null){
            doSend(inputText.value);
            
            
            var lastCommand = document.getElementById("last-command");
            lastCommand.innerHTML = "Last Command: " + inputText.value;
            inputText.value = ""
        }
			
	}

    function commandKeyUp(event){
        console.log(event)
        if(event.keyCode === 13){
            event.preventDefault();
            document.getElementById("sendCommand").click();
        } else if(event.keyCode === 38){
		
			var lastCommand = document.getElementById("last-command");
			var inputText = document.getElementById("command-input");

			inputText.value = lastCommand.innerHTML.substring(14);
		}
    }

    function clearConsole(){
        var output = document.getElementById("consoleWindow");
        output.innerHTML = "";
    }

	window.addEventListener("load", init, false);  
