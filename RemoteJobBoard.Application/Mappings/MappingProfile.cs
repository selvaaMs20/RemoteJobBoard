// MappingProfile.cs
using AutoMapper;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Application.DTOs.Company;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;
using RemoteJobBoard.Application.DTOs.Skill;
using RemoteJobBoard.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // JobPost mappings
        CreateMap<JobPost, JobPostDto>()
            .ForMember(dest => dest.JobType,
                opt => opt.MapFrom(src => src.JobType.ToString()))
            .ForMember(dest => dest.WorkMode,
                opt => opt.MapFrom(src => src.WorkMode.ToString()))
            .ForMember(dest => dest.ExperienceLevel,
                opt => opt.MapFrom(src => src.ExperienceLevel.ToString()))
            .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.CompanyLogoUrl,
                opt => opt.MapFrom(src => src.Company.LogoUrl))
            .ForMember(dest => dest.CompanyLocation,
                opt => opt.MapFrom(src => src.Company.Location))
            .ForMember(dest => dest.RequiredSkills,
                opt => opt.MapFrom(src =>
                    src.RequiredSkills.Select(s => s.Skill.Name).ToList()));

        CreateMap<CreateJobPostDto, JobPost>();
        CreateMap<UpdateJobPostDto, JobPost>();

        // Company mappings
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.AverageRating,
                opt => opt.MapFrom(src =>
                    src.Reviews.Any()
                        ? src.Reviews.Average(r => r.Rating)
                        : 0))
            .ForMember(dest => dest.TotalReviews,
                opt => opt.MapFrom(src => src.Reviews.Count));

        CreateMap<CreateCompanyDto, Company>();

        // Application mappings
        CreateMap<Core.Entities.Application, ApplicationDto>()
            .ForMember(dest => dest.JobTitle,
                opt => opt.MapFrom(src => src.JobPost.Title))
            .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.JobPost.Company.Name))
            .ForMember(dest => dest.ApplicantName,
                opt => opt.MapFrom(src => src.JobSeekerProfile.User.Name))
            .ForMember(dest => dest.ApplicantAvatarUrl,
                opt => opt.MapFrom(src => src.JobSeekerProfile.User.AvatarUrl))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.AppliedAt,
                opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<CreateApplicationDto, Core.Entities.Application>();

        // JobSeekerProfile mappings
        CreateMap<JobSeekerProfile, JobSeekerProfileDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.AvatarUrl,
                opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ForMember(dest => dest.Skills,
                opt => opt.MapFrom(src =>
                    src.Skills.Select(s => s.Skill.Name).ToList()));

        CreateMap<UpdateJobSeekerProfileDto, JobSeekerProfile>();

        // Skill mappings
        CreateMap<Skill, SkillDto>();
    }
}
