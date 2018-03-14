import { Event } from '@angular/router/src/events';
import { BrowserXhr } from '@angular/http';
import { Subject } from 'rxjs/Rx';
import { Injectable } from '@angular/core';

@Injectable()
export class ProgressService {
    private uploadProgress: Subject<any>;

    //
    startTracking() {
        this.uploadProgress = new Subject();
        return this.uploadProgress;
    }

    notify(progress: any) {
        if (this.uploadProgress)
            this.uploadProgress.next(progress);
    }

    endTracking() {
        if (this.uploadProgress)
            this.uploadProgress.complete();
    }
}


@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {

    constructor(private service: ProgressService) {
        super();
    }

    //override the method of BrowserXhr
    build(): XMLHttpRequest {
        var xhr: XMLHttpRequest = super.build();

        xhr.upload.onprogress = (event) => {
            this.service.notify(this.createProgress(event));
        }

        //unscripbe to prevent memory leaking
        xhr.upload.onloadend = () => {
            this.service.endTracking();
        }

        return xhr;
    }

    private createProgress(event: ProgressEvent) {
        return {
            total: event.total,
            percentage: Math.round(event.loaded / event.total * 100)
        }
    }
}