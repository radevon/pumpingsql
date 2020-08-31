(function () {
    var socket = new WebSocket("ws://localhost:5000");

    socket.onopen = function(e) {
        console.info("websocket соединение установлено");
        showbuttons(true);

        socket.send("Меня зовут Джон");
    };

    socket.onmessage = function(event) {
        alert('[message] Данные получены с сервера: ${event.data}');
    };

    socket.onclose = function(event) {
        if (event.wasClean) {
            alert('[close] Соединение закрыто чисто, код=${event.code} причина=${event.reason}');
            console.info('Соединение закрыто чисто, код='+event.code+' причина='+event.reason);
    } else {
        // например, сервер убил процесс или сеть недоступна
        // обычно в этом случае event.code 1006
        console.log('[close] websocket соединение прервано');
    }
};

    socket.onerror = function (error) {
        showbuttons(false);
        console.error('Ошибка сокета: '+error.message);
    };

    function showbuttons(shown) {
        
            $('#btn-start').disable(!shown);
            $('#btn-stop').disable(!shown);
            $('#btn-reset').disable(!shown);
        
       
    }
})();