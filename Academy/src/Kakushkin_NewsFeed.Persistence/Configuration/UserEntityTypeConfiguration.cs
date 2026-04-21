using Kakushkin_NewsFeed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kakushkin_NewsFeed.Persistence.Configuration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");
        
        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasCheckConstraint(
            "CK_user_email_minlength",
              "length(email) > 3");
        
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasMaxLength(25)
            .IsRequired();
        
        builder.HasCheckConstraint(
            "CK_user_name_minlength",
              "length(name) > 3");

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password")
            .IsRequired();

        builder.Property(u => u.Avatar)
            .HasColumnName("avatar")
            .IsRequired();

        builder.HasMany(user => user.News)
            .WithOne(news => news.Author)
            .HasForeignKey(news => news.AuthorId);
    }
}
