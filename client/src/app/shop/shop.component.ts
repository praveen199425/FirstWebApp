import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search',{static:true}) searchTerm:ElementRef;
  products:IProduct[];
  brands:IBrand[];
  producttype:IProductType[];
  shopParams=new ShopParams();
  sortOptions=[
    {name:'Alphabetical', value:'name'},
    {name:'Price: Low to High', value:'priceAsc'},
    {name:'Price: High to Low', value:'priceDesc'},
  
  ];
  totalCount:number;


  constructor(private shopServices:ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getProductTypes();   
  }
  getProducts(){
    this.shopServices.getProduct(this.shopParams).subscribe(response=>{
      this.products=response.data;
      this.shopParams.pageNumber=response.pageIndex;
      this.shopParams.pageSize=response.pageSize;
      this.totalCount=response.count;
    },error=>{console.error();}
    );
  }
  getBrands(){
    this.shopServices.getBrands().subscribe(response=>{
      this.brands=[{id:0,name:"ALL"},...response];
    },error=>{console.error();}
    );
  }

  getProductTypes(){
    this.shopServices.getProductTypes().subscribe(response=>{
      this.producttype=[{id:0,name:"ALL"},...response];
    },error=>{console.error();}
    );
  }

  onBrandSelected(brandId:number){
    this.shopParams.brandId=brandId;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }
  onTypeSelected(typeId:number){
    this.shopParams.typeId=typeId;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }

  onSortSelected(sort:string){
    this.shopParams.sort=sort;
    this.getProducts();
  }
  onPageChanged(event:any){
    if(this.shopParams.pageNumber!==event){
    this.shopParams.pageNumber=event;
    this.getProducts();
    }
  }

  onSearch(){
    this.shopParams.search=this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }
  onReset(){
    this.searchTerm.nativeElement.value='';
    this.shopParams=new ShopParams();
    this.getProducts();
  }

}
