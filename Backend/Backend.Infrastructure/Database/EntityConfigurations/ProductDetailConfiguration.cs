using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Backend.Domain.Entites;

namespace Backend.Infrastructure.Database.EntityConfigurations
{
    public partial class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> entity)
        {
           
            entity.HasKey(e => e.DetailID).HasName("PK__ProductD__135C314DA6C834E0");
            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails).HasConstraintName("FK_ProductDetails_ProductID");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ProductDetail> entity);
    }
}
