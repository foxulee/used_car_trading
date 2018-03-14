import { timeout } from 'rxjs/operator/timeout';
import { SaveVehicle, Vehicle } from '../../models/vehicle';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';
import { VehicleService } from './../../Services/vehicle.service';
import { Subscription } from 'rxjs/Rx';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {

  makes: any[];
  features: any[];
  models: any[];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      phone: '',
      email: ''
    }
  };

  constructor(
    private route: ActivatedRoute, //read route parameters
    private router: Router, //navigate
    private vehicleService: VehicleService,
    private toastyService: ToastyService) {

    route.params.subscribe(p => {
      this.vehicle.id = +p["id"] || 0;
      console.log(this.vehicle.id);

    })

  }



  ngOnInit() {

    let sources = [
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ];

    if (this.vehicle.id) {
      sources.push(this.vehicleService.getVehicle(this.vehicle.id))
    }

    //sending parallel request instead the following code
    Observable.forkJoin(sources).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      if (this.vehicle.id) {
        this.setVehicle(data[2]);
        this.populateModels();
      }

    }, err => {
      if (err.status == 404) { // if vehicle not found
        this.router.navigate(["/home"]);
      }
    });

    // if (this.vehicle.id > 0)
    //   this.vehicleService.getVehicle(this.vehicle.id)
    //     .subscribe(v => {
    //       this.vehicle = v;
    //       console.log(v);
    //     },
    //     err => {
    //       if (err.status == 404) { // if vehicle not found
    //         this.router.navigate(["/home"]);
    //       }
    //     });

    // this.vehicleService.getMakes()
    //   .subscribe(makes => {
    //     this.makes = makes;
    //     //should put this line in the block, because this is asyn call, otherwise makes is undefined
    //     console.log(this.makes)
    //   });

    // this.vehicleService.getFeatures()
    //   .subscribe(features => {
    //     this.features = features;
    //     console.log(this.features);

    //   });
  }

  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.contact = v.contact;
    this.vehicle.isRegistered = v.isRegistered;
    v.features.forEach((value, index) => {
      this.vehicle.features.push(value.id);
    });
  }

  onMakeChange(event: any) {
    this.populateModels();
    //The delete operator removes a property from an object.
    delete this.vehicle.modelId;
  }

  private populateModels() {
    let selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onFeatureToggle(featureId: number, $event: any) {
    if ($event.target.checked) {
      this.vehicle.features.push(featureId);
    } else {
      let index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit() {
    if (this.vehicle.id) {
      this.vehicleService.update(this.vehicle).subscribe(x => {
        this.toastyService.success({
          title: 'Success',
          msg: 'The vehicle was successfully updated.',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        })
      })
    }
    else {
      this.vehicleService.create(this.vehicle)
        .subscribe(
        x => {
          this.router.navigate(["/vehicles"]);
        }
        );
    }
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id).subscribe(x => {
        this.toastyService.success({
          title: 'Success',
          msg: 'The vehicle was successfully deleted.',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
        this.router.navigate(['/vehicles']);
      });
    }
  }
}
