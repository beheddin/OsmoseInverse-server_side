using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Domain.Entities;
using AutoMapper;
using Domain.Commands;
using Domain.DataTransferObjects;
using Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<StationController> _logger;

        public StationController(
            IMediator mediator,
            IMapper mapper,
            ILogger<StationController> logger
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
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.StationId == id, includes: query => query.Include(station => station.Atelier));

                Station station = await _mediator.Send(query);

                if (station == null)
                    return NotFound("Station not found");

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
        public async Task<ActionResult<string>> PostStation([FromBody] StationDTO stationDTO)
        {
            try
            {
                // check if the station already exists based on its label
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.StationLabel == stationDTO.StationLabel);
                Station existingStationByLabel = await _mediator.Send(query);

                if (existingStationByLabel != null)
                    return Conflict($"Station with label '{stationDTO.StationLabel}' already exists");


                //check if stationDTO.AtelierLabel exists
                Atelier existingAtelierByLabel = await _mediator.Send(new GetByGenericQuery<Atelier>(atelier => atelier.AtelierLabel == stationDTO.AtelierLabel));
                if (existingAtelierByLabel == null)
                    return NotFound($"Atelier with label '{stationDTO.AtelierLabel}' not found");

                Station station = _mapper.Map<Station>(stationDTO);
               
                station.FkAtelier = existingAtelierByLabel.AtelierId;

                PostGenericCommand<Station> command = new PostGenericCommand<Station>(station);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostStation.");
                throw new Exception($"An unexpected error occurred while handling PostStation: {ex.Message}", ex);
            }
        }

        [HttpPut("put/{id}")]
        public async Task<ActionResult<string>> PutStation([FromRoute] Guid id, [FromBody] StationDTO stationDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.StationId == id);
                Station existingStationById = await _mediator.Send(query);

                if (existingStationById == null)
                    return NotFound($"Station with id '{id}' not found");

                // check if another station with the same label as stationDTO.StationLabel exists
                GetByGenericQuery<Station> queryByLabel = new GetByGenericQuery<Station>(
                    station => station.StationLabel == stationDTO.StationLabel && station.StationId != id);
                Station existingStationByLabel = await _mediator.Send(queryByLabel);

                if (existingStationByLabel != null)
                    return Conflict($"Station with label '{stationDTO.StationLabel}' already exists");

                //check if stationDTO.AtelierLabel exists
                Atelier existingAtelierByLabel = await _mediator.Send(new GetByGenericQuery<Atelier>(atelier => atelier.AtelierLabel == stationDTO.AtelierLabel));
                if (existingAtelierByLabel == null)
                    return NotFound($"Atelier with label '{stationDTO.AtelierLabel}' not found");

                Station station = _mapper.Map<Station>(stationDTO);
                
                station.StationId = id;
                station.IsActif = existingStationById.IsActif;   //IsActif can only be changed in ActivateStation fct
                station.FkAtelier = existingAtelierByLabel.AtelierId;

                PutGenericCommand<Station> command = new PutGenericCommand<Station>(station);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutStation.");
                throw new Exception($"An unexpected error occurred while handling PutStation: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> DeleteStation([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.StationId == id);
                Station station = await _mediator.Send(query);

                if (station == null)
                    return NotFound($"Station with id '{id}' not found");

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Station>(id));

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteStation.");
                throw new Exception($"An unexpected error occurred while handling DeleteStation: {ex.Message}", ex);
            }
        }

        //only for superusers
        [HttpPut("activateStation/{id}")]
        public async Task<ActionResult<string>> ActivateStation([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Station> query = new GetByGenericQuery<Station>(station => station.StationId == id);
                Station station = await _mediator.Send(query);

                if (station == null)
                    return NotFound($"Station with id '{id}' don't exist");

                // Toggle the station's status
                station.IsActif = !station.IsActif;

                var mediatorResponse = await _mediator.Send(new PutGenericCommand<Station>(station));

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user's access status");
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }
    }
}
