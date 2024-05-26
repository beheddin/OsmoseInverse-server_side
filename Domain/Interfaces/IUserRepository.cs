using Domain.DataTransferObjects;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        //Task<User> CreateUser(UserDTO userDTO);    //async fct
        Task<RepositoryResponseDTO<User>> CreateUser(UserDTO userDTO);    //async fct
        //Task<User> UpdateUser(Gu<User>id id, UserDTO userDTO);  //async fct
        Task<RepositoryResponseDTO<User>> UpdateUser(Guid id, UserDTO userDTO);  //async fct
        Task<RepositoryResponseDTO<User>> Login(LoginDTO loginDTO);     //async fct
        Task<RepositoryResponseDTO<User>> GetAuthenticatedUser(string jwt);   //async fct
        Task<RepositoryResponseDTO<User>> ChangePassword(Guid id, string newPassword);    //async fct
        Task<RepositoryResponseDTO<User>> BlockUser(Guid id); //async fct
    }
}
