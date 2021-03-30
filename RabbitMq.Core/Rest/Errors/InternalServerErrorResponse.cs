using System.Net;

namespace RabbitMq.Shared.Rest.Errors
{
    public class InternalServerErrorResponse : IErrorResponse
    {
        public string Message { get; }
        public string Title { get; } = "Error";
        public int Status => (int) HttpStatusCode.InternalServerError;
        public string StatusDescription => "Internal server error";

        public static InternalServerErrorResponse Create(string message)
        {
            return new(message);
        }
        
        public static InternalServerErrorResponse Create()
        {
            string message = "An unknown error has occured";
            
            return new(message);
        }

        private InternalServerErrorResponse(string message)
        {
            Message = message;
        }
    }
}