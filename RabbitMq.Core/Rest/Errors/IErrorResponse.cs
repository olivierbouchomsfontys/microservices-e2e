namespace RabbitMq.Shared.Rest.Errors
{
    public interface IErrorResponse
    {
        string Message { get; }
        string Title { get; }
        int Status { get; }
        string StatusDescription { get; }
    }
}