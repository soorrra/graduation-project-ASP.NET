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
        var fromUserID = $("#fromUserID").val();
        var toUserID = $("#toUserID").val();

        connection.invoke("SendMessage", message, fromUserID, toUserID);
    });
}).catch(function (err) {
    return console.error(err.toString());
});
