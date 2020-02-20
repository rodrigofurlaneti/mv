import { Injectable } from '@angular/core';
import { Push, PushObject } from '@ionic-native/push';
import { PeopleProvider } from '../people/people';


@Injectable()
export class PushSetupProvider {

  constructor(
    private peopleProvider: PeopleProvider,
    private push: Push,
  ) {}

  pushsetup(userId: string) {
    const pushObject: PushObject = this.push.init({
      android: {
        senderID: '454295619129'
      },
      ios: {
        alert: "true",
        badge: true,
        sound: 'true'
      }
    });
    console.log("pushsetup")
    pushObject.on('registration').subscribe((data) => {
      this.peopleProvider.registerDevice(userId, data.registrationId);
    });

    pushObject.on('notification').subscribe((data) => {
      console.log('notification', data);
    });

    pushObject.on('error').subscribe((e) => {
      console.log('error', e.message);
    });
  }

}
