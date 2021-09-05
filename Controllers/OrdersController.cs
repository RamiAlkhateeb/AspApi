using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        
        private ILogger _logger;

        
        public OrdersController(AppDb db,ILogger<OrdersController> logger)
        {
            _logger = logger;
            Db = db;
            
        }

        public AppDb Db { get; }

         // GET api/orders
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new OrdersQuery(Db);
            var result = await query.LatestUsersAsync();
            return new OkObjectResult(result);
        }

        

        // POST api/orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Order body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // GET api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new OrdersQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // PUT api/orders/5
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutOne(int id, [FromBody]Product body)
        // {
        //     await Db.Connection.OpenAsync();
        //     var query = new OrdersQuery(Db);
        //     var result = await query.FindOneAsync(id);
        //     if (result is null)
        //         return new NotFoundResult();
        //     result.Name = body.Name;
        //     result.Description = body.Description;
        //     await result.UpdateAsync();
        //     return new OkObjectResult(result);
        // }

        // DELETE api/blog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new OrdersQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/blog
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new OrdersQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
    }
}
