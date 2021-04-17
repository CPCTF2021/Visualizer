var ws;
var messages;
var URL;

mergeInto(LibraryManager.library, {
	Init: function (_URL) {
    URL = Pointer_stringify(_URL);
		ws = new WebSocket( URL )
		messages = new Array()

		ws.onmessage = function (e) {
      console.log(e)
			messages.push(e.data)
		}
		ws.onopen = function () {
			console.log("connection established")
		}
		ws.onclose = function () {
      console.log("connection closed")
      ws = new WebSocket( URL )
    }
	},
  PopMessage: function () {
    var msg = ""

    if( messages.length > 0 ) {
        msg = messages.shift()
    }

    var len = lengthBytesUTF8(msg) + 1
    var buf = stackAlloc(len)

    stringToUTF8(msg, buf, len)
    return buf
  },
	SendMessage: function (message) {
    if (ws) {
        ws.send( Pointer_stringify(message) );
    }
  },
	Close: function () {
    if (ws) {
        ws.close();
    }
  }
})
