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
    public class SupplierService
    {
        private ISupplierRepository SupplierRepository { get; set; }
        public SupplierService(ISupplierRepository supplierRepository)
        {
            SupplierRepository = supplierRepository;
        }
        public static SupplierService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<SupplierService>();
        }
        public void Add(Supplier newSupplier)
        {
            SupplierRepository.Add(newSupplier);
        }
        public Supplier? GetById(int Id)
        {
            return SupplierRepository.GetById(Id);
        }
        public List<Supplier> GetAll()
        {
            return SupplierRepository.GetAll();
        }
        public bool Delete(int id)
        {
            return SupplierRepository.Delete(id);
        }
        public List<Supplier> SearchSuppliers(string searchTerm)
        {
            return SupplierRepository.SearchSuppliers(searchTerm);
        }
        public void Update(Supplier supplier)
        {
            SupplierRepository.Update(supplier);
        }
    }
}
