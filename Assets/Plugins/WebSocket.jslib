var ws;
var messages;
var URL;

mergeInto(LibraryManager.library, {
  Init: function (_URL) {
    URL = Pointer_stringify(_URL);
    function connect() {
      ws = new WebSocket(URL);
      messages = new Array();

      ws.onmessage = function (e) {
        messages.push(e.data);
      };
      ws.onopen = function () {
        console.log("connection established");
      };
      ws.onclose = function () {
        console.log("connection closed");
        setTimeout(function () {
          connect();
        }, 1000);
      };
    }
    connect();
  },
  PopMessage: function () {
    var msg = "";

    if (messages.length > 0) {
      msg = messages.shift();
    }

    var len = lengthBytesUTF8(msg) + 1;

    var buf = _malloc(len);
    stringToUTF8(msg, buf, len);

    return buf;
  },
  SendMessage: function (message) {
    if (ws) {
      ws.send(Pointer_stringify(message));
    }
  },
  Close: function () {
    if (ws) {
      ws.close();
    }
  },
});
