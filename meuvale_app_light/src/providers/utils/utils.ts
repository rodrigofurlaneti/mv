import { Injectable } from '@angular/core';
import { ToastController, LoadingController, AlertController, Loading, Alert } from 'ionic-angular';

@Injectable()
export class UtilsProvider {
  private currentLoader: Loading;
  private alert: Alert;
  constructor(
    private toastCtrl: ToastController,
    private loadingCtrl: LoadingController,
    private alertCtrl: AlertController
  ) {}

  public showLoader(message: string): void {
    this.hideLoader();
    this.currentLoader = this.loadingCtrl.create({content: message});
    this.currentLoader.present();
  }

  public hideLoader(): void {
    if(this.currentLoader) {
      this.currentLoader.dismiss();
      this.currentLoader = null;
    }
  }

  public showToast(_message: any, _timeToShow: number = 3000): void {
    _message = _message.message || _message
    this.toastCtrl.create({message: _message, duration: _timeToShow}).present();
  }

  public showAlert(_title: string, _message: string): void {
    this.hideAlert();
    this.alert = this.alertCtrl.create({
      title: _title,
      message: _message,
      buttons: ['ok']
    });
    this.alert.present();
  }
  
  public hideAlert(): any {
    if(this.alert) {
      this.alert.dismiss();
      this.alert = null;
    }
  }

  showError(message: any): any {
    this.hideLoader();
    this.showAlert('Erro', message);
  }
  showErrorComplete(titulo: any, message: any): any {
    this.hideLoader();
    this.showAlert(titulo, message);
  }

  formatData(Data: string): any {
    if(Data) {
      var d = Data.substr(0,10);
      var arrD = d.split('-');
      return arrD[2] + '/' + arrD[1] + '/' + arrD[0];
    }
    return '';
  }
  dataToFromBody(Data: string): any {
    if(Data) {
      var d = Data.substr(0,10);
      var arrD = d.split('/');
      return arrD[2] + '-' + arrD[1] + '-' + arrD[0];
    }
    return '';
  }

}
