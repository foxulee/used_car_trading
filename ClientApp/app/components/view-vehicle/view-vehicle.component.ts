import { Subscription } from 'rxjs/Rx';
import { AuthService } from './../../Services/auth.service';
import { BrowserXhr } from '@angular/http';
import { ProgressService, BrowserXhrWithProgress } from './../../Services/progress.service';
import { PhotoService } from './../../Services/photo.service';
import { VehicleService } from './../../Services/vehicle.service';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css'],
  // only want to provide this service in this specific components instead of the whole app
  providers: [
    //to enable showing progression on downloading/uploading, need to rewrite BrowerXhr class and provide ProgressService
    { provide: BrowserXhr, useClass: BrowserXhrWithProgress },
    ProgressService,    
    PhotoService,     
  ],
})
export class ViewVehicleComponent implements OnInit {
  vehicle: any
  vehicleId: number;
  photos: any[];
  progress: any;
  total: any;
  subscription: Subscription;
  @ViewChild("fileInput") fileInput: ElementRef;

  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toasty: ToastyService,
    private vehicleService: VehicleService,
    private photoService: PhotoService,
    private zone: NgZone,
    private auth: AuthService,
    private progressService: ProgressService
  ) {
    route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return;
      }
    });
  }

  
  ngOnInit() {
    this.vehicleService.getVehicle(this.vehicleId).subscribe(
      v => this.vehicle = v,
      err => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
          return;
        }
      });

    this.photoService.getPhotos(this.vehicleId).subscribe(
      p => this.photos = p
    )
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicleId).subscribe(x => {
        this.router.navigate(['/vehicles']);
      });
    }
  }

  uploadPhoto() {
    //showing progress bar
    this.subscription = this.progressService.startTracking().subscribe(
      progress => {
        
        //put in the zone
        this.zone.run(() => {
          console.log(progress)
          this.progress = progress;
        })
      },
      undefined, //error
      () => {
        this.progress = undefined;
      }     //complete  
    );

    let nativeElement = this.fileInput.nativeElement as HTMLInputElement;    
    //compiler is showing an error "object is possibly null" on the naitveElement.files[0] part of the photoService.upload statement in uploadPhoto function, simply append "!" between files property and the index like so
    let file = nativeElement.files![0];
    //clearing the filed (file name just uploaded besides the Choose file button)
    nativeElement.value = '';
    this.photoService.upload(this.vehicleId,file).subscribe(
      photo => {
        this.photos.push(photo);
      },
      error => {
        this.toasty.error({
          title: 'Error',
          msg: error.text(),
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
      });
  }

  cancelUpload(){
    if(this.subscription){
      this.subscription.unsubscribe();
      this.progress = undefined;
    }
  }



}
