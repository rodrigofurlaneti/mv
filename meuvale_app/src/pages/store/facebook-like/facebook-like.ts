import { Component } from '@angular/core';
import { IonicPage, ViewController, NavController } from 'ionic-angular';
import { InAppBrowser, InAppBrowserOptions } from '@ionic-native/in-app-browser';



@IonicPage()
@Component({
  selector: 'page-facebook-like',
  templateUrl: 'facebook-like.html'
})
export class FacebookLikePage {

  currentModal = null;

  constructor(
    public navCtrl: NavController,
    private theInAppBrowser: InAppBrowser,
    private viewCtrl: ViewController,
  ) { };

  options: InAppBrowserOptions = {
    location: 'yes',//Or 'no' 
    hidden: 'no', //Or  'yes'
    clearcache: 'yes',
    clearsessioncache: 'yes',
    zoom: 'yes',//Android only ,shows browser zoom controls 
    hardwareback: 'yes',
    mediaPlaybackRequiresUserAction: 'no',
    shouldPauseOnSuspend: 'no', //Android only 
    closebuttoncaption: 'Close', //iOS only
    disallowoverscroll: 'no', //iOS only 
    toolbar: 'yes', //iOS only 
    enableViewportScale: 'no', //iOS only 
    allowInlineMediaPlayback: 'no',//iOS only 
    presentationstyle: 'pagesheet',//iOS only 
    fullscreen: 'yes',//Windows only    
  };

  facebook() {
    //https://www.facebook.com/MeuValeBeneficios/
    let browser = 'https://www.facebook.com/MeuValeBeneficios/';
    let target = "_blank"; //_system //_self
    this.theInAppBrowser.create(browser, target, this.options);
  }

  close() {
      this.viewCtrl.dismiss();
    
  }
}
