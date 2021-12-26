using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiMongo.Entities;
using WebApiMongo.Models;
using WebApiMongo.Services;

namespace WebApiMongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortController
    {
        private readonly PortService _portsService;

        public PortController(PortService portsService) =>
            _portsService = portsService;

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Port>> Get(string id)
        {
            var port = await _portsService.GetAsync(id);

            if (port is null)
            {
                return new NotFoundObjectResult(nameof(Port));
            }

            return port;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Port newPort)
        {
            await _portsService.CreateAsync(newPort);

            return new CreatedAtActionResult(nameof(Get), nameof(PortController), new { id = newPort.Id }, newPort);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Port updatedPort)
        {
            var port = await _portsService.GetAsync(id);

            if (port is null)
            {
                return new NotFoundObjectResult(nameof(Port));
            }

            updatedPort.Id = port.Id;

            await _portsService.UpdateAsync(id, updatedPort);

            return new NoContentResult();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var port = await _portsService.GetAsync(id);

            if (port is null)
            {
                return new NotFoundObjectResult(nameof(Port));
            }

            await _portsService.RemoveAsync(port.Id);

            return new NoContentResult();
        }

        [HttpGet("findMatch")]
        public async Task<ActionResult<List<Port>>> FindMatch(string name)
        {
            var ports = await _portsService.FindMatch(name);

            return ports;
        }
    }
}
