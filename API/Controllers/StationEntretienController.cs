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
    public class StationEntretienController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<StationEntretienController> _logger;

        public StationEntretienController(
            IMediator mediator,
            IMapper mapper,
            ILogger<StationEntretienController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<StationEntretienDTO>>> GetStationEntretiens()
        {
            try
            {
                GetAllGenericQuery<StationEntretien> query = new GetAllGenericQuery<StationEntretien>(includes: query => query.Include(stationEntretien => stationEntretien.Station).Include(stationEntretien => stationEntretien.Fournisseur));
                IEnumerable<StationEntretien> stationEntretiens = await _mediator.Send(query);

                if (stationEntretiens.IsNullOrEmpty())
                    return NotFound(new { message = "No StationEntretiens were found" });

                IEnumerable<StationEntretienDTO> stationEntretiensDTO = stationEntretiens.Select(stationEntretien => _mapper.Map<StationEntretienDTO>(stationEntretien)).ToList();

                return Ok(stationEntretiensDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetStationEntretiens.");
                throw new Exception($"An unexpected error occurred while handling GetStationEntretiens: {ex.Message}", ex);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<StationEntretienDTO>> GetStationEntretienById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<StationEntretien> query = new GetByGenericQuery<StationEntretien>(stationEntretien => stationEntretien.IdStationEntretien == id, includes: query => query.Include(stationEntretien => stationEntretien.Station).Include(stationEntretien => stationEntretien.Fournisseur));

                StationEntretien stationEntretien = await _mediator.Send(query);

                if (stationEntretien == null)
                    return NotFound(new MessageResponseDTO { Message = $"StationEntretien with ID '{id}' not found" });

                StationEntretienDTO stationEntretienDTO = _mapper.Map<StationEntretienDTO>(stationEntretien);

                return Ok(stationEntretienDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetStationEntretienById.");
                throw new Exception($"An unexpected error occurred while handling GetStationEntretienById: {ex.Message}", ex);
            }
        }

        [HttpPost("post")]
        public async Task<ActionResult<MessageResponseDTO>> PostStationEntretien([FromBody] StationEntretienDTO stationEntretienDTO)
        {
            try
            {
                // check if the stationEntretienDTO.NomStationEntretien already exists
                GetByGenericQuery<StationEntretien> query = new GetByGenericQuery<StationEntretien>(stationEntretien => stationEntretien.NomStationEntretien == stationEntretienDTO.NomStationEntretien);
                StationEntretien existingStationEntretienByNom = await _mediator.Send(query);

                if (existingStationEntretienByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"StationEntretien with Nom '{stationEntretienDTO.NomStationEntretien}' already exists" });


                //check if stationEntretienDTO.NomStation exists
                Station existingStationByNom = await _mediator.Send(new GetByGenericQuery<Station>(station => station.NomStation == stationEntretienDTO.NomStation));
                if (existingStationByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Station with Nom '{stationEntretienDTO.NomStation}' not found" });

                //check if stationEntretienDTO.NomFournisseur exists
                Fournisseur existingFournisseurByNom = await _mediator.Send(new GetByGenericQuery<Fournisseur>(fournisseur => fournisseur.NomFournisseur == stationEntretienDTO.NomFournisseur));
                if (existingFournisseurByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Fournisseur with Nom '{stationEntretienDTO.NomFournisseur}' not found" });

                StationEntretien stationEntretien = _mapper.Map<StationEntretien>(stationEntretienDTO);

                stationEntretien.FkStation = existingStationByNom.IdStation;
                stationEntretien.FkFournisseur = existingFournisseurByNom.IdFournisseur;

                PostGenericCommand<StationEntretien> command = new PostGenericCommand<StationEntretien>(stationEntretien);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostStationEntretien.");
                throw new Exception($"An unexpected error occurred while handling PostStationEntretien: {ex.Message}", ex);
            }
        }

        [HttpPut("put/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> PutStationEntretien([FromRoute] Guid id, [FromBody] StationEntretienDTO stationEntretienDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<StationEntretien> query = new GetByGenericQuery<StationEntretien>(stationEntretien => stationEntretien.IdStationEntretien == id);
                StationEntretien existingStationEntretienById = await _mediator.Send(query);

                if (existingStationEntretienById == null)
                    return NotFound(new MessageResponseDTO { Message = $"StationEntretien with ID '{id}' not found" });

                // check if another stationEntretien with the same label as stationEntretienDTO.NomStationEntretien exists
                GetByGenericQuery<StationEntretien> queryByNom = new GetByGenericQuery<StationEntretien>(
                    stationEntretien => stationEntretien.NomStationEntretien == stationEntretienDTO.NomStationEntretien && stationEntretien.IdStationEntretien != id);
                StationEntretien existingStationEntretienByNom = await _mediator.Send(queryByNom);

                if (existingStationEntretienByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"StationEntretien with Nom '{stationEntretienDTO.NomStationEntretien}' already exists" });

                //check if stationEntretienDTO.NomStation exists
                Station existingStationByNom = await _mediator.Send(new GetByGenericQuery<Station>(station => station.NomStation == stationEntretienDTO.NomStation));
                if (existingStationByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Station with Nom '{stationEntretienDTO.NomStation}' not found" });

                //check if stationEntretienDTO.NomFournisseur exists
                Fournisseur existingFournisseurByNom = await _mediator.Send(new GetByGenericQuery<Fournisseur>(fournisseur => fournisseur.NomFournisseur == stationEntretienDTO.NomFournisseur));
                if (existingFournisseurByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Fournisseur with Nom '{stationEntretienDTO.NomFournisseur}' not found" });

                StationEntretien stationEntretien = _mapper.Map<StationEntretien>(stationEntretienDTO);

                stationEntretien.IdStationEntretien = id;
                stationEntretien.FkStation = existingStationByNom.IdStation;
                stationEntretien.FkFournisseur = existingFournisseurByNom.IdFournisseur;

                PutGenericCommand<StationEntretien> command = new PutGenericCommand<StationEntretien>(stationEntretien);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutStationEntretien.");
                throw new Exception($"An unexpected error occurred while handling PutStationEntretien: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> DeleteStationEntretien([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<StationEntretien> query = new GetByGenericQuery<StationEntretien>(stationEntretien => stationEntretien.IdStationEntretien == id);
                StationEntretien stationEntretien = await _mediator.Send(query);

                if (stationEntretien == null)
                    return NotFound(new MessageResponseDTO { Message = $"StationEntretien with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<StationEntretien>(id));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteStationEntretien.");
                throw new Exception($"An unexpected error occurred while handling DeleteStationEntretien: {ex.Message}", ex);
            }
        }
    }
}
