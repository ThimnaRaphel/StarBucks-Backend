using Microsoft.AspNetCore.Mvc;
using StarBucks_Backend.Models;
using StarBucks_Backend.Services;

namespace StarBucks_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        public IProductService _productService;
        public ISalesService _salesService;
        public IAuthService _authService;

        public ShopController(IProductService productService, ISalesService salesService,IAuthService authService)
        {
            _productService = productService;
            _salesService = salesService;
            _authService = authService;
        }

        [HttpPost("signup")]
        public IActionResult SignUp(User user)
        {
            try
            {
                _authService.SignUp(user);
                return Ok(new { Status = "success", Message = "User signed up successfully." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Status = "fail", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(string userName, string password)
        {
            if (_authService.Login(userName, password, out var role))
            {
                return Ok(new { Status = "success", Role = role });
            }
            return Unauthorized(new { Status = "fail", Error = "Invalid credentials" });
        }

        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<Products>>> Get()
        {
            try
            {
                var products = _productService.GetProducts();
                var response = new ApiResponse<IEnumerable<Products>>
                {
                    Status = "success",
                    Data = products,
                    Count = products.Count(),
                    Error = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<Products>>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ApiResponse<Products>> GetById(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {

                    var response = new ApiResponse<Products>
                    {
                        Status = "success",
                        Data = product,
                        Error = null
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<Products>
                    {
                        Status = "fail",
                        Data = null,
                        Error = $"Product with ID {id} not found."
                    };
                    return NotFound(response);
                }

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<Products>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost(Name = "AddProduct")]
        public IActionResult AddProduct(Products product)
        {
            try
            {
                _productService.AddProducts(product);
                var response = new ApiResponse<Products>
                {
                    Status = "success",
                    Data = product,
                    Error = null
                };
                return CreatedAtAction(nameof(GetById), new { id = product.ID }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("BuyProduct")]
        public ActionResult<ApiResponse<string>> BuyProductById(int ProductId, int Quantity)
        {
            try
            {
                var result = _productService.BuyProduct(ProductId, Quantity);

                if (result == "Purchase successful")
                {
                    var response = new ApiResponse<string>
                    {
                        Status = "success",
                        Data = result,
                        Count = null,
                        Error = null
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>
                    {
                        Status = "fail",
                        Data = null,
                        Count = null,
                        Error = result
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Count = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpDelete("DeleteProduct")]
        public ActionResult<ApiResponse<string>> DeleteProduct(int productId)
        {
            try
            {
                var product = _productService.GetProductById(productId);
                if (product != null)
                {
                    var result = _productService.DeleteProduct(productId);
                    var response = new ApiResponse<string>
                    {
                        Status = "success",
                        Data = result,
                        Error = null
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>
                    {
                        Status = "fail",
                        Data = null,
                        Error = $"Product with ID {productId} not found."
                    };
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("GetSalesRecord")]
        public ActionResult<ApiResponse<IEnumerable<Sales>>> GetSales()
        {
            try
            {
                var sales = _salesService.GetSalesRecord();
                var response = new ApiResponse<IEnumerable<Sales>>
                {
                    Status = "success",
                    Data = sales,
                    Count = sales.Count(),
                    Error = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<Sales>>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }


    }
}
