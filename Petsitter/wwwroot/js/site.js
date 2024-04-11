var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (fromUser, message) {
    var msg = fromUser + ": " + message;
    var li = document.createElement("li");
    li.textContent = msg;
    $("#list").prepend(li);
});

connection.start().then(function () {
    $("#btnSendMsg").on("click", function () {
        var message = $("#txtMsg").val();
        var fromUser = $("#txtUser").val(); 

        connection.invoke("SendMessage", fromUser, message);
    });
}).catch(function (err) {
    return console.error(err.toString());
});
