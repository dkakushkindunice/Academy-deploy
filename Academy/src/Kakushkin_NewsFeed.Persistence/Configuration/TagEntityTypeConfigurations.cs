using Kakushkin_NewsFeed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kakushkin_NewsFeed.Persistence.Configuration;

public class TagEntityTypeConfigurations : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id).HasColumnName("tag_id");
        
        builder.Property(t => t.Title)
            .HasColumnName("title")
            .IsRequired()
            .IsUnicode();
        
        builder.HasIndex(t => t.Title)
            .IsUnique();
        
        
    }
}
