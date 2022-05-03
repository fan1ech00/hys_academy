using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace webapi.Entities.Configurations;

public class UserEntityTypeConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        //
        // builder.Property(e => e.Name)
        //     .IsRequired()
        //     .HasMaxLength(100)
        //     .IsUnicode(true);
        //
        // builder.Property(e => e.Age)
        //     .IsRequired();
    }
}