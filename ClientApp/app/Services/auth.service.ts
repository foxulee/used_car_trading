import { JwtHelper } from 'angular2-jwt';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import 'rxjs/add/operator/filter';
import * as auth0 from 'auth0-js'; //have to modify tsconfig.ts ("noImplicitAny": false)




//follow auth0 instruction: https://manage.auth0.com/#/clients/OXkYK9ita60m21tLSlpctHDtdqIEf9Bo/quickstart
@Injectable()
export class AuthService {
  userProfile: {};
  roles: string[] = [];
  jwtHelper: JwtHelper = new JwtHelper();

  auth0 = new auth0.WebAuth({
    clientID: 'qSin37L0vWzgagvgSCs5qIqiijfjhlMO',
    domain: 'foxulee.auth0.com',
    responseType: 'token id_token',
    // audience: 'https://foxulee.auth0.com/userinfo', //As opaque strings
    audience: 'https://api.vega.com',   //As a JSON Web Token (JWT), This is the value of the Identifier field of the Auth0 Management API
    redirectUri: 'http://localhost:5619/home',
    scope: 'openid profile'     //get all user info
  });

  constructor(private router: Router) {
    //  console.log("jwt decodeToken", localStorage.getItem('id_token')? this.jwtHelper.decodeToken(localStorage.getItem('id_token')): null);
    //  console.log("jwt isTokenexpired", localStorage.getItem('id_token')?this.jwtHelper.isTokenExpired(localStorage.getItem('id_token')): true);
    this.readRolesFromSession();
    this.readUserProfileFromSession();
  }

  public login(): void {
    this.auth0.authorize();
  }

  public hasRole(role: string): boolean {
    if (this.roles.length > 0)
      return this.roles.indexOf(role) > -1;
    else
      return false
  }

  //looks for the result of authentication in the URL hash and processes it with the parseHash method from auth0.js
  public handleAuthentication(): void {
    //After authentication occurs, you can use the parseHash method to parse a URL hash fragment when the user is redirected back to your application in order to extract the result of an Auth0 authentication response. You may choose to handle this in a callback page that will then redirect to your main application, or in-page, as the situation dictates.
    this.auth0.parseHash({ hash: window.location.hash }, (err, authResult) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = '';
        this.setSession(authResult);
        console.log("authResult", authResult);


        this.readRolesFromSession();
        this.readUserProfileFromSession();

        //this.roles = authResult["idTokenPayload"]["https://vega.com/roles"]

        this.router.navigate(['/home']);
      } else if (err) {
        this.router.navigate(['/home']);
        console.log(err);
      }
    });
  }

  private readRolesFromSession() {
    // console.log("jwt decodeToken", localStorage.getItem('id_token') ? this.jwtHelper.decodeToken(localStorage.getItem('id_token')) : null);
    // console.log("jwt isTokenexpired", localStorage.getItem('id_token') ? this.jwtHelper.isTokenExpired(localStorage.getItem('id_token')) : true);

    let token = localStorage.getItem('id_token');
    if (token) {
      let decodedToken = this.jwtHelper.decodeToken(token);
      this.roles = decodedToken["https://vega.com/roles"] || [] //decodedToken["https://vega.com/roles"] ? decodedToken["https://vega.com/roles"] : [];
    }
  }

  private readUserProfileFromSession() {
    let userProfile = localStorage.getItem('user_profile');
    if (userProfile) {
      this.userProfile = userProfile;
    }
  }


  private setSession(authResult): void {
    // Set the time that the access token will expire at
    const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
    // access_token: to learn more, see the access token documentation
    // id_token: to learn more, see the ID token documentation
    // expires_in: the number of seconds before the access token expires
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', expiresAt);

    // get userProfile and store in the session
    this.auth0.client.userInfo(authResult.accessToken, (err, user) => {
      // console.log("user", user);
      localStorage.setItem('user_profile', user);
    })
  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');

    //reset the roles and userProfile
    this.roles = [];
    this.userProfile = null;

    // Go back to the home route
    // this.router.navigate(['/']);
  }

  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // access token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem("expires_at"));
    return new Date().getTime() < expiresAt;
  }

}