using DijitaruVatigoGengar.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DijitaruVatigoGengar.Data.Configs;

public class CollaboratorConfig : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder
            .HasKey(collaborator => collaborator.Id);

        builder.Property(collaborator => collaborator.Name)
            .IsRequired();

        builder.Property(collaborator => collaborator.Gender)
            .HasColumnType("nchar(1)")
            .IsRequired();

        builder.Property(collaborator => collaborator.BirthDate)
            .IsRequired();

        builder.Property(collaborator => collaborator.Modality)
            .IsRequired();

        builder.Property(collaborator => collaborator.CollaboratorRole)
            .IsRequired();

        // Configuração da relação um-para-muitos com PendingHour
        builder.HasMany(collaborator => collaborator.PendingHours)
            .WithOne(pendingHour => pendingHour.Collaborator)
            .HasForeignKey(pendingHour => pendingHour.CollaboratorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(collaborator => collaborator.PendingHours)
               .WithOne(pendingHour => pendingHour.Collaborator)
               .HasForeignKey(pendingHour => pendingHour.CollaboratorId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        // Configuração da relação muitos-para-muitos com Project
        builder.HasMany(collaborator => collaborator.ProjectCollaborators)
            .WithOne(projectCollaborator => projectCollaborator.Collaborator)
            .HasForeignKey(projectCollaborator => projectCollaborator.CollaboratorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
