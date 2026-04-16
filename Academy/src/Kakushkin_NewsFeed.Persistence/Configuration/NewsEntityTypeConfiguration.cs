using Kakushkin_NewsFeed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kakushkin_NewsFeed.Persistence.Configuration;

public class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable("news");

        builder.HasKey(x => x.Id);

        builder.Property(t => t.Id).HasColumnName("id");
        
        builder
            .Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(160)
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_news_title_minlength",
              "length(title) > 3");

        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(160)
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_news_description_minlength",
              "length(description) > 3");
        
        builder
            .Property(x => x.Image)
            .HasColumnName("image")
            .HasMaxLength(160)
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_news_image_minlength",
              "length(image) > 3");
        
        builder.Property(x => x.AuthorId)
            .HasColumnName("author_id");
        
        builder.HasOne(x=>x.Author)
            .WithMany(x=>x.News)
            .HasForeignKey(x=>x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
