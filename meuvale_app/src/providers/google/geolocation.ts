import { Injectable } from '@angular/core';
import { Response, Http } from "@angular/http";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class GeolocationProvider {

    private gmapsUrl: string = 'https://maps.googleapis.com/maps/api/geocode/';
    private gmapsKey: string = '&key=AIzaSyAbmeuKwXrauloY8YK7qHOa5YzEnyX_3ns';//'&key=AIzaSyDStwPWvpxHIHraTgrVMB-4otJuIBc3JVo';

    constructor(private http: Http) { };

    getGeolocationByAdress(adress):Observable<any>  {
        return this.http.get(this.gmapsUrl + "json?address=" + adress + this.gmapsKey).map((response: Response) => response.json());
    }

    getAdressByGeolocation(lat, lon):Observable<any>  {
        return this.http.get(this.gmapsUrl + "json?latlng=" + lat + "," + lon + this.gmapsKey).map((response: Response) => response.json());
    }

}