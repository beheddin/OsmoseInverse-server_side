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
    public class BassinController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BassinController> _logger;

        public BassinController(
            IMediator mediator,
            IMapper mapper,
            ILogger<BassinController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<BassinDTO>>> GetBassins()
        {
            try
            {
                GetAllGenericQuery<Bassin> query = new GetAllGenericQuery<Bassin>(includes: query => query.Include(bassin => bassin.Filiale));
                IEnumerable<Bassin> bassins = await _mediator.Send(query);

                if (bassins.IsNullOrEmpty())
                    return NotFound(new { message = "No Bassins were found" });

                IEnumerable<BassinDTO> bassinsDTO = bassins.Select(bassin => _mapper.Map<BassinDTO>(bassin)).ToList();

                return Ok(bassinsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetBassins.");
                throw new Exception($"An unexpected error occurred while handling GetBassins: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        public async Task<ActionResult<BassinDTO>> GetBassinById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Bassin> query = new GetByGenericQuery<Bassin>(bassin => bassin.IdSourceEau == id, includes: query => query.Include(bassin => bassin.Filiale));
                Bassin bassin = await _mediator.Send(query);

                if (bassin == null)
                    return NotFound(new { message = $"Bassin with ID '{id}' not found" });

                BassinDTO bassinDTO = _mapper.Map<BassinDTO>(bassin);

                return Ok(bassinDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetBassinById.");
                throw new Exception($"An unexpected error occurred while handling GetBassinById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        public async Task<ActionResult<MessageResponseDTO>> PostBassin([FromBody] BassinDTO bassinDTO)
        {
            try
            {
                //check if bassinDTO.NomSourceEau already exists
                GetByGenericQuery<Bassin> query = new GetByGenericQuery<Bassin>(bassin => bassin.NomSourceEau == bassinDTO.NomSourceEau);
                Bassin existingBassinByNom = await _mediator.Send(query);

                if (existingBassinByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"Bassin with Nom '{bassinDTO.NomSourceEau}' already exists" });

                //check if bassinDTO.NomFiliale exists
                Filiale existingFilialeByNom = await _mediator.Send(new GetByGenericQuery<Filiale>(filiale => filiale.NomFiliale == bassinDTO.NomFiliale));
                if (existingFilialeByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Filiale with Nom '{bassinDTO.NomFiliale}' not found" });

                Bassin bassin = _mapper.Map<Bassin>(bassinDTO);

                bassin.FkFiliale = existingFilialeByNom.IdFiliale;

                PostGenericCommand<Bassin> command = new PostGenericCommand<Bassin>(bassin);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostBassin.");
                throw new Exception($"An unexpected error occurred while handling PostBassin: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> PutBassin([FromRoute] Guid id, [FromBody] BassinDTO bassinDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Bassin> query = new GetByGenericQuery<Bassin>(bassin => bassin.IdSourceEau == id);
                Bassin existingBassinById = await _mediator.Send(query);

                if (existingBassinById == null)
                    return NotFound(new MessageResponseDTO { Message = $"Bassin with ID '{id}' not found" });

                // check if another bassin with the same Nom exists
                GetByGenericQuery<Bassin> queryByLabel = new GetByGenericQuery<Bassin>(
                    bassin => bassin.NomSourceEau == bassinDTO.NomSourceEau && bassin.IdSourceEau != id);
                Bassin existingBassinByLabel = await _mediator.Send(queryByLabel);

                if (existingBassinByLabel != null)
                    return Conflict(new MessageResponseDTO { Message = $"Bassin with Nom '{bassinDTO.NomSourceEau}' already exists" });

                //check if bassinDTO.NomFiliale exists
                Filiale existingFilialeByNom = await _mediator.Send(new GetByGenericQuery<Filiale>(filiale => filiale.NomFiliale == bassinDTO.NomFiliale));
                if (existingFilialeByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Filiale with Nom '{bassinDTO.NomFiliale}' not found" });

                Bassin bassin = _mapper.Map<Bassin>(bassinDTO);

                bassin.IdSourceEau = id;
                bassin.FkFiliale = existingFilialeByNom.IdFiliale;

                PutGenericCommand<Bassin> command = new PutGenericCommand<Bassin>(bassin);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutBassin.");
                throw new Exception($"An unexpected error occurred while handling PutBassin: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> DeleteBassin([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Bassin> query = new GetByGenericQuery<Bassin>(bassin => bassin.IdSourceEau == id);
                Bassin bassin = await _mediator.Send(query);

                if (bassin == null)
                    return NotFound(new MessageResponseDTO { Message = $"Bassin with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Bassin>(id));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteBassin.");
                throw new Exception($"An unexpected error occurred while handling DeleteBassin: {ex.Message}", ex);
            }
        }
    }
}
