using HMS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DAL.Configurations
{
    public class PatienceCommentConfiguration : IEntityTypeConfiguration<PatienceComment>
    {
        public void Configure(EntityTypeBuilder<PatienceComment> builder)
        {
            builder.ToTable("PatienceComments");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.PatienceFullname)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(x=>x.Comment)
                .IsRequired()
                .HasMaxLength(256);
            builder.HasOne(x => x.Patience)
                .WithOne(x => x.PatienceComment)
                .HasForeignKey<PatienceComment>(x => x.PatienceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
