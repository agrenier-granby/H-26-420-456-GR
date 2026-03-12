using Exercices_Formulaire_Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exercices_Formulaire_Web.Data.Configuration
{
    public class DepartementConfiguration : IEntityTypeConfiguration<Departement>
    {
        public void Configure(EntityTypeBuilder<Departement> builder)
        {
            builder.Property(d => d.Nom)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
