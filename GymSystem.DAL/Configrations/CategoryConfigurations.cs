using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemG03.DAL.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(X => X.CategoryName)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.HasData(
                             new Category { Id = 1, CategoryName = "Cardio" },
                             new Category { Id = 2, CategoryName = "Strength" },
                             new Category { Id = 3, CategoryName = "Yoga" },
                             new Category { Id = 4, CategoryName = "Boxing" },
                             new Category { Id = 5, CategoryName = "CrossFit" }
                         );

        }
    }
}