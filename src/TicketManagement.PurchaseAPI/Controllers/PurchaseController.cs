using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.PurchaseAPI.Models;
using TicketManagement.PurchaseAPI.Services;

namespace TicketManagement.PurchaseAPI.Controllers
{
    /// <summary>
    /// Purchase Flow (API).
    /// </summary>
    [Route("purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly EventApiService _eventApiService;
        private readonly UserApiService _userApiService;
        private readonly PurchaseService _purchaseService;
        private readonly TransactionService _transactionService;

        public PurchaseController(EventApiService eventApiService, UserApiService userApiService, PurchaseService purchaseService, TransactionService transactionService)
        {
            _eventApiService = eventApiService;
            _userApiService = userApiService;
            _purchaseService = purchaseService;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get all reserved and bought seats by user.
        /// </summary>
        [Authorize]
        [HttpGet("order/getall")]
        public IActionResult GetOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = from purchase in _purchaseService.GetAll()
                        where purchase.UserId == userId
                        select purchase;
            return Ok(model.ToList());
        }

        /// <summary>
        /// Get selected order.
        /// </summary>
        /// <param name="id">order.</param>
        [Authorize]
        [HttpGet("order/{id}")]
        public IActionResult GetOrder(int id)
        {
            var model = _purchaseService.GetById(id);
            if (model != null)
            {
                return Ok(model);
            }

            return Forbid();
        }

        /// <summary>
        /// Get selected transaction.
        /// </summary>
        /// <param name="id">transaction.</param>
        [Authorize]
        [HttpGet("transaction/{id}")]
        public IActionResult GetTransaction(int id)
        {
            var model = _transactionService.GetById(id);
            if (model != null)
            {
                return Ok(model);
            }

            return Forbid();
        }

        /// <summary>
        /// Payment info of selected order.
        /// </summary>
        /// <param name="id">order.</param>
        [Authorize]
        [HttpGet("payment/{id}")]
        public async Task<IActionResult> GetPaymentAsync(int id)
        {
            var accesToken = Request.Headers["Authorization"];

            // cut "Bearer " part of string
            var token = accesToken.ToString().Substring(7);

            var purchase = _purchaseService.GetById(id);
            var seat = await _eventApiService.GetSeatAsync(purchase.EventSeatId, token);
            var area = await _eventApiService.GetAreaAsync(seat.EventAreaId, token);
            var eve = await _eventApiService.GetEventAsync(area.EventId, token);
            var model = new PaymentModel
            {
                Id = purchase.Id,
                EventSeatId = seat.Id,
                EventAreaDescription = area.Description,
                Number = seat.Number,
                Row = seat.Row,
                Price = seat.Price,
                EventName = eve.Name,
                EventId = eve.Id,
                TransactionId = purchase.TransactionId,
                UserId = purchase.UserId,
            };

            return Ok(model);
        }

        /// <summary>
        /// Reserve selected seat to user.
        /// </summary>
        /// <param name="id">Seat.</param>
        [Authorize]
        [HttpPost("reserve/")]
        public async Task<IActionResult> ReserveAsync([FromForm] int id)
        {
            var accesToken = Request.Headers["Authorization"];

            // cut "Bearer " part of string
            var token = accesToken.ToString().Substring(7);

            var model = await _eventApiService.GetSeatAsync(id, token);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (model.State != StateSeat.Free)
            {
                return Forbid();
            }

            model.State = StateSeat.Reserved;
            await _eventApiService.UpdateSeatAsync(model, token);
            _purchaseService.Insert(new DataAccess.Entities.Purchase
            {
                EventSeatId = model.Id,
                UserId = userId,
            });

            return Ok();
        }

        /// <summary>
        /// Payment selected order.
        /// </summary>
        /// <param name="id">Order.</param>
        [Authorize]
        [HttpPost("payment/")]
        public async Task<IActionResult> PaymentAsync([FromForm] int id)
        {
            var accesToken = Request.Headers["Authorization"];

            // cut "Bearer " part of string
            var token = accesToken.ToString().Substring(7);

            var purchase = _purchaseService.GetById(id);
            var seat = await _eventApiService.GetSeatAsync(purchase.EventSeatId, token);
            var area = await _eventApiService.GetAreaAsync(seat.EventAreaId, token);
            var login = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _userApiService.GetUser(login, token);
            if (user.Balance >= area.Price)
            {
                var transactionId = _transactionService.Insert(new DataAccess.Entities.Transaction { Amount = -area.Price, DateOfTransaction = DateTime.Now, UserId = user.Id });
                user.Balance -= area.Price;
                await _userApiService.UpdateBalance(user, token);
                seat.State = StateSeat.Bought;
                await _eventApiService.UpdateSeatAsync(seat, token);
                purchase.TransactionId = transactionId;
                _purchaseService.Update(purchase);
                return Ok(transactionId);
            }

            return Forbid();
        }
    }
}
