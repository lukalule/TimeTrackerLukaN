using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TimeTrackerLukaN.Controllers
{
    public class DummyAuthController : Controller
    {
        private readonly IConfiguration _configuration;
        // WARNING: For demo only
        public DummyAuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/get-token")]
        public IActionResult GetToken(string name = "etf-workshop", bool admin = false)
        {
            var jwt = JwtTokenGenerator.Generate(name, admin, _configuration["Tokens:Issuer"], _configuration["Tokens:Key"]);

            return Ok(jwt);
        }
    }
}