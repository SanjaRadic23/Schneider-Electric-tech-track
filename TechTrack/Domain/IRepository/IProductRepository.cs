using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;
using static TechTrack.Repository.ProductRepository;

namespace TechTrack.Domain.IRepository
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        void Add(Product product);
        Product? GetById(int Id);
        int NextId();
        bool Delete(int id);
        Product? GetByNameAndSupplierId(string name, int id);
        List<Product> Search(string searchTerm);
        void Update(Product product);
        Product? GetBySupplierId(int Id);
        List<Product> FilterByPriceRange(decimal minPrice, decimal maxPrice, ProductFilterDelegate filter);
        List<Product> GetProductsInPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> GetProductsByName(string name);
        List<Product> GetAvailableProducts();
    }
}
