using Domain.DataTransferObjects;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        //Task<User> CreateUser(UserDTO userDTO);    //async fct
        Task<EntityResponseDTO<User>> CreateUser(UserDTO userDTO);    //async fct
        Task<EntityResponseDTO<User>> UpdateUser(Guid id, UserDTO userDTO);  //async fct
        Task<LoginResponseDTO> Login(LoginDTO loginDTO);     //async fct
        Task<EntityResponseDTO<User>> GetAuthenticatedUser(string jwt);   //async fct
        Task<MessageResponseDTO> ChangePassword(Guid id, string newPassword);    //async fct
        //Task<MessageResponseDTO> BlockUser(string cin); //async fct
    }
}
