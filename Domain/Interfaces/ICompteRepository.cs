using Domain.DataTransferObjects;
using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICompteRepository
    {
        //Task<Compte> CreateCompte(CompteDTO compteDTO);    //async fct
        Task<EntityResponseDTO<Compte>> CreateCompte(CompteDTO compteDTO);    //async fct
        Task<EntityResponseDTO<Compte>> UpdateCompte(Guid id, CompteDTO compteDTO);  //async fct
        Task<LoginResponseDTO> Login(LoginDTO loginDTO);     //async fct
        Task<EntityResponseDTO<Compte>> GetAuthenticatedCompte(string jwt);   //async fct
        //Task<MessageResponseDTO> ChangePassword(string cin, ChangePasswordDTO changePasswordDTO);    //async fct
        Task<MessageResponseDTO> ChangePassword(Guid id, ChangePasswordDTO changePasswordDTO);    //async fct
        //Task<MessageResponseDTO> SwitchAccountAccess( Guid id); //async fct
    }
}
