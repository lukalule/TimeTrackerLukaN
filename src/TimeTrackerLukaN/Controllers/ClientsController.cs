//using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackerLukaN.Data;
using TimeTrackerLukaN.Models;

namespace TimeTrackerLukaN.Controllers
{
    [ApiController]
    [Route("/api/clients")]
    public class ClientsController : Controller
    {
        private readonly TimeTrackerDbContext _dbContext;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(TimeTrackerDbContext dbContext,ILogger<ClientsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientModel>> GetById(long id)
        {
            _logger.LogInformation($"Get client by id: {id}");
            var client = await _dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning($"Client with id: {id} is not found");
                return NotFound();
            }
            return ClientModel.FromClient(client);

        }
        [HttpGet]
        public async Task<ActionResult<PagedListModel<ClientModel>>> GetPage(int page = 1, int size = 5)
        {
            _logger.LogInformation($"Getting a page {page} and page size {size}");

            var clients = await _dbContext.Clients
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            var totalClients = await _dbContext.Clients.CountAsync();

            var pagedModel = new PagedListModel<ClientModel>
            {
                Items = clients.Select(ClientModel.FromClient),
                Page = page,
                PageSize = size,
                TotalCount = totalClients
            };
            return pagedModel;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientModel>> Create(ClientInputModel model)
        {
            _logger.LogInformation($"create user with name {model.Name}");
            var client = new Domain.Client();
            model.MapTo(client);

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            var resultModel = ClientModel.FromClient(client);

            return CreatedAtAction(nameof(GetById), "clients", new { id = client.Id }, resultModel);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ClientModel>> Update(long id,ClientInputModel model)
        {
            _logger.LogInformation($"Update Client with id: {id}");
            var client = await _dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning($"No user found with id: {id}");
                return NotFound();
            }
            model.MapTo(client);
            _dbContext.Clients.Update(client);

            await _dbContext.SaveChangesAsync();
            return ClientModel.FromClient(client);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientModel>> Delete(long id)
        {
            _logger.LogInformation($"Deleting client with id: {id}");
            var client = await _dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning($"No user found with id: {id}");
                return NotFound();
            }
            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}