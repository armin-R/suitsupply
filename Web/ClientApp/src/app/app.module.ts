import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchProductComponent } from "./fetchproduct/fetchproduct.component"
import { CreateProduct } from "./addproduct/addproduct.component"
import { ProductsSrv } from './services/ProductsSrv.service';
import { ViewProduct } from "./viewproduct/viewproduct.component"

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchProductComponent,
    CreateProduct,
    ViewProduct
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'fetch-product', component: FetchProductComponent },
      { path: 'register-product', component: CreateProduct },
      { path: 'product/edit/:id', component: CreateProduct },
      { path: 'product/view/:id', component: ViewProduct }
    ])
  ],
  providers: [ProductsSrv],
  bootstrap: [AppComponent]
})
export class AppModule {
}
