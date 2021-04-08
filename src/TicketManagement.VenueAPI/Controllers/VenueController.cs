using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.DataAccess.Entities;
using TicketManagement.VenueAPI.Services;

namespace TicketManagement.VenueAPI.Controllers
{
    [Route("venues")]
    public class VenueController : ControllerBase
    {
        private readonly VenueService _venueService;

        public VenueController(VenueService venueService)
        {
            _venueService = venueService;
        }

        /// <summary>
        /// Get venue by id from base.
        /// </summary>
        /// <param name="id">Venue ID.</param>
        /// <returns>Ok - 200 and VenueModel. 400 - BadRequest.</returns>
        [HttpGet("{id}")]
        public IActionResult GetVenue(int id)
        {
            var result = _venueService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get All venues from database.
        /// </summary>
        /// <returns>List of Venues.</returns>
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _venueService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        /// <summary>
        /// Insert new Venue into database. Roles needed - Venue or Administrator.
        /// </summary>
        /// <param name="model">VenueModel.</param>
        /// <returns>Ok - 200 + id of inserted model.</returns>
        [Authorize(Roles = "Venue, Administrator")]
        [HttpPost("create")]
        public IActionResult Insert([FromForm] Venue model)
        {
            var result = _venueService.Insert(model);

            return Ok(result);
        }

        /// <summary>
        /// Update Venue into database. Roles needed - Venue or Administrator.
        /// </summary>
        /// <param name="model">VenueModel.</param>
        /// <returns>Ok - 200.</returns>
        [Authorize(Roles = "Venue, Administrator")]
        [HttpPut("update")]
        public IActionResult Update([FromForm] Venue model)
        {
            var result = _venueService.Update(model);

            return Ok(result);
        }

        /// <summary>
        /// Delete Venue from database. Roles needed - Venue or Administrator.
        /// </summary>
        /// <param name="id">VenueModel.</param>
        /// <returns>Ok - 200.</returns>
        [Authorize(Roles ="Venue, Administrator")]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _venueService.DeleteById(id);

            return Ok(result);
        }
    }
}
