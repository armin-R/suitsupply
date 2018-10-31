import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class ProductsSrv {
  myAppUrl: string = "";
  apiUrl: string = "api/v1/products/";

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.myAppUrl = baseUrl;
  }

  getProducts(name?: string) {
    if (undefined === name || null === name) name = "";
    return this.http.get(this.myAppUrl + this.apiUrl + "search/" + encodeURIComponent(name))
      .catch(this.errorHandler);
  }

  getExcelLink(name?: string) {
    if (undefined === name || null === name) name = "";
    return this.myAppUrl + this.apiUrl + "excel/" + encodeURIComponent(name);
  }


  getProductById(id: number) {
    return this.http.get(this.myAppUrl + this.apiUrl + id)
      .catch(this.errorHandler);
  }

  saveProduct(product) {
    return this.http.post(this.myAppUrl + this.apiUrl, product)
      .catch(this.errorHandler);
  }

  updateProduct(id, product) {
    return this.http.put(this.myAppUrl + this.apiUrl + id, product)
      .catch(this.errorHandler);
  }

  deleteEmployee(id) {
    return this.http.delete(this.myAppUrl + this.apiUrl + id)
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}
