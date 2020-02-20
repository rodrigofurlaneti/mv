import { Component, ViewChild } from '@angular/core';
import { Nav, Platform, AlertController } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { MENU_LIST_OPEN, MENU_LIST_CONSUMIDOR, MENU_LIST_LOJISTA, MENU_LIST_ADMIN, MENU_LIST_FIRST_LOGIN } from './config/menu-perfil.config';
import { AuthProvider } from '../providers/auth/auth';
import { Storage } from '@ionic/storage';
import { HttpProvider } from '../providers/http/http';
import { ShoppingProvider } from '../providers/shopping/shopping';
import { MainStorePage } from '../pages/main-store/main-store';
import { MainAdminPage } from '../pages/main-admin/main-admin';

@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;

  rootPage: any = "LoginPage";
  private static _ref: MyApp;
  pages: Array<{ title: string, icon: string, page: string }> = [];
  public static redirect = "";
  public static lojaRedirect: any;
  public static tabRedirect: number = 0;
  public static modalRedirectLogin: any;
  public static prodRedirect: any;

  constructor(public platform: Platform,
    public statusBar: StatusBar,
    private storage: Storage,
    private alertCtr: AlertController,
    public splashScreen: SplashScreen,
    private authProvider: AuthProvider) {
    this.initializeApp();
    MyApp._ref = this;
  }

  public static get ref(): MyApp {
    return MyApp._ref;
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.authProvider.getUserLogged()
        .subscribe(data => {
          this.handleAuthChange(data);
          this.prepareApp();
        }, error => {
          this.prepareApp();
        });
      AuthProvider.eventDispatcher.subscribe(
        data => this.handleAuthChange(data)
      );
    });
  }

  private prepareApp() {
    this.statusBar.styleDefault();
    this.splashScreen.hide();
  }

  public handleAuthChange(data: any) {
    if (data) {
      if (data.PrimeiroLogin && data.PrimeiroLogin.toString().toUpperCase() == "TRUE") {
        this.pages = MENU_LIST_FIRST_LOGIN;
        this.handleLoginData(data);
        return;
      }
      switch (data.PerfilId) {
        case "1": //1	Root
          this.pages = MENU_LIST_CONSUMIDOR;
          break;
        case "2": //2	App Default (Fase 1)
          this.pages = MENU_LIST_LOJISTA;
          break;
        case "99":
          this.pages = MENU_LIST_ADMIN;
          break;
      }
    } else {
      this.pages = MENU_LIST_OPEN;
    }
    this.handleLoginData(data);
  }

  private handleLoginData(data: any) {
    
    if (data.PrimeiroLogin && data.PrimeiroLogin.toString().toUpperCase() == "TRUE") {
      this.nav.setRoot("FirstLoginPage", { perfil: data.PerfilId });
      return;
    }

    if (MyApp.redirect) {
      if (MyApp.lojaRedirect && MyApp.redirect == "MainStorePage") {
        MainStorePage.ref.showTab(MyApp.tabRedirect);
        return;
      }
      if (MyApp.lojaRedirect && MyApp.redirect == "MainAdminPage") {
        MainAdminPage.ref.showTab(MyApp.tabRedirect);
        return;
      }
      if (MyApp.lojaRedirect) {
        this.nav.setRoot(MyApp.redirect, { "loja": MyApp.lojaRedirect });
        return;
      }
      else {
        this.nav.setRoot(MyApp.redirect);
        return;
      }
    }
    else if (data) {
      if (data.PerfilId == 2) {
        this.nav.setRoot("MainStorePage");
        return;
      }
      else if (data.PerfilId == 99) {
        this.nav.setRoot("MainAdminPage");
        return;
      }
      else {
        this.nav.setRoot("MainHomePage");
        return;
      }
    }
  }

  public resetRoot() {
    if (HttpProvider.userAuth.PerfilId == '2')
      this.nav.setRoot("MainStorePage");
    else if (HttpProvider.userAuth.PerfilId == "99")
      this.nav.setRoot("MainAdminPage");
    else
      this.nav.setRoot("MainHomePage");
  }

  openPage(page: string) {
    if (page) {
      if (page == "LoginPage" || page == "MainStorePage" || page == "MainHomePage")
        this.nav.setRoot(page)
      else
        this.nav.push(page);
    } else {
      this.alertCtr.create({
        title: "Sair",
        message: "Deseja mesmo sair do app?",
        buttons: [
          "Não",
          {
            text: "Sim",
            handler: () => {
              this.logout();
            }
          }
        ]
      }).present();
    }
  }

  public logout(): void {
    this.pages = MENU_LIST_OPEN;
    this.storage.clear();
    ShoppingProvider.setCurrentLista(null);
    HttpProvider.userAuth = null;
    this.nav.setRoot("LoginPage");
  }
}
