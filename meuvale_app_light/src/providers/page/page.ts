import { Injectable } from '@angular/core';

@Injectable()
export class PageProvider {

    public static _pageSelected: any;
  public static _tabSelected: any;
    
    constructor() { };

    public static setCurrentStore(page: string, tab: number): any {
        PageProvider._pageSelected = page;
        PageProvider._tabSelected = tab;
      }

}