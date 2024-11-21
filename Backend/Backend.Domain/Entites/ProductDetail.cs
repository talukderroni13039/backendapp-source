using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entites;

public partial class ProductDetail
{
    [Key]
    public int DetailID { get; set; }

    public int? ProductID { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("ProductDetails")]
    public virtual Product Product { get; set; }
}