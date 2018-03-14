import { AuthService } from './../../Services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    
    constructor(private auth: AuthService) {
        this.auth.handleAuthentication();


    }

    ngOnInit() {
        //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
        //Add 'implements OnInit' to the class.
        
        console.log("isAuthenticated:", this.auth.isAuthenticated());              
        

    }
}
