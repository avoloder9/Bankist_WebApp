import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';

interface Auth2FRequest {
  key: string;
}

@Component({
  selector: 'app-two-factor-authentication',
  templateUrl: './two-factor-authentication.component.html',
  styleUrls: ['./two-factor-authentication.component.scss'],
})
export class TwoFactorAuthenticationComponent implements OnInit {
  @ViewChild('inputContainer') inputContainer: ElementRef;
  twoFactorKey: string[] = ['', '', '', '', '', ''];
  username: string = '';
  allInputsFilled: boolean = false;
  notValidKey: boolean = false;
  constructor(
    private router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.username = params['username'];
    });
  }
  onInput(index: number, event: any) {
    const value = event.target.value;
    if (value.length === 1) {
      const nextInput = document.querySelector(
        `.numInput:nth-child(${index + 1})`
      ) as HTMLInputElement;
      if (nextInput) {
        nextInput.focus();
      }
    }
    this.allInputsFilled = this.twoFactorKey.every((val) => val.length === 1);
  }
  confirm2F() {
    const code = this.twoFactorKey.map((val) => val).join('');
    const body: Auth2FRequest = { key: code };
    console.log(code);
    this.httpClient
      .post(`${MyConfig.serverAddress}/Auth/2fKey`, body)
      .subscribe({
        next: () => {
          this.router.navigate([
            '/bank-selection',
            { username: this.username },
          ]);
        },
        error: (error) => {
          if (error.status === 400) {
            this.notValidKey = true;
            setTimeout(() => {
              this.notValidKey = false;
              this.twoFactorKey = ['', '', '', '', '', ''];
            }, 1500);
          }
        },
      });
  }

  onPaste(event: ClipboardEvent) {
    event.preventDefault();
    const clipboardData =
      event.clipboardData || (window as any)['clipboardData'];
    const pastedText = clipboardData.getData('text');
    const inputs =
      this.inputContainer.nativeElement.querySelectorAll('.numInput');

    for (let i = 0; i < pastedText.length; i++) {
      if (inputs[i]) {
        inputs[i].value = pastedText[i];
        this.twoFactorKey[i] = pastedText[i];
      }
    }

    this.allInputsFilled = this.twoFactorKey.every((val) => val.length === 1);
  }
}
