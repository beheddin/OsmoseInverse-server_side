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
//using System.Web.Http.Results;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Domain.Interfaces;
using Domain.DataTransferObjects;
using Domain.Queries;
using Domain.Models;
using Domain.Commands;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ComptesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICompteRepository _repository;
        private readonly ILogger<ComptesController> _logger;

        public ComptesController(
            IMediator mediator,
            IMapper mapper,
            ICompteRepository repository,
            ILogger<ComptesController> logger
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
        //public async Task<ActionResult<IEnumerable<Compte>>> GetComptes()
        //{
        //    return await _context.Comptes.ToListAsync();
        //}

        //public async Task<ActionResult<IEnumerable<Compte>>> GetComptes()
        //{
        //    var comptes = await _context.Comptes
        //        //.Include(u => u.Role) // Include the Role navigation property
        //        //.Include(u => u.Filiale) // Include the Filiale navigation property
        //        //.Select(u => new
        //        //{
        //        //    u.IdCompte,
        //        //    u.FullName,
        //        //    u.Email,
        //        //    u.CIN,
        //        //    u.Password,
        //        //    u.IsAllowed,
        //        //    u.FkFiliale,
        //        //    NomFiliale = u.Filiale.NomFiliale, // Include the NomFiliale
        //        //    u.FkRole,
        //        //    NomRole = u.Role.NomRole // Include the NomRole
        //        //})
        //        .ToListAsync();

        //    return Ok(comptes);
        //}

        //Mediator pattern
        //public IEnumerable<Compte> GetComptes()  //sync meth
        //public async Task<IEnumerable<CompteDTO>> GetComptes()   //async meth
        public async Task<ActionResult<IEnumerable<CompteDTO>>> GetComptes()
        //public async Task<ActionResult<EntityResponseDTO<IEnumerable<CompteDTO>>>> GetComptes()
        {
            try
            {
                //return _mediator.Send(new GetAllGenericQuery<Compte>()).Result.Select(v => _mapper.Map<Compte>(v));
                //OR
                GetAllGenericQuery<Compte> query = new GetAllGenericQuery<Compte>(
                    includes: query => query
                        .Include(compte => compte.Role) //get a compte with its role
                        .Include(compte => compte.Filiale));   //get a compte with its filiale

                IEnumerable<Compte> comptes = await _mediator.Send(query);

                if (comptes.IsNullOrEmpty())
                    //return NotFound("Compte not found");
                    //return NotFound(new EntityResponseDTO<IEnumerable<CompteDTO>> { Message = "Comptes not found" });
                    return NotFound(new { message = "Comptes not found" });

                // Map Compte entities to CompteDTO using AutoMapper
                IEnumerable<CompteDTO> comptesDTO = comptes.Select(compte => _mapper.Map<CompteDTO>(compte)).ToList();
                //return comptes.Select(compte => _mapper.Map<CompteDTO>(compte)).ToList();

                return Ok(comptesDTO);
                //return Ok(new EntityResponseDTO<IEnumerable<CompteDTO>> { IsSuccessful = true, Message = "Comptes found", Entity = comptesDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetComptes.");
                throw new Exception($"An unexpected error occurred while handling GetComptes: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        //default method
        //public async Task<ActionResult<Role>> GetRoleById( Guid id)
        //{
        //    var role = await _context.Role.FindAsync(id);

        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    return role;
        //}

        //Mediator pattern
        //public async Task<CompteDTO> GetCompteById( Guid id)
        public async Task<ActionResult<CompteDTO>> GetCompteById([FromRoute] Guid id)
        //public async Task<ActionResult<EntityResponseDTO<CompteDTO>>> GetCompteById([FromRoute] Guid id)
        //=> await (new GetByGenericHandler<Compte>(_repository)).Handle(new GetByGenericQuery<Compte>(condition: x => x.IdCompte.Equals(id), null), new CancellationToken());
        {
            try
            {
                GetByGenericQuery<Compte> query = new GetByGenericQuery<Compte>(compte => compte.IdCompte == id,
                    includes: query => query
                        .Include(compte => compte.Role) //get a compte with its role
                        .Include(compte => compte.Filiale));   //get a compte with its filiale

                Compte compte = await _mediator.Send(query);

                if (compte == null)
                    //return NotFound("Compte not found");
                    //return NotFound(new EntityResponseDTO<CompteDTO> { Message = "Compte not found" });
                    return NotFound(new { message = $"Compte with ID '{id}' not found" });

                // map the retrieved Compte entity to CompteDTO
                CompteDTO compteDTO = _mapper.Map<CompteDTO>(compte);

                return Ok(compteDTO);
                //return Ok(new EntityResponseDTO<CompteDTO> { IsSuccessful = true, Message = "Compte found", Entity = compteDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetCompteById.");
                throw new Exception($"An unexpected error occurred while handling GetCompteById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/cin={cin}")]
        //public async Task<CompteDTO> GetCompteByCIN(string cin)
        public async Task<ActionResult<EntityResponseDTO<CompteDTO>>> GetCompteByCIN([FromRoute] string cin)
        {
            try
            {
                GetByGenericQuery<Compte> query = new GetByGenericQuery<Compte>(compte => compte.CIN == cin,
                    includes: query => query
                        .Include(compte => compte.Role) //get a compte with its role
                        .Include(compte => compte.Filiale));   //get a compte with its filiale

                Compte compte = await _mediator.Send(query);

                if (compte == null)
                    //return NotFound("Compte not found");
                    //return NotFound(new EntityResponseDTO<CompteDTO> { Message = "Compte not found" });
                    return NotFound(new { message = $"Compte with CIN '{cin}' not found" });

                // map the retrieved Compte entity to CompteDTO
                CompteDTO compteDTO = _mapper.Map<CompteDTO>(compte);

                return Ok(compteDTO);
                //return Ok(new EntityResponseDTO<CompteDTO> { IsSuccessful = true, Message = "Compte found", Entity = compteDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetCompteByCIN.");
                throw new Exception($"An unexpected error occurred while handling GetCompteByCIN: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        //public async Task<ActionResult<EntityResponseDTO<Compte>>> PostCompte([FromBody] CompteDTO compteDTO)
        public async Task<ActionResult<MessageResponseDTO>> PostCompte([FromBody] CompteDTO compteDTO)
        /*
        The controller method calls the repository method and checks the IsSuccessful flag.
        Based on the response, it returns the appropriate HTTP response (Ok, Conflict, or InternalServerError).
        */
        {
            try
            {
                //the repo fct handles the business logic
                EntityResponseDTO<Compte> response = await _repository.CreateCompte(compteDTO);

                // handle failure
                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "A compte with a similar CIN already exists":
                            // return Conflict($"Compte with cin '{compteDTO.CIN}' already exists");
                            return Conflict(new MessageResponseDTO { Message = response.Message });

                        case "Invalid role label":
                            // return NotFound($"Role with label '{compteDTO.NomRole}' does not exist");
                            return BadRequest(new MessageResponseDTO { Message = response.Message });

                        case "Role not found":
                            // return NotFound($"Role with label '{compteDTO.NomRole}' not found");
                            return NotFound(new MessageResponseDTO { Message = response.Message });

                        case "Filiale not found":
                            // return NotFound($"Filiale with label '{compteDTO.NomFiliale}' not found");
                            return NotFound(new MessageResponseDTO { Message = response.Message });

                        default:
                            //return StatusCode(500, response.Message);
                            return StatusCode(500, new MessageResponseDTO { Message = "An error occurred while processing your request" });
                    }

                //handle success
                //compte creation is handled by PostGenericHandler
                PostGenericCommand<Compte> command = new PostGenericCommand<Compte>(response.Entity);
                string mediatorResponse = await _mediator.Send(command);

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostCompte.");
                throw new Exception($"An unexpected error occurred while handling PostCompte: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //default method
        //public async Task<IActionResult> PutCompte( Guid id, Compte compte)
        //{
        //    if (id != compte.IdCompte)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(compte).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CompteExists(id))
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
        //public async Task<string> PutCompte( Guid id, [FromBody] CompteDTO compteDTO)
        public async Task<ActionResult<MessageResponseDTO>> PutCompte([FromRoute] Guid id, [FromBody] CompteDTO compteDTO)
        //public async Task<ActionResult<string>> PutCompte([FromRoute]  Guid id, [FromBody] CompteDTO compteDTO)
        //=> await (new PutGenericHandler<Compte>(_repository)).Handle(new PutGeneric<Compte>(compte), new CancellationToken());
        //OR
        {
            try
            {
                EntityResponseDTO<Compte> response = await _repository.UpdateCompte(id, compteDTO);

                // handle failure
                if (!response.IsSuccessful)
                    //switch (response.Message)
                    //{
                    //    case "Compte not found":
                    //        return NotFound($"Compte with id '{id}' not found");
                    //    case "Invalid role label":
                    //        return NotFound($"Role with label '{compteDTO.NomRole}' is invalid");

                    //    case "Role not found":
                    //        return NotFound($"Role with label '{compteDTO.NomRole}' not found");

                    //    case "Filiale not found":
                    //        return NotFound($"Filiale with label '{compteDTO.NomFiliale}' not found");

                    //    default:
                    //        return StatusCode(500, response.Message);
                    //}
                    //OR
                    return response.Message switch
                    {
                        "Compte with the provided ID not found" => NotFound(new MessageResponseDTO { Message = response.Message }),
                        "Invalid role label" => BadRequest(new MessageResponseDTO { Message = response.Message }),
                        "Role not found" => NotFound(new MessageResponseDTO { Message = response.Message }),
                        "Filiale not found" => NotFound(new MessageResponseDTO { Message = response.Message }),
                        _ => StatusCode(500, new MessageResponseDTO { Message = "An error occurred while processing your request" }),
                    };

                // handle success
                //compte update is handled by PutGenericCommand
                //var query = new PutGenericCommand<Compte>(response.Compte);
                PutGenericCommand<Compte> query = new PutGenericCommand<Compte>(response.Entity);
                string mediatorResponse = await _mediator.Send(query);

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutCompte.");
                throw new Exception($"An unexpected error occurred while handling PutCompte: {ex.Message}", ex);
            }
        }

        /**/

        [HttpDelete("delete/{id}")]
        //default method
        //public async Task<ActionResult<Compte>> DeleteCompte( Guid id)
        //{
        //    var compte = await _context.Compte.FindAsync(id);
        //    if (compte == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Compte.Remove(compte);
        //    await _context.SaveChangesAsync();

        //    return compte;
        //}

        //Mediator pattern
        //public async Task<ActionResult<string>> DeleteCompte( Guid id)
        public async Task<ActionResult<MessageResponseDTO>> DeleteCompte([FromRoute] Guid id)
        //=> await (new DeleteGenericHandler<Compte>(_repository)).Handle(new DeleteGeneric<Compte>(id), new CancellationToken());
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Compte> query = new GetByGenericQuery<Compte>(compte => compte.IdCompte == id);
                Compte compte = await _mediator.Send(query);

                if (compte == null)
                    return NotFound(new MessageResponseDTO { Message = $"Compte with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Compte>(id));

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteCompte.");
                throw new Exception($"An unexpected error occurred while handling DeleteCompte: {ex.Message}", ex);
            }
        }
        #endregion

        #region Authentication functions
        [HttpPost("login")]
        //public async Task<ActionResult<EntityResponseDTO<Compte>>> Login([FromBody] LoginDTO loginDTO)
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                LoginResponseDTO response = await _repository.Login(loginDTO);

                // handle failure
                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "Invalid credentials":
                        case "Access denied":
                            return Unauthorized(new LoginResponseDTO { Message = response.Message });

                        default:
                            return StatusCode(500, new LoginResponseDTO { Message = "An error occurred while processing your request" });
                    }
                // Set the JWT cookie in the response
                //Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = true });
                //Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = false });

                // if successful, set the JWT token as a secure HTTP-only cookie
                Response.Cookies.Append("jwt", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                //if successful, return response (which contains the token + the compte)
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling Login.");
                throw new Exception($"An unexpected error occurred while handling Login: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("logout")]
        public ActionResult<MessageResponseDTO> Logout() //logout fct don't need to be async
        {
            Response.Cookies.Delete(key: "jwt");    // delete the JWT cookie

            _logger.LogInformation("Compte successfully logged out at {Time}", DateTime.UtcNow);  // Log the logout action

            return Ok(new MessageResponseDTO { IsSuccessful = true, Message = "Compte successfully logged out" });
        }

        /**/

        //get a compte based on the jwt that we set in the cookies
        [HttpGet("authenticatedCompte")]
        public async Task<ActionResult<EntityResponseDTO<CompteDTO>>> GetAuthenticatedCompte()
        {
            try
            {
                string jwt = Request.Cookies["jwt"];    // Get the token from the JWT cookie

                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized(new EntityResponseDTO<CompteDTO> { Message = "JWT token is missing" });
                }

                //get the authenticated compte
                EntityResponseDTO<Compte> response = await _repository.GetAuthenticatedCompte(jwt);

                //handle failure
                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        //case "JWT token is missing":
                        case "Invalid token":
                            return Unauthorized(new EntityResponseDTO<CompteDTO> { Message = response.Message });

                        case "Compte not found":
                            return NotFound(new EntityResponseDTO<CompteDTO> { Message = response.Message });

                        //case "Invalid compte ID format":
                        case "Invalid or missing compte ID in token claims":
                            return BadRequest(new EntityResponseDTO<CompteDTO> { Message = response.Message });

                        default:
                            // Handle other specific messages or default to public server error
                            _logger.LogError("Unexpected error: {0}", response.Message);
                            return StatusCode(500, new EntityResponseDTO<CompteDTO> { Message = "An error occurred while processing your request" });
                    }

                // map the retrieved Compte entity to CompteDTO
                CompteDTO compteDTO = _mapper.Map<CompteDTO>(response.Entity);

                //if successful, return the authenticated compte 
                //return Ok(response.Entity);
                return Ok(new EntityResponseDTO<CompteDTO> { IsSuccessful = true, Message = response.Message, Entity = compteDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetAuthenticatedCompte.");
                throw new Exception($"An error occurred while handling GetAuthenticatedCompte: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("changePassword/{cin}")]
        public async Task<ActionResult<MessageResponseDTO>> ChangePassword([FromRoute] string cin, [FromBody] string newPassword)
        {
            try
            {
                MessageResponseDTO response = await _repository.ChangePassword(cin, newPassword);

                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "Compte not found":
                            return NotFound(new MessageResponseDTO { Message = response.Message });

                        case "New password and old password must be different":
                            return BadRequest(new MessageResponseDTO { Message = response.Message });

                        case "New password hashing is invalid":
                            return BadRequest(new MessageResponseDTO { Message = response.Message });

                        default:
                            _logger.LogError("Unexpected error: {0}", response.Message);
                            return StatusCode(500, new MessageResponseDTO { Message = "An error occurred while processing your request" });
                    }

                //var mediatorResponse = await _mediator.Send(new PutGenericCommand<Compte>(response.Compte));
                //return Ok(mediatorResponse);

                //return Ok(response.Message);   
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling ChangePassword.");
                throw new Exception($"An error occurred while handling ChangePassword: {ex.Message}", ex);
            }
        }

        /**/

        //only for supercomptes
        [HttpPut("blockCompte/{cin}")]
        //public async Task<ActionResult<MessageResponseDTO>> BlockCompte([FromRoute]  Guid id)
        public async Task<ActionResult<MessageResponseDTO>> BlockCompte([FromRoute] string cin)
        {
            try
            {
                //check if id is valid
                //GetByGenericQuery<Compte> query = new GetByGenericQuery<Compte>(compte => compte.IdCompte == id);
                GetByGenericQuery<Compte> query = new GetByGenericQuery<Compte>(compte => compte.CIN == cin);
                Compte compte = await _mediator.Send(query);

                if (compte == null)
                    //return NotFound($"Compte with id '{id}' don't exist");
                    return NotFound(new MessageResponseDTO { Message = $"Compte with CIN '{cin}' not found" });

                // Toggle the compte's access status
                compte.IsAllowed = !compte.IsAllowed;

                string mediatorResponse = await _mediator.Send(new PutGenericCommand<Compte>(compte));

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling BlockCompte.");
                throw new Exception($"An unexpected error occurred while handling BlockCompte: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
