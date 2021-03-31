using System;
using System.Net;
using RabbitMq.Shared.Rest.Errors.Action;

namespace RabbitMq.Shared.Rest.Errors
{
    public class NotFoundResponse : IErrorResponse
    {
        public string Title { get; } = "Error";
        public string Message { get; }

        public int Status => (int) HttpStatusCode.NotFound;
        public string StatusDescription => "Not found";

        public static NotFoundResponse Create<T>(object identifier)
        {
            return Create(typeof(T), identifier, CrudAction.Find);
        }

        public static NotFoundResponse Create<T>(object identifier, CrudAction action)
        {
            return Create(typeof(T), identifier, action);
        }
        
        private static NotFoundResponse Create(Type type, object id, CrudAction action)
        {
            string message = $"Could not {action.ToString().ToLower()} {type.Name.ToLower()} with id = {id}";
            
            return new (message);
        }

        private NotFoundResponse(string message)
        {
            Message = message;
        }
    }
}