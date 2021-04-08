namespace TicketManagement.EventAPI.Validation.ValidationRules
{
    internal interface IValidationRule<T>
        where T : class
    {
        bool Validate(T arg);
    }
}
