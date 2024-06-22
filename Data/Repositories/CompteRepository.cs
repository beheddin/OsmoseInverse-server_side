using Data.Context;
using Domain.DataTransferObjects;
using Domain.Interfaces;
using Domain.Models;
using Domain.Queries;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Data.Repositories
{
    //repository fcts handle all database interactions and return responses to controller
    public class CompteRepository : ICompteRepository
    {
        private readonly OsmoseInverseDbContext _context;
        private readonly AuthService _authService;
        private readonly ILogger<CompteRepository> _logger;
        private readonly IMapper _mapper;

        //dependency injection
        public CompteRepository(OsmoseInverseDbContext context, AuthService authService, ILogger<CompteRepository> logger, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }

        #region CRUD functions
        //public async Task<EntityResponseDTO<Compte>> CreateCompte(Compte compte)
        public async Task<EntityResponseDTO<Compte>> CreateCompte([FromBody] CompteDTO compteDTO)
        //public async Task<Compte> CreateCompte(Compte compte)
        {
            try
            {
                // check if a compte with the provided cin already exists
                Compte existingCompteByCIN = await _context.Comptes.SingleOrDefaultAsync(compte => compte.CIN == compteDTO.CIN);
                /*
                SingleOrDefaultAsync: expects zero or one element in the result set.
                It returns null if no elements are found, and throws an InvalidOperationException if more than one element is found.
                Entity = null
                SingleOrDefaultAsync: expects exactly one element in the result set.
                It throws an InvalidOperationException if the result set contains zero or more than one element.
                */

                if (existingCompteByCIN != null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "A Compte with a similar CIN already exists",
                        Entity = null
                    };

                // check if compteDTO NomRole exists
                TypeRole typeRole;

                if (!Enum.TryParse(compteDTO.NomRole, true, out typeRole))  //compteDTO.NomRole: string
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Invalid NomRole",
                        Entity = null
                    };

                Role existingRoleByLabel = await _context.Roles.SingleOrDefaultAsync(role => role.NomRole == typeRole);

                if (existingRoleByLabel == null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Role not found",
                        Entity = null
                    };

                // check if compteDTO.NomFiliale exists
                Filiale existingFilialeByLabel = await _context.Filiales.SingleOrDefaultAsync(filiale => filiale.NomFiliale == compteDTO.NomFiliale);
                if (existingFilialeByLabel == null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Filiale not found",
                        Entity = null
                    };

                // map CompteDTO to Compte 
                Compte compte = _mapper.Map<Compte>(compteDTO);

                compte.Password = _authService.HashPassword(compteDTO.Password);    //hash the pwd
                compte.FkRole = existingRoleByLabel.IdRole;  //set the role foreign key in compte
                compte.FkFiliale = existingFilialeByLabel.IdFiliale;   //set the filiale foreign key in compte

                //_context.Set<Compte>().Add(compte);   //more generic way
                //_context.Comptes.Add(compte);

                //await _context.SaveChangesAsync();

                return new EntityResponseDTO<Compte>
                {
                    IsSuccessful = true,
                    //Message = "Compte created successfully",    //the success msg is returned by AddAsync(T entity) in GenericRepository
                    Entity = compte
                };
            }
            // Handle database update errors
            catch (DbUpdateException ex)
            {
                //Console.WriteLine($"DbUpdateException: Failed to add the compte due to a database update error: {ex}");
                _logger.LogError(ex, "DbUpdateException: Failed to add the compte due to a database update error.");
                throw new DbUpdateException($"Failed to add due to a database update error: {ex.Message}", ex);
            }
            // Handle other unexpected exceptions
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling CreateCompte.");
                throw new Exception($"An unexpected error occurred while handling CreateCompte: {ex.Message}", ex);
            }
        }

        /**/

        public async Task<EntityResponseDTO<Compte>> UpdateCompte([FromRoute] Guid id, [FromBody] CompteDTO compteDTO)
        //public async Task<Compte> UpdateCompte( Guid id, Compte compte)
        {
            try
            {
                // check if id is valid
                Compte existingCompteById = await _context.Comptes.SingleOrDefaultAsync(compte => compte.IdCompte == id);

                if (existingCompteById == null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Compte with the provided ID not found",
                        Entity = null
                    };

                // check if compteDTO.NomRole exists
                TypeRole typeRole;

                if (!Enum.TryParse(compteDTO.NomRole, true, out typeRole))  //compteDTO.NomRole: string
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Invalid NomRole",
                        Entity = null
                    };

                //Role role = await _context.Roles.SingleOrDefaultAsync(role => role.NomRole == Enum.Parse<TypeRole>(compteDTO.NomRole));
                Role existingRoleByLabel = await _context.Roles.SingleOrDefaultAsync(role => role.NomRole == typeRole);

                if (existingRoleByLabel == null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Role not found",
                        Entity = null
                    };

                // check if compteDTO.NomFiliale exists
                Filiale existingFilialeByLabel = await _context.Filiales.SingleOrDefaultAsync(filiale => filiale.NomFiliale == compteDTO.NomFiliale);
                if (existingFilialeByLabel == null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Filiale not found",
                        Entity = null
                    };

                // map CompteDTO to Compte 
                Compte compte = _mapper.Map<Compte>(compteDTO);

                compte.IdCompte = id;   // set the id of compteDTO to match the id of existingCompteById
                compte.Password = existingCompteById.Password;  //changing the pwd can only be done using the ChangePassword() fct
                compte.FkRole = existingRoleByLabel.IdRole;  //set the role foreign key in compte
                compte.FkFiliale = existingFilialeByLabel.IdFiliale;   //set the filiale foreign key in compte

                return new EntityResponseDTO<Compte>
                {
                    IsSuccessful = true,
                    //Message = "Compte successfully updated",    //the success msg is returned by UpdateAsync(T entity) in GenericRepository
                    Entity = compte
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to update the compte due to a database update error");
                throw new DbUpdateException($"Failed to update due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling UpdateCompte.");
                throw new Exception($"An unexpected error occurred while handling UpdateCompte: {ex.Message}", ex);
            }
        }
        #endregion

        #region Authentication functions
        public async Task<LoginResponseDTO> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                // Find a compte in in the db based on its CIN
                Compte existingCompte = await _context.Comptes.SingleOrDefaultAsync(compte => compte.CIN == loginDTO.CIN);

                // Validate compte credentials
                if (existingCompte == null || !_authService.VerifyPassword(loginDTO.Password, existingCompte.Password)) //(password, hashedPassword)                                                                //return (false, "Invalid credentials",                                                              //return new EntityResponseDTO<Compte> { IsSuccessful = false, Message = "Invalid credentials", Token = null };
                    return new LoginResponseDTO
                    {
                        IsSuccessful = false,
                        Message = "Invalid credentials",
                        Token = null
                    };

                // Check if compte access is denied
                if (!existingCompte.Access)
                    return new LoginResponseDTO
                    {
                        IsSuccessful = false,
                        Message = "Access denied",
                        Token = null
                    };

                //generate token
                //string token = _authService.GenerateToken(existingCompte.IdCompte);
                string token = _authService.GenerateToken(existingCompte);

                return new LoginResponseDTO
                {
                    IsSuccessful = true,
                    Message = "Compte successfully logged in",
                    Token = token,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling Login.");
                throw new Exception($"An unexpected error occurred while handling Login: {ex.Message}", ex);
            }
        }

        /**/

        public async Task<EntityResponseDTO<Compte>> GetAuthenticatedCompte(string jwt)
        {
            //if (string.IsNullOrEmpty(jwt))
            //    return new EntityResponseDTO<Compte>
            //    {
            //        IsSuccessful = false,
            //        Message = "JWT token is missing",
            //        Entity = null
            //    };

            var token = _authService.VerifyToken(jwt);

            if (token == null)
                return new EntityResponseDTO<Compte>
                {
                    IsSuccessful = false,
                    Message = "Invalid token",
                    Entity = null
                };

            try
            {
                /*
                 Unlike "Guid.Parse" which throws an exception if the string is not a valid GUID,
                "Guid.TryParse" fails gracefully without throwing an exception.
                The out  Guid compteId: if the parsing is successful, the resulting  Guid will be stored in the variable compteId.
                 */
                // Guid compteId = Guid.Parse(token.Issuer);

                var compteIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);  //m2
                //if (!Guid.TryParse(token.Issuer, out  Guid compteId))    //m1
                if (compteIdClaim == null || !Guid.TryParse(compteIdClaim.Value, out Guid compteId))  //m2
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        //Message = "Invalid compte ID format", //m1
                        Message = "Invalid or missing compte ID in token claims", //m2
                        Entity = null
                    };

                //Compte existingCompte = await _context.Comptes.SingleOrDefaultAsync(compte => compte.IdCompte == compteId);
                Compte existingCompte = await _context.Comptes.FindAsync(compteId);

                if (existingCompte == null)
                    return new EntityResponseDTO<Compte>
                    {
                        IsSuccessful = false,
                        Message = "Compte not found",
                        Entity = null
                    };

                return new EntityResponseDTO<Compte>
                {
                    IsSuccessful = true,
                    Message = "Compte found",
                    Entity = existingCompte
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetAuthenticatedCompte.");
                throw new Exception($"An error occurred while handling GetAuthenticatedCompte: {ex.Message}", ex);
            }
        }

        /**/

        public async Task<MessageResponseDTO> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                //Compte existingCompteByCin = await _context.Comptes.SingleOrDefaultAsync(compte => compte.CIN == cin);
                Compte existingCompte= await _context.Comptes.FirstOrDefaultAsync(compte => compte.IdCompte == id);

                if (existingCompte == null)
                    return new MessageResponseDTO
                    {
                        IsSuccessful = false,
                        Message = "Compte not found",
                    };


                //verify if the current password is correct
                bool currentPasswordIsValid = _authService.VerifyPassword(changePasswordDTO.CurrentPassword, existingCompte.Password);

                if (!currentPasswordIsValid)
                    return new MessageResponseDTO { IsSuccessful = false, Message = "Current password is incorrect" };


                //ensure new pwd & old pwd are different
                bool newPasswordIsIdentical = _authService.VerifyPassword(changePasswordDTO.NewPassword, existingCompte.Password); //(password, hashedPassword)

                if (newPasswordIsIdentical)
                    return new MessageResponseDTO
                    {
                        IsSuccessful = false,
                        Message = "New password and old password must be different",
                    };

                //hash the new pwd
                string newHashedPassword = _authService.HashPassword(changePasswordDTO.NewPassword);

                ////verify the new pwd hashing
                //bool newHashedPasswordIsValid = _authService.VerifyPassword(newPassword, newHashedPassword);

                //if (!newHashedPasswordIsValid)
                //    return new MessageResponseDTO
                //    {
                //        IsSuccessful = false,
                //        Message = "New password hashing is invalid",
                //    };

                existingCompte.Password = newHashedPassword;  //update the compte pwd

                // Save the changes to the database
                _context.Comptes.Update(existingCompte);
                await _context.SaveChangesAsync();

                return new MessageResponseDTO
                {
                    IsSuccessful = true,
                    Message = "Password successfully changed",
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to change the password due to a database update error.");
                throw new DbUpdateException($"Failed to change the password due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling ChangePassword.");
                throw new Exception($"An error occurred while handling ChangePassword: {ex.Message}", ex);
            }
        }

        //public async Task<MessageResponseDTO> SwitchAccountAccess( Guid id)
        //{
        //    try
        //    {
        //        Compte compte = await _context.Comptes.SingleOrDefaultAsync(compte => compte.IdCompte == id);

        //        if (compte == null)
        //            return new MessageResponseDTO
        //            {
        //                IsSuccessful = false,
        //                Message = "Compte not found",
        //            };

        //        // Toggle the compte's access status
        //        compte.Access = !compte.Access;

        //        // Save the changes to the database
        //        _context.Comptes.Update(compte);
        //        await _context.SaveChangesAsync();

        //        return new MessageResponseDTO
        //        {
        //            IsSuccessful = true,
        //            Message = "Compte access status successfully updated",
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Exception: An unexpected error occurred while handling BlockCompte");
        //        throw new Exception($"An unexpected error occurred while handling BlockCompte: {ex.Message}", ex);
        //    }
        //}
        #endregion
    }
}
