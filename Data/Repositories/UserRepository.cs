using Data.Context;
using Domain.DataTransferObjects;
using Domain.Interfaces;
using Domain.Entities;
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
    public class UserRepository : IUserRepository
    {
        private readonly OsmoseInverseDbContext _context;
        private readonly AuthService _authService;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper _mapper;

        //dependency injection
        public UserRepository(OsmoseInverseDbContext context, AuthService authService, ILogger<UserRepository> logger, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }

        #region CRUD functions
        //public async Task<RepositoryResponseDTO<User>> CreateUser(User user)
        public async Task<RepositoryResponseDTO<User>> CreateUser(UserDTO userDTO)
        //public async Task<User> CreateUser(User user)
        {
            try
            {
                // check if a user with the provided cin already exists
                User existingUserByCin = await _context.Users.SingleOrDefaultAsync(user => user.Cin == userDTO.Cin);
                /*
                SingleOrDefaultAsync: expects zero or one element in the result set.
                It returns null if no elements are found, and throws an InvalidOperationException if more than one element is found.
                Entity = null
                SingleOrDefaultAsync: expects exactly one element in the result set.
                It throws an InvalidOperationException if the result set contains zero or more than one element.
                */

                if (existingUserByCin != null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "User already exists",
                        Entity = null
                    };

                // check if userDTO RoleLabel exists
                RoleType roleType;

                if (!Enum.TryParse(userDTO.RoleLabel, true, out roleType))  //userDTO.RoleLabel: string
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Invalid role label",
                        Entity = null
                    };

                Role existingRoleByLabel = await _context.Roles.SingleOrDefaultAsync(role => role.RoleLabel == roleType);

                if (existingRoleByLabel == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Role not found",
                        Entity = null
                    };

                // check if userDTO.FilialeLabel exists
                Filiale existingFilialeByLabel = await _context.Filiales.SingleOrDefaultAsync(filiale => filiale.FilialeLabel == userDTO.FilialeLabel);
                if (existingFilialeByLabel == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Filiale not found",
                        Entity = null
                    };

                // map UserDTO to User 
                User user = _mapper.Map<User>(userDTO);

                user.Password = _authService.HashPassword(userDTO.Password);    //hash the pwd
                user.FkRole = existingRoleByLabel.RoleId;  //set the role foreign key in user
                user.FkFiliale = existingFilialeByLabel.FilialeId;   //set the filiale foreign key in user

                //_context.Set<User>().Add(user);   //more generic way
                //_context.Users.Add(user);

                //await _context.SaveChangesAsync();

                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = true,
                    //Message = "User created successfully",    //the success msg is returned by AddAsync(T entity) in GenericRepository
                    Entity = user
                };
            }
            // Handle database update errors
            catch (DbUpdateException ex)
            {
                //Console.WriteLine($"DbUpdateException: Failed to add the user due to a database update error: {ex}");
                _logger.LogError(ex, "DbUpdateException: Failed to add the user due to a database update error.");
                throw new DbUpdateException($"Failed to add due to a database update error: {ex.Message}", ex);
            }
            // Handle other unexpected exceptions
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while adding the user.");
                throw new Exception($"An unexpected error occurred while adding the user: {ex.Message}", ex);
            }
        }

        /**/

        public async Task<RepositoryResponseDTO<User>> UpdateUser([FromRoute] Guid id, [FromBody] UserDTO userDTO)
        //public async Task<User> UpdateUser(Guid id, User user)
        {
            try
            {
                // check if id is valid
                User existingUserById = await _context.Users.SingleOrDefaultAsync(user => user.UserId == id);

                if (existingUserById == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "User not found",
                        Entity = null
                    };

                // check if userDTO.RoleLabel exists
                RoleType roleType;

                if (!Enum.TryParse(userDTO.RoleLabel, true, out roleType))  //userDTO.RoleLabel: string
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Invalid role label",
                        Entity = null
                    };

                //Role role = await _context.Roles.SingleOrDefaultAsync(role => role.RoleLabel == Enum.Parse<RoleType>(userDTO.RoleLabel));
                Role existingRoleByLabel = await _context.Roles.SingleOrDefaultAsync(role => role.RoleLabel == roleType);

                if (existingRoleByLabel == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Role not found",
                        Entity = null
                    };

                // check if userDTO.FilialeLabel exists
                Filiale existingFilialeByLabel = await _context.Filiales.SingleOrDefaultAsync(filiale => filiale.FilialeLabel == userDTO.FilialeLabel);
                if (existingFilialeByLabel == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Filiale not found",
                        Entity = null
                    };

                // map UserDTO to User 
                User user = _mapper.Map<User>(userDTO);

                user.UserId = id;   // set the id of userDTO to match the id of existingUserById
                user.Password = existingUserById.Password;  //changing the pwd can only be done using the ChangePassword() fct
                user.FkRole = existingRoleByLabel.RoleId;  //set the role foreign key in user
                user.FkFiliale = existingFilialeByLabel.FilialeId;   //set the filiale foreign key in user

                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = true,
                    //Message = "User updated successfully",    //the success msg is returned by UpdateAsync(T entity) in GenericRepository
                    Entity = user
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to update the user due to a database update error.");
                throw new DbUpdateException($"Failed to update due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while updating the user.");
                throw new Exception($"An unexpected error occurred while updating the user: {ex.Message}", ex);
            }
        }
        #endregion

        #region Authentication functions
        public async Task<RepositoryResponseDTO<User>> Login(LoginDTO loginDTO)
        {
            try
            {
                // Find a user in in the db based on its Cin
                User existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Cin == loginDTO.Cin);

                // Validate user credentials
                if (existingUser == null || !_authService.VerifyPassword(loginDTO.Password, existingUser.Password)) //(password, hashedPassword)                                                                //return (false, "Invalid credentials",                                                              //return new RepositoryResponseDTO<User> { IsSuccessful = false, Message = "Invalid credentials", Token = null };
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Invalid credentials",
                        Token = null
                    };

                // Check if user access is denied
                if (!existingUser.Access)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "Access denied",
                        Token = null
                    };

                //generate token
                //string token = _authService.GenerateToken(existingUser.UserId);
                string token = _authService.GenerateToken(existingUser);

                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = true,
                    Message = "User successfully logged in",
                    Token = token,
                    //Entity = existingUser we don't need to return the authenticated user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while logging the user.");
                throw new Exception($"An unexpected error occurred while logging the user: {ex.Message}", ex);
            }
        }

        /**/

        public async Task<RepositoryResponseDTO<User>> GetAuthenticatedUser(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = false,
                    Message = "JWT token is missing",
                    Entity = null
                };

            var token = _authService.VerifyToken(jwt);

            if (token == null)
                return new RepositoryResponseDTO<User>
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
                The out Guid userId: if the parsing is successful, the resulting Guid will be stored in the variable userId.
                 */
                //Guid userId = Guid.Parse(token.Issuer);

                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);  //m2
                //if (!Guid.TryParse(token.Issuer, out Guid userId))    //m1
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))  //m2
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        //Message = "Invalid user ID format", //m1
                        Message = "Invalid or missing user ID in token claims", //m2
                        Entity = null
                    };

                //User existingUser = await _context.Users.SingleOrDefaultAsync(user => user.UserId == userId);
                User existingUser = await _context.Users.FindAsync(userId);

                if (existingUser == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "User not found",
                        Entity = null
                    };

                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = true,
                    Message = "User found",
                    Entity = existingUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while retrieving the authenticated user.");
                throw new Exception($"An unexpected error occurred while retrieving the authenticated user: {ex.Message}", ex);
            }
        }

        /**/

        //public async Task<RepositoryResponseDTO<User>> ChangePassword(Guid id, string newPassword)
        public async Task<RepositoryResponseDTO<User>> ChangePassword(Guid id, string newPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "New password cannot be empty",
                    };

                User existingUser = await _context.Users.SingleOrDefaultAsync(user => user.UserId == id);

                if (existingUser == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "User not found",
                        //Entity = null,
                    };

                //new pwd & old pwd must be different
                bool passwordIsIdentical = _authService.VerifyPassword(newPassword, existingUser.Password); //(password, hashedPassword)

                if (passwordIsIdentical)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "New password and old password must be different",
                        //Entity = null,
                    };

                //hash the new pwd
                string newHashedPassword = _authService.HashPassword(newPassword);

                //verify the new pwd hashing
                bool newHashedPasswordIsValid = _authService.VerifyPassword(newPassword, newHashedPassword);

                if (!newHashedPasswordIsValid)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "New password hashing is invalid",
                        //Entity = null,
                    };

                existingUser.Password = newHashedPassword;  //update the user pwd

                // Save the changes to the database
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = true,
                    Message = "Password changed successfully",
                    //User = existingUser,
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to change the password due to a database update error.");
                throw new DbUpdateException($"Failed to change the password due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while changing the password.");
                throw new Exception($"An unexpected error occurred while changing the password: {ex.Message}", ex);
            }
        }

        public async Task<RepositoryResponseDTO<User>> BlockUser(Guid id)
        {
            try
            {
                //verify if current user is a super-user
                //User superUser = _userService.GetBy(superUserId);
                //string roleName = await _context.GetRoleNameByRoleIdAsync(superUser.RoleId);
                //if the superUser RoleName IS NOT "SuperAdmin"
                //if (!string.Equals(roleName, "SuperAdmin", StringComparison.Ordinal))
                //return BadRequest();

                // get the user that we want to block/allow
                User user = await _context.Users.SingleOrDefaultAsync(user => user.UserId == id);

                if (user == null)
                    return new RepositoryResponseDTO<User>
                    {
                        IsSuccessful = false,
                        Message = "User not found",
                        //Entity = null,
                    };

                // Toggle the user's access status
                user.Access = !user.Access;

                // Save the changes to the database
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new RepositoryResponseDTO<User>
                {
                    IsSuccessful = true,
                    Message = "User access status updated successfully",
                    //User = user,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while changing the access status of the user.");
                throw new Exception($"An unexpected error occurred while changing the access status of the user: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
