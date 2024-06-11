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
    public class StationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<StationsController> _logger;

        public StationsController(
            IMediator mediator,
            IMapper mapper,
            ILogger<StationsController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<StationDTO>>> GetStations()
        {
            try
            {
                GetAllGenericQuery<Station> query = new GetAllGenericQuery<Station>(includes: query => query.Include(station => station.Atelier));
                IEnumerable<Station> stations = await _mediator.Send(query);

                if (stations.IsNullOrEmpty())
                    return NotFound(new { message = "No Stations were found" });

                IEnumerable<StationDTO> stationsDTO = stations.Select(station => _mapper.Map<StationDTO>(station)).ToList();

                return Ok(stationsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetStations.");
                throw new Exception($"An unexpected error occurred while handling GetStations: {ex.Message}", ex);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<StationDTO>> GetStationById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.IdStation == id, includes: query => query.Include(station => station.Atelier));

                Station station = await _mediator.Send(query);

                if (station == null)
                    return NotFound(new MessageResponseDTO { Message = $"Station with ID '{id}' not found" });

                StationDTO stationDTO = _mapper.Map<StationDTO>(station);

                return Ok(stationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetStationById.");
                throw new Exception($"An unexpected error occurred while handling GetStationById: {ex.Message}", ex);
            }
        }

        [HttpPost("post")]
        public async Task<ActionResult<MessageResponseDTO>> PostStation([FromBody] StationDTO stationDTO)
        {
            try
            {
                // check if the station already exists based on its label
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.NomStation == stationDTO.NomStation);
                Station existingStationByNom = await _mediator.Send(query);

                if (existingStationByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"Station with Nom '{stationDTO.NomStation}' already exists" });


                //check if stationDTO.NomAtelier exists
                Atelier existingAtelierByNom = await _mediator.Send(new GetByGenericQuery<Atelier>(atelier => atelier.NomAtelier == stationDTO.NomAtelier));
                if (existingAtelierByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Atelier with Nom '{stationDTO.NomAtelier}' not found" });

                Station station = _mapper.Map<Station>(stationDTO);

                station.FkAtelier = existingAtelierByNom.IdAtelier;

                PostGenericCommand<Station> command = new PostGenericCommand<Station>(station);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostStation.");
                throw new Exception($"An unexpected error occurred while handling PostStation: {ex.Message}", ex);
            }
        }

        [HttpPut("put/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> PutStation([FromRoute] Guid id, [FromBody] StationDTO stationDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.IdStation == id);
                Station existingStationById = await _mediator.Send(query);

                if (existingStationById == null)
                    return NotFound(new MessageResponseDTO { Message = $"Station with ID '{id}' not found" });

                // check if another station with the same label as stationDTO.NomStation exists
                GetByGenericQuery<Station> queryByNom = new GetByGenericQuery<Station>(
                    station => station.NomStation == stationDTO.NomStation && station.IdStation != id);
                Station existingStationByNom = await _mediator.Send(queryByNom);

                if (existingStationByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"Station with Nom '{stationDTO.NomStation}' already exists" });

                //check if stationDTO.NomAtelier exists
                Atelier existingAtelierByNom = await _mediator.Send(new GetByGenericQuery<Atelier>(atelier => atelier.NomAtelier == stationDTO.NomAtelier));
                if (existingAtelierByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Atelier with Nom '{stationDTO.NomAtelier}' not found" });

                Station station = _mapper.Map<Station>(stationDTO);

                station.IdStation = id;
                station.IsActif = existingStationById.IsActif;   //IsActif can only be changed in ActivateStation fct
                station.FkAtelier = existingAtelierByNom.IdAtelier;

                PutGenericCommand<Station> command = new PutGenericCommand<Station>(station);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutStation.");
                throw new Exception($"An unexpected error occurred while handling PutStation: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> DeleteStation([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.IdStation == id);
                Station station = await _mediator.Send(query);

                if (station == null)
                    return NotFound(new MessageResponseDTO { Message = $"Station with id '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Station>(id));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteStation.");
                throw new Exception($"An unexpected error occurred while handling DeleteStation: {ex.Message}", ex);
            }
        }

        //only for supercomptes
        [HttpPut("activateStation/{code}")]
        public async Task<ActionResult<MessageResponseDTO>> ActivateStation([FromRoute] string code)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.CodeStation == code);
                Station station = await _mediator.Send(query);

                if (station == null)
                    return NotFound(new MessageResponseDTO { Message = $"Station with Code '{code}' not found" });

                // Toggle the station's status
                station.IsActif = !station.IsActif;

                var mediatorResponse = await _mediator.Send(new PutGenericCommand<Station>(station));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling ActivateStation.");
                throw new Exception($"An unexpected error occurred while handling ActivateStation: {ex.Message}", ex);
            }
        }
    }
}
