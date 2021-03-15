using DepsWebApp.Models;
using DepsWebApp.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using DepsWebApp.Services;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DepsWebApp.Controllers
{
    /// <summary>
    /// Controller for Authorization and Registration of new
    /// users
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly UserStorageService _storage;

        /// <summary>
        /// public constructor of Authorization controller
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AuthorizationController(ILoggerFactory loggerFactory, UserStorageService storage)
        {
            _logger = loggerFactory.CreateLogger<AuthorizationController>();
            _storage = storage;
        }

        /// <summary>
        /// Post method for registration
        /// </summary>
        /// <param name="model">User login and password</param>
        /// <returns>Exception code</returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(ProcessedError), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterValidationModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            if(! await _storage.TryAddUserAsync(model)) return Conflict();
            return Ok();
        }
    }
}
