import { SaveVehicle } from '../models/vehicle';
import { Http, RequestOptionsArgs, Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';

@Injectable()
export class VehicleService {
  private readonly vehiclesEndpoint = "/api/vehicles";

  constructor(
    private http: Http,

  ) { }

  private provideTokenInRequesetOptions(): RequestOptionsArgs {
    let token = localStorage.getItem('access_token');

    let headers = new Headers({
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    });
    if (token)
      headers.set('Authorization', "Bearer " + token);

    let options: RequestOptionsArgs = {
      headers: headers
    }

    return options;
  }

  getMakes() {
    return this.http.get("/api/makes").map(res => res.json());
  }

  getFeatures() {
    return this.http.get("/api/features").map(res => res.json());
  }

  create(vehicle: any) {
    delete vehicle.id;

    return this.http.post(this.vehiclesEndpoint, vehicle, this.provideTokenInRequesetOptions()).map(res => res.json());
  }

  getVehicle(id: number) {
    return this.http.get(this.vehiclesEndpoint + "/" + id.toString()).map(res => res.json());
  }

  getVehicles(filter: any) {
    return this.http.get(this.vehiclesEndpoint + "?" + this.toQueryString(filter)).map(res => res.json());

  }

  toQueryString(obj: any): string {
    let parts = [];
    for (var property in obj) {
      var value = obj[property] //obj.property doesn't work
      if (value != null && value != undefined) {
        // If you're encoding a string to put in a URL component (a querystring parameter), you should call encodeURIComponent.
        parts.push(encodeURIComponent(property) + "=" + encodeURIComponent(value))
      }
    }
    return parts.join("&");
  }

  update(vehicle: SaveVehicle) {
    return this.http.put(this.vehiclesEndpoint + "/" + vehicle.id.toString(), vehicle, this.provideTokenInRequesetOptions()).map(res => res.json());
  }

  delete(id: number) {
    return this.http.delete(this.vehiclesEndpoint + "/" + id.toString(), this.provideTokenInRequesetOptions()).map(res => res.json());
  }

}
