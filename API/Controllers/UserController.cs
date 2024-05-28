using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using System.Data;

using Domain.Interfaces;
using Domain.DataTransferObjects;
using Domain.Queries;
using Domain.Entities;
using Domain.Commands;
//using System.Web.Http.Results;
using Microsoft.Extensions.Logging;
using Domain.Services;
using Polly;
using Data.Context;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IMediator mediator,
            IMapper mapper,
            IUserRepository repository,
            ILogger<UserController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        #region CRUD functions
        [HttpGet("get/all")]
        //default method
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    var users = await _context.Users
        //        //.Include(u => u.Role) // Include the Role navigation property
        //        //.Include(u => u.Filiale) // Include the Filiale navigation property
        //        //.Select(u => new
        //        //{
        //        //    u.UserId,
        //        //    u.FullName,
        //        //    u.Email,
        //        //    u.Cin,
        //        //    u.Password,
        //        //    u.Access,
        //        //    u.FkFiliale,
        //        //    FilialeLabel = u.Filiale.FilialeLabel, // Include the FilialeLabel
        //        //    u.FkRole,
        //        //    RoleLabel = u.Role.RoleLabel // Include the RoleLabel
        //        //})
        //        .ToListAsync();

        //    return Ok(users);
        //}

        //Mediator pattern
        //public IEnumerable<User> GetUsers()  //sync meth
        //public async Task<IEnumerable<UserDTO>> GetUsers()   //async meth
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                //return _mediator.Send(new GetAllGenericQuery<User>()).Result.Select(v => _mapper.Map<User>(v));
                //OR
                GetAllGenericQuery<User> query = new GetAllGenericQuery<User>(
                    includes: query => query
                        .Include(user => user.Role) //get a user with its role
                        .Include(user => user.Filiale));   //get a user with its filiale

                IEnumerable<User> users = await _mediator.Send(query);

                // Map User entities to UserDTO using AutoMapper
                IEnumerable<UserDTO> usersDTO = users.Select(user => _mapper.Map<UserDTO>(user)).ToList();
                //return users.Select(user => _mapper.Map<UserDTO>(user)).ToList();

                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetUsers.");
                throw new Exception($"An unexpected error occurred while handling GetUsers: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        //default method
        //public async Task<ActionResult<Role>> GetRoleById(Guid id)
        //{
        //    var role = await _context.Role.FindAsync(id);

        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    return role;
        //}

        //Mediator pattern
        //public async Task<UserDTO> GetUserById(Guid id)
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] Guid id)
        //=> await (new GetByGenericHandler<User>(_repository)).Handle(new GetByGenericQuery<User>(condition: x => x.UserId.Equals(id), null), new CancellationToken());
        {
            try
            {
                //var query = new GetByGenericQuery<User>(user => user.UserId == id);
                GetByGenericQuery<User> query = new GetByGenericQuery<User>(user => user.UserId == id,
                    includes: query => query
                        .Include(user => user.Role) //get a user with its role
                        .Include(user => user.Filiale));   //get a user with its filiale

                User user = await _mediator.Send(query);

                if (user == null)
                    return NotFound("User not found");

                // Map the retrieved User entity to UserDTO
                UserDTO userDTO = _mapper.Map<UserDTO>(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetUserById.");
                throw new Exception($"An unexpected error occurred while handling GetUserById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/cin={cin}")]
        //public async Task<UserDTO> GetUserByCin(string cin)
        public async Task<ActionResult<User>> GetUserByCin(string cin)
        {
            try
            {
                GetByGenericQuery<User> query = new GetByGenericQuery<User>(user => user.Cin == cin,
                    includes: query => query
                        .Include(user => user.Role) //get a user with its role
                        .Include(user => user.Filiale));   //get a user with its filiale

                User user = await _mediator.Send(query);

                if (user == null)
                    return NotFound("User not found");

                // Map the retrieved User entity to UserDTO
                UserDTO userDTO = _mapper.Map<UserDTO>(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetUserByCin.");
                throw new Exception($"An unexpected error occurred while handling GetUserByCin: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        public async Task<ActionResult<string>> PostUser([FromBody] UserDTO userDTO)
        /*
        The controller method calls the repository method and checks the IsSuccessful flag.
        Based on the response, it returns the appropriate HTTP response (Ok, Conflict, or InternalServerError).
        */
        {
            try
            {
                //the repo fct handles the business logic
                RepositoryResponseDTO<User> response = await _repository.CreateUser(userDTO);

                // handle failure
                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "User already exists":
                            return Conflict($"User with cin '{userDTO.Cin}' already exists");

                        case "Invalid role label":
                            return NotFound($"Role with label '{userDTO.RoleLabel}' does not exist");

                        case "Role not found":
                            return NotFound($"Role with label '{userDTO.RoleLabel}' not found");

                        case "Filiale not found":
                            return NotFound($"Filiale with label '{userDTO.FilialeLabel}' not found");

                        default:
                            //return StatusCode(500, new { message = "An error occurred while processing your request" });
                            return StatusCode(500, response.Message);
                    }

                //handle success
                //user creation is handled by PostGenericHandler
                PostGenericCommand<User> command = new PostGenericCommand<User>(response.Entity);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
                //return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostUser.");
                throw new Exception($"An unexpected error occurred while handling PostUser: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //default method
        //public async Task<IActionResult> PutUser(Guid id, User user)
        //{
        //    if (id != user.UserId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return NoContent();
        //}

        //Mediator pattern
        //public async Task<string> PutUser(Guid id, [FromBody] UserDTO userDTO)
        public async Task<ActionResult<string>> PutUser([FromRoute] Guid id, [FromBody] UserDTO userDTO)
        //public async Task<ActionResult<string>> PutUser([FromRoute] Guid id, [FromBody] UserDTO userDTO)
        //=> await (new PutGenericHandler<User>(_repository)).Handle(new PutGeneric<User>(user), new CancellationToken());
        //OR
        {
            try
            {
                RepositoryResponseDTO<User> response = await _repository.UpdateUser(id, userDTO);

                // handle failure
                if (!response.IsSuccessful)
                    //switch (response.Message)
                    //{
                    //    case "User not found":
                    //        return NotFound($"User with id '{id}' not found");

                    //    case "Invalid role label":
                    //        return NotFound($"Role with label '{userDTO.RoleLabel}' is invalid");

                    //    case "Role not found":
                    //        return NotFound($"Role with label '{userDTO.RoleLabel}' not found");

                    //    case "Filiale not found":
                    //        return NotFound($"Filiale with label '{userDTO.FilialeLabel}' not found");

                    //    default:
                    //        return StatusCode(500, response.Message);
                    //}
                    //OR
                    return response.Message switch
                    {
                        "User not found" => NotFound($"User with id '{id}' not found"),
                        "Invalid role label" => BadRequest($"Role with label '{userDTO.RoleLabel}' is invalid"),
                        "Role not found" => NotFound($"Role with label '{userDTO.RoleLabel}' not found"),
                        "Filiale not found" => NotFound($"Filiale with label '{userDTO.FilialeLabel}' not found"),
                        _ => StatusCode(500, response.Message),
                    };

                // handle success
                //user update is handled by PutGenericCommand
                //var query = new PutGenericCommand<User>(response.User);
                PutGenericCommand<User> query = new PutGenericCommand<User>(response.Entity);
                string mediatorResponse = await _mediator.Send(query);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutUser.");
                throw new Exception($"An unexpected error occurred while handling PutUser: {ex.Message}", ex);
            }
        }

        /**/

        [HttpDelete("delete/{id}")]
        //default method
        //public async Task<ActionResult<User>> DeleteUser(Guid id)
        //{
        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.User.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        //Mediator pattern
        //public async Task<ActionResult<string>> DeleteUser(Guid id)
        public async Task<ActionResult<string>> DeleteUser([FromRoute] Guid id)
        //=> await (new DeleteGenericHandler<User>(_repository)).Handle(new DeleteGeneric<User>(id), new CancellationToken());
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<User> query = new GetByGenericQuery<User>(user => user.UserId == id);
                User user = await _mediator.Send(query);

                if (user == null)
                    return NotFound($"User with id '{id}' don't exist");

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<User>(id));

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteUser.");
                throw new Exception($"An unexpected error occurred while handling DeleteUser: {ex.Message}", ex);
            }
        }
        #endregion

        #region Authentication functions
        [HttpPost("login")]
        //public async Task<ActionResult<RepositoryResponseDTO<User>>> Login([FromBody] LoginDTO loginDTO)
        public async Task<ActionResult<RepositoryResponseDTO<User>>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                // Validate the input
                if (loginDTO == null)
                {
                    return BadRequest(new { message = "Login data is null" });
                }

                //RepositoryResponseDTO<User> response = await _repository.Login(loginDTO);
                RepositoryResponseDTO<User> response = await _repository.Login(loginDTO);

                // handle failure
                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "Invalid credentials":
                        case "Access denied":
                            return Unauthorized(new RepositoryResponseDTO<User> { Message = response.Message });

                        default:
                            return StatusCode(500, new { message = "An error occurred while processing your request" });
                    }
                // Set the JWT cookie in the response
                //Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = true });
                //Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = false });
                Response.Cookies.Append("jwt", response.Token, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, SameSite = SameSiteMode.None });

                //if successful, return response (which contains the token + the user)
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling Login.");
                throw new Exception($"An unexpected error occurred while handling Login: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("logout")]
        public ActionResult<string> Logout() //logout fct don't need to be async
        {
            Response.Cookies.Delete(key: "jwt");    // delete the JWT cookie

            _logger.LogInformation("User successfully logged out at {Time}", DateTime.UtcNow);  // Log the logout action

            return Ok(new { message = "User successfully logged out" });
        }

        /**/

        //get a user based on the jwt that we set in the cookies
        [HttpGet("authenticatedUser")]
        public async Task<ActionResult<User>> GetAuthenticatedUser()
        {
            string jwt = Request.Cookies["jwt"];    // Get the token from the JWT cookie

            RepositoryResponseDTO<User> response = await _repository.GetAuthenticatedUser(jwt);

            //handle failure
            if (!response.IsSuccessful)
                switch (response.Message)
                {
                    //if "JWT token is missing" or "Invalid token"
                    case "JWT token is missing":
                    case "Invalid token":
                        return Unauthorized(new { message = response.Message });

                    case "User not found":
                        return NotFound(new { message = response.Message });

                    case "Invalid user ID format":
                        return BadRequest(new { message = response.Message });

                    default:
                        // Handle other specific messages or default to internal server error
                        _logger.LogError("Unexpected error: {0}", response.Message);
                        return StatusCode(500, new { message = response.Message });
                }

            //if successful, return the authenticated user 
            return Ok(response.Entity);
        }

        /**/

        [HttpPut("changePassword/{id}")]
        public async Task<ActionResult<string>> ChangePassword([FromRoute] Guid id, [FromBody] string newPassword)
        {
            try
            {
                RepositoryResponseDTO<User> response = await _repository.ChangePassword(id, newPassword);

                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "User not found":
                            return NotFound(new { message = response.Message });

                        case "New password and old password are identical":
                            return BadRequest(new { message = response.Message });

                        case "New password hashing is invalid":
                            return BadRequest(new { message = response.Message });

                        default:
                            _logger.LogError("Unexpected error: {0}", response.Message);
                            return StatusCode(500, new { message = response.Message });
                    }

                //var mediatorResponse = await _mediator.Send(new PutGenericCommand<User>(response.User));
                //return Ok(mediatorResponse);

                return Ok(response.Message);    //print success msg
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the password");
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        /**/

        //only for superusers
        [HttpPut("blockUser/{id}")]
        public async Task<ActionResult<string>> BlockUser([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<User> query = new GetByGenericQuery<User>(user => user.UserId == id);
                User user = await _mediator.Send(query);

                if (user == null)
                    return NotFound($"User with id '{id}' don't exist");

                // Toggle the user's access status
                user.Access = !user.Access;

                var mediatorResponse = await _mediator.Send(new PutGenericCommand<User>(user));

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user's access status");
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }
        #endregion
    }
}
