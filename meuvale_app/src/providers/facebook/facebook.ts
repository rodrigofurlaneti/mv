import { Injectable } from '@angular/core';
import { Facebook, FacebookLoginResponse } from '@ionic-native/facebook';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class FacebookProvider {

  constructor(private fb: Facebook) {}

    public facebookLogin() : Observable<any> {
      return Observable.create(
        observer => {
          this.fb.login(['email'])
          .then((res: FacebookLoginResponse) => {
            this.fb.api('me?fields=email',['email'])
              .then( res=> {
                console.log("resposta facebook")
                console.log(res);
                observer.next({email: res.email, facebookId: res.id});
                observer.complete();
              });
          })
          .catch(e => {
            observer.error(e);
            observer.complete();
          });
          }
      );    
    }

}
