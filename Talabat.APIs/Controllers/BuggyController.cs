using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : APIBaseController
    {
        private readonly StoreDbContext _dbContext;

        public BuggyController(StoreDbContext dbContext)
        {
           _dbContext = dbContext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if(product == null) return NotFound(new ApiResponse(404));
            return Ok(product);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var product=_dbContext.Products.Find(100);
            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id) 
        {
            return Ok();
        }
    }
}
