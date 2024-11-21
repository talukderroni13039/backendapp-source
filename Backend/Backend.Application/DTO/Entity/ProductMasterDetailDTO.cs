using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTO.Entity
{
    public class ProductMasterDetailDTO
    {
        public ProductDTO productDTO { get; set; }
        public List<ProductDetailDTO> productDetilsDTO { get; set; }
    }
}
