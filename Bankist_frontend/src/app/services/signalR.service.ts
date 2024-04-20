import { EventEmitter, Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr"
import { MyConfig } from "../myConfig";

@Injectable({ providedIn: 'root' })

export class SignalRService {

    public static ConnectionId: string | null;
    public onConnectionIdChange: EventEmitter<string> = new EventEmitter<string>();
    public reloadTransactions: EventEmitter<void> = new EventEmitter<void>();

    open_ws_connection() {
        let connection = new signalR.HubConnectionBuilder().withUrl(`${MyConfig.serverAddress}/path-signalR`).build();


        connection.on("message", (p) => {
            alert(p);
            this.reloadTransactions.emit();
        });

        connection.start().then(() => {

            SignalRService.ConnectionId = connection.connectionId;
            if (connection.connectionId) {

                this.onConnectionIdChange.emit(connection.connectionId);
                console.log("connection open" + connection.connectionId);
            }
        });

    }
    emitReloadTransactions() {

        this.reloadTransactions.emit();
    }
}