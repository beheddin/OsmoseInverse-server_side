using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Domain.Models;
using AutoMapper;
using Domain.Commands;
using Domain.DataTransferObjects;
using Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FournisseurController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FournisseurController> _logger;

        public FournisseurController(
            IMediator mediator,
            IMapper mapper,
            ILogger<FournisseurController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<FournisseurDTO>>> GetFournisseurs()
        {
            try
            {
                GetAllGenericQuery<Fournisseur> query = new GetAllGenericQuery<Fournisseur>();
                IEnumerable<Fournisseur> fournisseurs = await _mediator.Send(query);

                if (fournisseurs.IsNullOrEmpty())
                    return NotFound(new { message = "No Fournisseurs were found" });

                IEnumerable<FournisseurDTO> fournisseursDTO = fournisseurs.Select(fournisseur => _mapper.Map<FournisseurDTO>(fournisseur)).ToList();

                return Ok(fournisseursDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetFournisseurs.");
                throw new Exception($"An unexpected error occurred while handling GetFournisseurs: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        public async Task<ActionResult<FournisseurDTO>> GetFournisseurById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Fournisseur> query = new GetByGenericQuery<Fournisseur>(fournisseur => fournisseur.IdFournisseur == id);
                Fournisseur fournisseur = await _mediator.Send(query);

                if (fournisseur == null)
                    return NotFound(new { message = $"Fournisseur with ID '{id}' not found" });

                FournisseurDTO fournisseurDTO = _mapper.Map<FournisseurDTO>(fournisseur);

                return Ok(fournisseurDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetFournisseurById.");
                throw new Exception($"An unexpected error occurred while handling GetFournisseurById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        public async Task<ActionResult<MessageResponseDTO>> PostFournisseur([FromBody] FournisseurDTO fournisseurDTO)
        {
            try
            {
                //check if fournisseurDTO.NomFournisseur already exists
                GetByGenericQuery<Fournisseur> query = new GetByGenericQuery<Fournisseur>(fournisseur => fournisseur.NomFournisseur == fournisseurDTO.NomFournisseur);
                Fournisseur existingFournisseurByCode = await _mediator.Send(query);

                if (existingFournisseurByCode != null)
                    return Conflict(new MessageResponseDTO { Message = $"Fournisseur with Nom '{fournisseurDTO.NomFournisseur}' already exists" });

                Fournisseur fournisseur = _mapper.Map<Fournisseur>(fournisseurDTO);

                PostGenericCommand<Fournisseur> command = new PostGenericCommand<Fournisseur>(fournisseur);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostFournisseur.");
                throw new Exception($"An unexpected error occurred while handling PostFournisseur: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> PutFournisseur([FromRoute] Guid id, [FromBody] FournisseurDTO fournisseurDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Fournisseur> query = new GetByGenericQuery<Fournisseur>(fournisseur => fournisseur.IdFournisseur == id);
                Fournisseur existingFournisseurById = await _mediator.Send(query);

                if (existingFournisseurById == null)
                    return NotFound(new MessageResponseDTO { Message = $"Fournisseur with ID '{id}' not found" });

                // check if another fournisseur with the same Nom exists
                GetByGenericQuery<Fournisseur> queryByLabel = new GetByGenericQuery<Fournisseur>(
                    fournisseur => fournisseur.NomFournisseur == fournisseurDTO.NomFournisseur && fournisseur.IdFournisseur != id);
                Fournisseur existingFournisseurByLabel = await _mediator.Send(queryByLabel);

                if (existingFournisseurByLabel != null)
                    return Conflict(new MessageResponseDTO { Message = $"Fournisseur with Nom '{fournisseurDTO.NomFournisseur}' already exists" });

                Fournisseur fournisseur = _mapper.Map<Fournisseur>(fournisseurDTO);

                fournisseur.IdFournisseur = id;

                PutGenericCommand<Fournisseur> command = new PutGenericCommand<Fournisseur>(fournisseur);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutFournisseur.");
                throw new Exception($"An unexpected error occurred while handling PutFournisseur: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> DeleteFournisseur([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Fournisseur> query = new GetByGenericQuery<Fournisseur>(fournisseur => fournisseur.IdFournisseur == id);
                Fournisseur fournisseur = await _mediator.Send(query);

                if (fournisseur == null)
                    return NotFound(new MessageResponseDTO { Message = $"Fournisseur with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Fournisseur>(id));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteFournisseur.");
                throw new Exception($"An unexpected error occurred while handling DeleteFournisseur: {ex.Message}", ex);
            }
        }
    }
}
