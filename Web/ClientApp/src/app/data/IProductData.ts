export interface IProductData {
  id: number;
  name: string;
  photo: string;
  price: number;
  lastUpdate: string;
}  

export class ProductData implements IProductData {
  constructor() {
  }
  id: number;
  name: string;
  photo: string;
  price: number;
  lastUpdate: string;
}
