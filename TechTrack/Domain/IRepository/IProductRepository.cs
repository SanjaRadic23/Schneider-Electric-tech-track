using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

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
    }
}
