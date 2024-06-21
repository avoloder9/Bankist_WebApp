import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TranslationService {
  private translations: any = {};
  private defaultLang: string = 'en';
  private availableLangs: string[] = ['en', 'bh', 'hr'];

  constructor() {
    this.loadTranslations();
  }

  public async loadTranslations(): Promise<void> {
    if (!localStorage.getItem('lang')) {
      localStorage.setItem('lang', 'en');
    }
    const langKey = localStorage.getItem('lang') || this.defaultLang;
    const lang = this.availableLangs.includes(langKey)
      ? langKey
      : this.defaultLang;

    try {
      const translations = await this.importTranslations(lang);
      this.translations = translations;
    } catch (error) {
      console.error(`Error loading translations for ${lang}:`, error);
      if (lang !== this.defaultLang) {
        this.translations = await this.importTranslations(this.defaultLang);
      }
    }
  }

  private async importTranslations(lang: string): Promise<any> {
    switch (lang) {
      case 'bh':
        return import('./i18n/bh.json');
      case 'en':
      default:
        return import('./i18n/en.json');
    }
  }

  getTranslations(): any {
    return this.translations;
  }
}
