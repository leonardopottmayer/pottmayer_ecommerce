using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Users.Entities;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.Data.Modules.Users.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("usr001_user");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.Key).HasColumnName("key").HasColumnType("UUID").IsRequired();
            builder.Property(u => u.Username).HasColumnName("username").HasMaxLength(255).IsRequired();
            builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            builder.Property(u => u.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            builder.Property(u => u.Role).HasColumnName("role").HasConversion<int>().IsRequired();
            builder.Property(u => u.Status).HasColumnName("status").HasConversion<int>().IsRequired();
            builder.Property(u => u.Password).HasColumnName("password").IsRequired();
            builder.Property(u => u.PasswordSalt).HasColumnName("password_salt").IsRequired();

            builder.HasIndex(u => u.Key).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
