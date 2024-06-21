using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;

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

            .ReverseMap()
            //configures individual members of the destination type when mapping from CompteDTO to Compte
            .ForMember(dest => dest.FkRole, opt => opt.Ignore())     //ignores this property during the mapping
            .ForMember(dest => dest.FkFiliale, opt => opt.Ignore());    //ignores this property during the mapping

            //OR instead of ReverseMap():
            //CreateMap<CompteDTO, Compte>()
            //.ForMember(dest => dest.FkRole, opt => opt.Ignore())     //ignores this property during the mapping
            //.ForMember(dest => dest.FkFiliale, opt => opt.Ignore());    //ignores this property during the mapping

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
            .ReverseMap()
            .ForMember(dest => dest.FkFiliale, opt => opt.Ignore());

            CreateMap<Station, StationDTO>()
            //.ForMember(dest => dest.NomAtelier, opt => opt.MapFrom(src => src.Atelier.NomAtelier))
            .ForMember(dest => dest.NomAtelier, opt => opt.MapFrom(src => src.Atelier != null ? src.Atelier.NomAtelier : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.FkAtelier, opt => opt.Ignore());
            //public ICollection<Objectif> Objectifs { get; set; }
            //public ICollection<Equipment> Equipments { get; set; }
            //public ICollection<StationParametre> StationParametres { get; set; }
            //public ICollection<ProduitConsommable> ProduitsConsommables { get; set; }
            //public ICollection<StationEntretien> StationEntretiens { get; set; }
            //public ICollection<LavageChimique> LavagesChimiques { get; set; }

            CreateMap<StationEntretien, StationEntretienDTO>()
            .ForMember(dest => dest.NomStation, opt => opt.MapFrom(src => src.Station != null ? src.Station.NomStation : string.Empty))
            .ForMember(dest => dest.NomFournisseur, opt => opt.MapFrom(src => src.Fournisseur != null ? src.Fournisseur.NomFournisseur : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.FkStation, opt => opt.Ignore())
            .ForMember(dest => dest.FkFournisseur, opt => opt.Ignore());

            //CreateMap<SourceEau, SourceEauDTO>()
            //.ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.NomFiliale : string.Empty))
            //.ReverseMap()
            //.ForMember(dest => dest.FkFiliale, opt => opt.Ignore());

            CreateMap<Bassin, BassinDTO>()
            .ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.NomFiliale : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.FkFiliale, opt => opt.Ignore());

            CreateMap<Puit, PuitDTO>()
            .ForMember(dest => dest.NomFiliale, opt => opt.MapFrom(src => src.Filiale != null ? src.Filiale.NomFiliale : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.FkFiliale, opt => opt.Ignore());

            CreateMap<SourceEauEntretien, SourceEauEntretienDTO>()
            //.ForMember(dest => dest.NomPuit, opt => opt.MapFrom(src => src.SourceEau is Puit ? ((Puit)src.SourceEau).NomPuit : string.Empty))
            //.ForMember(dest => dest.NomPuit, opt => opt.MapFrom(src => src.SourceEau != null ? ((Puit)src.SourceEau).NomPuit : string.Empty))

            //.ForMember(dest => dest.NomBassin, opt => opt.MapFrom(src => src.SourceEau is Bassin ? ((Bassin)src.SourceEau).NomBassin : string.Empty))
            //.ForMember(dest => dest.NomBassin, opt => opt.MapFrom(src => src.SourceEau != null ? ((Bassin)src.SourceEau).NomBassin : string.Empty))

            .ForMember(dest => dest.NomFournisseur, opt => opt.MapFrom(src => src.Fournisseur != null ? src.Fournisseur.NomFournisseur : string.Empty))
            .ForMember(dest => dest.NomSourceEau, opt => opt.MapFrom(src => src.SourceEau != null ? src.SourceEau.NomSourceEau : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.FkSourceEau, opt => opt.Ignore())
            .ForMember(dest => dest.FkFournisseur, opt => opt.Ignore());

            //CreateMap<PuitEntretien, PuitEntretienDTO>()
            //.ForMember(dest => dest.NomPuit, opt => opt.MapFrom(src => src.Puit != null ? src.Puit.NomPuit : string.Empty))
            //.ForMember(dest => dest.NomFournisseur, opt => opt.MapFrom(src => src.Fournisseur != null ? src.Fournisseur.NomFournisseur : string.Empty))
            //.ReverseMap()
            //.ForMember(dest => dest.FkPuit, opt => opt.Ignore())
            //.ForMember(dest => dest.FkFournisseur, opt => opt.Ignore());

            //CreateMap<BassinEntretien, BassinEntretienDTO>()
            //.ForMember(dest => dest.NomBassin, opt => opt.MapFrom(src => src.Bassin != null ? src.Bassin.NomBassin : string.Empty))
            //.ForMember(dest => dest.NomFournisseur, opt => opt.MapFrom(src => src.Fournisseur != null ? src.Fournisseur.NomFournisseur : string.Empty))
            //.ReverseMap()
            //.ForMember(dest => dest.FkBassin, opt => opt.Ignore())
            //.ForMember(dest => dest.FkFournisseur, opt => opt.Ignore());

            CreateMap<Fournisseur, FournisseurDTO>()
            .ReverseMap();
        }
    }
}
