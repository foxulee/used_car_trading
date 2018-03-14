import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class PhotoService {
    
    constructor(private http: Http) {

    }

    upload(vehicleId: number, photo: any) {
        //FormData is the native javascript object. The FormData object lets you compile a set of key/value pairs to send using XMLHttpRequest. It is primarily intended for use in sending form data, but can be used independently from forms in order to transmit keyed data. The transmitted data is in the same format that the form's submit() method would use to send the data if the form's encoding type were set to multipart/form-data.
        var formData = new FormData();
        //name of 'file' should be exactly the same with API's parameter name (PhotosController.Upload(... IFormFile file))
        formData.append('file', photo);
        //p.s.: is ``, not ''
        return this.http.post(`/api/vehicles/${vehicleId}/photos`, formData).map(res => res.json());
    }

    getPhotos(vehicleId: number){
        return this.http.get(`/api/vehicles/${vehicleId}/photos`).map(res => res.json());
    }

    
}