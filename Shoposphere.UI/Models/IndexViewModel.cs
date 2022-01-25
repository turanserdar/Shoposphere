using Shoposphere.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI.Models
{
    public class IndexViewModel
    {

        public List<CategoryViewModel> Categories { get; set; }
        public List<ProductViewModel> Products { get; set; }


    }
}
