using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TechTrack.Domain.Model;
using TechTrack.Service;

namespace TechTrack.Helpers.Converters
{
    public class SupplierIdToSupplierName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int supplierId = (int)value;
            Supplier? supplier = new Supplier();
            supplier = SupplierService.GetInstance().GetById(supplierId);
            if (supplier != null)
                return supplier.Name;

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
