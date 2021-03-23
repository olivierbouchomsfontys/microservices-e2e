namespace CustomerOrderService.Messaging.Model
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public record CustomerCreatedModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}