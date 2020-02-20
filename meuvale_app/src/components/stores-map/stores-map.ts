import { Component, OnInit } from '@angular/core';
import { Geolocation } from '@ionic-native/geolocation';
import { GoogleMapOptions, GoogleMaps, GoogleMap, Environment, Marker, MarkerCluster, GoogleMapsEvent } from '@ionic-native/google-maps';
import { Platform } from 'ionic-angular';

@Component({
  selector: 'stores-map',
  templateUrl: 'stores-map.html'
})
export class StoresMapComponent implements OnInit {

  map: GoogleMap;
  zoom: number = 16;
  lat: number;
  lng: number;

  constructor(
    private geolocation: Geolocation,
    private platform: Platform
  ) { }

  ngOnInit(): void {

    this.geolocation.getCurrentPosition({
      timeout: 30000,
      enableHighAccuracy: true
    }).then((resp) => {
      this.lat = resp.coords.latitude;
      this.lng = resp.coords.longitude;

      this.loadMap()
    }).catch((error) => {
      console.log(error);
    });

  }

  public goByLatLong(lat: any, lng: any) {
    this.lat = lat;
    this.lng = lng;

    this.loadMap()
  }

  private loadMap(): void {
    if (!this.map) {

      if (!(this.platform.is('ios') || this.platform.is('android')))
        Environment.setEnv({
          'API_KEY_FOR_BROWSER_RELEASE': 'AIzaSyAn_KF6Fqp9vVuE5KaPW_ePfbJU_56vL5Y',
          'API_KEY_FOR_BROWSER_DEBUG': 'AIzaSyCIkP9kTwXjeNb2dvwE-AT86adoQWyVwsg'
        });

      let mapOptions: GoogleMapOptions = {
        controls: {
          myLocationButton: true,
          myLocation: true
        }, center: {
          lat: this.lat,
          lng: this.lng
        },
        camera: {
          target: {
            lat: this.lat,
            lng: this.lng
          },
          zoom: this.zoom,
          tilt: 30
        }
      }

      this.map = GoogleMaps.create('map_canvas', mapOptions);
    }
  }

  public showMarks(stores: StoresMark[], name: string = 'Estou aqui'): void {
    if (this.map) {
      this.map.clear();

      this.map.addMarkerSync({
        title: name,
        icon: 'assets/image/pin.png',
        animation: 'DROP',
        position: {
          lat: this.lat,
          lng: this.lng
        }
      });

      let markers = [];

      stores.forEach(m => {
        let marker = {
          position: {
            lat: m.lat,
            lng: m.lng
          },
          name: m.name,
          title: m.name,
          placeName: m.name,
          icon: 'assets/image/IconesMapa/adiantamento.png',
        };
        markers.push(marker);
      });

      this.map.addMarkerCluster({
        boundsDraw: true,
        markers: markers,
        icons: [
          {
            min: 1, max: 10000,
            url: "assets/image/IconesMapa/adiantamento.png",
            label: { color: "white" }
           },
          // {
          //   min: 3, max: 9,
          //   url: "assets/image/IconesMapa/adiantamento.png",
          //   label: { color: "white" }
          // },
          // {
          //   min: 10,
          //   url: "assets/image/IconesMapa/adiantamento.png",
          //   label: { color: "white" }
          // }
        ]
        // icons: [
          // {min: 2, max: 100, url: "assets/image/IconesMapa/adiantamento.png", anchor: {x: 16, y: 16}},
          // {min: 100, max: 1000, url: "assets/image/IconesMapa/adiantamento.png", anchor: {x: 16, y: 16}},
          // {min: 1000, max: 2000, url: "assets/image/IconesMapa/adiantamento.png", anchor: {x: 24, y: 24}},
          // {min: 2000, url: "assets/image/IconesMapa/adiantamento.png",anchor: {x: 32,y: 32}}
        // ]
       }).then((markerCluster: MarkerCluster) => {
        markerCluster.on(GoogleMapsEvent.MARKER_CLICK).subscribe((params) => {
          let marker: Marker = params[1];
          marker.setTitle(marker.get("name"));
          marker.setSnippet(marker.get("address"));
          marker.showInfoWindow();
        });
      });
      //.then((markerCluster: MarkerCluster) => {
      //   markerCluster.one(GoogleMapsEvent.MARKER_CLICK)
      //     .then(data => {
      //       console.log(data);

      //     });
      // })
    }
  }
}

export interface StoresMark {
  name: string,
  lat: number,
  lng: number
}
