using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using skinet.Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
 
    public class ProductsController : BaseApiController
    {
 
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo,IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;

            
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDtos>>> GetProducts()
        {
            var spec=new ProductWithTypesAndBrandsSpecification();
            var products=await _productsRepo.GetListAsync(spec);
            return Ok(_mapper
            .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDtos>>(products));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) ,StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
        {
            var spec=new ProductWithTypesAndBrandsSpecification(id);
            var product= await _productsRepo.GetEntityWithSpec(spec);
            if(product==null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product,ProductToReturnDtos>(product);
        }
         [HttpGet("brands")]
        public async Task<ActionResult<List<Product>>> GetProductBrands()
        {
            var products=await _productBrandRepo.GetAllListAsync();
            return Ok(products);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<Product>>> GetProductTypes()
        {
            var products=await _productTypeRepo.GetAllListAsync();
            return Ok(products);
        }
    }
}