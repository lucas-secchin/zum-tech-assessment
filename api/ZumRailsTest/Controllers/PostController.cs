using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZumRailsTest.Models.DTOs;
using ZumRailsTest.Services.Interface;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZumRailsTest.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : Controller
    {
        IBooksService _booksService;

        public PostController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] PostInputDto request)
        {
            (var isValid, var errorMessage) = _booksService.ValidateInput(request);

            if (!isValid)
            {
                return BadRequest(errorMessage);
            }

            var result = await _booksService.GetBooks(request);
            
            return Ok(result);
        }
    }
}

