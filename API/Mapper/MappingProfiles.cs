using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Entities;
using System;

namespace API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region User
            CreateMap<User, UserDTO>()
            //Check if src.Role is null before accessing RoleLabel
            .ForMember(dest => dest.RoleLabel, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleLabel.ToString() : string.Empty))
            //.ForMember(dest => dest.RoleLabel, opt => opt.MapFrom(src => src.Role.RoleLabel.ToString()))
            //.ForMember(dest => dest.FkRole, opt => opt.MapFrom(src => Guid.Empty)) // since Role is not part of UserDTO, FkRole might be empty.

            //Check if src.Filiale is null before accessing FilialeLabel
            .ForMember(dest => dest.FilialeLabel, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.FilialeLabel : string.Empty))
            //.ForMember(dest => dest.FilialeLabel, opt => opt.MapFrom(src => src.Filiale.FilialeLabel))
            .ReverseMap()
            .ForMember(dest => dest.FkRole, opt => opt.Ignore())
            .ForMember(dest => dest.FkFiliale, opt => opt.Ignore());

            //From UserDTO to User: maps RoleLabel back to the Role entity by creating a new Role object with the corresponding RoleLabel
            //.ForMember(user => user.Role, opt => opt.MapFrom(userDTO => userDTO.RoleLabel != null ? new Role { RoleLabel = userDTO.RoleLabel } : null));

            //// you might want to ignore mapping Password in UserDTO for security reasons
            //.CreateMap<UserDTO, User>()
            //.ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<User, LoginDTO>().ReverseMap();
            #endregion

            CreateMap<Role, RoleDTO>()
           .ForMember(dest => dest.RoleLabel, opt => opt.MapFrom(src => src.RoleLabel.ToString()))
           .ReverseMap();

            CreateMap<Filiale, FilialeDTO>()
            .ReverseMap();

            CreateMap<Atelier, AtelierDTO>()
            //.ForMember(dest => dest.FilialeLabel, opt => opt.MapFrom(src => src.Filiale.FilialeLabel))
            .ForMember(dest => dest.FilialeLabel, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.FilialeLabel : string.Empty))
            .ReverseMap();

            CreateMap<Station, StationDTO>()
            //.ForMember(dest => dest.AtelierLabel, opt => opt.MapFrom(src => src.Atelier.AtelierLabel))
            .ForMember(dest => dest.AtelierLabel, opt => opt.MapFrom(src => src.Atelier != null ? src.Atelier.AtelierLabel : string.Empty))
            .ReverseMap();
        }
    }
}
