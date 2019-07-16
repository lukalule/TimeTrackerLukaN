using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLukaN.Data;
using TimeTrackerLukaN.Models;

namespace TimeTrackerLukaN.Controllers
{
    /// <summary>
    /// encapsulates functionality for adding modifying and deleting users.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("/api/users")]
    public class UsersController : Controller
    {
        private readonly TimeTrackerDbContext _dbContext;
        private readonly ILogger<UsersController> _logger;
        public UsersController(TimeTrackerDbContext dbContext, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("{id}")]
        //[Route("{id}")]
        public async Task<ActionResult<UserModel>> /*IActionResult*/ /*ActionResult<UserModel>*/ GetById(long id)
        {
            _logger.LogInformation($"Getting user by:{id}");

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with id {id} is not found");
                return NotFound();
            }
            return UserModel.FromUser(user);

        }

        /// <summary>
        /// gets a single page of users.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Page size.</param>
        /// <returns>Paged list of users.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<UserModel>>> GetPage(int page = 1, int size = 5)
        {
            _logger.LogInformation($"Getting a page {page} of users with page size {size}");

            var users = await _dbContext.Users
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            var totalUsers = await _dbContext.Users.CountAsync();

            return new PagedList<UserModel>
            {
                Items = users.Select(UserModel.FromUser),
                Page = page,
                PageSize = size,
                TotalCount = totalUsers
            };
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(long id)
        {
            _logger.LogInformation($"Deleting User with id {id}");
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"No user found with id {id}");
                return NotFound();
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserModel>> Create(UserInputModel model)
        {
            _logger.LogInformation($"Creating new user with {model.Name}");

            var user = new Domain.User();
            model.MapTo(user);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var resultModel = UserModel.FromUser(user);
            return CreatedAtAction(nameof(GetById), "users", new { id = user.Id }, resultModel);

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserModel>>Update(long id, UserInputModel model)
        {
            _logger.LogInformation($"Update user with id: {id}");

            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"No user found with id {id}");
                return NotFound();
            }
            model.MapTo(user);
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();

            return UserModel.FromUser(user);



        }

    }
}