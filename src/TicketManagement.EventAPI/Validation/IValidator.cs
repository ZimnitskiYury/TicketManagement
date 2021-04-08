namespace TicketManagement.EventAPI.Validation
{
    internal interface IValidator<T>
        where T : class
    {
        bool Validate(T entity);
    }
}
