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
    public class ProductIdToProductName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int productId = (int)value;
            Product? product = new Product();
            product = ProductService.GetInstance().GetById(productId);
            if (product != null)
                return product.Name;

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
