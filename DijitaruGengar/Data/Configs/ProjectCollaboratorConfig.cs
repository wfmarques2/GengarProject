using DijitaruVatigoGengar.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DijitaruVatigoGengar.Data.Configs;

public class ProjectCollaboratorConfig : IEntityTypeConfiguration<ProjectCollaborator>
{
    public void Configure(EntityTypeBuilder<ProjectCollaborator> builder)
    {
        // Configuração da chave primária composta
        builder
            .HasKey(projectCollaborator => new { projectCollaborator.CollaboratorId, projectCollaborator.ProjectId });

        // Configuração da relação muitos-para-muitos com Collaborator
        builder
            .HasOne(projectCollaborator => projectCollaborator.Collaborator)
            .WithMany(collaborator => collaborator.ProjectCollaborators)
            .HasForeignKey(projectCollaborator => projectCollaborator.CollaboratorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração da relação muitos-para-muitos com Projeto
        builder
            .HasOne(projectCollaborator => projectCollaborator.Project)
            .WithMany(project => project.ProjectCollaborators)
            .HasForeignKey(projectCollaborator => projectCollaborator.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}