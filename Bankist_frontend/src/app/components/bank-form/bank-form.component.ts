import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-bank-form',
  templateUrl: './bank-form.component.html',
  styleUrls: ['./bank-form.component.scss'],
})
export class BankFormComponent {
  @Output() event = new EventEmitter<string>();

  closeModal() {
    this.event.emit('close');
  }
  register() {
    this.closeModal();
  }
}
