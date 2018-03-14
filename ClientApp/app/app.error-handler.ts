import { ToastyService } from 'ng2-toasty';
import { ErrorHandler, Inject, NgZone, isDevMode } from '@angular/core';
import * as Raven from 'raven-js';


export class AppErrorHandler implements ErrorHandler {

    constructor(
        @Inject(NgZone) private ngZone: NgZone,
        //beacause Errorhandler is initialized before ToastyService, so we have to manually inject ToastyService to the ErrorHandler
        @Inject(ToastyService) private toastyService: ToastyService
    ) { }

    handleError(error: any): void {
        //displaying toasty
        this.ngZone.run(() => {
            //put this in the zone.run() to make sure angular zone doesn't know about toasy, otherwise there will be not change deteced.
            if (typeof (window) !== 'undefined') {  //If the window object is still not being created by the time toasty renders the component, then will throw 'window is not defined ReferenceError'
                this.toastyService.error({
                    title: 'Error',
                    msg: 'An unexpected error happened.',
                    theme: 'bootstrap',
                    showClose: true,
                    timeout: 5000
                });
                console.log(error);

            }
        });

        //log error in sentry
        // Raven.captureException(error.originalError || error);
        if (!isDevMode())
            Raven.captureException(error.originalError || error)
        else
            throw error;
    }

}