using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;

namespace TechTrack.Service
{
    public class ProductService
    {
        private IProductRepository ProductRepository { get; set; }
        public ProductService(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }
        public static ProductService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ProductService>();
        }
        public void Add(Product newProduct)
        {
            ProductRepository.Add(newProduct);
        }
        public Product? GetById(int Id)
        {
            return ProductRepository.GetById(Id);
        }
        public List<Product> GetAll()
        {
            return ProductRepository.GetAll();
        }
        public bool Delete(int id)
        {
            return ProductRepository.Delete(id);
        }

        public Product? GetByNameAndSupplierId(string name, int id)
        {
            return ProductRepository.GetByNameAndSupplierId(name, id);
        }

        public List<Product> Search(string searchTerm)
        {
            return ProductRepository.Search(searchTerm);
        }

        public void Update(Product newProduct)
        {
            ProductRepository.Update(newProduct);
        }
    }
}
