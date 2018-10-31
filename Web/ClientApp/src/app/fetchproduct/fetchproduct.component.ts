import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductsSrv } from "../services/ProductsSrv.service"
import ProductData = require("../data/IProductData");
import IProductData = ProductData.IProductData;

@Component({
  selector: 'fetchproduct',
  templateUrl: './fetchproduct.component.html'
})

export class FetchProductComponent {
  nameQuery: string = "";
  excelLink: string = "#";
  public productsList: IProductData[];

  constructor(public http: HttpClient, private router : Router, private productsSrv: ProductsSrv) {
    this.getProducts();
  }

  getProducts() {
    this.excelLink = this.productsSrv.getExcelLink(this.nameQuery);
    this.productsSrv.getProducts(this.nameQuery).subscribe(
      data => this.productsList = data
    );
  }

  delete(productId) {
    var ans = confirm("Do you want to delete product with Id: " + productId);
    if (ans) {
      this.productsSrv.deleteEmployee(productId).subscribe((data) => {
          this.getProducts();
        },
        error => console.error(error));
    }
  }
}
