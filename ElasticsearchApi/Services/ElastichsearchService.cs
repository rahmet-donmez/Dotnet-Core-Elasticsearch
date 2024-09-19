using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using ElasticsearchAPI.Models;
using System;

namespace ElasticsearchAPI.Services
{
    public class ElastichsearchService
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        public ElastichsearchService()
        {
            ElasticsearchClientSettings setting = new(new Uri("http://localhost:9200"));
            setting.DefaultIndex("products");
            ElasticsearchClient elasticsearchClient = new ElasticsearchClient(setting);
            _elasticsearchClient = elasticsearchClient;

        }

      
        public async Task<List<Product>> GetAllProductAsync()
        {
            SearchResponse<Product> searchResponse = await _elasticsearchClient.SearchAsync<Product>("products");
            if (!searchResponse.IsValidResponse)
            {
                throw new Exception("Veri listeleme işleminde hata meydana geldi");
            }

            return searchResponse.Documents.ToList();

        }
        public async Task SaveProductAsync(Product product)
        {
            CreateRequest<Product> createRequest = new(product.Id.ToString())
            {
                Document = product
            };
            var createResponse = await _elasticsearchClient.CreateAsync(createRequest);
            if (!createResponse.IsValidResponse)
            {
                throw new Exception("Veri ekleme işleminde hata meydana geldi");
            }

        }
        public async Task UpdateProductAsync(Product product)
        {

            UpdateRequest<Product, Product> updateRequest = new("products", product.Id.ToString())
            {
                Doc = product
            };
            var updateResponse = await _elasticsearchClient.UpdateAsync(updateRequest);
            if (!updateResponse.IsValidResponse)
            {
                throw new Exception("Veri güncelleme işleminde hata meydana geldi");
            }


        }
        public async Task DeleteProductAsync(int id)
        {

            DeleteRequest deleteRequest = new("products", id.ToString());
            CancellationToken cancellationToken = new CancellationToken();
            var deleteResponse = await _elasticsearchClient.DeleteAsync(deleteRequest, cancellationToken);
            if (!deleteResponse.IsValidResponse)
            {
                throw new Exception("Veri silme işleminde hata meydana geldi");

            }

        }

        //Bulk, birden fazla işlemi (ekleme, güncelleme, silme)
        //tek bir istekle gerçekleştirmeye olanak tanır
        public async Task BulkAsync(int id)
        {

            var bulkRequest = new BulkRequest();

            bulkRequest.Operations.Add(new BulkIndexOperation<Product>(new Product { Id = 1, Name = "Product 1", Price = 10 }));
            bulkRequest.Operations.Add(new BulkIndexOperation<Product>(new Product { Id = 2, Name = "Product 2", Price = 20 }));
            bulkRequest.Operations.Add(new BulkDeleteOperation<Product>("3"));  // ID'si 3 olan ürünü sil

            var bulkResponse = await _elasticsearchClient.BulkAsync(bulkRequest);

            if (!bulkResponse.IsValidResponse)
            {
                throw new Exception("Toplu işlem başarısız oldu.");
            }


        }
    }
}
