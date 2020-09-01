(function () {
    var socket = new WebSocket("ws://localhost:50000");

    socket.onopen = function(e) {
        console.info("websocket соединение установлено");
        //showbuttons(true);
        setMessage('успешное соединеие с управляющим модулем!');
        
        // отправка сообщения о получении статуса
        var cmd = {
            command: "status",
            station: stationNumber,
            time: new Date(),
            result: {
                code: 0,
                message: ''
            }
        };
        showbuttons(false);
        sendCommand(cmd);
        setMessage('отправлена команда ' + cmd.command + ', time = ' + cmd.time.getHours() + ':' + cmd.time.getMinutes() + ':' + cmd.time.getSeconds());
    };

    socket.onmessage = function (event) {
        var msg = JSON.parse(event.data);

        if (msg.station != stationNumber)
            return;

        switch (msg.command) {
            case "start":
                // действие на ответ на запуск
                break;
            case "stop":
                // действие на ответ на остановку
                break;
            case "reset":
                // действие на ответ на сброс
                break;
            case "status":
                // действие на ответ на получение текущего статуса
                switch (msg.result.code) {
                    // ошибка получения статуса или др
                    case -1:
                        showbuttons(false);
                        break;
                    // остановлен
                    case 0:
                        enableButton('#btn-start', true);
                        enableButton('#btn-stop', false);
                        enableButton('#btn-reset', true);
                        break;
                    // работает
                    case 1:
                        enableButton('#btn-start', false);
                        enableButton('#btn-stop', true);
                        enableButton('#btn-reset', true);
                        break;
                    // хз что ещё, м б авария?
                    //case 2:
                        //break;
                    default:
                        showbuttons(false);
                        console.log('неизвестный ответ от сервера на комманду status: ' + msg.result.code);


                }
                break;
            default:
                console.log('неизвестный ответ от сервера ' + msg.command);
        }
        setMessage('ответ на команду ' + msg.command+': код - '+  msg.result.code +', сообщение - '+msg.result.message);
    };

    socket.onclose = function(event) {
        if (event.wasClean) {
            console.info('соединение закрыто чисто, код='+event.code+' причина='+event.reason);
    } else {
        // например, сервер убил процесс или сеть недоступна
        // обычно в этом случае event.code 1006
        console.log('[close] websocket соединение прервано');
        }
        showbuttons(false);
        setMessage('обрыв соединения с управляющим сервером!');
};

    socket.onerror = function (error) {
        showbuttons(false);
        setMessage('ошибка при подключении к управляющему серверу');
        console.error('Ошибка сокета: '+error.message);
    };

    // активация/деактивация кнопки по её ID
    function enableButton(btnId, enabled) {
        if (enabled) {
            $(btnId).removeAttr('disabled');
        }else
        {
            $(btnId).attr('disabled', true);
        }
    }

    // активация/деактивация кнопок управления
    function showbuttons(shown) {
        enableButton('#btn-start', shown);
        enableButton('#btn-stop', shown);
        enableButton('#btn-reset', shown);                  
       
    }

    // вывод сообщения в инф окно
    function setMessage(msg) {
        var textarea = document.querySelector("#socet_msg");
        textarea.value=textarea.value + "\n- " + msg;
        textarea.scrollTop=textarea.scrollHeight;
    }

    // сериализация объекта и отправка через сокет
    function sendCommand(obj) {
        var msgtext = JSON.stringify(obj);
        socket.send(msgtext);
    }

    // отправить старт
    $('#btn-start').click(function () {
        var cmd = {
            command: "start",
            station: $(this).attr('data-station'),
            time: new Date(),
            result: {
                code: 0,
                message: ''
            }
        };
        showbuttons(false);
        sendCommand(cmd);
        setMessage('отправлена команда ' + cmd.command +', time = '+cmd.time.getHours()+':'+cmd.time.getMinutes()+':'+cmd.time.getSeconds());
    });

    // отправить стоп
    $('#btn-stop').click(function () {
        var cmd = {
            command: "stop",
            station: $(this).attr('data-station'),
            time: new Date(),
            result: {
                code: 0,
                message: ''
            }
        };
        showbuttons(false);
        sendCommand(cmd);
        setMessage('отправлена команда ' + cmd.command + ', time = ' + cmd.time.getHours() + ':' + cmd.time.getMinutes() + ':' + cmd.time.getSeconds());

    });


    // отправить сброс
    $('#btn-reset').click(function () {
        var cmd = {
            command: "reset",
            station: $(this).attr('data-station'),
            time: new Date(),
            result: {
                code: 0,
                message: ''
            }
        };
        showbuttons(false);
        sendCommand(cmd);
        setMessage('отправлена команда ' + cmd.command + ', time = ' + cmd.time.getHours() + ':' + cmd.time.getMinutes() + ':' + cmd.time.getSeconds());

    });



})();