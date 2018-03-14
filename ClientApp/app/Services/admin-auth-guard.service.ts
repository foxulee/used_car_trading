import { AuthService } from './auth.service';
import { AuthGuard } from './auth-guard.service';
import { Injectable } from '@angular/core';

@Injectable()
export class AdminAuthGuard extends AuthGuard {

  constructor(auth: AuthService) {
    super(auth);
  }

  canActivate() {
    let isAuthenticated = super.canActivate();
    return isAuthenticated ? this.auth.hasRole('Admin') : false;
  }

}
