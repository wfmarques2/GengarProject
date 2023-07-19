using DijitaruVatigoGengar.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DijitaruVatigoGengar.Data.Configs;

public class ProjectCollaboratorConfig : IEntityTypeConfiguration<ProjectCollaborator>
{
    public void Configure(EntityTypeBuilder<ProjectCollaborator> builder)
    {
        // Configuração da chave primária composta
        builder.HasKey(pc => new { pc.CollaboratorId, pc.ProjectId });

        // Configuração das chaves estrangeiras
        builder.HasOne(pc => pc.Collaborator)
            .WithMany(c => c.ProjectCollaborators)
            .HasForeignKey(pc => pc.CollaboratorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pc => pc.Project)
            .WithMany(p => p.ProjectCollaborators)
            .HasForeignKey(pc => pc.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}