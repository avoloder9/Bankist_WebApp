import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';
import { LoaderComponent } from '../loader/loader.component';
import { TranslationService } from 'src/app/services/TranslationService';

interface Bank {
  name: string;
  image: string;
}

@Component({
  selector: 'app-bank-selection',
  templateUrl: './bank-selection.component.html',
  styleUrls: ['./bank-selection.component.scss'],
})
export class BankSelectionComponent implements OnInit {
  banks: Bank[] = [];
  isLoading: Boolean = false;
  username: any;
  translations: any;
  constructor(
    private router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private translationService: TranslationService
  ) {}

  onImageClick(bankName: string) {
    this.router.navigate([
      '/userTransactionList',
      bankName,
      { username: this.username },
    ]);
  }
  ngOnInit() {
    this.translations = this.translationService.getTranslations();

    this.route.params.subscribe((params) => {
      this.username = params['username'];
    });

    this.isLoading = true;
    /*const headers = new HttpHeaders({
      User: localStorage.getItem('token') ?? '',
    });*/
    this.httpClient
      .get<any>(
        `${MyConfig.serverAddress}/Bank/active-banks` /*, {
        headers: headers,
      }*/
      )
      .subscribe({
        next: (response: any) => {
          console.log(response);
          this.setBanks(response);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  setBanks(data: any) {
    data.banks.forEach((item: { name: any }) => {
      this.banks.push({
        name: item.name,
        image: `../assets/images/${item.name}.png`,
      });
    });
    this.isLoading = false;
  }
  addNewBank() {
    this.router.navigate(['/new-bank', { username: this.username }]);
  }
}
