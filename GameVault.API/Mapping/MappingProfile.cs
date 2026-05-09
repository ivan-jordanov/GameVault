
using System;
using System.Linq;
using AutoMapper;
using GameVault.API.DTOs;
using GameVault.API.Models;

namespace GameVault.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Game to DTOs
        CreateMap<Game, GameSummaryDto>()
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src =>
                src.ReleaseDate.HasValue ? src.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name).ToList()))
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom(src => src.Platforms.Select(p => p.Name).ToList()))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count()))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                src.Reviews.Count() >= 5
                    ? (double?)src.Reviews.Average(r => r.Rating)
                    : null));

        CreateMap<Game, GameDetailDto>()
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src =>
                src.ReleaseDate.HasValue ? src.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name).ToList()))
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom(src => src.Platforms.Select(p => p.Name).ToList()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                src.GameImages.OrderBy(gi => gi.DisplayOrder).ToList()))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count()))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                src.Reviews.Count() >= 5
                    ? (double?)src.Reviews.Average(r => r.Rating)
                    : null));

        // GameImage to DTO
        CreateMap<GameImage, GameImageDto>();

        // Review to DTO
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.GameTitle, opt => opt.MapFrom(src => src.Game.Title))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl));

        // Category to DTO
        CreateMap<Category, CategoryDto>();

        // Platform to DTO
        CreateMap<Platform, PlatformDto>();

        // News to DTO
        CreateMap<News, NewsSummaryDto>()
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src =>
                src.PublishDate.HasValue ? src.PublishDate.Value : DateTime.MinValue));

        // WebResource to DTO
        CreateMap<WebResource, WebResourceDto>()
            .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.ResourceId));
    }
}