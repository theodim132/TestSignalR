// signalrConnection.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().catch(err => console.error(err.toString()));

export default connection;
