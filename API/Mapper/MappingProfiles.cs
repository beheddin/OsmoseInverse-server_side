using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Models;
using System;

namespace API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Compte
            CreateMap<Compte, CompteDTO>()
            //Check if src.Role is null before accessing NomRole
            .ForMember(dest => dest.NomRole, opt => opt.MapFrom(src => src.Role != null ? src.Role.NomRole.ToString() : string.Empty))
            //.ForMember(dest => dest.NomRole, opt => opt.MapFrom(src => src.Role.NomRole.ToString()))
            //.ForMember(dest => dest.FkRole, opt => opt.MapFrom(src => Guid.Empty)) // since Role is not part of CompteDTO, FkRole might be empty.

            //Check if src.Filiale is null before accessing NomFiliale
            .ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.NomFiliale : string.Empty))
            //.ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale.NomFiliale))

            //configures individual members of the destination type when mapping from CompteDTO to Compte
            .ReverseMap()
            .ForMember(dest => dest.FkRole, opt => opt.Ignore())     //ignores this property during the mapping
            .ForMember(dest => dest.FkFiliale, opt => opt.Ignore());    //ignores this property during the mapping

            //From CompteDTO to Compte: maps NomRole back to the Role entity by creating a new Role object with the corresponding NomRole
            //.ForMember(compte => compte.Role, opt => opt.MapFrom(compteDTO => compteDTO.NomRole != null ? new Role { NomRole = compteDTO.NomRole } : null));

            //// you might want to ignore mapping Password in CompteDTO for security reasons
            //.CreateMap<CompteDTO, Compte>()
            //.ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<Compte, LoginDTO>().ReverseMap();
            #endregion

            CreateMap<Role, RoleDTO>()
           .ForMember(dest => dest.NomRole, opt => opt.MapFrom(src => src.NomRole.ToString()))
           .ReverseMap();

            CreateMap<Filiale, FilialeDTO>()
            .ReverseMap();

            CreateMap<Atelier, AtelierDTO>()
            //.ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale.NomFiliale))
            .ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.NomFiliale : string.Empty))
            .ReverseMap();

            CreateMap<Station, StationDTO>()
            //.ForMember(dest => dest.NomAtelier, opt => opt.MapFrom(src => src.Atelier.NomAtelier))
            .ForMember(dest => dest.NomAtelier, opt => opt.MapFrom(src => src.Atelier != null ? src.Atelier.NomAtelier : string.Empty))
            .ReverseMap();
        }
    }
}
