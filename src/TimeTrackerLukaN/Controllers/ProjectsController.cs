using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp;
using TimeTrackerLukaN.Data;
using TimeTrackerLukaN.Models;

namespace TimeTrackerLukaN.Controllers
{
    [ApiController]
    [Route("/api/projects")]
    public class ProjectsController : Controller
    {
        private readonly TimeTrackerDbContext _dbContext;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(TimeTrackerDbContext dbContext, ILogger<ProjectsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProjectModel>> GetById(long id)
        //{
        //    _logger.LogInformation($"Get project by id: {id}");
        //    var project = await _dbContext.Projects
        //        .Include(x => x.Client)
        //        .SingleOrDefaultAsync(x => x.Id == id);

        //    if (project == null)
        //    {
        //        _logger.LogWarning($"project with id {id} is not found");
        //        return NotFound();
        //    }

        //    return ProjectModel.FromProject(project);
        //}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectModel>> GetById(long id)
        {
            _logger.LogDebug($"Getting a project with id {id}");

            var project = await _dbContext.Projects
                .Include(x => x.Client)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                _logger.LogWarning($"project with id {id} is not found");
                return NotFound();
            }

            return ProjectModel.FromProject(project);
        }

        [HttpGet]
        public async Task<ActionResult<PagedListModel<ProjectModel>>> GetPage(int page = 1, int size = 5)
        {
            _logger.LogInformation($"Getting a page {page} of users with page size {size}");

            var projects = await _dbContext.Projects
                .Include(x => x.Client)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            var totalProjects = await _dbContext.Projects.CountAsync();

            return new PagedListModel<ProjectModel>
            {
                Items = projects.Select(ProjectModel.FromProject),
                Page = page,
                PageSize = size,
                TotalCount = totalProjects
            };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectModel>> Update(long id, ProjectInputModel model)
        {
            _logger.LogInformation($"update user with id: {id}");

            var project = await _dbContext.Projects.FindAsync(id);
            var client = await _dbContext.Clients.FindAsync(model.ClientId);


            if (project == null || client == null)
            {
                _logger.LogWarning($"user with id {id} is not found");

                return NotFound();
            }

            project.Client = client;
            model.MapTo(project);

            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();

            return ProjectModel.FromProject(project);



        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectModel>> Create(ProjectInputModel model)
        {
            _logger.LogInformation($"create new project with name: {model.Name}");
            var client = await _dbContext.Clients.FindAsync(model.ClientId);
            if (client == null)
            {
                return NotFound();
            }

            var project = new Domain.Project {Client = client};
            model.MapTo(project);

            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            var resultModel = ProjectModel.FromProject(project);

            return CreatedAtAction(nameof(GetById), "projects", new {id = project.Id}, resultModel);

        }
        [HttpDelete("{id}")]

        public async Task<ActionResult<ProjectModel>> Delete(long id)
        {
            _logger.LogInformation($"delete project with id: {id}");
            var project = await _dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogWarning($"project with id {id} does not exist");
                return NotFound();
            }
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return Ok();


        }
    }
}