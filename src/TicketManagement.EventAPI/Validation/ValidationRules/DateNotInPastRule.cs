using System;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.EventAPI.Validation.ValidationRules
{
    internal class DateNotInPastRule : IValidationRule<EventData>
    {
        public bool Validate(EventData arg)
        {
            return arg.StartDateTime > DateTime.Now;
        }
    }
}
