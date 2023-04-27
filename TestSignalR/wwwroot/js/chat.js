const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

// Define the function to be called when the "ReceiveMessage" event is triggered
connection.on("ReceiveMessage", (user, message) => {
    const listItem = document.createElement("li");
    listItem.textContent = `${user}: ${message}`;
    document.getElementById("messagesList").appendChild(listItem);
});

// Start the connection
connection.start().catch(err => console.error(err.toString()));

// Handle the "Send" button click event
document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
    event.preventDefault();
});