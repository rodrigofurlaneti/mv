import { Injectable } from '@angular/core';
import { Response, Http } from "@angular/http";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class CepProvider {

    private Url: string = 'https://viacep.com.br/ws/';
    
    constructor(private http: Http) { };

    getAdressByCep(cep):Observable<any>  {
        return this.http.get(this.Url + cep + '/json/').map((response: Response) => response.json());
    }

}