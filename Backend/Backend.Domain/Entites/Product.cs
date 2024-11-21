using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entites;

[Table("Product")]
public partial class Product
{
    [Key]
    public int ProductID { get; set; }

    [StringLength(15)]
    public string Name { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
 
    public decimal? Price { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}