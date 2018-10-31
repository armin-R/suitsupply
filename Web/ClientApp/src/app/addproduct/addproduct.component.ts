import { Component, OnInit, ViewChild } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FetchProductComponent } from "../fetchproduct/fetchproduct.component"
import { ProductsSrv } from "../services/ProductsSrv.service"
import ProductData = require("../data/IProductData");
import IProductData = ProductData.IProductData;

@Component({
  selector: 'create-product',
  templateUrl: './addproduct.component.html'
})
export class CreateProduct implements OnInit {
  productForm: FormGroup;
  title: string = "Create";
  id: number;
  errorMessage: any;
  currentProduct: IProductData = new ProductData.ProductData();
  @ViewChild("fileInput") fileInput;

  constructor(private builder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private productsService: ProductsSrv,
    private router: Router) {
    if (this.activatedRoute.snapshot.params["id"]) {
      this.id = this.activatedRoute.snapshot.params["id"];
    }
    this.productForm = this.builder.group({
      id: 0,
      name: [this.currentProduct.name, [Validators.required]],
      price: [this.currentProduct.price, [Validators.required]],
      photo: 0,

    });
  }

  ngOnInit() {
    if (this.id > 0) {
      this.title = "Edit";
      this.productsService.getProductById(this.id)
        .subscribe(
          resp => {
            this.currentProduct = <ProductData.IProductData>(resp);
            this.productForm.setValue(resp);
          },
          error => this.errorMessage = error);
    }
  }

  save() {

    const formData = new FormData();
    formData.set("name", this.name.value);
    formData.set("price", this.price.value);

    let fi = this.fileInput.nativeElement;
    if (fi.files && fi.files[0]) {
      let fileToUpload = fi.files[0];
      formData.append("photo", fileToUpload, fileToUpload.name);
    }

    if (this.title == "Create") {
      this.productsService.saveProduct(formData)
        .subscribe((data) => {
            this.router.navigate(['/fetch-product']);
          },
          error => this.errorMessage = error);
    } else if (this.title == "Edit") {
      this.productsService.updateProduct(this.id, formData)
        .subscribe((data) => {
            this.router.navigate(['/fetch-product']);
          },
        error => this.errorMessage = error);
    }
  }

  cancel() {
    this.router.navigate(['/fetch-product']);
  }

  get name() { return this.productForm.get('name'); }

  get price() { return this.productForm.get('price'); }
}
