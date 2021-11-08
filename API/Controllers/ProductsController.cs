using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;

        //update the two methods to return data from the database
        //we need to inject our store context into product controller,by injecting that practicular class that will give us access to the methods in the class
        //we need to generate the constructor
        /*  private readonly StoreContext _context;

          public ProductsController(StoreContext context)
          {//when we inject something into our controllers this way or any class then its given a lifetime
           //when a request comes in it hits our productscontroller then a new instance of the productcontroller is created its gonna see what its dependencies are
           //in this case it has dependencies on store context,And this is going to create a new instance of the store context that we can then work with.Now, ASP.NET core controls the lifetime of how long this store context is available and when we added Now, ASP.NET core controls the lifetime of how long this store context is available
           // and when we added this in StartUps class ,Then this is given a very specific lifetime
              _context = context;
          }
          private readonly IProductRepository _repo;
    public ProductsController(IProductRepository repository){

        _repo=repository;

       }
    */
        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo)
        {
            _productTypeRepo = productTypeRepo;
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
        }
        [HttpGet]
        //   public async Task<ActionResult<List<Product>>> GetProducts()
        //we've got ActionResult avaliable becouse we are using ControllerBase
        //we specifie the type what are we returning example List -we are returning a list,type is products 
        //this is what we are rturning it is gonna be an action result,some kind of http response stater  
        //we have control of what we return inside this method
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {//   this is going to achieve the same thing that we did before, except it's going to run asynchronously 
         //and we're creating a task that's going to pass after our request to a delegate.
         //And it's not going to wait and block the threads that this is running on until that task is completed.
         //var products = await _context.Products.ToListAsync();//when we call this command  tolist this is gonna select the query in our data base and return the results and install them in the var
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
           return products.Select(product => new ProductToReturnDto{
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name

           }).ToList();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name

            };
        }

        [HttpGet("brands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}