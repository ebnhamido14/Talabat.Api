using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo, IMapper mapper,IGenericRepository<ProductType> TypeRepo,IGenericRepository<ProductBrand> BrandRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
        }
        //Get All Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var Specs = new ProductWithBrandAndTypeSpecifications();
            var Products = await _productRepo.GetAllWithSpecsAsync(Specs);
            var MappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(Products);
            return Ok(MappedProducts);
        }

        //GetProductById
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specs = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _productRepo.GetByIdWithSpecsAsync(specs);
            if (Product == null)
                return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetTypes()
        {
            var types =  await _typeRepo.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}
