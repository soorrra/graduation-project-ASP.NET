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

        if (message.trim() !== "") {
            connection.invoke("SendMessage", message, parseInt(fromUserID), parseInt(toUserID));

            $("#txtMsg").val("");
        }
    });
}).catch(function (err) {
    return console.error(err.toString());
});
