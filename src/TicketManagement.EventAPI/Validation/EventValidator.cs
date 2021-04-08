using System.Collections.Generic;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.EventAPI.Validation.ValidationRules;

namespace TicketManagement.EventAPI.Validation
{
    internal class EventValidator : IValidator<EventData>
    {
        private readonly List<IValidationRule<EventData>> _rules = new List<IValidationRule<EventData>>();

        public EventValidator()
        {
            _rules.Add(new DateNotInPastRule());
            _rules.Add(new StartDateEarlyThanEndDateRule());
        }

        public bool Validate(EventData entity)
        {
            return _rules.All(s => s.Validate(entity));
        }
    }
}