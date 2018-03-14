import { AuthService } from './../../Services/auth.service';
import { KeyValuePair, Vehicle } from './../../models/vehicle';
import { VehicleService } from './../../Services/vehicle.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  private readonly PAGE_SIZE = 3;

  makes: KeyValuePair[];
  models: KeyValuePair[];
  
  query: any = {
    pageSize: this.PAGE_SIZE
  };
  queryResult: any = {};

  constructor(private vehicleService: VehicleService, private auth: AuthService) { }

  ngOnInit() {
    this.populateVehicles();

    this.vehicleService.getMakes().subscribe(m => {
      this.makes = m;
    })
  }

  onMakeChange() {
    let selectedMake: any = this.makes.find(m => m.id == this.query.makeId)
    delete this.query.modelId;
    if (selectedMake) {
      this.models = selectedMake.models
    } else {
      
      this.models = [];
    }

  }

  onQueryChange() {
    //querying on client side
    // let vehicles = this.allVehicles;

    // //apply querys
    // if (this.query.makeId) {
    //   vehicles = vehicles.query(v => v.make.id == this.query.makeId);
    // }

    // if(this.query.modelId){
    //   vehicles = vehicles.query(v => v.model.id == this.query.modelId);
    // }

    // this.vehicles = vehicles;


    //querying on server side
    this.query.page = 1;
    this.populateVehicles();

  }

  private populateVehicles() {
    this.vehicleService.getVehicles(this.query).subscribe(result => { //result: {items, totalItems}
      this.queryResult.items = result.items;
      this.queryResult.totalItems = result.totalItems;
    })
  }

  resetQuery() {
    this.query = {
      page: 1,
      pageSize:this.PAGE_SIZE
    };
    this.populateVehicles();
  }

  sortBy(columnName: string) {
    this.query.sortBy = columnName;
    this.query.IsSortAscending = this.query.IsSortAscending ? false : true;
    this.populateVehicles();
  }

  onPageChange(page: any) {
    this.query.page = page;
    this.populateVehicles();
  }

}
