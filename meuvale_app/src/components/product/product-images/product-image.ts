import { Component, OnInit, Input } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ProductLoja } from '../../../model/product-loja';

/**
 * Generated class for the ProductImageComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */

@Component({
  selector: 'product-image',
  templateUrl: 'product-image.html'
})
export class ProductImageComponent implements OnInit {

  @Input("item") private item: ProductLoja;

  constructor(public navCtrl: NavController) { }

  ngOnInit(): void {
    console.log(this.item);
  } 
}
