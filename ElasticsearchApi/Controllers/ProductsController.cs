using Elastic.Clients.Elasticsearch;
using ElasticsearchAPI.Models;
using ElasticsearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ElastichsearchService service;

        public ProductsController(ElastichsearchService service)
        {
            this.service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Product product)
        {
            await service.SaveProductAsync(product);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await service.GetAllProductAsync();
            return Ok(products);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            await service.UpdateProductAsync(product);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteProductAsync(id);
            return Ok();
        }


    }
}
