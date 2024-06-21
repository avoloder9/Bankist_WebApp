import { Injectable } from '@angular/core';
import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { MyAuthService } from 'src/app/services/MyAuthService';

@Injectable()
export class MyAuthInterceptor implements HttpInterceptor {
  constructor(private auth: MyAuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.auth.getAuthorizationToken()?.value ?? '';

    const authReq = req.clone({
      headers: req.headers.set('Token', authToken),
    });
    return next.handle(authReq);
  }
}
