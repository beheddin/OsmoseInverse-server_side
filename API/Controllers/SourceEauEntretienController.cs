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
    [Route("[controller]")]
    [ApiController]
    public class SourceEauEntretienController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SourceEauEntretienController> _logger;

        public SourceEauEntretienController(
            IMediator mediator,
            IMapper mapper,
            ILogger<SourceEauEntretienController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<SourceEauEntretienDTO>>> GetSourceEauEntretiens()
        {
            try
            {
                GetAllGenericQuery<SourceEauEntretien> query = new GetAllGenericQuery<SourceEauEntretien>(includes: query => query.Include(sourceEauEntretien => sourceEauEntretien.SourceEau).Include(sourceEauEntretien => sourceEauEntretien.Fournisseur));
                IEnumerable<SourceEauEntretien> sourceEauEntretiens = await _mediator.Send(query);

                if (sourceEauEntretiens.IsNullOrEmpty())
                    return NotFound(new { message = "No SourceEauEntretiens were found" });

                IEnumerable<SourceEauEntretienDTO> sourceEauEntretiensDTO = sourceEauEntretiens.Select(sourceEauEntretien => _mapper.Map<SourceEauEntretienDTO>(sourceEauEntretien)).ToList();

                return Ok(sourceEauEntretiensDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetSourceEauEntretiens.");
                throw new Exception($"An unexpected error occurred while handling GetSourceEauEntretiens: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        public async Task<ActionResult<SourceEauEntretienDTO>> GetSourceEauEntretienById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<SourceEauEntretien> query = new GetByGenericQuery<SourceEauEntretien>(sourceEauEntretien => sourceEauEntretien.IdSourceEauEntretien == id, includes: query => query.Include(sourceEauEntretien => sourceEauEntretien.SourceEau).Include(sourceEauEntretien => sourceEauEntretien.Fournisseur));
                SourceEauEntretien sourceEauEntretien = await _mediator.Send(query);

                if (sourceEauEntretien == null)
                    return NotFound(new { message = $"SourceEauEntretien with ID '{id}' not found" });

                SourceEauEntretienDTO sourceEauEntretienDTO = _mapper.Map<SourceEauEntretienDTO>(sourceEauEntretien);

                return Ok(sourceEauEntretienDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetSourceEauEntretienById.");
                throw new Exception($"An unexpected error occurred while handling GetSourceEauEntretienById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        public async Task<ActionResult<MessageResponseDTO>> PostSourceEauEntretien([FromBody] SourceEauEntretienDTO sourceEauEntretienDTO)
        {
            try
            {
                //check if sourceEauEntretienDTO.NomSourceEauEntretien already exists
                GetByGenericQuery<SourceEauEntretien> query = new GetByGenericQuery<SourceEauEntretien>(sourceEauEntretien => sourceEauEntretien.NomSourceEauEntretien == sourceEauEntretienDTO.NomSourceEauEntretien);
                SourceEauEntretien existingSourceEauEntretienByNom = await _mediator.Send(query);

                if (existingSourceEauEntretienByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"SourceEauEntretien with Nom '{sourceEauEntretienDTO.NomSourceEauEntretien}' already exists" });

                //check if sourceEauEntretienDTO.NomSourceEau exists
                SourceEau existingSourceEauByNom = await _mediator.Send(new GetByGenericQuery<SourceEau>(puit => puit.NomSourceEau == sourceEauEntretienDTO.NomSourceEau));
                if (existingSourceEauByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"SourceEau with Nom '{sourceEauEntretienDTO.NomSourceEau}' not found" });

                //check if sourceEauEntretienDTO.NomFournisseur exists
                Fournisseur existingFournisseurByNom = await _mediator.Send(new GetByGenericQuery<Fournisseur>(puit => puit.NomFournisseur == sourceEauEntretienDTO.NomFournisseur));
                if (existingFournisseurByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Fournisseur with Nom '{sourceEauEntretienDTO.NomFournisseur}' not found" });

                //check if Descriminant is valid (PuitEntretien ou BassinEntretien)
                TypeSourceEauEntretien typeSourceEauEntretien;

                if (!Enum.TryParse(sourceEauEntretienDTO.Descriminant, true, out typeSourceEauEntretien))
                    return NotFound(new MessageResponseDTO { Message = $"Invalid Descriminant '{sourceEauEntretienDTO.Descriminant}'" });

                SourceEauEntretien sourceEauEntretien = _mapper.Map<SourceEauEntretien>(sourceEauEntretienDTO);

                sourceEauEntretien.FkSourceEau = existingSourceEauByNom.IdSourceEau;
                sourceEauEntretien.FkFournisseur = existingFournisseurByNom.IdFournisseur;
                sourceEauEntretien.Descriminant = typeSourceEauEntretien;

                PostGenericCommand<SourceEauEntretien> command = new PostGenericCommand<SourceEauEntretien>(sourceEauEntretien);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostSourceEauEntretien.");
                throw new Exception($"An unexpected error occurred while handling PostSourceEauEntretien: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> PutSourceEauEntretien([FromRoute] Guid id, [FromBody] SourceEauEntretienDTO sourceEauEntretienDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<SourceEauEntretien> query = new GetByGenericQuery<SourceEauEntretien>(sourceEauEntretien => sourceEauEntretien.IdSourceEauEntretien == id);
                SourceEauEntretien existingSourceEauEntretienById = await _mediator.Send(query);

                if (existingSourceEauEntretienById == null)
                    return NotFound(new MessageResponseDTO { Message = $"SourceEauEntretien with ID '{id}' not found" });

                // check if another sourceEauEntretien with the same Code exists
                GetByGenericQuery<SourceEauEntretien> queryByLabel = new GetByGenericQuery<SourceEauEntretien>(
                    sourceEauEntretien => sourceEauEntretien.NomSourceEauEntretien == sourceEauEntretienDTO.NomSourceEauEntretien && sourceEauEntretien.IdSourceEauEntretien != id);
                SourceEauEntretien existingSourceEauEntretienByLabel = await _mediator.Send(queryByLabel);

                if (existingSourceEauEntretienByLabel != null)
                    return Conflict(new MessageResponseDTO { Message = $"SourceEauEntretien with Nom '{sourceEauEntretienDTO.NomSourceEauEntretien}' already exists" });

                //check if sourceEauEntretienDTO.NomSourceEau exists
                SourceEau existingSourceEauByNom = await _mediator.Send(new GetByGenericQuery<SourceEau>(puit => puit.NomSourceEau == sourceEauEntretienDTO.NomSourceEau));
                if (existingSourceEauByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"SourceEau with Nom '{sourceEauEntretienDTO.NomSourceEau}' not found" });

                //check if sourceEauEntretienDTO.NomFournisseur exists
                Fournisseur existingFournisseurByNom = await _mediator.Send(new GetByGenericQuery<Fournisseur>(puit => puit.NomFournisseur == sourceEauEntretienDTO.NomFournisseur));
                if (existingFournisseurByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Fournisseur with Nom '{sourceEauEntretienDTO.NomFournisseur}' not found" });

                //check if Descriminant is valid (PuitEntretien ou BassinEntretien)
                TypeSourceEauEntretien typeSourceEauEntretien;

                if (!Enum.TryParse(sourceEauEntretienDTO.Descriminant, true, out typeSourceEauEntretien))
                    return NotFound(new MessageResponseDTO { Message = $"Invalid Descriminant '{sourceEauEntretienDTO.Descriminant}'" });

                SourceEauEntretien sourceEauEntretien = _mapper.Map<SourceEauEntretien>(sourceEauEntretienDTO);

                sourceEauEntretien.IdSourceEauEntretien = id;
                sourceEauEntretien.FkSourceEau = existingSourceEauByNom.IdSourceEau;
                sourceEauEntretien.FkFournisseur = existingFournisseurByNom.IdFournisseur;
                sourceEauEntretien.Descriminant = typeSourceEauEntretien;

                PutGenericCommand<SourceEauEntretien> command = new PutGenericCommand<SourceEauEntretien>(sourceEauEntretien);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutSourceEauEntretien.");
                throw new Exception($"An unexpected error occurred while handling PutSourceEauEntretien: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> DeleteSourceEauEntretien([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<SourceEauEntretien> query = new GetByGenericQuery<SourceEauEntretien>(sourceEauEntretien => sourceEauEntretien.IdSourceEauEntretien == id);
                SourceEauEntretien sourceEauEntretien = await _mediator.Send(query);

                if (sourceEauEntretien == null)
                    return NotFound(new MessageResponseDTO { Message = $"SourceEauEntretien with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<SourceEauEntretien>(id));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteSourceEauEntretien.");
                throw new Exception($"An unexpected error occurred while handling DeleteSourceEauEntretien: {ex.Message}", ex);
            }
        }
    }
}
