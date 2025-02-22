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
    public class PatienceConfiguration : IEntityTypeConfiguration<Patience>
    {
        public void Configure(EntityTypeBuilder<Patience> builder)
        {
            builder.ToTable("Patiences");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Fullname)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(x=>x.Gender)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(x => x.DateOfBirth)
                .IsRequired();

            builder.HasMany(x=>x.Appointments)
                .WithOne(x=>x.Patience)
                .HasForeignKey(x=>x.PatienceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.User)
                .WithOne(x=>x.Patience)
                .HasForeignKey<Patience>(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
