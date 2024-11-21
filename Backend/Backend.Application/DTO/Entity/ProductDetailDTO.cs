using Backend.Domain.Entites;

namespace Backend.Application.DTO.Entity;

public partial class ProductDetailDTO : ProductDetail
{
    public int tag { get; set; }
}