using DijitaruVatigoGengar.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DijitaruVatigoGengar.Data.Configs;

public class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder
            .HasKey(project => project.Id);

        builder.Property(project => project.Name)
            .IsRequired();

        builder.Property(project => project.Budget)
            .IsRequired();

        builder.Property(project => project.Type)
            .IsRequired();

        // Configuração da relação um-para-muitos com PendingHour
        builder.HasMany(project => project.PendingHours)
            .WithOne(pendingHour => pendingHour.Project)
            .HasForeignKey(pendingHour => pendingHour.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(project => project.PendingHours)
              .WithOne(pendingHour => pendingHour.Project)
              .HasForeignKey(pendingHour => pendingHour.ProjectId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

        // Configuração da relação muitos-para-muitos com Collaborator
        builder.HasMany(project => project.ProjectCollaborators)
            .WithOne(projectCollaborator => projectCollaborator.Project)
            .HasForeignKey(projectCollaborator => projectCollaborator.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
