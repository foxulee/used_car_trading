import { AUTH_PROVIDERS } from 'angular2-jwt';
import { AdminAuthGuard } from './Services/admin-auth-guard.service';
import { AuthGuard } from './Services/auth-guard.service';
import { AdminComponent } from './components/admin/admin.component';
import { AuthService } from './Services/auth.service';

import { PhotoService } from './Services/photo.service';
import { ViewVehicleComponent } from './components/view-vehicle/view-vehicle.component';
import { PaginationComponent } from './components/shared/pagination.component';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import * as Raven from 'raven-js';
import { AppErrorHandler } from './app.error-handler';
import { ToastyModule } from 'ng2-toasty';
import { VehicleService } from './Services/vehicle.service';
import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, BrowserXhr } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ChartModule } from 'angular2-chartjs';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { VehicleFormComponent } from './components/vehicle-form/vehicle-form.component';
import { BrowserXhrWithProgress, ProgressService } from './Services/progress.service';

// using sentry to loging errors
Raven
    .config('https://250cda1b3a6140f394b7775106206f0c@sentry.io/237660')
    .install();


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        VehicleFormComponent,
        VehicleListComponent,
        PaginationComponent,
        ViewVehicleComponent,
        AdminComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ChartModule,
        ToastyModule.forRoot(),
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            {
                path: 'vehicles',
                children: [
                    {
                        path: "",
                        component: VehicleListComponent,
                        pathMatch: 'full'
                    },
                    {
                        path: "new",
                        component: VehicleFormComponent,
                        canActivate: [AuthGuard]
                    },
                    {
                        path: ":id",
                        component: ViewVehicleComponent
                    },
                    {
                        path: 'edit/:id',
                        component: VehicleFormComponent,
                        canActivate: [AuthGuard]
                    }
                ]
            },
            // { path: 'vehicles/new', component: VehicleFormComponent },

            { path: 'counter', component: CounterComponent },
            {
                path: 'admin',
                component: AdminComponent,
                canActivate: [AdminAuthGuard]
            },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        //tell angular wheneven need ErrorHandler class, create a AppErrorHandler instance instead of ErrorHandler
        { provide: ErrorHandler, useClass: AppErrorHandler },
        AuthService,
        VehicleService,
        AuthGuard,
        AdminAuthGuard,
        PhotoService,
       
    ]
})
export class AppModuleShared {
}
