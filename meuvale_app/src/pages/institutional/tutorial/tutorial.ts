import { Component, ViewChild } from '@angular/core';
import { IonicPage, ViewController, NavController, Slides, NavParams } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';
import { MyApp } from '../../../app/app.component';
import { HttpProvider } from '../../../providers/http/http';

@IonicPage()
@Component({
  selector: 'page-tutorial',
  templateUrl: 'tutorial.html',
})
export class TutorialPage {

  @ViewChild(Slides) slides: Slides;

  tutorial: any;
  logado: boolean = true;
  contador: number = 1;

  constructor(private viewCtrl: ViewController,
    public navCtrl: NavController,
    private navParams: NavParams,
    private storeProvider: StoreProvider) {
    this.logado = this.navParams.get("logado") || HttpProvider.userAuth != null
  }

  ionViewDidLoad() {
    this.storeProvider.getTutorial()
      .subscribe(data => this.tutorial = data)
  }

  dismiss(): void {
    if (MyApp.redirect == "TutorialPage")
      this.navCtrl.setRoot("MainHomePage");
    else
      this.viewCtrl.dismiss();
  }

  click() {
    if (!this.logado)
      if (this.slides.getActiveIndex() == 1)
        this.navCtrl.setRoot("LoginPage");
    if (this.logado)
      if (this.slides.getActiveIndex() == 14)
        this.navCtrl.setRoot("MainHomePage");

    this.slides.slideNext();


  }
}
