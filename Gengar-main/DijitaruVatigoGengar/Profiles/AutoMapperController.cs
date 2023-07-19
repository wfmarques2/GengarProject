using AutoMapper;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Models;

namespace DijitaruVatigoGengar.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Collaborator
        CreateMap<Collaborator, CreateCollaboratorDto>().ReverseMap();
        CreateMap<Collaborator, UpdateCollaboratorDto>().ReverseMap();
        CreateMap<Collaborator, ReadCollaboratorDto>();
        CreateMap<CreateCollaboratorDto, Collaborator>();

        // Project
        CreateMap<Project, CreateProjectDto>().ReverseMap();
        CreateMap<Project, UpdateProjectDto>().ReverseMap();
        CreateMap<Project, ReadProjectDto>();
        CreateMap<CreateProjectDto, Project>();

        // PendingHour
        CreateMap<PendingHour, CreatePendingHourDto>().ReverseMap();
        CreateMap<PendingHour, UpdatePendingHourDto>().ReverseMap();
        CreateMap<PendingHour, ReadPendingHourDto>();
        CreateMap<CreatePendingHourDto, PendingHour>();
    }
}
