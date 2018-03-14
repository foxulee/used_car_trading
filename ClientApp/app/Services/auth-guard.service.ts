import { CanActivate } from '@angular/router';
import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(
    protected auth: AuthService
  ) { }


  public canActivate(): boolean {
    if (this.auth.isAuthenticated()) {
      console.log("isAuth in guard", this.auth.isAuthenticated());

      return true;
    }
    window.location.href = "https://foxulee.auth0.com/login?client=qSin37L0vWzgagvgSCs5qIqiijfjhlMO";
    return false;
  }
}
