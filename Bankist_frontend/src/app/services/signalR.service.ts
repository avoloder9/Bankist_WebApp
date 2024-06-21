import { EventEmitter, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { MyConfig } from '../myConfig';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  public static ConnectionId: string | null;
  public onConnectionIdChange: EventEmitter<string> =
    new EventEmitter<string>();
  public reloadTransactions: EventEmitter<void> = new EventEmitter<void>();
  public reloadLoans: EventEmitter<void> = new EventEmitter<void>();
  private connection: signalR.HubConnection | null = null;

  open_ws_connection() {
    this.close_ws_connection();
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${MyConfig.serverAddress}/path-signalR`)
      .build();

    this.connection.on('message', (p) => {
      this.reloadTransactions.emit();
      setTimeout(() => {
        alert(p);
      }, 2000);
    });

    this.connection.on('rate', (p) => {
      this.reloadTransactions.emit();
      alert(p);
    });

    this.connection.on('loan', (p) => {
      this.reloadLoans.emit();
      alert(p);
    });

    this.connection
      .start()
      .then(() => {
        SignalRService.ConnectionId = this.connection?.connectionId ?? null;
        if (this.connection?.connectionId) {
          this.onConnectionIdChange.emit(this.connection.connectionId);
          console.log('connection open ' + this.connection.connectionId);
        }
      })
      .catch((err) => console.error('Error while starting connection: ' + err));
  }

  close_ws_connection() {
    if (this.connection) {
      this.connection
        .stop()
        .then(() => {
          console.log('connection closed');
          SignalRService.ConnectionId = null;
        })
        .catch((err) =>
          console.error('Error while closing connection: ' + err)
        );
    }
  }

  emitReloadTransactions() {
    this.reloadTransactions.emit();
  }

  emitReloadLoans() {
    this.reloadLoans.emit();
  }
}
