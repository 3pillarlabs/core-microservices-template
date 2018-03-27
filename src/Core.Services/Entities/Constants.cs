namespace Core.Services.Entities
{
    public enum ErrorMessageType
    {
        Validation,
        Error
    }
    public enum ErrorsType
    {
        ValidationError=100,
        ResourceNotFoundError = 102,
        NoRecordFound =101,
        DatabaseError=103,
        UnhandledError=104
    }
}
