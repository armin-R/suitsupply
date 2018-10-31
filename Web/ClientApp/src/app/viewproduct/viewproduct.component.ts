import { Component, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductsSrv } from "../services/ProductsSrv.service"
import ProductData = require("../data/IProductData");
import IProductData = ProductData.IProductData;

@Component({
  selector: 'view-product',
  templateUrl: './viewproduct.component.html'
})
export class ViewProduct implements OnInit {
  id: number = null;
  currentProduct: IProductData;

  constructor(private activatedRoute: ActivatedRoute,
    private productsService: ProductsSrv,
    private router: Router) {
    if (this.activatedRoute.snapshot.params["id"]) {
      this.id = this.activatedRoute.snapshot.params["id"];
    }

  }

  ngOnInit() {
    if (this.id !== null) {
      this.productsService.getProductById(this.id)
        .subscribe(
          resp => {
            this.currentProduct = <ProductData.IProductData>(resp);
          },
          error => alert(error));
    }
  }

  goBack() {
    this.router.navigate(['/fetch-product']);
  }

}
