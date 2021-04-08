using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.DataAccess.Entities;
using TicketManagement.EventAPI.Manager;

namespace TicketManagement.EventAPI.Controllers
{
    /// <summary>
    /// Controller for all actions.
    /// </summary>
    [Route("events")]
    public class EventController : ControllerBase
    {
        private readonly EventManager _eventManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="eventManager">Instance from DI.</param>
        public EventController(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Get Event by Id. AllowAnonymous.
        /// </summary>
        /// <param name="id">Id of needed Event.</param>
        /// <returns>200 - returns Event. 400 - bad request.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            var result = _eventManager.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        /// <summary>
        /// Gets all seats by event.
        /// </summary>
        /// <param name="id">Event Id.</param>
        /// <returns>List of seats.</returns>
        [Authorize]
        [HttpGet("{id}/seats")]
        public IActionResult GetSeats(int id)
        {
            var model = _eventManager.GetSeats(id);
            if (model != null)
            {
                return Ok(model);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get selected Event seat.
        /// </summary>
        /// <param name="id">id of seat.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize]
        [HttpGet("seats/{id}")]
        public IActionResult GetSeat(int id)
        {
            var result = _eventManager.GetSeat(id);
            return Ok(result);
        }

        /// <summary>
        /// Get selected Event area.
        /// </summary>
        /// <param name="id">id of area.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize]
        [HttpGet("areas/{id}")]
        public IActionResult GetArea(int id)
        {
            var result = _eventManager.GetArea(id);
            return Ok(result);
        }

        /// <summary>
        /// Get Areas with price of selected Event.
        /// Only authorized users.
        /// </summary>
        /// <param name="id">Id of Event.</param>
        /// <returns>List of EventArea.</returns>
        [Authorize]
        [HttpGet("{id}/prices")]
        public IActionResult GetPrices(int id)
        {
            var result = _eventManager.GetAreasOfEvent(id);

            return Ok(result);
        }

        /// <summary>
        /// Gets all events from database.
        /// </summary>
        /// <returns>200 - list of events. 400 - bad request.</returns>
        [AllowAnonymous]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _eventManager.GetAll();
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        /// <summary>
        /// Adds new event.
        /// </summary>
        /// <param name="model">Event.</param>
        /// <returns>Id of inserted Event.</returns>
        [Authorize(Roles = "Event, Venue, Administrator")]
        [HttpPost("create")]
        public IActionResult Insert([FromForm] EventData model)
        {
            var result = _eventManager.Insert(model);

            return Ok(result);
        }

        /// <summary>
        /// Update Event.
        /// </summary>
        /// <param name="model">Model of event.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize(Roles = "Event, Venue, Administrator")]
        [HttpPost("update")]
        public IActionResult Update([FromForm] EventData model)
        {
            var result = _eventManager.Update(model);

            return Ok(result);
        }

        /// <summary>
        /// Update selected area.
        /// </summary>
        /// <param name="model">Area with price for update.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize(Roles = "Event, Venue, Administrator")]
        [HttpPost("prices/update")]
        public IActionResult UpdatePrice([FromForm] EventArea model)
        {
            var result = _eventManager.UpdateArea(model);
            return Ok(result);
        }

        /// <summary>
        /// Update selected seat.
        /// </summary>
        /// <param name="model">Seat with state for update.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize]
        [HttpPost("seats/update")]
        public IActionResult UpdateSeat([FromForm] EventSeat model)
        {
            var result = _eventManager.UpdateSeat(model);
            return Ok(result);
        }

        /// <summary>
        /// Delete selected Event by Id.
        /// </summary>
        /// <param name="id">Event by Id.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize(Roles = "Event, Venue, Administrator")]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _eventManager.Delete(id);

            return Ok(result);
        }
    }
}
