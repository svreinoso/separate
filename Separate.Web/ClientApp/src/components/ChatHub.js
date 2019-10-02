import React, { Component } from 'react';
const signalR = require("@aspnet/signalr");

export class ChatHub extends Component {
    static displayName = ChatHub.name;

    constructor(props) {
        super(props);
        this.state = { currentCount: 0 };
    }

    componentDidMount() {
        let connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44347/api/chatHub")
            .configureLogging(signalR.LogLevel.Information)
            //.withAutomaticReconnect()
            .build();

        connection.on("send", data => {
            console.log(data);
        });

        connection.start().then((connectionId) => {
            console.log(`Connection reestablished. Connected with connectionId "${connectionId}".`);
            connection.invoke("send", "Hello");
        });

        //connection.onreconnecting((error) => {
        //    console.log(`Connection lost due to error "${error}". Reconnecting.`);
        //});

        //connection.onreconnected((connectionId) => {
        //    console.log(`Connection reestablished. Connected with connectionId "${connectionId}".`);
        //});

        connection.onclose((error) => {
            console.log(`Connection closed due to error "${error}". Try refreshing this page to restart the connection.`);
        });
    }
    
    render() {
        return (
            <div>
                <h1>ChatHub</h1>
            </div>
        );
    }
}
