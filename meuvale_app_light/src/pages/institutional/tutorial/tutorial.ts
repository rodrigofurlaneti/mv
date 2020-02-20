import { Component } from '@angular/core';
import { IonicPage, ViewController } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';

@IonicPage()
@Component({
  selector: 'page-tutorial',
  templateUrl: 'tutorial.html',
})
export class TutorialPage {

  tutorial: any;

  constructor(private viewCtrl: ViewController,
    private storeProvider: StoreProvider) {
  }

  ionViewDidLoad() {
    this.storeProvider.getTutorial()
      .subscribe(data => this.tutorial = data)
  }

  dismiss():void {
    this.viewCtrl.dismiss();
  }

}
