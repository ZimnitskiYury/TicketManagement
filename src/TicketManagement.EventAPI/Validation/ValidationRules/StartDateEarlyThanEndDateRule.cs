using TicketManagement.DataAccess.Entities;

namespace TicketManagement.EventAPI.Validation.ValidationRules
{
    internal class StartDateEarlyThanEndDateRule : IValidationRule<EventData>
    {
        public bool Validate(EventData arg)
        {
            return arg.StartDateTime < arg.EndDateTime;
        }
    }
}
