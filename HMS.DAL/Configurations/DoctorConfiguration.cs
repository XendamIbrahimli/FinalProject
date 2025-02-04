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
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Fullname)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(x=>x.FullInfo)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(x=>x.ImageUrl)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(x => x.ExperienceYear)
                .IsRequired();

            builder.HasOne(x=>x.User)
                .WithOne(x=>x.Doctor)
                .HasForeignKey<Doctor>(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Doctors)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x=>x.Appointments)
                .WithOne(x=>x.Doctor)
                .HasForeignKey(x=>x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
