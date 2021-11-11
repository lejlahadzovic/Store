using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;

        private readonly IMapper _mapper;
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
        IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {//   this is going to achieve the same thing that we did before, except it's going to run asynchronously 
         //and we're creating a task that's going to pass after our request to a delegate.
         //And it's not going to wait and block the threads that this is running on until that task is completed.
         //var products = await _context.Products.ToListAsync();//when we call this command  tolist this is gonna select the query in our data base and return the results and install them in the var
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return Ok(_mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));

        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
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